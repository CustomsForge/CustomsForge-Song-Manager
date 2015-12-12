using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using RocksmithToolkitLib.DLCPackage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using RocksmithToolkitLib;
using CFSM.Utils.PSARC;
using SevenZip;
using Extensions = CustomsForgeManager.CustomsForgeManagerLib.Extensions;

namespace CustomsForgeManager.UControls
{
    public partial class CachePsarcEditor : UserControl
    {
        private RSDataJsonDictionary<RS2SongsData> CacheDisabledEntireCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CacheDisabledSongCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CacheEntireCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CacheSongCollection = new RSDataJsonDictionary<RS2SongsData>();

        private RSDataJsonDictionary<RS2SongsData> CombinedDisabledEntireCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CombinedDisabledSongCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CombinedEntireCollection = new RSDataJsonDictionary<RS2SongsData>();
        private RSDataJsonDictionary<RS2SongsData> CombinedSongCollection = new RSDataJsonDictionary<RS2SongsData>();

        private RSDataJsonDictionary<RS1DiscData> Rs1DiscDisabledEntireCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> Rs1DiscDisabledSongCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> Rs1DiscEntireCollection = new RSDataJsonDictionary<RS1DiscData>();
        private RSDataJsonDictionary<RS1DiscData> Rs1DiscSongCollection = new RSDataJsonDictionary<RS1DiscData>();

        private RSDataJsonDictionary<RS1DlcData> Rs1DlcDisabledEntireCollection = new RSDataJsonDictionary<RS1DlcData>();
        private RSDataJsonDictionary<RS1DlcData> Rs1DlcDisabledSongCollection = new RSDataJsonDictionary<RS1DlcData>();
        private RSDataJsonDictionary<RS1DlcData> Rs1DlcEntireCollection = new RSDataJsonDictionary<RS1DlcData>();
        private RSDataJsonDictionary<RS1DlcData> Rs1DlcSongCollection = new RSDataJsonDictionary<RS1DlcData>();
        private bool allSelected = false;

        public CachePsarcEditor()
        {
            InitializeCachePsarcEditor();
        }

        public void UpdateToolStrip()
        {
            if (Globals.ReloadCachePsarcEditor)
                InitializeCachePsarcEditor();
            else
            {
                Globals.TsLabel_MainMsg.Text = string.Format("Song Count: {0}", dgvSongs.Rows.Count);
                Globals.TsLabel_MainMsg.Visible = true;
                var tsldcMsg = String.Format("Disabled DLC: {0}", DisabledSongColorizerCounter());
                Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
                Globals.TsLabel_DisabledCounter.Visible = true;
            }
        }

