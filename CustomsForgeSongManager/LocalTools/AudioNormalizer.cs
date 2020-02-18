using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using GenTools;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.XML;
using RocksmithToolkitLib.DLCPackage.Manifest.Functions;
using RocksmithToolkitLib;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using RocksmithToolkitLib.PSARC;
using System.Reflection;
using Newtonsoft.Json.Linq;
using StringExtensions = RocksmithToolkitLib.Extensions.StringExtensions;
using System.Drawing;

// AudioNormalizer is a bit of a misnomer ... the audio is never re-encoded
// algo uses normalizer analysis to auto adjust toolkit volume settings
// eventually move/add method to repairs after POC anlysis is complete

namespace CustomsForgeSongManager.LocalTools
{
    public class AudioNormalizer
    {
        private static ProgressStatus normalizerStatus;
        private static StringBuilder sbErrors;

        public static bool ValidateFfmpeg()
        {
            // validate ffmpeg installation
            var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var ffmpegDir = Path.Combine(assemblyDir, "ffmpeg");
            var ffmpegExePath = Path.Combine(ffmpegDir, "ffmpeg.exe");

            if (File.Exists(ffmpegExePath))
                return true;

            if (!Directory.Exists(ffmpegDir))
                Directory.CreateDirectory(ffmpegDir);

            Globals.Log("<WARNING> To use the 'Auto Adjust CDLC Volume' feature, " +
                        "please download FFmpegSetup.rar from http://goo.gl/i9jXEZ ...");
            frmNoteViewer.ViewExternalFile("ffmpeg/ReadMe.txt", "Auto Adjust CDLC Volume");

            return false;
        }

        private static frmNoteViewer cmdWin;
        public static bool NormalizeSong(string srcPath, AudioOptions audioOptions)
        {
            if (!FileTools.CreateBackupOfType(srcPath, Constants.BackupsFolder, Constants.EXT_BAK))
                return false;

            if (Path.GetFileName(srcPath).ToLower().EndsWith(Constants.BASESONGS) ||
                Path.GetFileName(srcPath).ToLower().EndsWith(Constants.BASESONGSDISABLED) ||
                Path.GetFileName(srcPath).ToLower().Contains(Constants.RS1COMP) ||
                Path.GetFileName(srcPath).ToLower().Contains(Constants.SONGPACK) ||
                Path.GetFileName(srcPath).ToLower().Contains(Constants.ABVSONGPACK))
            {
                Globals.Log("<WARNING> Audio from SongPacks is not available for normalization ...");
                return false;
            }

            try
            {
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                { Globals.TsProgressBar_Main.Value = 20; });

                // extract the audio...
                var oggPath = Path.Combine(Constants.TempWorkFolder, Path.GetFileNameWithoutExtension(srcPath) + ".ogg");
                if (!PsarcBrowser.ExtractAudio(srcPath, oggPath, ""))
                {
                    Globals.Log(Properties.Resources.CouldNotExtractTheAudio);
                    return false;
                }

                if (!File.Exists(oggPath))
                {
                    Globals.Log("<ERROR> Could not find ogg file: " + oggPath);
                    return false;
                }

                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                { Globals.TsProgressBar_Main.Value = 40; });

                // open custom command window
                var sb = new StringBuilder();
                cmdWin = new frmNoteViewer();
                cmdWin.Size = new Size(500, 500);
                cmdWin.StartPosition = FormStartPosition.CenterScreen;
                cmdWin.TopMost = true;
                cmdWin.Text = "CFSM Third Party Application Process Window ...";
                cmdWin.rtbBlank.BackColor = Color.Black;
                cmdWin.rtbText.BackColor = Color.Black;
                cmdWin.rtbText.ForeColor = Color.LimeGreen;
                cmdWin.rtbText.ScrollBars = RichTextBoxScrollBars.None;
                cmdWin.rtbText.Text = String.Empty;
                cmdWin.Show();
                Application.DoEvents();

