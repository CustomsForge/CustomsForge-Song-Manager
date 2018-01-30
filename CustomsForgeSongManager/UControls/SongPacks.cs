using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.UITheme;
using GenTools;
using CFSM.RSTKLib.PSARC;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using DataGridViewTools;
using SevenZip;
using RocksmithToolkitLib.XmlRepository;

namespace CustomsForgeSongManager.UControls
{
    public partial class SongPacks : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        // TODO: Impliment Safe Threading for long duration processes
        // HINT: search for occurences of 'this.Enabled = false;'

        private RSDataJsonDictionary<RS2SongsData> CacheDisabledEntireCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CacheDisabledSongCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CacheEntireCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CacheSongCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS1DiscData> CustomDisabledEntireCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> CustomDisabledSongCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> CustomEntireCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> CustomSongCollection = new RSDataJsonDictionary<RS1DiscData>();

        private RSDataJsonDictionary<RS1DiscData> Rs1DiscDisabledEntireCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> Rs1DiscDisabledSongCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> Rs1DiscEntireCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> Rs1DiscSongCollection = new RSDataJsonDictionary<RS1DiscData>();

        private RSDataJsonDictionary<RS1DlcData> Rs1DlcDisabledEntireCollection = new RSDataJsonDictionary<RS1DlcData>();
        private RSDataJsonDictionary<RS1DlcData> Rs1DlcDisabledSongCollection = new RSDataJsonDictionary<RS1DlcData>();
        private RSDataJsonDictionary<RS1DlcData> Rs1DlcEntireCollection = new RSDataJsonDictionary<RS1DlcData>();
        private RSDataJsonDictionary<RS1DlcData> Rs1DlcSongCollection = new RSDataJsonDictionary<RS1DlcData>();

        private bool allSelected = false;
        private bool bindingCompleted = false;
        private string customInternalHsanPath;
        private string customPackPsarcPath;
        private bool dgvPainted = false;
        private string extractedCustomHsanPath;
        private List<SongPackData> songPackList = new List<SongPackData>(); // prevents filtering from being inherited

        public SongPacks()
        {
            InitializeSongPacks();
        }

        public void UpdateToolStrip()
        {
            if (Globals.ReloadSongPacks)
                InitializeSongPacks();
            else
            {
                Globals.TsLabel_MainMsg.Text = string.Format("Song Count: {0}", dgvSongPacks.Rows.Count);
                Globals.TsLabel_MainMsg.Visible = true;
                var tsldcMsg = String.Format("Disabled CDLC: {0}", DisabledSongColorizerCounter());
                Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
                Globals.TsLabel_DisabledCounter.Visible = true;
            }
        }

        // ... filtering
        private void AddSongsToNestedDictionary(dynamic songsToBeAddedCollection, dynamic songCollection, dynamic fullSongCollection, bool enabled, bool RS2014 = false)
        {
            foreach (dynamic song in songsToBeAddedCollection)
            {
                foreach (dynamic songAttributes in song.Value)
                {
                    dynamic songData = songAttributes.Value;
                    if (songData.SongName != null)
                    {
                        SongPackData sngData = AttributesToSongPackData(songData, false);

                        // only add once
                        if (!sngData.Title.Contains("$[") && !sngData.Title.Contains("RS2") && !sngData.SongKey.Contains("GE_FE_") && !DictionaryContains(songCollection, songData))
                            songCollection.Add(song.Key, song.Value);

                        if (RS2014)
                        {
                            if (sngData.Title.Contains("$[") || sngData.Title.Contains("RS2") || sngData.SongKey.Contains("GE_FE_"))
                                fullSongCollection.Remove(song.Key);
                        }
                    }
                }
            }
        }

        private SongPackData AttributesToSongPackData(dynamic songAttributes, bool enabled = true)
        {
            SongPackData song = new SongPackData();
            string tuning = String.Empty;

            song.Selected = false;
            song.Enabled = enabled ? "Yes" : "No";
            song.Title = songAttributes.SongName;
            song.Artist = songAttributes.ArtistName;
            song.Album = songAttributes.AlbumName;
            song.SongYear = songAttributes.SongYear;
            var seconds = Convert.ToInt32(songAttributes.SongLength) % 60;
            var minutes = Convert.ToInt32(songAttributes.SongLength) / 60;
            song.SongLength = String.Format("{0:00}:{1:00}", minutes, seconds);
            song.SongKey = songAttributes.SongKey;

            if (songAttributes.ArrangementName != "Vocals")
            {
                foreach (KeyValuePair<string, int> stringTuning in songAttributes.Tuning)
                    tuning += stringTuning.Value;

                song.Tuning = PsarcExtensions.TuningStringToName(tuning);
            }
            else
            {
                song.Tuning = "Other";
            }
            return song;
        }

        private void ClearCollections()
        {
            CacheDisabledEntireCollection.Clear();
            CacheDisabledSongCollection.Clear();
            CacheEntireCollection.Clear();
            CacheSongCollection.Clear();

            Rs1DiscDisabledEntireCollection.Clear();
            Rs1DiscDisabledSongCollection.Clear();
            Rs1DiscEntireCollection.Clear();
            Rs1DiscSongCollection.Clear();

            Rs1DlcDisabledEntireCollection.Clear();
            Rs1DlcDisabledSongCollection.Clear();
            Rs1DlcEntireCollection.Clear();
            Rs1DlcSongCollection.Clear();

            CustomDisabledEntireCollection.Clear();
            CustomDisabledSongCollection.Clear();
            CustomEntireCollection.Clear();
            CustomSongCollection.Clear();
        }