        private void AddSongsToNestedDictionary(dynamic songsToBeAddedCollection, dynamic songCollection, dynamic fullSongCollection, bool enabled, bool RS2014 = false)
        {
            foreach (dynamic song in songsToBeAddedCollection)
            {
                foreach (dynamic songAttributes in song.Value)
                {
                    dynamic songData = songAttributes.Value;
                    if (songData.SongName != null)
                    {
                        CacheSongData sngData = AttributesToSongData(songData, false);

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

        private void AddToDGV(dynamic songCollection, bool enabled = true)
        {
            foreach (dynamic song in songCollection)
            {
                foreach (dynamic songAttributes in song.Value)
                {
                    dynamic songData = songAttributes.Value;
                    CacheSongData sngData = AttributesToSongData(songData, enabled);

                    dgvSongs.Rows.Add(false, sngData.Enabled, sngData.Title, sngData.Artist, sngData.Album, sngData.Tuning, sngData.SongKey);
                }
            }
        }

        private CacheSongData AttributesToSongData(dynamic songAttributes, bool enabled = true)
        {
            CacheSongData song = new CacheSongData();
            string tuning = "";

            song.Title = songAttributes.SongName;
            song.Artist = songAttributes.ArtistName;
            song.Album = songAttributes.AlbumName;
            song.SongKey = songAttributes.SongKey;
            song.Enabled = enabled ? "Yes" : "No";
            if (songAttributes.ArrangementName != "Vocals")
            {
                foreach (KeyValuePair<string, int> stringTuning in songAttributes.Tuning)
                {
                    tuning += stringTuning.Value;
                }
                song.Tuning = CustomsForgeManagerLib.Extensions.TuningStringToName(tuning);
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

            CombinedDisabledEntireCollection.Clear();
            CombinedDisabledSongCollection.Clear();
            CombinedEntireCollection.Clear();
            CombinedSongCollection.Clear();

            Rs1DiscDisabledEntireCollection.Clear();
            Rs1DiscDisabledSongCollection.Clear();
            Rs1DiscEntireCollection.Clear();
            Rs1DiscSongCollection.Clear();

            Rs1DlcDisabledEntireCollection.Clear();
            Rs1DlcDisabledSongCollection.Clear();
            Rs1DlcEntireCollection.Clear();
            Rs1DlcSongCollection.Clear();      
        }

        private bool ConditionalBackup(string sourcePath, string backupPath, bool forceBackup = false, bool writeProtect = true)
        {
            if (!File.Exists(sourcePath)) return false;

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
                // it is critical that backup of originals was sucessful before proceeding
                throw new Exception("Backup of: " + sourcePath + " ... FAILED" + Environment.NewLine + ex.Message + Environment.NewLine + "Please correct issue and make sure you have" + Environment.NewLine + "backup copies of your original song files.");
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

            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                if (row.Cells["colEnabled"].Value.ToString() == "No")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    disabledCount++;
                }
                else
                    row.DefaultCellStyle.BackColor = Color.White;
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


        // sheer evil genuis code :) ... very nice
        // TODO: evil genious .. plz help code InjectArchiveFiles method
        private void ExtractArchiveFiles(string path, string name, string outPath)
        {
            using (PSARC p = new PSARC(true))
            using (var FS = File.OpenRead(path))
            {
                p.Read(FS, true);
                var e = p.TOC.Where(entry => entry.Name.Contains(name)).FirstOrDefault();
                if (e != null)
                {
                    if (!Directory.Exists(outPath))
                        Directory.CreateDirectory(outPath);
                    p.InflateEntry(e, Path.Combine(outPath, name));
                }
                else
                    Globals.Log("Error: psarc entry not found.");
            }
        }

        // evil genuis copy cat code :) ...

        private bool ExtractSongsHsan()
        {
            if (File.Exists(Constants.ExtractedSongsHsanPath))
                File.Delete(Constants.ExtractedSongsHsanPath);

            if (ZipUtilities.ExtractSingleFile(Constants.CpeWorkDirectory, Constants.Cache7zPath, Constants.SongsHsanInternalPath))
            {
                FileInfo f = new FileInfo(Constants.ExtractedSongsHsanPath);
                if (f.Length > 0)
                    return true;
            }

            MessageBox.Show("Extracting song cache failed!", "Extracting failed!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                        if (songAttributes.Value.SongName.ToLower().Contains(tbSearch.Text.ToLower()) || songAttributes.Value.AlbumName.ToLower().Contains(tbSearch.Text.ToLower()) || songAttributes.Value.ArtistName.ToLower().Contains(tbSearch.Text.ToLower()) || songAttributes.Value.Tuning.ToString().TuningToName().ToLower().Contains(tbSearch.Text.ToLower()) || songAttributes.Value.SongKey.ToLower().Contains(tbSearch.Text.ToLower()))
                            matchingSongs.Add(songAttributes.Value);
                    }
                }
            }

            return matchingSongs;
        }

        private void InitializeCachePsarcEditor()
        {
            // provides for complete fresh reload of tabpage
            InitializeComponent();
            Leave += CachePsarcEditor_Leave;
            this.Enabled = true;
            Globals.ReloadCachePsarcEditor = false;
            PopulateCachePsarcEditor();
        }

        private void CachePsarcEditor_Leave(object sender, EventArgs e)
        {
            if (chkDeleteWorkDir.Checked)
                if(Directory .Exists(Constants.CpeWorkDirectory))
                    ZipUtilities.DeleteDirectory(Constants.CpeWorkDirectory);
        }

        // TODO: evil genious help ... with InjectArchiveFiles method
        private void InjectArchiveFiles(string path, string name, string outPath)
        {
            using (PSARC p = new PSARC(true))
            using (var FS = File.OpenRead(path))
            {
                p.Read(FS, true);
                var e = p.TOC.Where(entry => entry.Name.Contains(name)).FirstOrDefault();
                if (e != null)
                {
                    // here's where we need to inject our modded file
                    //  p.InflateEntry(e, Path.Combine(outPath, name));
                    //  psarc.Write(psarcStream, false);
                }
                else
                    Globals.Log("Error: psarc entry not found.");
            }
        }

        private void PopulateCachePsarcEditor()
        {
            Globals.Log("Populating Cache.psarc Editor GUI ...");

            Type dgvType = dgvSongs.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgvSongs, true, null);

            cmbGameChoice.SelectedIndex = 1;
            Globals.TuningXml = TuningDefinitionRepository.LoadTuningDefinitions(GameVersion.RS2014);

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
                        Globals.Log("PopulateSongLists ... FAILED");
                else
                    Globals.Log("SmartSongLoader ... FAILED");

            }
            else
            {
                Globals.ReloadCachePsarcEditor = true;
                Globals.Log("Method PopulateCachePsarcEditor ... FAILED");
                Globals.Log("Could not find Rocksmith Installation Directory");
            }
        }

        private void PopulateDataGridView()
        {
            dgvSongs.Rows.Clear();

            switch (cmbGameChoice.SelectedIndex)
            {
                case 0: // All
                    // AddToDGV(CombinedDisabledSongCollection, false);
                    // AddToDGV(CombinedSongCollection, true);
                    break;
                case 1: // cache.psarc
                    AddToDGV(CacheDisabledSongCollection, false);
                    AddToDGV(CacheSongCollection, true);
                    break;
                case 2: // rs1compatibilitydisc_p.psarc
                    AddToDGV(Rs1DiscDisabledSongCollection, false);
                    AddToDGV(Rs1DiscSongCollection, true);
                    break;
                case 3: // rs1compatibilitydlc_p.psarc
                    AddToDGV(Rs1DlcDisabledSongCollection, false);
                    AddToDGV(Rs1DlcSongCollection, true);
                    break;
                default:
                    throw new Exception("ComboGameChoice Failure");
            }

            UpdateToolStrip();
        }

        private void PopulateSongList<T>(string songFilePath, RSDataJsonDictionary<T> songCollection, ref RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection, ref RSDataJsonDictionary<T> fullDisabledSongCollection) where T : RSDataAbstractBase
        {
            try
            {
                string songFileContent = File.ReadAllText(songFilePath);
                var songsJson = JObject.Parse(songFileContent);

                var songsList = songsJson["Entries"];

                fullSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<T>>(songsList.ToString());

                if (songsJson.ToString().Contains("DisabledSongs"))
                {
                    var disabledSongsList = songsJson["DisabledSongs"];
                    fullDisabledSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<T>>(disabledSongsList.ToString());

                    AddSongsToNestedDictionary(fullDisabledSongCollection, disabledSongCollection, fullSongCollection, false);
                }

                AddSongsToNestedDictionary(fullSongCollection, songCollection, fullSongCollection, true);
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

            // TODO: combine all song lists
            // CombinedSongCollection = CacheSongCollection.Union(Rs1DiscSongCollection).Union(Rs1DlcSongCollection).ToDictionary();

            var songCount = CacheSongCollection.Count + Rs1DiscSongCollection.Count + Rs1DlcSongCollection.Count;

            if (songCount == 0)
                return false;

            return true;
        }

        private void RefreshDGVAfterSearching(dynamic songCollection, dynamic disabledSongCollection)
        {
            var filteredSongCollection = GetMatchingSongs(disabledSongCollection, tbSearch.Text);
            dgvSongs.Rows.Clear();

            foreach (var song in filteredSongCollection)
                dgvSongs.Rows.Add(false, "No", song.SongName, song.ArtistName, song.AlbumName, CustomsForgeManagerLib.Extensions.TuningStringToName(TuningJsonToStrings(song.Tuning)), song.SongKey);

            filteredSongCollection = GetMatchingSongs(songCollection, tbSearch.Text);

            foreach (var song in filteredSongCollection)
                dgvSongs.Rows.Add(false, "Yes", song.SongName, song.ArtistName, song.AlbumName, CustomsForgeManagerLib.Extensions.TuningStringToName(TuningJsonToStrings(song.Tuning)), song.SongKey);
        }

        private bool RepackCachePsarc()
        {
            try
            {
                if (!Directory.Exists(Constants.CachePcPath))
                    throw new IOException();

                if (!File.Exists(Path.Combine(Constants.CachePcPath, "sltsv1_aggregategraph.nt")))
                    Extensions.ExtractEmbeddedResource(Constants.CachePcPath, "CustomsForgeManager.Resources", new string[] { "sltsv1_aggregategraph.nt" });

                ZipUtilities.InjectFile(
                    Constants.ExtractedSongsHsanPath, 
                    Constants.Cache7zPath,
                    Constants.SongsHsanInternalPath, 
                    OutArchiveFormat.SevenZip, 
                    CompressionMode.Append);
              
                Packer.Pack(Constants.CachePcPath, Constants.CachePsarcPath);

                Globals.Log("cache.psarc repackaged with your song selections ... SUCESSFUL");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to repack cache.psarc" + Environment.NewLine + "Error: " + ex.Message.ToString());
            }

            return true;
        }

        private bool RepackRs1DiscPsarc()
        {
            //InjectArchiveFile();
            //Packer.Pack(Constants.Cache7zPath, Constants.Rs1DiscPsarcPath);
            return true;
        }

        private bool RepackRs1DlcPsarc()
        {
            //InjectArchiveFile();
            //Packer.Pack(Constants.Cache7zPath, Constants.Rs1DlcPsarcPath);
            return true;
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

                    if (File.Exists(Constants.Rs1DiscPsarcPath.Replace("_p.", "_p.disabled.")))
                        File.Delete(Constants.Rs1DiscPsarcPath.Replace("_p.", "_p.disabled."));

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

                if (Directory.Exists(Constants.CpeWorkDirectory))
                {
                    Globals.TsProgressBar_Main.Value = 80;
                    ZipUtilities.DeleteDirectory(Constants.CpeWorkDirectory);
                    Globals.Log("Removed: " + Constants.CpeWorkDirectory + " ...");
                }

                Globals.TsProgressBar_Main.Value = 100;
                Globals.Log("Restored to original condition ... SUCESSFUL");
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

        private void SerializeSongFile<T>(string songHsanPath, RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> fullDisabledSongCollection, Type vdType) where T : RSDataAbstractBase
        {
            // TODO: LOVRO ... this is not producing hasn files that are equivalent to the original, much smaller in size
            // problem may be issue with class object conversions (no album artwork in game)
            // help plz
            //
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

            using (StreamWriter file = new StreamWriter(songHsanPath))
            {
                dynamic songJson = new
                    {
                        Entries = tempFullSongCollection,
                        DisabledSongs = tempFullDisabledSongCollection,
                        InsertRoot = "Static.Songs.Headers"
                    };

                JToken serializedJson = JsonConvert.SerializeObject(songJson, Formatting.Indented, new JsonSerializerSettings { });
                file.Write(serializedJson.ToString());
            }
        }

        private bool SmartSongLoader()
        {
            Globals.Log("Loading songs ... ");
            ClearCollections();
            // SAFETY FIRST - make sure a backup of the original cache.psarc exists
            if (!File.Exists(Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.CachePsarcPath), ".psarc.org"))) ||
                !File.Exists(Constants.ExtractedSongsHsanPath))
            {
                if (!UnpackPsarcFiles())
                    return false;
            }

            return true;
        }