                // analyze the audio
                Globals.Log(" - Running EBU R128 Normalization Analysis ...");
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var ffmpegDir = Path.Combine(baseDir, "ffmpeg");
                var ffmpegExe = Path.Combine(ffmpegDir, "ffmpeg.exe");
                var ebuPass1Args = "-nostats -hide_banner -i \"" + oggPath + "\"";
                ebuPass1Args += " -af loudnorm=I=-16:TP=-1.5:LRA=11:print_format=json"; // I=-12 LUFS (very loud/rude)                
                ebuPass1Args += " -f null -";

                var startInfo = new ProcessStartInfo
                {
                    FileName = ffmpegExe,
                    WorkingDirectory = Path.GetDirectoryName(srcPath),
                    Arguments = ebuPass1Args,
                    UseShellExecute = false,
                    CreateNoWindow = true, // hide command window
                    RedirectStandardError = true // ffmpeg uses std error for output
                };

                using (var ffmpeg = new Process())
                {
                    ffmpeg.EnableRaisingEvents = true;
                    ffmpeg.StartInfo = startInfo;
                    ffmpeg.Start();

                    // display processing status in a custom cmd window and capture output
                    while (!ffmpeg.StandardError.EndOfStream)
                    {
                        var line = ffmpeg.StandardError.ReadLine();
                        sb.AppendLine(line);
                        UpdateCmdWin(line);
                    }

                    // just to be sure ffmpeg exited
                    ffmpeg.WaitForExit(1000 * 60 * 15); // wait for 15 minutes, crunchy solution for AV-sandboxing issues
                }

                var ebuPass1Json = "{" + GenTools.StringExtensions.GetStringInBetween("{", "}", sb.ToString()) + "}";
                JToken jtoken = JObject.Parse(ebuPass1Json);
                var input_i = jtoken["input_i"].ToString();
                //var input_lra = jtoken["input_lra"].ToString();
                //var input_tp = jtoken["input_tp"].ToString();
                //var input_thresh = jtoken["input_thresh"].ToString();
                //var target_offset = jtoken["target_offset"].ToString();
                //var normalization_type = jtoken["normalization_type"].ToString().ToLower();

                // developer debug mode
                if (Constants.DebugMode)
                {
                    // write normalizer output to file
                    using (TextWriter tw = new StreamWriter(Constants.FfmpegLogPath, true))
                    {
                        tw.WriteLine(sb.ToString() + Environment.NewLine + "====================" + Environment.NewLine);
                        tw.Close();
                    }

                    // get the cmd window out of the way so we can debug
                    cmdWin.Close();
                    cmdWin.Dispose();
                    Globals.Log(" - Debugging Mode for Developer Use Only ...");

                    // POC constant overrides for testing/reference
                    //audioOptions.CorrectionFactor = 1.0f;
                    //audioOptions.CorrectionMultiplier = -1.0f;
                    //audioOptions.TargetAudioVolume = -7.0f; // new toolkit range +30 -30
                    //audioOptions.TargetPreviewVolume = -5.0f;
                    //audioOptions.TargetToneVolume = -20.0f;
                    //audioOptions.TargetLUFS = -16.0f;
                }

                Globals.Log(" - Correction Factor: " + Math.Round((float)audioOptions.CorrectionFactor, 1));
                Globals.Log(" - Correction Multiplier: " + Math.Round((float)audioOptions.CorrectionMultiplier, 1));
                Globals.Log(" - Target Audio Volume: " + Math.Round((float)audioOptions.TargetAudioVolume, 1));
                Globals.Log(" - Target Preview Volume: " + Math.Round((float)audioOptions.TargetPreviewVolume, 1));
                Globals.Log(" - Target Tone Volume: " + Math.Round((float)audioOptions.TargetToneVolume, 1));
                Globals.Log(" - Target LUFS: " + Math.Round((float)audioOptions.TargetLUFS, 1));
                Globals.Log(" - Integrated LUFS: " + input_i);

                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                { Globals.TsProgressBar_Main.Value = 60; });
                Globals.Log(" - Reading current volume data ...");

