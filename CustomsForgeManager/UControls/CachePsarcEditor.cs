using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using RocksmithToolkitLib.DLCPackage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using RocksmithToolkitLib.Xml;
using RocksmithToolkitLib;
using RocksmithToolkitLib.PSARC;

namespace CustomsForgeManager.UControls
{
    public partial class CachePsarcEditor : UserControl
    {
        private bool allSelected, isModifiying = false;
        private int fullSongCount;
        private string pZipPath = "7za.exe";
        private ProcessStartInfo pZip = new ProcessStartInfo();
        private Process x;

        RSDataJsonDictionary<RSSongData> RS2014SongCollection = new RSDataJsonDictionary<RSSongData>();
        RSDataJsonDictionary<RS1DLCData> RS1DLCSongCollection = new RSDataJsonDictionary<RS1DLCData>();
        RSDataJsonDictionary<RS1SongData> RS1SongCollection = new RSDataJsonDictionary<RS1SongData>();

        RSDataJsonDictionary<RSSongData> RS2014FullSongCollection = new RSDataJsonDictionary<RSSongData>();
        RSDataJsonDictionary<RS1DLCData> RS1DLCFullSongCollection = new RSDataJsonDictionary<RS1DLCData>();
        RSDataJsonDictionary<RS1SongData> RS1FullSongCollection = new RSDataJsonDictionary<RS1SongData>();

        RSDataJsonDictionary<RSSongData> RS2014DisabledSongCollection = new RSDataJsonDictionary<RSSongData>();
        RSDataJsonDictionary<RS1DLCData> RS1DLCDisabledSongCollection = new RSDataJsonDictionary<RS1DLCData>();
        RSDataJsonDictionary<RS1SongData> RS1DisabledSongCollection = new RSDataJsonDictionary<RS1SongData>();

        RSDataJsonDictionary<RSSongData> RS2014FullDisabledSongCollection = new RSDataJsonDictionary<RSSongData>();
        RSDataJsonDictionary<RS1DLCData> RS1DLCFullDisabledSongCollection = new RSDataJsonDictionary<RS1DLCData>();
        RSDataJsonDictionary<RS1SongData> RS1FullDisabledSongCollection = new RSDataJsonDictionary<RS1SongData>();

        RSDataJsonDictionary<RSSongData> DisabledCacheSongCollection = new RSDataJsonDictionary<RSSongData>();
        RSDataJsonDictionary<RSSongData> CacheSongCollection = new RSDataJsonDictionary<RSSongData>();

        RSDataJsonDictionary<RSSongData> DisabledTempSongCollection = new RSDataJsonDictionary<RSSongData>();
        RSDataJsonDictionary<RSSongData> TempSongCollection = new RSDataJsonDictionary<RSSongData>();

        public CachePsarcEditor()
        {
            InitializeComponent();
            PopulateCachePsarcEditor();
        }
        #region Load Songs
        public void PopulateCachePsarcEditor()
        {
            Globals.Log("Populating Cache.psarc Editor GUI ...");

            Type dgvType = dgvSongs.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgvSongs, true, null);

            comboGameChoice.SelectedIndex = 2;
            Globals.TuningXml = TuningDefinitionRepository.LoadTuningDefinitions(GameVersion.RS2014);

            if (Directory.Exists(AppSettings.Instance.RSInstalledDir))
            {
                if (checkDeleteExtractedOnClose.Checked)
                    LoadSongsFromHsans();
                else
                    LoadSongsFromFolders();
            }
        }