        private string TuningJsonToStrings(Dictionary<string, int> tuningJson)
        {
            string tuning = "";
            foreach (KeyValuePair<string, int> stringTuning in tuningJson)
            {
                tuning += stringTuning.Value;
            }
            return tuning;
        }

        private bool UnpackPsarcFiles()
        {
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

                Packer.Unpack(Constants.CachePsarcPath, Constants.CpeWorkDirectory);
                ExtractSongsHsan();
                ConditionalBackup(Constants.CachePsarcPath, Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.CachePsarcPath), ".psarc.org")));
                ConditionalBackup(Constants.ExtractedSongsHsanPath, Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.ExtractedSongsHsanPath), ".hsan.org")));

                if (File.Exists(Constants.Rs1DiscPsarcPath))
                {
                    ExtractArchiveFiles(Constants.Rs1DiscPsarcPath, "songs_rs1disc.hsan", Constants.CpeWorkDirectory);
                    ConditionalBackup(Constants.Rs1DiscPsarcPath, Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.Rs1DiscPsarcPath), ".psarc.org")));
                    ConditionalBackup(Constants.ExtractedRs1DiscHsanPath, Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.ExtractedRs1DiscHsanPath), ".hsan.org")));
                }

                if (File.Exists(Constants.Rs1DlcPsarcPath))
                {
                    ExtractArchiveFiles(Constants.Rs1DlcPsarcPath, "songs_rs1dlc.hsan", Constants.CpeWorkDirectory);
                    ConditionalBackup(Constants.Rs1DlcPsarcPath, Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.Rs1DlcPsarcPath), ".psarc.org")));
                    ConditionalBackup(Constants.ExtractedRs1DlcHsanPath, Path.Combine(Constants.Rs2BackupDirectory, Path.ChangeExtension(Path.GetFileName(Constants.ExtractedRs1DlcHsanPath), ".hsan.org")));
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("One or more required files is missing, please install the application!", "Required files missing!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        private void btnDisableSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colSelect"];
                if (chk.Value.ToString().ToLower() == "true" || row.Selected)
                {
                    string rowSongKey = row.Cells["colSongKey"].Value.ToString();

                    switch (cmbGameChoice.SelectedIndex)
                    {
                        case 0: // All
                            //DisableSong(CombinedSongCollection, CombinedEntireCollection, CombinedDisabledSongCollection, CombinedDisabledEntireCollection, rowSongKey);
                            break;
                        case 1: // cache.psarc
                            DisableSong(CacheSongCollection, CacheEntireCollection, CacheDisabledSongCollection, CacheDisabledEntireCollection, rowSongKey);
                            break;
                        case 2: // rs1compatibilitydisc_p.psarc
                            DisableSong(Rs1DiscSongCollection, Rs1DiscEntireCollection, Rs1DiscDisabledSongCollection, Rs1DiscDisabledEntireCollection, rowSongKey);
                            break;
                        case 3: // rs1compatibilitydlc_p.psarc
                            DisableSong(Rs1DlcSongCollection, Rs1DlcEntireCollection, Rs1DlcDisabledSongCollection, Rs1DlcDisabledEntireCollection, rowSongKey);
                            break;
                        default:
                            throw new Exception("ComboGameChoice Failure");
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
        }

        private void btnEnableSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colSelect"];
                if (chk.Value.ToString().ToLower() == "true" || row.Selected)
                {
                    string rowSongKey = row.Cells["colSongKey"].Value.ToString();

                    switch (cmbGameChoice.SelectedIndex)
                    {
                        case 0: // All
                            //EnableSong(CombinedSongCollection, CacheEntireCollection, CombinedDisabledSongCollection, CacheDisabledEntireCollection, rowSongKey);
                            break;
                        case 1: // cache.psarc
                            EnableSong(CacheSongCollection, CacheEntireCollection, CacheDisabledSongCollection, CacheDisabledEntireCollection, rowSongKey);
                            break;
                        case 2: // rs1compatibilitydisc_p.psarc
                            EnableSong(Rs1DiscSongCollection, Rs1DiscEntireCollection, Rs1DiscDisabledSongCollection, Rs1DiscDisabledEntireCollection, rowSongKey);
                            break;
                        case 3: // rs1compatibilitydlc_p.psarc
                            EnableSong(Rs1DlcSongCollection, Rs1DlcEntireCollection, Rs1DlcDisabledSongCollection, Rs1DlcDisabledEntireCollection, rowSongKey);
                            break;
                        default:
                            throw new Exception("ComboGameChoice Failure");
                    }

                    if (row.Index != -1)
                    {
                        row.Cells["colEnabled"].Value = "Yes";
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.Cells["colSelect"].Value = false;
                        row.Selected = false;
                    }
                }
            }
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            RestoreOriginalSongBackups();
            PopulateCachePsarcEditor();
        }

        private void btnSaveSongs_Click(object sender, EventArgs e)
        {
            Globals.Log("Saving " + cmbGameChoice.Text + " songs ...");
            Cursor = Cursors.WaitCursor;
            switch (cmbGameChoice.SelectedIndex)
            {
                case 0: // All
                    // TODO: impliment foreach
                    break;
                case 1: // cache.psarc
                    if (File.Exists(Constants.ExtractedSongsHsanPath))
                        File.Delete(Constants.ExtractedSongsHsanPath);
                    // for debugging
                    var stop1 = CacheSongCollection;
                    var stop2 = CacheEntireCollection;
                    var stop3 = CacheDisabledSongCollection;
                    var stop4 = CacheDisabledEntireCollection;
                    var stop5 = "";

                    SerializeSongFile(Constants.ExtractedSongsHsanPath, CacheEntireCollection, CacheDisabledEntireCollection, typeof(RS2VocalsData));
                    RepackCachePsarc();
                    break;
                case 2: // rs1compatibilitydisc_p.psarc
                    if (File.Exists(Constants.Rs1DiscPsarcPath))
                    {
                        if (File.Exists(Constants.ExtractedRs1DiscHsanPath))
                            File.Delete(Constants.ExtractedRs1DiscHsanPath);

                        SerializeSongFile(Constants.ExtractedRs1DiscHsanPath, Rs1DiscEntireCollection, Rs1DiscDisabledEntireCollection, typeof(RS1DiscVocalsData));
                        RepackRs1DiscPsarc();
                    }
                    break;
                case 3: // rs1compatibilitydlc_p.psarc
                    if (File.Exists(Constants.Rs1DlcPsarcPath))
                    {
                        if (File.Exists(Constants.ExtractedRs1DlcHsanPath))
                            File.Delete(Constants.ExtractedRs1DlcHsanPath);

                        SerializeSongFile(Constants.SongsRs1DlcInternalPath, Rs1DlcEntireCollection, Rs1DlcDisabledEntireCollection, typeof(RS1DLCVocalsData));
                        RepackRs1DlcPsarc();
                    }
                    break;
                default:
                    throw new Exception("ComboGameChoice Failure");
            }

            Cursor = Cursors.Default;
        }

        private void btnSelectAllNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colSelect"];
                chk.Value = !allSelected;
            }
            allSelected = !allSelected;
        }

        private void cmbGameChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            allSelected = false;
            tbSearch.Text = "";

            switch (cmbGameChoice.SelectedIndex)
            {
                case 0: // All
                    // TODO: Impliment
                    RefreshDGVAfterSearching(CombinedSongCollection, CombinedDisabledSongCollection);
                    break;
                case 1: // cache.psarc
                    RefreshDGVAfterSearching(CacheSongCollection, CacheDisabledSongCollection);
                    break;
                case 2: // rs1compatibilitydisc_p.psarc
                    RefreshDGVAfterSearching(Rs1DiscSongCollection, Rs1DiscDisabledSongCollection);
                    break;
                case 3: // rs1compatibilitydlc_p.psarc
                    RefreshDGVAfterSearching(Rs1DlcSongCollection, Rs1DlcDisabledSongCollection);
                    break;
                default:
                    throw new Exception("ComboGameChoice Failure");
            }

            UpdateToolStrip();
        }

        private void dgvSongs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == colSelect.Index)
            {
                dgvSongs.EndEdit();
            }
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbSearch.Text = "";
        }

        private void lnkRefreshAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateSongLists();
            Globals.Log("GUI Refreshed ...");
        }

   
    }
}