                // load package artifacts
                DLCPackageData packageData;
                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcPath);

                // read current volumes
                var volume = packageData.Volume;
                var previewVol = packageData.PreviewVolume;
                var toneVol = new double[packageData.TonesRS2014.Count];
                for (int i = 0; i < packageData.TonesRS2014.Count; i++)
                    toneVol[i] = packageData.TonesRS2014[i].Volume;

                // calculate and write new volumes
                var lufsOffset = (Convert.ToSingle(input_i) + audioOptions.TargetLUFS * -1) * audioOptions.CorrectionFactor * audioOptions.CorrectionMultiplier;
                packageData.Volume = audioOptions.TargetAudioVolume + lufsOffset;
                packageData.PreviewVolume = audioOptions.TargetPreviewVolume + lufsOffset;

                Globals.Log(" - POC Auto Adjust CDLC Volume Equation ...");
                Globals.Log("   NewAudioVolume = TargetAudioVolume + (IntegratedLUFS + TargetLUFS * -1) * CorrectionFactor * CorrectionMultiplier");
                Globals.Log("   New CDLC Audio Volume (Toolkit LF): " + packageData.Volume + " = " + audioOptions.TargetAudioVolume + " + (" + Convert.ToSingle(input_i) + " + " + audioOptions.TargetLUFS + " * -1) * " + audioOptions.CorrectionFactor + " * " + audioOptions.CorrectionMultiplier);

                for (int i = 0; i < packageData.TonesRS2014.Count; i++)
                {
                    var newToneVol = audioOptions.TargetToneVolume + lufsOffset;
                    packageData.TonesRS2014[i].Volume = newToneVol;
                }

                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                { Globals.TsProgressBar_Main.Value = 80; });
                Globals.Log(" - Writing new volume data ...");

                try
                {
                    // regenerate new XML, JSON, and SNG files and repackage artifacts               
                    using (var psarcNew = new PsarcPackager(true))
                        psarcNew.WritePackage(srcPath, packageData);
                }
                catch (Exception ex)
                {
                    throw new Exception("<ERROR> Writing Package: " + ex.Message + Environment.NewLine +
                    srcPath + Environment.NewLine);
                }

                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                { Globals.TsProgressBar_Main.Value = 100; });

                cmdWin.Close();
                cmdWin.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        // CAREFUL ... this is called from a background worker ... threading issues
        public static StringBuilder NormalizeSongs(List<SongData> songs, AudioOptions audioOptions)
        {
            GenericWorker.InitReportProgress();
            sbErrors = new StringBuilder();
            normalizerStatus = new ProgressStatus();
            FileTools.VerifyCfsmFolders();
            GenExtensions.DeleteFile(Constants.FfmpegLogPath);
            FileTools.CleanDlcFolder();
            Globals.Log("Auto Adjusting CDLC Volume ...");

            var srcFilePaths = FileTools.SongFilePaths(songs);
            var total = srcFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.InitReportProgress();

            foreach (var srcFilePath in srcFilePaths)
            {
                var isSkipped = false;
                Globals.Log("Processing: " + Path.GetFileName(srcFilePath));
                processed++;
                GenericWorker.ReportProgress(processed, total, skipped, failed);

                var isOfficialRepairedDisabled = FileTools.IsOfficialRepairedDisabled(srcFilePath);
                isOfficialRepairedDisabled = ""; // for testing ODLC
                if (!String.IsNullOrEmpty(isOfficialRepairedDisabled))
                {
                    if (isOfficialRepairedDisabled.Contains("Official"))
                    {
                        Globals.Log(" - Skipped ODLC File");
                        skipped++;
                        isSkipped = true;
                    }

                    if (isOfficialRepairedDisabled.Contains("Normalized"))
                    {
                        Globals.Log(" - Skipped Previously Normalized File");
                        skipped++;
                        isSkipped = true;
                    }
                }

                // Normalize Audio
                if (!isSkipped)
                {
                    var result = NormalizeSong(srcFilePath, audioOptions);
                    if (!result)
                    {
                        var lines = sbErrors.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        Globals.Log(String.Format(" - CDLC audio can not be adjusted ..."));

                        failed++;

                        // remove corrupt CDLC from SongCollection
                        //var song = Globals.MasterCollection.FirstOrDefault(s => s.FilePath == srcFilePath);
                        //int index = Globals.MasterCollection.IndexOf(song);
                        //Globals.MasterCollection.AllowRemove = true;
                        //Globals.MasterCollection.RemoveAt(index);
                        //Globals.ReloadSongManager = true; // set quick reload flag
                    }
                }

                if (!Constants.DebugMode)
                {
                    // cleanup after every nth record
                    if (processed % 50 == 0)
                        GenExtensions.CleanLocalTemp();
                }
            }

            if (!String.IsNullOrEmpty(sbErrors.ToString())) //failed > 0)
            {
                // error log can be turned into CSV file
                sbErrors.Insert(0, "File Path, Error Message" + Environment.NewLine);
                sbErrors.Insert(0, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine);
                using (TextWriter tw = new StreamWriter(Constants.RepairsErrorLogPath, true))
                {
                    tw.WriteLine(sbErrors + Environment.NewLine);
                    tw.Close();
                }

                Globals.Log(" - For file and error details, see: " + Constants.RepairsErrorLogPath);
            }

            GenericWorker.ReportProgress(processed, total, skipped, failed);

            if (processed > 0)
            {
                Globals.Log("Auto Adjust CDLC Volume Completed ...");
                Globals.ReloadSongManager = true;

                if (!Constants.DebugMode)
                    GenExtensions.CleanLocalTemp();
            }
            else
                Globals.Log("There were no CDLC volume adjustments ...");

            return sbErrors;
        }

        private static void UpdateCmdWin(string line)
        {
            // stole this code and concept from toolkit
            try
            {
                GenExtensions.InvokeIfRequired(cmdWin, a =>
                {
                    cmdWin.rtbText.Text += Environment.NewLine + line;
                    cmdWin.rtbText.SelectionStart = cmdWin.rtbText.Text.Length;
                    cmdWin.rtbText.ScrollToCaret();
                    Application.DoEvents();
                });
            }
            catch (Exception ex) // for Mac Wine/Mono compatiblity
            {
                var errMsg = new StringBuilder();
                errMsg.AppendLine("");
                errMsg.AppendLine("UpdateCmdWin ...");
                errMsg.AppendLine("If you are running toolkit on Mac Wine make sure Environmental Variable 'WINE_INSTALLED' is set to '1'");
                errMsg.AppendLine("");
                throw new SystemException(errMsg.ToString() + ex.Message + Environment.NewLine);
            }

            Debug.WriteLine(line);
        }
    }


    public class AudioOptions
    {
        // set default POC values here
        private float _correctionFactor = 1.0f;
        private float _correctionMultiplier = -1.0f;
        private float _targetAudioVolume = -7.0f;
        private float _targetPreviewVolume = -5.0f;
        private float _targetToneVolume = -20.0f;
        private float _targetLUFS = -16.0f;

        public float CorrectionFactor
        {
            get { return _correctionFactor; }
            set { _correctionFactor = value; }
        }

        public float CorrectionMultiplier
        {
            get { return _correctionMultiplier; }
            set { _correctionMultiplier = value; }
        }

        public float TargetAudioVolume
        {
            get { return _targetAudioVolume; }
            set { _targetAudioVolume = value; }
        }

        public float TargetPreviewVolume
        {
            get { return _targetPreviewVolume; }
            set { _targetPreviewVolume = value; }
        }

        public float TargetToneVolume
        {
            get { return _targetToneVolume; }
            set { _targetToneVolume = value; }
        }

        public float TargetLUFS
        {
            get { return _targetLUFS; }
            set { _targetLUFS = value; }
        }

    }
}