        private void ExtractArchiveFiles(string path, string name, string outPath)
        {
            using (PSARC p = new PSARC())
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
                    Globals.Log("Error: song entry not found.");
            }
        }

        public bool UnpackSongCache()
        {
            try
            {
                if (Directory.Exists(AppSettings.Instance.RSInstalledDir))
                {
                    Packer.Unpack(Constants.CachePsarcPath, Constants.CFEWorkingFolder);
                    if (File.Exists(Constants.RS1PackPath))
                        ExtractArchiveFiles(Constants.RS1PackPath, "songs_rs1disc.hsan", Constants.RS1SongsFilePath);

                    if (File.Exists(Constants.RS1DLCPath))
                        ExtractArchiveFiles(Constants.RS1DLCPath, "songs_rs1dlc.hsan", Constants.RS1DLCSongsFilePath);

                    return true;
                }
                else
                {
                    MessageBox.Show("Please enter your Rocksmith path or copy the app into your Rocksmith folder!", "Rocksmith path missing!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("One or more required files is missing, please install the application!", "Required files missing!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        public void ExtractSongFile(bool extractAll = true)
        {
            if (File.Exists(Constants.RSSongsFilePath))
                File.Delete(Constants.RSSongsFilePath);

            if (Run7Zip("e", Constants.Cache7zPath, Constants.RSSongsFilePath, "-aoa") != "")
            {
                FileInfo f = new FileInfo("songs.hsan");
                if (f.Length > 0)
                {
                    if (!Directory.Exists(Constants.ManifestsFolderPath))
                        Directory.CreateDirectory(Constants.ManifestsFolderPath);
                    File.Copy("songs.hsan", Constants.RSSongsFilePath);
                }
                else
                    MessageBox.Show("Extracting song cache failed!", "Extracting failed!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                if (extractAll)
                    GetSongsFromSongFile();
            }
            else
                MessageBox.Show("Extracting song cache failed!", "Extracting failed!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public void GetSongsFromSongFile()
        {
            if (File.Exists(Constants.RSSongsFilePath))
                PopulateSongList(Constants.RSSongsFilePath, RS2014SongCollection, ref RS2014FullSongCollection,
                    RS2014DisabledSongCollection, ref RS2014FullDisabledSongCollection);

            if (File.Exists(Constants.RS1SongsFilePath))
                PopulateSongList(Constants.RS1SongsFilePath, RS1SongCollection, ref RS1FullSongCollection,
                    RS1DisabledSongCollection, ref RS1FullDisabledSongCollection);

            if (File.Exists(Constants.RS1DLCSongsFilePath))
                PopulateSongList(Constants.RS1DLCSongsFilePath, RS1DLCSongCollection, ref RS1DLCFullSongCollection,
                    RS1DLCDisabledSongCollection, ref RS1DLCFullDisabledSongCollection);

            fullSongCount = RS2014FullSongCollection.Count + RS1FullSongCollection.Count + RS1DLCFullSongCollection.Count;

            if (fullSongCount > 0)
                PopulateDataGridView();
        }

        private void ClearCollections()
        {
            RS2014FullDisabledSongCollection.Clear();
            RS2014FullSongCollection.Clear();
            RS2014DisabledSongCollection.Clear();
            RS2014SongCollection.Clear();

            RS1FullDisabledSongCollection.Clear();
            RS1FullSongCollection.Clear();
            RS1DisabledSongCollection.Clear();
            RS1SongCollection.Clear();

            RS1DLCFullDisabledSongCollection.Clear();
            RS1DLCFullSongCollection.Clear();
            RS1DLCDisabledSongCollection.Clear();
            RS1DLCSongCollection.Clear();

            CacheSongCollection.Clear();
            DisabledCacheSongCollection.Clear();
        }

        private void LoadSongsFromFolders()
        {
            Globals.Log("Loading songs - this may take up to one minute!");
            ClearCollections();
            if (!Directory.Exists(Constants.ExtractedRSPackPath) || !Directory.Exists(Constants.ExtractedRS1PackPath) || !Directory.Exists(Constants.ExtractedRS1DLCPackPath))
                ExtractAndLoadSongs();
            else
                GetSongsFromSongFile();
            Globals.Log("Songs loaded!");
        }

        private void UpdateSongs(bool cache, bool rs1, bool rs1dlc, bool cacheEnabled = false, bool rs1Enabled = false, bool rs1DlcEnabled = false)
        {
            Globals.Log("Modifying selected offical pack(s)! Please wait!");
            if (cache)
            {
                isModifiying = true;

                if (!cacheEnabled && File.Exists("songs.hsan.temp"))
                {
                    RS2014SongCollection.Clear();
                    RS2014FullSongCollection.Clear();
                    RS2014DisabledSongCollection.Clear();
                    RS2014FullDisabledSongCollection.Clear();

                    PopulateSongList("songs.hsan.temp", RS2014SongCollection, ref RS2014FullSongCollection, RS2014DisabledSongCollection, ref RS2014FullDisabledSongCollection);
                    File.Copy("songs.hsan.temp", Constants.RSSongsFilePath, true);
                }
                else
                {
                    if (File.Exists(Constants.CachePsarcPath) && !Directory.Exists(Constants.ExtractedRSPackPath))
                    {
                        ExtractAndLoadSongs();
                    }

                    if (RS1FullDisabledSongCollection.Count == 0 && RS1FullSongCollection.Count == 0 && Directory.Exists(Constants.ExtractedRSPackPath))
                        PopulateSongList(Constants.RSSongsFilePath, RS2014SongCollection, ref RS2014FullSongCollection, RS2014DisabledSongCollection, ref RS2014FullDisabledSongCollection);

                    if (RS2014FullDisabledSongCollection.Count > 0 || RS2014FullSongCollection.Count > 0)
                    {
                        if (cacheEnabled)
                        {
                            foreach (var song in RS2014FullSongCollection.ToList())
                            {
                                RS2014FullDisabledSongCollection.Add(song.Key, RS2014FullSongCollection[song.Key]);
                                RS2014FullSongCollection.Remove(song.Key);
                            }

                            foreach (var song in RS2014SongCollection.ToList())
                            {
                                RS2014DisabledSongCollection.Add(song.Key, RS2014SongCollection[song.Key]);
                                RS2014SongCollection.Remove(song.Key);
                            }
                        }
                        else
                        {
                            foreach (var song in RS2014FullDisabledSongCollection.ToList())
                            {
                                RS2014FullSongCollection.Add(song.Key, RS2014FullDisabledSongCollection[song.Key]);
                                RS2014FullDisabledSongCollection.Remove(song.Key);
                            }

                            foreach (var song in RS2014DisabledSongCollection.ToList())
                            {
                                RS2014SongCollection.Add(song.Key, RS2014DisabledSongCollection[song.Key]);
                                RS2014DisabledSongCollection.Remove(song.Key);
                            }
                        }
                    }

                    if (File.Exists(Constants.RSSongsFilePath))
                        SerializeSongFile(Constants.RSSongsFilePath, RS2014FullSongCollection, RS2014FullDisabledSongCollection, typeof(RSVocalsData));
                }
                if (comboGameChoice.SelectedIndex == 2)
                {
                    RefreshDGVAfterSearching(RS2014SongCollection, RS2014DisabledSongCollection);
                }

                isModifiying = false;
            }

            if (rs1)
            {
                isModifiying = true;
                if (File.Exists(Constants.RS1PackPath) && !Directory.Exists(Constants.ExtractedRS1PackPath))
                {
                    ExtractAndLoadSongs();
                }

                if (RS1FullDisabledSongCollection.Count == 0 && RS1FullSongCollection.Count == 0 && Directory.Exists(Constants.ExtractedRS1PackPath))
                    PopulateSongList(Constants.RS1SongsFilePath, RS1SongCollection, ref RS1FullSongCollection, RS1DisabledSongCollection, ref RS1FullDisabledSongCollection);

                if (RS1FullDisabledSongCollection.Count > 0 || RS1FullSongCollection.Count > 0)
                {
                    if (!rs1Enabled)
                    {
                        foreach (var song in RS1FullDisabledSongCollection.ToList())
                        {
                            RS1FullSongCollection.Add(song.Key, RS1FullDisabledSongCollection[song.Key]);
                            RS1FullDisabledSongCollection.Remove(song.Key);
                        }

                        foreach (var song in RS1DisabledSongCollection.ToList())
                        {
                            RS1SongCollection.Add(song.Key, RS1DisabledSongCollection[song.Key]);
                            RS1DisabledSongCollection.Remove(song.Key);
                        }

                        if (File.Exists(Constants.RS1SongsFilePath))
                            SerializeSongFile(Constants.RS1SongsFilePath, RS1FullSongCollection,
                                RS1FullDisabledSongCollection, typeof(RS1VocalsData));

                        if (comboGameChoice.SelectedIndex == 0)
                        {
                            RefreshDGVAfterSearching(RS1SongCollection, RS1DisabledSongCollection);
                        }
                    }
                }
                isModifiying = false;
            }

            if (rs1dlc)
            {
                isModifiying = true;
                if (File.Exists(Constants.RS1DLCPath) && !Directory.Exists(Constants.ExtractedRS1DLCPackPath))
                {
                    ExtractAndLoadSongs();
                }

                if (RS1DLCFullDisabledSongCollection.Count == 0 && RS1DLCFullSongCollection.Count == 0 && Directory.Exists(Constants.ExtractedRS1DLCPackPath))
                    PopulateSongList(Constants.RS1DLCSongsFilePath, RS1DLCSongCollection, ref RS1DLCFullSongCollection, RS1DLCDisabledSongCollection, ref RS1DLCFullDisabledSongCollection);

                if (RS1DLCFullDisabledSongCollection.Count > 0 || RS1DLCFullSongCollection.Count > 0)
                {
                    if (!rs1DlcEnabled)
                    {
                        foreach (var song in RS1DLCFullDisabledSongCollection.ToList())
                        {
                            RS1DLCFullSongCollection.Add(song.Key, RS1DLCFullDisabledSongCollection[song.Key]);
                            RS1DLCFullDisabledSongCollection.Remove(song.Key);
                        }

                        foreach (var song in RS1DLCDisabledSongCollection.ToList())
                        {
                            RS1DLCSongCollection.Add(song.Key, RS1DLCDisabledSongCollection[song.Key]);
                            RS1DLCDisabledSongCollection.Remove(song.Key);
                        }

                        if (File.Exists(Constants.RS1DLCSongsFilePath))
                            SerializeSongFile(Constants.RS1DLCSongsFilePath, RS1DLCFullSongCollection,
                                RS1DLCFullDisabledSongCollection, typeof(RS1DLCVocalsData));

                        if (comboGameChoice.SelectedIndex == 1)
                        {
                            RefreshDGVAfterSearching(RS1DLCSongCollection, RS1DLCDisabledSongCollection);
                        }
                    }
                }
                isModifiying = false;
            }
            Globals.Log("Done!");
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

        //doesn't get called anywhere
        //private void UpdateCacheEnabled()
        //{
        //    string Constants.RSSongsFilePath = @"manifests\songs\songs.hsan";
        //    RSDataJsonDictionary<RSSongData> tempCacheCollection = new RSDataJsonDictionary<RSSongData>();
        //    RSDataJsonDictionary<RSSongData> tempFullCacheCollection = new RSDataJsonDictionary<RSSongData>();
        //    RSDataJsonDictionary<RSSongData> tempDisabledCacheCollection = new RSDataJsonDictionary<RSSongData>();
        //    RSDataJsonDictionary<RSSongData> tempFullDisabledCacheCollection = new RSDataJsonDictionary<RSSongData>();

        //    if (File.Exists(Constants.RSSongsFilePath))
        //    {
        //        PopulateSongList(Constants.RSSongsFilePath, tempCacheCollection, ref tempFullCacheCollection, 
        //            tempDisabledCacheCollection, ref tempFullDisabledCacheCollection);

        //        //if (tempCacheCollection.Count == 0 && tempDisabledCacheCollection.Count > 0)
        //        //    dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells["colOfficialSongPack"].Value == "cache.psarc").Cells["colOfficialEnabled"].Value = "No";
        //        //else
        //        //    dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells["colOfficialSongPack"].Value == "cache.psarc").Cells["colOfficialEnabled"].Value = "Yes";
        //    }
        //    else if (dgvSongs.Rows.Count > 0)
        //    {
        //        //if (dgvSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colEnabled"].Value == "No").Count() == dgvSongs.Rows.Count)
        //        //    dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells["colOfficialSongPack"].Value == "cache.psarc").Cells["colOfficialEnabled"].Value = "No";
        //        //else
        //        //    dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells["colOfficialSongPack"].Value == "cache.psarc").Cells["colOfficialEnabled"].Value = "Yes";
        //    }
        //}

        public void LoadSongsFromHsans()
        {
            GetSongsFromSongFile();
            isModifiying = true;
            UnpackSongCache();
            Globals.Log("Song packs unpacked, feel free to enable/disable them!");
            isModifiying = false;
        }

        private void RestoreOfficialSongBackup()
        {
            try
            {
                if (File.Exists(Constants.RS1PackBackupPath))
                {
                    if (File.Exists(Constants.RS1PackPath.Replace("_p.", "_p.disabled.")))
                        File.Delete(Constants.RS1PackPath.Replace("_p.", "_p.disabled."));

                    File.Copy(Constants.RS1PackBackupPath, Constants.RS1PackPath, true);

                    //    if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("disc")).ToList().Count == 0)
                    //        dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydisc_p.psarc");
                    //    dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault((row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("disc"))).DefaultCellStyle.BackColor = Color.White;
                }
                if (File.Exists(Constants.RS1DLCBackupPath))
                {
                    if (File.Exists(Constants.RS1DLCPath.Replace("_p.", "_p.disabled.")))
                        File.Delete(Constants.RS1DLCPath.Replace("_p.", "_p.disabled."));

                    File.Copy(Constants.RS1DLCPath, Constants.RS1DLCBackupPath, true);

                    //if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("dlc")).ToList().Count == 0)
                    //    dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydlc_p.psarc");
                    //dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault((row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("dlc"))).DefaultCellStyle.BackColor = Color.White;
                }
                if (File.Exists(Constants.CachePsarcBackupPath))
                {
                    File.Copy(Constants.CachePsarcBackupPath, Constants.CachePsarcPath, true);

                    //if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("cache")).ToList().Count == 0)
                    //    dgvOfficialSongs.Rows.Add(false, "Yes", "cache.psarc");
                    //dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault((row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("cache"))).DefaultCellStyle.BackColor = Color.White;
                }
                if (File.Exists(Constants.CachePsarcPath) || File.Exists(Constants.RS1PackPath) || File.Exists(Constants.RS1DLCPath))
                {
                    if (File.Exists(Constants.RS1PackPath))
                    {
                        string[] disabledSongPacks = Directory.GetFiles(Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"), "rs1*disc_p.disabled.psarc", SearchOption.TopDirectoryOnly);
                        foreach (string disabledSongPack in disabledSongPacks)
                            File.Delete(disabledSongPack);
                    }

                    if (File.Exists(Constants.RS1DLCPath))
                    {
                        string[] disabledSongPacks = Directory.GetFiles(Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"), "rs1*dlc_p.disabled.psarc", SearchOption.TopDirectoryOnly);
                        foreach (string disabledSongPack in disabledSongPacks)
                            File.Delete(disabledSongPack);
                    }

                    //foreach (DataGridViewRow row in dgvOfficialSongs.Rows)
                    //    row.Cells["colOfficialEnabled"].Value = "Yes";

                    Globals.Log("Official song backup restored!");

                    EnableDisableCachePsarc(false);
                    UpdateSongs(true, true, true);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to restore backup! Error:\n\n" + ex.Message.ToString(), "Error");
            }
        }

        private void ExtractAndLoadSongs()
        {
            if (UnpackSongCache())
            {
                ExtractSongFile();
            }
            else
                Globals.Log("Unpacking song cache failed!");
        }

        private void EnableDisableCachePsarc(bool enabled)
        {
            if (enabled)
            {
                if (File.Exists("songs.hsan.temp"))
                    File.Delete("songs.hsan.temp");

                SerializeSongFile("songs.hsan.temp", RS2014FullSongCollection, RS2014FullDisabledSongCollection, typeof(RSVocalsData));

                isModifiying = true;

                if (!File.Exists("cache-disabled.psarc"))
                {
                    if (!Directory.Exists(Constants.BackupFolderPath))
                        Directory.CreateDirectory(Constants.BackupFolderPath);

                    if (!File.Exists(Constants.CachePsarcBackupPath))
                        File.Copy(Constants.CachePsarcPath, Constants.CachePsarcBackupPath);

                    CacheSongCollection.Clear();
                    DisabledCacheSongCollection.Clear();

                    Globals.Log("Disabling main song pack, this might take a few minutes!");

                    if (!Directory.Exists(Constants.ExtractedRSPackPath))
                    {
                        if (UnpackSongCache())
                        {
                            ExtractSongFile();
                        }
                    }

                    if (File.Exists(Constants.RSSongsFilePath))
                    {
                        string songFileContent = File.ReadAllText(Constants.RSSongsFilePath);
                        var songsJson = JObject.Parse(songFileContent);

                        var songsList = songsJson["Entries"];

                        CacheSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<RSSongData>>(songsList.ToString());

                        if (songsJson.ToString().Contains("DisabledSongs"))
                        {
                            var disabledSongsList = songsJson["DisabledSongs"];
                            DisabledCacheSongCollection = JsonConvert.DeserializeObject<RSDataJsonDictionary<RSSongData>>(disabledSongsList.ToString());
                        }

                        foreach (var song in RS2014FullSongCollection)
                        {
                            DisabledCacheSongCollection.Add(song.Key, RS2014FullSongCollection[song.Key]);
                            CacheSongCollection.Remove(song.Key);
                        }

                        File.Delete(Constants.RSSongsFilePath);

                        SerializeSongFile(Constants.RSSongsFilePath, CacheSongCollection, DisabledCacheSongCollection, typeof(RSVocalsData));

                        if (File.Exists(Constants.RSSongsFilePath))
                            File.Copy(Constants.RSSongsFilePath, "songs.hsan.disabled", true);

                        Directory.CreateDirectory("cache-disabled_Pc");

                        foreach (string newPath in Directory.GetFiles(Constants.ExtractedRSPackPath, "*.*", SearchOption.AllDirectories))
                            File.Copy(newPath, newPath.Replace(Constants.ExtractedRSPackPath, "cache-disabled_Pc"), true);

                        if (Run7Zip("u", Constants.Cache7zPath, Constants.RSSongsFilePath, "") != "")
                        {
                            if (Directory.Exists("cache-disabled_Pc"))
                            {
                                try
                                {
                                    if (!File.Exists(Path.Combine(Constants.ExtractedRSPackPath, "sltsv1_aggregategraph.nt")))
                                        File.Copy("sltsv1_aggregategraph.nt", Path.Combine(Constants.ExtractedRSPackPath, "sltsv1_aggregategraph.nt"));

                                    if (File.Exists("cache-disabled.psarc"))
                                        File.Delete("cache-disabled.psarc");

                                    RepackSong("cache-disabled");
                                }
                                catch (IOException ex)
                                {
                                    MessageBox.Show("Unable to repack main song pack! Error: " + ex.Message.ToString());
                                }
                            }
                        }

                        if (File.Exists("cache-disabled.psarc"))
                        {
                            if (!File.Exists(Constants.CachePsarcBackupPath))
                                File.Copy(Constants.CachePsarcPath, Constants.CachePsarcBackupPath);

                            File.Copy("cache-disabled.psarc", Constants.CachePsarcPath, true);
                            Globals.Log("Cache disabled!");
                        }
                    }
                    else
                        Globals.Log("Unpacking song cache failed!");

                    UpdateSongs(true, false, false, true);
                }
                else
                {
                    if (!File.Exists(Constants.CachePsarcBackupPath))
                        File.Copy(Constants.CachePsarcPath, Constants.CachePsarcBackupPath);

                    File.Copy("cache-disabled.psarc", Constants.CachePsarcPath, true);


                    if (File.Exists("songs.hsan.disabled"))
                        File.Copy("songs.hsan.disabled", Constants.RSSongsFilePath, true);

                    if (File.Exists(Constants.CachePsarcPath))
                        Globals.Log("Cache disabled!");

                    UpdateSongs(true, false, false, true);
                }
                isModifiying = false;
            }
            else
            {
                if (TempSongCollection.Count == 0 && DisabledTempSongCollection.Count == 0)
                {
                    if (File.Exists(Constants.CachePsarcBackupPath))
                        File.Copy(Constants.CachePsarcBackupPath, Constants.CachePsarcPath, true);
                    else
                        Globals.Log("No backup file found, please redownload cache.psarc file (verify integrity of game cache on Steam)!");
                }
                else
                {
                    File.Delete(Constants.RSSongsFilePath);

                    SerializeSongFile(Constants.RSSongsFilePath, TempSongCollection, DisabledTempSongCollection, typeof(RSVocalsData));

                    if (Run7Zip("u", Constants.Cache7zPath, Constants.RSSongsFilePath, "") != "")
                    {
                        if (Directory.Exists("cache_Pc"))
                        {
                            try
                            {
                                if (!File.Exists(Path.Combine(Constants.ExtractedRSPackPath, "sltsv1_aggregategraph.nt")))
                                    File.Copy("sltsv1_aggregategraph.nt", Path.Combine(Constants.ExtractedRSPackPath, "sltsv1_aggregategraph.nt"));

                                if (File.Exists("cache.psarc"))
                                    File.Delete("cache.psarc");

                                RepackSong("cache");

                                File.Copy("cache.psarc", Constants.CachePsarcPath, true);

                                Globals.Log("Main song pack enabled!");
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show("Unable to repack main song pack! Error: " + ex.Message.ToString());
                            }
                        }
                    }
                }
            }

            UpdateSongs(true, false, false);
        }


        public void PopulateDataGridView()
        {
            dgvSongs.Rows.Clear();

            if (comboGameChoice.SelectedIndex == 2)
            {
                AddToDGV(RS2014DisabledSongCollection, false);
                AddToDGV(RS2014SongCollection, true);
            }
            else if (comboGameChoice.SelectedIndex == 1)
            {
                AddToDGV(RS1DLCDisabledSongCollection, false);
                AddToDGV(RS1DLCSongCollection, true);
            }
            else
            {
                AddToDGV(RS1DisabledSongCollection, false);
                AddToDGV(RS1SongCollection, true);
            }
        }



        private void PopulateSongList<T>(string songFilePath, RSDataJsonDictionary<T> songCollection,
        ref RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection,
        ref RSDataJsonDictionary<T> fullDisabledSongCollection) where T : RSDataAbstractBase
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
        #endregion

        #region Save Songs
        public void UpdateHSANSongFile()
        {
            if (comboGameChoice.SelectedIndex == 2)
            {
                if (File.Exists(Constants.RSSongsFilePath))
                {
                    File.Delete(Constants.RSSongsFilePath);
                }
                SerializeSongFile(Constants.RSSongsFilePath, RS2014FullSongCollection, RS2014FullDisabledSongCollection, typeof(RSVocalsData));
            }
            else if (comboGameChoice.SelectedIndex == 1)
            {
                if (File.Exists(Constants.RS1DLCSongsFilePath))
                {
                    SerializeSongFile(Constants.RS1DLCSongsFilePath, RS1DLCFullSongCollection, RS1DLCFullDisabledSongCollection, typeof(RS1DLCVocalsData));
                }
            }
            else
            {
                if (File.Exists(Constants.RS1SongsFilePath))
                {
                    SerializeSongFile(Constants.RS1SongsFilePath, RS1FullSongCollection, RS1FullDisabledSongCollection, typeof(RS1VocalsData));
                }
            }
        }

        private void UpdateSongFile()
        {
            Run7Zip("u", Constants.Cache7zPath, Constants.RSSongsFilePath, "");
        }

        private bool RepackSong(string songPath)
        {
            string songExtractedFolder = "";
            string songPsarc = songPath + ".psarc";
            string songRSPsarc = "";
            string rsBackupFolder = Constants.BackupFolderPath;
            string songRSPsarcBackup = "";

            if (comboGameChoice.SelectedIndex == 0)
            {
                songExtractedFolder = Constants.ExtractedRS1PackPath;
                songRSPsarc = Constants.RS1PackPath;
                songRSPsarcBackup = Constants.RS1PackBackupPath;
            }
            else if (comboGameChoice.SelectedIndex == 1)
            {
                songExtractedFolder = Constants.ExtractedRS1DLCPackPath;
                songRSPsarc = Constants.RS1DLCPath;
                songRSPsarcBackup = Constants.RS1DLCBackupPath;
            }
            else
            {
                songExtractedFolder = Constants.ExtractedRSPackPath;
                songRSPsarc = Constants.CachePsarcPath;
                songRSPsarcBackup = Constants.CachePsarcBackupPath;
            }

            if (Directory.Exists(songExtractedFolder))
            {
                if (File.Exists(songPsarc))
                {
                    File.SetAttributes(songPsarc, FileAttributes.Normal);
                    File.Delete(songPsarc);
                }

                Packer.Pack(songExtractedFolder, songPath);

                if (Directory.Exists(AppSettings.Instance.RSInstalledDir))
                {
                    if (!Directory.Exists(Constants.BackupFolderPath))
                        Directory.CreateDirectory(Constants.BackupFolderPath);

                    if (!File.Exists(songRSPsarcBackup) && File.Exists(songRSPsarc))
                        File.Copy(songRSPsarc, songRSPsarcBackup);

                    try
                    {
                        File.Copy(songPsarc, songRSPsarc, true);
                    }
                    catch (DirectoryNotFoundException) { }
                }
                return true;
            }
            else
                return false;
        }

        public void RepackSongCache()
        {
            try
            {
                if (comboGameChoice.SelectedIndex == 2)
                {
                    if (Directory.Exists(Constants.ExtractedRSPackPath))
                    {
                        if (!File.Exists("sltsv1_aggregategraph.nt"))
                        {
                            CustomsForgeManagerLib.Extensions.ExtractEmbeddedResource("", "CustomsForgeManager.Resources", new string[] { "sltsv1_aggregategraph.nt" });
                        }

                        if (!File.Exists(Path.Combine(Constants.ExtractedRSPackPath, "sltsv1_aggregategraph.nt")))
                            File.Copy("sltsv1_aggregategraph.nt", Path.Combine(Constants.ExtractedRSPackPath, "sltsv1_aggregategraph.nt"));

                        RepackSong("cache");
                        MessageBox.Show("RS2014 songs saved!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (comboGameChoice.SelectedIndex == 1)
                {
                    if (RepackSong("rs1compatibilitydlc_p"))
                        MessageBox.Show("RS1 DLC songs saved!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Repacking RS1 DLC file failed! ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (RepackSong("rs1compatibilitydisc_p"))
                        MessageBox.Show("RS1 disc songs saved!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Repacking RS1 song file failed! ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (AccessViolationException)
            {
                MessageBox.Show("Please close Rocksmith and then try to save the songs again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SerializeSongFile<T>(string songFilePath, RSDataJsonDictionary<T> fullSongCollection,
            RSDataJsonDictionary<T> fullDisabledSongCollection, Type vdType)
             where T : RSDataAbstractBase
        {
            var tempFullSongCollection = new Dictionary<string, Dictionary<string, dynamic>>();
            var tempDisabledSongCollection = new Dictionary<string, Dictionary<string, dynamic>>();

            var DataConstructor = vdType.GetConstructor(new Type[] { });


            RSDataJsonDictionary<T>[] dicArray = new RSDataJsonDictionary<T>[] 
           { 
               fullSongCollection, fullDisabledSongCollection
           };

            Dictionary<string, Dictionary<string, dynamic>>[] tempDicArray =
                new Dictionary<string, Dictionary<string, dynamic>>[] { 
                   tempFullSongCollection, tempDisabledSongCollection };

            for (int i = 0; i < dicArray.Length; i++)
            {
                foreach (string song in dicArray[i].Keys.ToList())
                {
                    foreach (var songAttributes in dicArray[i][song])
                    {
                        var sngVal = songAttributes.Value;
                        if (sngVal.ArrangementName == "Vocals")
                        {
                            var rsv = (RSVocalsData)DataConstructor.Invoke(new object[] { });
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
                            if (rsv is RS1VocalsData)
                                ((RS1VocalsData)rsv).DLCKey = (sngVal as RS1SongData).DLCKey;
                            if (rsv is RS1DLCVocalsData)
                                ((RS1DLCVocalsData)rsv).DLCRS1Key = (sngVal as RS1DLCData).DLCRS1Key;
                            var currSng = new Dictionary<string, dynamic>();
                            currSng.Add("Attributes", rsv);
                            tempDicArray[i].Add(song, currSng);
                        }
                        else if (sngVal.SongName.Contains("[") || sngVal.SongName == "")
                        {
                            RSSongData rsv;
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

            using (StreamWriter file = new StreamWriter(songFilePath))
            {
                dynamic songJson = new
                {
                    Entries = tempFullSongCollection,
                    DisabledSongs = tempDisabledSongCollection,
                    InsertRoot = "Static.Songs.Headers"
                };

                JToken serializedJson = JsonConvert.SerializeObject(songJson, Formatting.Indented,
                    new JsonSerializerSettings { });

                file.Write(serializedJson.ToString());
            }
        }

        #endregion

        #region Enable/Disable

        private void DisableSong<T>(RSDataJsonDictionary<T> songCollection,
            RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection,
            RSDataJsonDictionary<T> fullDisabledSongCollection, string rowSongKey) where T : RSDataAbstractBase
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


        private void EnableSong<T>(RSDataJsonDictionary<T> songCollection,
            RSDataJsonDictionary<T> fullSongCollection, RSDataJsonDictionary<T> disabledSongCollection,
            RSDataJsonDictionary<T> fullDisabledSongCollection, string rowSongKey) where T : RSDataAbstractBase
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

        #endregion

        #region Extensions
        string Run7Zip(string command, string sourcePath, string destinationPath, string extraArgs)
        {
            pZip.FileName = pZipPath;
            pZip.UseShellExecute = false;
            pZip.RedirectStandardOutput = true;
            pZip.CreateNoWindow = true;
            pZip.Arguments = extraArgs != "" ? string.Format("{0} {1} {2} {3}", command, sourcePath, destinationPath, extraArgs) : string.Format("{0} {1} {2}", command, sourcePath, destinationPath);

            try
            {
                x = Process.Start(pZip);
                x.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                x.WaitForExit();
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Please make sure that all required files in the app folder (7za.exe, etc.)!", "Dependencies missing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            using (StreamReader reader = x.StandardOutput)
            {
                string rdr = reader.ReadToEnd();
                return rdr;
            }
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
                        if (songAttributes.Value.SongName.ToLower().Contains(tbSearch.Text.ToLower()) || songAttributes.Value.AlbumName.ToLower().Contains(tbSearch.Text.ToLower())
                            || songAttributes.Value.ArtistName.ToLower().Contains(tbSearch.Text.ToLower()) ||
                            songAttributes.Value.Tuning.ToString().TuningToName().ToLower().Contains(tbSearch.Text.ToLower()) || songAttributes.Value.SongKey.ToLower().Contains(tbSearch.Text.ToLower()))
                            matchingSongs.Add(songAttributes.Value);
                    }
                }
            }

            return matchingSongs;
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

        private string TuningJsonToStrings(Dictionary<string, int> tuningJson)
        {
            string tuning = "";
            foreach (KeyValuePair<string, int> stringTuning in tuningJson)
            {
                tuning += stringTuning.Value;
            }
            return tuning;
        }

        #endregion

        #region Events
        private void btnDisableSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colSelect"];
                if (chk.Value.ToString().ToLower() == "true" || row.Selected)
                {
                    string rowSongKey = row.Cells["colSongKey"].Value.ToString();
                    if (comboGameChoice.SelectedIndex == 2)
                    {
                        DisableSong(RS2014SongCollection, RS2014FullSongCollection, RS2014DisabledSongCollection, RS2014FullDisabledSongCollection, rowSongKey);
                    }
                    else if (comboGameChoice.SelectedIndex == 0)
                    {
                        DisableSong(RS1SongCollection, RS1FullSongCollection, RS1DisabledSongCollection, RS1FullDisabledSongCollection, rowSongKey);
                    }
                    else
                    {
                        DisableSong(RS1DLCSongCollection, RS1DLCFullSongCollection, RS1DLCDisabledSongCollection, RS1DLCFullDisabledSongCollection, rowSongKey);
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
                    if (comboGameChoice.SelectedIndex == 2)
                    {
                        EnableSong(RS2014SongCollection, RS2014FullSongCollection, RS2014DisabledSongCollection, RS2014FullDisabledSongCollection, rowSongKey);
                    }
                    else if (comboGameChoice.SelectedIndex == 0)
                    {
                        EnableSong(RS1SongCollection, RS1FullSongCollection, RS1DisabledSongCollection, RS1FullDisabledSongCollection, rowSongKey);
                    }
                    else
                    {
                        EnableSong(RS1DLCSongCollection, RS1DLCFullSongCollection, RS1DLCDisabledSongCollection, RS1DLCFullDisabledSongCollection, rowSongKey);
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

        private void btnSaveSongs_Click(object sender, EventArgs e)
        {
            Globals.Log("Saving " + comboGameChoice.Text + " songs!");
            Cursor = Cursors.WaitCursor;
            try
            {
                UpdateHSANSongFile();
                UpdateSongFile();
                RepackSongCache();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            Globals.Log(comboGameChoice.Text + " songs saved!");
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            RestoreOfficialSongBackup();
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

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbSearch.Text = "";
        }

        private void lnkRefreshAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadSongsFromFolders();
            Globals.Log("Song packs refreshed!");
        }
        private void comboGameChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            allSelected = false;
            tbSearch.Text = "";

            if (comboGameChoice.SelectedIndex == 2)
                RefreshDGVAfterSearching(RS2014SongCollection, RS2014DisabledSongCollection);
            else if (comboGameChoice.SelectedIndex == 1)
            {
                //DataGridViewRow dgvRow = dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("dlc"));
                //if (dgvRow != null && dgvRow.Cells["colOfficialEnabled"].Value == "Yes")
                RefreshDGVAfterSearching(RS1DLCSongCollection, RS1DLCDisabledSongCollection);
                //else
                //    dgvSongs.Rows.Clear();
            }
            else
            {
                //DataGridViewRow dgvRow = dgvOfficialSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells["colOfficialSongPack"].Value.ToString().Contains("disc"));
                //if (dgvRow != null && dgvRow.Cells["colOfficialEnabled"].Value == "Yes")
                RefreshDGVAfterSearching(RS1SongCollection, RS1DisabledSongCollection);
                //else
                //    dgvSongs.Rows.Clear();
            }
        }

        private void dgvSongs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == colSelect.Index)
            {
                dgvSongs.EndEdit();
            }
        }
        #endregion
    }
}