        private bool ConditionalBackup(string sourcePath, string backupPath, bool forceBackup = false, bool writeProtect = true)
        {
            if (!File.Exists(sourcePath))
                return false;

            if (!Directory.Exists(Path.GetDirectoryName(backupPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(backupPath));

            if (File.Exists(backupPath))
                if (!forceBackup)
                    return false;

            if (File.Exists(backupPath))
                File.SetAttributes(backupPath, FileAttributes.Normal);

            try
            {
                Globals.Log("Making backup copy of: " + sourcePath);
                File.Copy(sourcePath, backupPath, true);
            }
            catch (Exception ex)
            {
                // it is critical that backup of originals was successful before proceeding
                throw new Exception("<ERROR> Backup of: " + sourcePath + " ... FAILED" + Environment.NewLine + ex.Message + Environment.NewLine + "Please correct issue and make sure you have" + Environment.NewLine + "backup copies of your original song files.");
            }

            if (writeProtect)
                File.SetAttributes(backupPath, FileAttributes.ReadOnly | FileAttributes.Archive);

            return true;
        }

        private bool DictionaryContains(dynamic dictionary, dynamic songData)
        {
            foreach (dynamic song in dictionary)
            {
                foreach (dynamic songAttributes in song.Value)
                {
                    if (songAttributes.Value.SongName == songData.SongName && songAttributes.Value.AlbumName == songData.AlbumName)
                        return true;
                }
            }
            return false;
        }

        private void DisableSong<T>(RSDataJsonDictionary<T> songCollection, RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection, RSDataJsonDictionary<T> fullDisabledSongCollection, string rowSongKey) where T : RSDataAbstractBase
        {
            var matchingSongs = fullSongCollection.Where(x => x.Value.Any(song => song.Key == "Attributes" && song.Value.SongKey == rowSongKey)).Select(z => z.Key).ToList();

            if (matchingSongs.Count > 0)
            {
                matchingSongs.ForEach(sngKey =>
                    {
                        fullDisabledSongCollection.Add(sngKey, fullSongCollection[sngKey]);
                        fullSongCollection.Remove(sngKey);
                        songCollection.Remove(sngKey);
                    });

                disabledSongCollection.Add(matchingSongs[0], fullDisabledSongCollection[matchingSongs[0]]);
            }
        }

        private int DisabledSongColorizerCounter()
        {
            int disabledCount = 0;

            foreach (DataGridViewRow row in dgvSongPacks.Rows)
            {
                if (row.Cells["colEnabled"].Value.ToString() == "No")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    disabledCount++;
                }
                else
                    row.DefaultCellStyle.BackColor = Color.Empty; // Color.White;
            }

            return disabledCount;
        }

        private void EnableSong<T>(RSDataJsonDictionary<T> songCollection, RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection, RSDataJsonDictionary<T> fullDisabledSongCollection, string rowSongKey) where T : RSDataAbstractBase
        {
            var matchingSongs = fullDisabledSongCollection.Where(x => x.Value.Any(song => song.Value.SongKey == rowSongKey)).Select(z => z.Key).ToList();

            if (matchingSongs.Count > 0)
            {
                matchingSongs.ForEach(sngKey =>
                    {
                        fullSongCollection.Add(sngKey, fullDisabledSongCollection[sngKey]);
                        fullDisabledSongCollection.Remove(sngKey);
                        disabledSongCollection.Remove(sngKey);
                    });

                fullSongCollection.OrderBy(sng => sng.Key);
                songCollection.Add(matchingSongs[0], fullSongCollection[matchingSongs[0]]);
            }
        }

        private bool ExtractSongsHsan()
        {
            if (File.Exists(Constants.ExtractedSongsHsanPath))
                File.Delete(Constants.ExtractedSongsHsanPath);

            if (ZipUtilities.ExtractSingleFile(Constants.SongPacksFolder, Constants.Cache7zPath, Constants.SongsHsanInternalPath))
            {
                FileInfo f = new FileInfo(Constants.ExtractedSongsHsanPath);
                if (f.Length > 0)
                    return true;
            }

            MessageBox.Show("<ERROR> Extracting song cache ... FAILED", "Extraction Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }

        private List<dynamic> GetMatchingSongs(dynamic dictionary, string matchingValue)
        {
            List<dynamic> matchingSongs = new List<dynamic>();

            foreach (dynamic song in dictionary)
            {
                foreach (dynamic songAttributes in song.Value)
                {
                    if (songAttributes.Value.ArtistName != null)
                    {
                        if (songAttributes.Value.SongName.ToLower().Contains(matchingValue) || songAttributes.Value.AlbumName.ToLower().Contains(matchingValue) || songAttributes.Value.ArtistName.ToLower().Contains(matchingValue) || PsarcExtensions.TuningStringToName(TuningJsonToStrings(songAttributes.Value.Tuning)).ToLower().Contains(matchingValue) || songAttributes.Value.SongKey.ToLower().Contains(matchingValue))
                            matchingSongs.Add(songAttributes.Value);
                    }
                }
            }

            return matchingSongs;
        }

        private void InitializeSongPacks()
        {
            // provides for complete fresh reload of tabpage
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            this.Enabled = true;
            Globals.ReloadSongPacks = false;
            InitializeSongPacksCombo();
            PopulateSongPacks();
        }

        private void InitializeSongPacksCombo()
        {
            cmbSongPacks.Items.Clear();
            cmbSongPacks.Items.Add("Rocksmith 2014 Base Songs");
            cmbSongPacks.Items.Add("RS1 Compatibility Disc");
            cmbSongPacks.Items.Add("RS1 Compatibility DLC");
            cmbSongPacks.Items.Add("Custom Song Pack");

            cmbSongPacks.SelectedIndex = 0; // gens call to EH cmbSongPacks.Index_Changed
        }

        private void LoadCustomSongPack()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Song Pack Files (*.psarc)|*_songpack_p.psarc; *_sp_p.psarc|All Files (*.psarc)|*.psarc";
                ofd.Title = "Select a Custom Song Pack File";
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                customPackPsarcPath = ofd.FileName;
                txtFileName.Text = Path.GetFileName(customPackPsarcPath);
            }

            dgvSongPacks.Rows.Clear();
            CustomDisabledEntireCollection.Clear();
            CustomDisabledSongCollection.Clear();
            CustomEntireCollection.Clear();
            CustomSongCollection.Clear();

            // song pack validation
            var version = String.Empty;
            var tkVersionData = PsarcExtensions.ExtractArchiveFile(customPackPsarcPath, "toolkit.version");
            if (tkVersionData != null)
            {
                ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(tkVersionData));
                version = tkInfo.PackageVersion ?? "N/A";

                if (!version.Contains("SongPack"))
                    if (DialogResult.No == MessageBox.Show("<WARNING> File: " + txtFileName.Text + Environment.NewLine +
                        "is missing the expected Custom Song Pack identifier.  " + Environment.NewLine + Environment.NewLine +
                        "Would you like to try to load the file anyhow?", "Select a Song Pack File", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
                        return;
            }
            else // ODLC
                return;

            // song pack produced by toolkit has only one hsan file
            customInternalHsanPath = PsarcExtensions.ExtractArchiveFile(customPackPsarcPath, "hsan", Constants.SongPacksFolder);
            var extractedCustomHsanFile = Path.GetFileName(customInternalHsanPath);
            extractedCustomHsanPath = Path.Combine(Constants.SongPacksFolder, extractedCustomHsanFile);
            ConditionalBackup(customPackPsarcPath, Path.Combine(Constants.Rs2OriginalsFolder, Path.ChangeExtension(Path.GetFileName(customPackPsarcPath), ".org.psarc")));
            ConditionalBackup(extractedCustomHsanPath, Path.Combine(Constants.SongPacksFolder, Path.ChangeExtension(extractedCustomHsanFile, ".org.hsan")));
            PopulateSongList(extractedCustomHsanPath, CustomSongCollection, ref CustomEntireCollection, CustomDisabledSongCollection, ref CustomDisabledEntireCollection);

            if (CustomSongCollection.Count > 0)
                LoadSongPackList(CustomSongCollection, CustomDisabledSongCollection);
            else
                Globals.Log("Error: No songs found in: " + txtFileName.Text + " ...");
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSongPacks.AutoGenerateColumns = false;
            var fbl = new FilteredBindingList<SongPackData>(list);
            var bs = new BindingSource { DataSource = fbl };
            dgvSongPacks.DataSource = bs;
        }

        private void LoadSongPackList(dynamic enabledSongCollection, dynamic disabledSongCollection)
        {
            songPackList = new List<SongPackData>();
            var filteredDisabled = GetMatchingSongs(disabledSongCollection, cueSearch.Text);

            foreach (var song in filteredDisabled)
                songPackList.Add(AttributesToSongPackData(song, false));

            var filteredEnabled = GetMatchingSongs(enabledSongCollection, cueSearch.Text);

            foreach (var song in filteredEnabled)
                songPackList.Add(AttributesToSongPackData(song));

            LoadFilteredBindingList(songPackList);
        }

        private void PopulateDataGridView()
        {
            Globals.Log("Populating Song Packs GUI ...");
            DgvExtensions.DoubleBuffered(dgvSongPacks);
            RefreshDgvSongs();
            CFSMTheme.InitializeDgvAppearance(dgvSongPacks);

            if (RAExtensions.ManagerGridSettings != null)
                dgvSongPacks.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            else
                Globals.Settings.SaveSettingsToFile(dgvSongPacks);
        }

        // ... deserialize filePath
        private void PopulateSongList<T>(string songFilePath, RSDataJsonDictionary<T> songCollection, ref RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection, ref RSDataJsonDictionary<T> fullDisabledSongCollection) where T : RSDataAbstractBase
        {
            Globals.TsProgressBar_Main.Value = 0;

            try
            {
                string songFileContent = File.ReadAllText(songFilePath);
                var songsJson = JObject.Parse(songFileContent);
                var songsList = songsJson["Entries"];
                Globals.TsProgressBar_Main.Value = 50;

                fullSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<T>>(songsList.ToString());
                AddSongsToNestedDictionary(fullSongCollection, songCollection, fullSongCollection, true);
                Globals.TsProgressBar_Main.Value = 70;

                if (songsJson.ToString().Contains("DisabledSongs"))
                {
                    var disabledSongsList = songsJson["DisabledSongs"];
                    Globals.TsProgressBar_Main.Value = 90;
                    fullDisabledSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<T>>(disabledSongsList.ToString());
                    AddSongsToNestedDictionary(fullDisabledSongCollection, disabledSongCollection, fullSongCollection, false);
                }
                 else if(songsJson.ToString().Contains("DisabledEntries")) // allow CFSM to read/convert to standard DisabledEntries
                {
                    var disabledSongsList = songsJson["DisabledEntries"];
                    Globals.TsProgressBar_Main.Value = 90;
                    fullDisabledSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<T>>(disabledSongsList.ToString());
                    AddSongsToNestedDictionary(fullDisabledSongCollection, disabledSongCollection, fullSongCollection, false);
                }
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Please reload songs, it seems that a song pack is corrupt!", "JSON error");
            }
        }

        private bool PopulateSongLists()
        {
            ClearCollections();

            if (File.Exists(Constants.ExtractedSongsHsanPath))
                PopulateSongList(Constants.ExtractedSongsHsanPath, CacheSongCollection, ref CacheEntireCollection, CacheDisabledSongCollection, ref CacheDisabledEntireCollection);

            if (File.Exists(Constants.ExtractedRs1DiscHsanPath))
                PopulateSongList(Constants.ExtractedRs1DiscHsanPath, Rs1DiscSongCollection, ref Rs1DiscEntireCollection, Rs1DiscDisabledSongCollection, ref Rs1DiscDisabledEntireCollection);

            if (File.Exists(Constants.ExtractedRs1DlcHsanPath))
                PopulateSongList(Constants.ExtractedRs1DlcHsanPath, Rs1DlcSongCollection, ref Rs1DlcEntireCollection, Rs1DlcDisabledSongCollection, ref Rs1DlcDisabledEntireCollection);

            var songCount = CacheSongCollection.Count + Rs1DiscSongCollection.Count + Rs1DlcSongCollection.Count;

            if (songCount == 0)
                return false;

            return true;
        }

        private void PopulateSongPacks()
        {
            Globals.TsProgressBar_Main.Value = 0;
            Globals.Log("Populating Song Packs GUI ...");
            Globals.Settings.LoadSettingsFromFile(dgvSongPacks);
            Globals.TuningXml = TuningDefinitionRepository.Instance.LoadTuningDefinitions(GameVersion.RS2014);
            var rsDir = AppSettings.Instance.RSInstalledDir;

            if (Directory.Exists(rsDir))
            {
                // make sure we have write access to the RSInstallDir
                if (!ZipUtilities.EnsureWritableDirectory(rsDir))
                    ZipUtilities.RemoveReadOnlyAttribute(rsDir);

                if (SmartSongLoader())
                    if (PopulateSongLists())
                        PopulateDataGridView();
                    else
                        Globals.Log("<ERROR> PopulateSongLists ... FAILED");
                else
                    Globals.Log("<ERROR> SmartSongLoader ... FAILED");
            }
            else
            {
                Globals.ReloadSongPacks = true;
                Globals.Log("<ERROR> Method PopulateSongPacks ... FAILED");
                Globals.Log("Could not find Rocksmith Installation Directory");
            }

            // free up memory
            Globals.TuningXml.Clear();
            Globals.TsProgressBar_Main.Value = 100;
        }

        private void RefreshDgvSongs()
        {
            allSelected = false;
            cueSearch.Text = String.Empty;

            switch (cmbSongPacks.SelectedIndex)
            {
                case 0: // cache.psarc
                    txtFileName.Text = "cache.psarc";
                    LoadSongPackList(CacheSongCollection, CacheDisabledSongCollection);
                    break;
                case 1: // rs1compatibilitydisc_p.psarc
                    txtFileName.Text = " rs1compatibilitydisc" + Constants.PsarcExtension;
                    LoadSongPackList(Rs1DiscSongCollection, Rs1DiscDisabledSongCollection);
                    break;
                case 2: // rs1compatibilitydlc_p.psarc
                    txtFileName.Text = "rs1compatibilitydlc" + Constants.PsarcExtension;
                    LoadSongPackList(Rs1DlcSongCollection, Rs1DlcDisabledSongCollection);
                    break;
                case 3: // custom song pack
                    txtFileName.Text = Path.GetFileName(customPackPsarcPath);
                    LoadSongPackList(CustomSongCollection, CustomDisabledSongCollection);
                    break;
                default:
                    throw new Exception("Song Packs Combobox Failure");
            }

            UpdateToolStrip();
        }

        private void RemoveFilter()
        {
            // save current sorting before removing filter
            DgvExtensions.SaveSorting(dgvSongPacks);
            // remove the filter
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongPacks);
            UpdateToolStrip();
            // reapply sort direction to reselect the filtered song
            DgvExtensions.RestoreSorting(dgvSongPacks);
        }

        private void RepackCachePsarc()
        {
            try
            {
                if (!Directory.Exists(Constants.CachePcPath))
                    throw new IOException();

                if (!File.Exists(Path.Combine(Constants.CachePcPath, "sltsv1_aggregategraph.nt")))
                    GenExtensions.ExtractEmbeddedResource(Constants.CachePcPath, Assembly.GetExecutingAssembly(), "CustomsForgeSongManager.Resources", new string[] { "sltsv1_aggregategraph.nt" });

                ZipUtilities.InjectFile(Constants.ExtractedSongsHsanPath, Constants.Cache7zPath, Constants.SongsHsanInternalPath, OutArchiveFormat.SevenZip, CompressionMode.Append);
                Packer.Pack(Constants.CachePcPath, Constants.CachePsarcPath);
                Globals.Log("cache.psarc repackaged with your song selections ...");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to repack cache.psarc" + Environment.NewLine + "Error: " + ex.Message.ToString());
            }
        }

        private void RestoreOriginalSongBackups()
        {
            Cursor = Cursors.WaitCursor;
            Globals.Log("Restoring Original Backup Files ...");
            Globals.TsProgressBar_Main.Value = 0;
            try
            {
                if (File.Exists(Constants.Rs1DiscPsarcBackupPath))
                {
                    Globals.TsProgressBar_Main.Value = 20;

                    if (File.Exists(Constants.Rs1DiscPsarcPath.Replace(Constants.PsarcExtension, Constants.DisabledPsarcExtension)))
                        File.Delete(Constants.Rs1DiscPsarcPath.Replace(Constants.PsarcExtension, Constants.DisabledPsarcExtension));

                    File.Copy(Constants.Rs1DiscPsarcBackupPath, Constants.Rs1DiscPsarcPath, true);
                    File.SetAttributes(Constants.Rs1DiscPsarcPath, FileAttributes.Normal);
                    Globals.Log("rs1compatibilitydisc_p.psarc backup restored ...");
                }

                if (File.Exists(Constants.Rs1DlcPsarcBackupPath))
                {
                    Globals.TsProgressBar_Main.Value = 40;

                    if (File.Exists(Constants.Rs1DlcPsarcPath.Replace("_p.", "_p.disabled.")))
                        File.Delete(Constants.Rs1DlcPsarcPath.Replace("_p.", "_p.disabled."));

                    File.Copy(Constants.Rs1DlcPsarcBackupPath, Constants.Rs1DlcPsarcPath, true);
                    File.SetAttributes(Constants.Rs1DlcPsarcPath, FileAttributes.Normal);
                    Globals.Log("rs1compatibilitydlc_p.psarc backup restored ...");
                }

                if (File.Exists(Constants.CachePsarcBackupPath))
                {
                    Globals.TsProgressBar_Main.Value = 60;
                    File.Copy(Constants.CachePsarcBackupPath, Constants.CachePsarcPath, true);
                    File.SetAttributes(Constants.CachePsarcPath, FileAttributes.Normal);
                    Globals.Log("cache.psarc backup restored ...");
                }

                if (Directory.Exists(Constants.SongPacksFolder))
                {
                    Globals.TsProgressBar_Main.Value = 80;
                    ZipUtilities.DeleteDirectory(Constants.SongPacksFolder);
                    Globals.Log("Removed: " + Constants.SongPacksFolder + " ...");
                }

                Globals.TsProgressBar_Main.Value = 100;
                Globals.Log("Restored to original condition ... SUCCESSFUL");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to restore backup! Error:\n\n" + ex.Message.ToString(), "Error");
            }

            Cursor = Cursors.Default;

            // restoring orignal backup may impact other areas of app
            // so rescan on tabpage change
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
        }

        private void SerializeSongFile<T>(string destHsanPath, RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> fullDisabledSongCollection, Type vdType) where T : RSDataAbstractBase
        {
            var tempFullSongCollection = new Dictionary<string, Dictionary<string, dynamic>>();
            var tempFullDisabledSongCollection = new Dictionary<string, Dictionary<string, dynamic>>();

            var DataConstructor = vdType.GetConstructor(new Type[] { });

            RSDataJsonDictionary<T>[] dicArray = new RSDataJsonDictionary<T>[] { fullSongCollection, fullDisabledSongCollection };

            Dictionary<string, Dictionary<string, dynamic>>[] tempDicArray = new Dictionary<string, Dictionary<string, dynamic>>[] { tempFullSongCollection, tempFullDisabledSongCollection };

            for (int i = 0; i < dicArray.Length; i++)
            {
                foreach (string song in dicArray[i].Keys.ToList())
                {
                    foreach (var songAttributes in dicArray[i][song])
                    {
                        var sngVal = songAttributes.Value;

                        if (sngVal.ArrangementName == "Vocals")
                        {
                            var rsv = (RS2VocalsData)DataConstructor.Invoke(new object[] { });
                            rsv.AlbumArt = sngVal.AlbumArt;
                            rsv.ArrangementName = "Vocals";
                            rsv.DLC = sngVal.DLC;
                            rsv.LeaderboardChallengeRating = sngVal.LeaderboardChallengeRating;
                            rsv.ManifestUrn = sngVal.ManifestUrn;
                            rsv.MasterID_RDV = sngVal.MasterID_RDV;
                            rsv.PersistentID = sngVal.PersistentID;
                            rsv.SKU = sngVal.SKU;
                            rsv.Shipping = sngVal.Shipping;
                            rsv.SongKey = sngVal.SongKey;

                            if (rsv is RS1DiscVocalsData)
                                ((RS1DiscVocalsData)rsv).DLCKey = (sngVal as RS1DiscData).DLCKey;

                            if (rsv is RS1DLCVocalsData)
                                ((RS1DLCVocalsData)rsv).DLCKey = (sngVal as RS1DlcData).DLCKey;

                            var currSng = new Dictionary<string, dynamic>();
                            currSng.Add("Attributes", rsv);
                            tempDicArray[i].Add(song, currSng);
                        }
                        else if (sngVal.SongName.Contains("[") || sngVal.SongName == "")
                        {
                            RS2SongsData rsv;
                            if (!sngVal.AlbumName.Contains("Ubisoft") || sngVal.SongName == "")
                                rsv = new RSLessonSongData();
                            else
                                rsv = new RSUbisoftLessonSongData();

                            rsv.AlbumArt = sngVal.AlbumArt;
                            rsv.AlbumName = sngVal.AlbumName;
                            rsv.AlbumNameSort = sngVal.AlbumNameSort;
                            rsv.ArrangementName = sngVal.ArrangementName;
                            rsv.ArtistName = sngVal.ArtistName;
                            rsv.ArtistNameSort = sngVal.ArtistNameSort;
                            rsv.CentOffset = sngVal.CentOffset;
                            rsv.DLC = sngVal.DLC;
                            rsv.DNA_Chords = sngVal.DNA_Chords;
                            rsv.DNA_Riffs = sngVal.DNA_Riffs;
                            rsv.DNA_Solo = sngVal.DNA_Solo;
                            rsv.EasyMastery = sngVal.EasyMastery;
                            rsv.LeaderboardChallengeRating = sngVal.LeaderboardChallengeRating;
                            rsv.ManifestUrn = sngVal.ManifestUrn;
                            rsv.MasterID_RDV = sngVal.MasterID_RDV;
                            rsv.MediumMastery = sngVal.MediumMastery;
                            rsv.NotesEasy = sngVal.NotesEasy;
                            rsv.NotesHard = sngVal.NotesHard;
                            rsv.NotesMedium = sngVal.NotesMedium;
                            rsv.PersistentID = sngVal.PersistentID;
                            rsv.Representative = sngVal.Representative;
                            rsv.RouteMask = sngVal.RouteMask;
                            rsv.SKU = sngVal.SKU;
                            rsv.Shipping = sngVal.Shipping;
                            rsv.SongDiffEasy = sngVal.SongDiffEasy;
                            rsv.SongDiffHard = sngVal.SongDiffHard;
                            rsv.SongDiffMed = sngVal.SongDiffMed;
                            rsv.SongDifficulty = sngVal.SongDifficulty;
                            rsv.SongKey = sngVal.SongKey;
                            rsv.SongLength = sngVal.SongLength;
                            rsv.SongName = sngVal.SongName;
                            rsv.SongNameSort = sngVal.SongNameSort;
                            rsv.SongYear = sngVal.SongYear;
                            rsv.Tuning = sngVal.Tuning;

                            var currSng = new Dictionary<string, dynamic>();
                            currSng.Add("Attributes", rsv);
                            tempDicArray[i].Add(song, currSng);
                        }
                        else
                        {
                            var currSng = new Dictionary<string, dynamic>();
                            currSng.Add("Attributes", songAttributes.Value);
                            tempDicArray[i].Add(song, currSng);
                        }
                    }
                }
            }

            using (StreamWriter fs = new StreamWriter(destHsanPath))
            {
                // save to standard DisabledEntries format
                dynamic songJson = new { Entries = tempFullSongCollection, DisabledEntries = tempFullDisabledSongCollection, InsertRoot = "Static.Songs.Headers" };
                JToken serializedJson = JsonConvert.SerializeObject(songJson, Formatting.Indented, new JsonSerializerSettings { });
                fs.Write(serializedJson.ToString());
            }
        }

        private bool SmartSongLoader()
        {
            Globals.Log("Smart Song Pack Loader Working ... ");
            ClearCollections();
            // SAFETY FIRST - make sure a backup of the original cache.psarc exists
            if (!File.Exists(Path.Combine(AppSettings.Instance.RSInstalledDir, Path.ChangeExtension(Path.GetFileName(Constants.CachePsarcPath), ".org.psarc"))) || !File.Exists(Constants.ExtractedSongsHsanPath))
            {
                if (!UnpackPsarcFiles())
                    return false;
            }

            return true;
        }

        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvSongPacks.DataBindingComplete -= dgvSongPacks_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvSongPacks.DataBindingComplete += dgvSongPacks_DataBindingComplete;
            }
        }

        private string TuningJsonToStrings(Dictionary<string, int> tuningJson)
        {
            string tuning = String.Empty;
            foreach (KeyValuePair<string, int> stringTuning in tuningJson)
            {
                tuning += stringTuning.Value;
            }
            return tuning;
        }

        private bool UnpackPsarcFiles()
        {
            Globals.TsProgressBar_Main.Value = 0;

            try
            {
                // if cache.psarc not found then exit (avoids RocksmithToolkitLib exception)
                // avoid checking condition in ValidateRsDir() to give greater app flexibility
                if (!File.Exists(Constants.CachePsarcPath))
                {
                    this.Enabled = false; // any further user entry could hang app
                    MessageBox.Show("Could not find required file: cache.psarc" + Environment.NewLine + "inside the Rocksmith Installation Directory.  ", "Required files missing!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                this.Enabled = false;
                Globals.TsProgressBar_Main.Value = 50;
                Packer.Unpack(Constants.CachePsarcPath, Constants.SongPacksFolder);
                Globals.TsProgressBar_Main.Value = 75;
                ExtractSongsHsan();
                ConditionalBackup(Constants.CachePsarcPath, Path.Combine(Constants.Rs2OriginalsFolder, Path.ChangeExtension(Path.GetFileName(Constants.CachePsarcPath), ".org.psarc")));
                ConditionalBackup(Constants.ExtractedSongsHsanPath, Path.Combine(Constants.SongPacksFolder, Path.ChangeExtension(Path.GetFileName(Constants.ExtractedSongsHsanPath), ".org.hsan")));

                if (File.Exists(Constants.Rs1DiscPsarcPath))
                {
                    Globals.TsProgressBar_Main.Value = 50;
                    PsarcExtensions.ExtractArchiveFile(Constants.Rs1DiscPsarcPath, Constants.SongsRs1DiscInternalPath, Constants.SongPacksFolder);
                    Globals.TsProgressBar_Main.Value = 75;
                    ConditionalBackup(Constants.Rs1DiscPsarcPath, Path.Combine(Constants.Rs2OriginalsFolder, Path.ChangeExtension(Path.GetFileName(Constants.Rs1DiscPsarcPath), ".org.psarc")));
                    ConditionalBackup(Constants.ExtractedRs1DiscHsanPath, Path.Combine(Constants.SongPacksFolder, Path.ChangeExtension(Path.GetFileName(Constants.ExtractedRs1DiscHsanPath), ".org.hsan")));
                }

                if (File.Exists(Constants.Rs1DlcPsarcPath))
                {
                    Globals.TsProgressBar_Main.Value = 50;
                    PsarcExtensions.ExtractArchiveFile(Constants.Rs1DlcPsarcPath, Constants.SongsRs1DlcInternalPath, Constants.SongPacksFolder);
                    Globals.TsProgressBar_Main.Value = 75;
                    ConditionalBackup(Constants.Rs1DlcPsarcPath, Path.Combine(AppSettings.Instance.RSInstalledDir, Path.ChangeExtension(Path.GetFileName(Constants.Rs1DlcPsarcPath), ".org.psarc")));
                    ConditionalBackup(Constants.ExtractedRs1DlcHsanPath, Path.Combine(Constants.SongPacksFolder, Path.ChangeExtension(Path.GetFileName(Constants.ExtractedRs1DlcHsanPath), ".org.hsan")));
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("One or more required files is missing, please install the application!", "Required files missing!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnDisableSongs_Click(object sender, EventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvSongPacks.Rows)
                    {
                        if ((bool)row.Cells["colSelect"].Value)
                        {
                            string rowSongKey = row.Cells["colSongKey"].Value.ToString();

                            switch (cmbSongPacks.SelectedIndex)
                            {
                                case 0: // cache.psarc
                                    DisableSong(CacheSongCollection, CacheEntireCollection, CacheDisabledSongCollection, CacheDisabledEntireCollection, rowSongKey);
                                    break;
                                case 1: // rs1compatibilitydisc_p.psarc
                                    DisableSong(Rs1DiscSongCollection, Rs1DiscEntireCollection, Rs1DiscDisabledSongCollection, Rs1DiscDisabledEntireCollection, rowSongKey);
                                    break;
                                case 2: // rs1compatibilitydlc_p.psarc
                                    DisableSong(Rs1DlcSongCollection, Rs1DlcEntireCollection, Rs1DlcDisabledSongCollection, Rs1DlcDisabledEntireCollection, rowSongKey);
                                    break;
                                case 3: // custom song pack
                                    DisableSong(CustomSongCollection, CustomEntireCollection, CustomDisabledSongCollection, CustomDisabledEntireCollection, rowSongKey);
                                    break;
                                default:
                                    throw new Exception("Song Packs Combobox Failure");
                            }

                            if (row.Index != -1)
                            {
                                row.Cells["colEnabled"].Value = "No";
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                                row.Cells["colSelect"].Value = false;
                                row.Selected = false;
                            }
                        }
                    }

                    dgvSongPacks.EndEdit();
                });
        }

        private void btnEnableSongs_Click(object sender, EventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvSongPacks.Rows)
                    {
                        if ((bool)row.Cells["colSelect"].Value)
                        {
                            string rowSongKey = row.Cells["colSongKey"].Value.ToString();

                            switch (cmbSongPacks.SelectedIndex)
                            {
                                case 0: // cache.psarc
                                    EnableSong(CacheSongCollection, CacheEntireCollection, CacheDisabledSongCollection, CacheDisabledEntireCollection, rowSongKey);
                                    break;
                                case 1: // rs1compatibilitydisc_p.psarc
                                    EnableSong(Rs1DiscSongCollection, Rs1DiscEntireCollection, Rs1DiscDisabledSongCollection, Rs1DiscDisabledEntireCollection, rowSongKey);
                                    break;
                                case 2: // rs1compatibilitydlc_p.psarc
                                    EnableSong(Rs1DlcSongCollection, Rs1DlcEntireCollection, Rs1DlcDisabledSongCollection, Rs1DlcDisabledEntireCollection, rowSongKey);
                                    break;
                                case 3: // custom song pack
                                    EnableSong(CustomSongCollection, CustomEntireCollection, CustomDisabledSongCollection, CustomDisabledEntireCollection, rowSongKey);
                                    break;
                                default:
                                    throw new Exception("Song Packs Combobox Failure");
                            }

                            if (row.Index != -1)
                            {
                                row.Cells["colEnabled"].Value = "Yes";
                                row.DefaultCellStyle.BackColor = Color.Empty; // Color.White;
                                row.Cells["colSelect"].Value = false;
                                row.Selected = false;
                            }
                        }
                    }

                    dgvSongPacks.EndEdit();
                });
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            RestoreOriginalSongBackups();
            PopulateSongPacks();
            this.Enabled = true;
        }

        private void btnSaveSongs_Click(object sender, EventArgs e)
        {
            Globals.Log("Saving " + cmbSongPacks.Text + " ...");
            Globals.TsProgressBar_Main.Value = 0;
            Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            switch (cmbSongPacks.SelectedIndex)
            {
                case 0: // cache.psarc
                    if (File.Exists(Constants.ExtractedSongsHsanPath))
                        File.Delete(Constants.ExtractedSongsHsanPath);

                    Globals.TsProgressBar_Main.Value = 50;
                    SerializeSongFile(Constants.ExtractedSongsHsanPath, CacheEntireCollection, CacheDisabledEntireCollection, typeof(RS2VocalsData));
                    Globals.TsProgressBar_Main.Value = 75;
                    RepackCachePsarc();
                    break;
                case 1: // rs1compatibilitydisc_p.psarc
                    if (File.Exists(Constants.Rs1DiscPsarcPath))
                    {
                        if (File.Exists(Constants.ExtractedRs1DiscHsanPath))
                            File.Delete(Constants.ExtractedRs1DiscHsanPath);

                        Globals.TsProgressBar_Main.Value = 50;
                        SerializeSongFile(Constants.ExtractedRs1DiscHsanPath, Rs1DiscEntireCollection, Rs1DiscDisabledEntireCollection, typeof(RS1DiscVocalsData));
                        Globals.TsProgressBar_Main.Value = 75;
                        PsarcExtensions.InjectArchiveEntry(Constants.Rs1DiscPsarcPath, Constants.SongsRs1DiscInternalPath, Constants.ExtractedRs1DiscHsanPath);
                    }
                    break;
                case 2: // rs1compatibilitydlc_p.psarc
                    if (File.Exists(Constants.Rs1DlcPsarcPath))
                    {
                        if (File.Exists(Constants.ExtractedRs1DlcHsanPath))
                            File.Delete(Constants.ExtractedRs1DlcHsanPath);

                        Globals.TsProgressBar_Main.Value = 50;
                        SerializeSongFile(Constants.ExtractedRs1DlcHsanPath, Rs1DlcEntireCollection, Rs1DlcDisabledEntireCollection, typeof(RS1DLCVocalsData));
                        Globals.TsProgressBar_Main.Value = 75;
                        PsarcExtensions.InjectArchiveEntry(Constants.Rs1DlcPsarcPath, Constants.SongsRs1DlcInternalPath, Constants.ExtractedRs1DlcHsanPath);
                    }
                    break;
                case 3: // custom song pack
                    if (File.Exists(customPackPsarcPath))
                    {
                        if (File.Exists(extractedCustomHsanPath))
                            File.Delete(extractedCustomHsanPath);

                        Globals.TsProgressBar_Main.Value = 50;
                        SerializeSongFile(extractedCustomHsanPath, CustomEntireCollection, CustomDisabledEntireCollection, typeof(RS1DiscVocalsData));
                        Globals.TsProgressBar_Main.Value = 75;
                        PsarcExtensions.InjectArchiveEntry(customPackPsarcPath, customInternalHsanPath, extractedCustomHsanPath);
                    }
                    break;
                default:
                    throw new Exception("Song Packs Combobox Failure");
            }

            Globals.TsProgressBar_Main.Value = 100;
            Globals.Log("SUCCESSFUL ...");
            this.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void btnSelectAllNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongPacks.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colSelect"];
                chk.Value = !allSelected;
            }
            allSelected = !allSelected;
        }

        private void cmbSongPacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSongPacks.SelectedIndex == 3)
                LoadCustomSongPack();
            else
                RefreshDgvSongs();
        }

        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            allSelected = false;

            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
            {
                switch (cmbSongPacks.SelectedIndex)
                {
                    case 0: // cache.psarc
                        LoadSongPackList(CacheSongCollection, CacheDisabledSongCollection);
                        break;
                    case 1: // rs1compatibilitydisc_p.psarc
                        LoadSongPackList(Rs1DiscSongCollection, Rs1DiscDisabledSongCollection);
                        break;
                    case 2: // rs1compatibilitydlc_p.psarc
                        LoadSongPackList(Rs1DlcSongCollection, Rs1DlcDisabledSongCollection);
                        break;
                    case 3: // custom song pack
                        LoadSongPackList(CustomSongCollection, CustomDisabledSongCollection);
                        break;
                    default:
                        throw new Exception("Song Packs Combobox Failure");
                }
            }
            else
                RefreshDgvSongs();
        }

        private void dgvSongPacks_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var s = DgvExtensions.GetObjectFromRow<SongPackData>(dgvSongPacks, e.RowIndex);
                    s.Selected = !s.Selected;
                }
                // select multiple rows by Shift-Click on two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvSongPacks.SelectedRows.Count > 0)
                    {
                        var first = dgvSongPacks.SelectedRows[0];
                        var start = first.Index;
                        var end = e.RowIndex + 1;

                        if (start > end)
                        {
                            var tmp = start;
                            start = end;
                            end = tmp;
                        }
                        TemporaryDisableDatabindEvent(() =>
                            {
                                for (int i = start; i < end; i++)
                                {
                                    var s = DgvExtensions.GetObjectFromRow<SongPackData>(dgvSongPacks, i);
                                    s.Selected = !s.Selected;
                                }
                            });
                    }
                }
            }
        }

        private void dgvSongPacks_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSongPacks")
                return;

            if (!bindingCompleted)
            {
                Debug.WriteLine("DataBinding Complete ... ");
                bindingCompleted = true;
            }

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongPacks);
            // filter applied
            if (!String.IsNullOrEmpty(filterStatus) && dgvPainted)
            {
                Globals.TsLabel_StatusMsg.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_StatusMsg.Text = "Show &All";
                Globals.TsLabel_StatusMsg.IsLink = true;
                Globals.TsLabel_StatusMsg.LinkBehavior = LinkBehavior.HoverUnderline;
                Globals.TsLabel_StatusMsg.Visible = true;
                Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_DisabledCounter.Text = filterStatus;
                Globals.TsLabel_DisabledCounter.Visible = true;
            }

            // filter removed
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvSongPacks.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvSongPacks_Paint(object sender, PaintEventArgs e)
        {
            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.DebugLog("dgvSongPacks Painted ... ");
                // it is now safe to do cell formatting (coloring)
            }
        }

        private void dgvSongs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown
            var grid = (DataGridView)sender;
            var rowIndex = e.RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                if (rowIndex != -1)
                {
                    grid.Rows[e.RowIndex].Selected = true;
                    // TODO: impliment cmsDuplicates action menu consistent with other grids
                    //  cmsDuplicates.Show(Cursor.Position);
                }
                else
                {
                    // TODO: impliment cms column header menu consistent with other grids
                    //PopulateMenuWithColumnHeaders(cmsDuplicateColumns);
                    //cmsDuplicateColumns.Show(Cursor.Position);
                }
            }

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                TemporaryDisableDatabindEvent(() => { dgvSongPacks.EndEdit(); });
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvSongPacks.Refresh();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            RefreshDgvSongs();
        }

        private void lnkRefreshAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UnpackPsarcFiles();
            PopulateSongLists();
            dgvSongPacks.Refresh();
            Globals.Log("GUI Refreshed ...");
        }

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongPacks.Rows)
                row.Cells["colSelect"].Value = !allSelected;

            allSelected = !allSelected;
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void lnkToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongPacks.Rows)
                row.Cells["colSelect"].Value = !Convert.ToBoolean(row.Cells["colSelect"].Value);
        }

        public DataGridView GetGrid()
        {
            return dgvSongPacks;
        }

        public void TabEnter()
        {
            Globals.DgvCurrent = dgvSongPacks;
            Globals.Log("SongPacks GUI Activated ...");
        }

        public void TabLeave()
        {
            if (songPackList.Any())
                Globals.Settings.SaveSettingsToFile(dgvSongPacks);

            Globals.Log("SongPacks GUI Deactivated ...");
        }
    }
}