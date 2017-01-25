using System.Windows.Forms;
namespace VoxSharp
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Xml;
	using System.Xml.XPath;
	using MediaInfoLib;
	
	static class tools
	{
	    private static string app_path = AppDomain.CurrentDomain.BaseDirectory, settings_path = @"\settings.xml";
        	    
		private static string GetInfo(string file_from, string param)
		{
			string result = string.Empty;
			MediaInfo mi = new MediaInfo();			
			mi.Open(file_from);
			result = mi.Get(StreamKind.Audio, 0, param).ToUpperInvariant();
			return result;
		}
		
		public static bool convertToVox(FileInfo file_from, string savePath, bool normalise)
        {
			LogText ("convertToVox> from: " + file_from.FullName + ", to: " + savePath);
			
            bool Result = false;
            
            string file_name = file_from.Name, file_from_path = file_from.FullName, file_ext = file_from.Extension.ToUpperInvariant(), file_ext_actual = file_from.Extension;
			string file_format = GetInfo(file_from_path, "Format").ToUpperInvariant(), file_save_path = savePath + @"\" + file_name.Replace(file_ext_actual,"") + ".vox";
            
            if (File.Exists(file_save_path))
            {
            	LogText("convertToVox> file: " + file_save_path + " already exists!");
            	return Result;
            }
            
            string sox_path = getSetting("soxpath", false), lame_path = getSetting("lamepath", false), ffmpeg_path = getSetting("ffmpegpath", false);
            string Args = string.Empty, file_name_temp = file_from.DirectoryName + @"\" + file_name + ".wav";;
            try
            {
            	LogText(string.Format(CultureInfo.InvariantCulture, "convertToVox converting file: '{0}', audio format: '{1}', extension: '{2}' to: '{3}'",file_from, file_format, file_ext_actual, file_save_path));

                switch(file_format)
                {
                	case "ADPCM": case "GSM 6.10": case "PCM":
                    // convert wav to vox using sox       
                    try
                    {                        	
                    	Result = WavToVox(file_from_path, file_save_path, normalise, false);
                    }
                    catch (Exception ex)
                    {
                        LogText("convertToVox> fileEncode wav to vox exception occured: " + ex.Message);
                        Result = false;
                    }
                    break;
                    
                    case "MPEG AUDIO":
                    // convert mp3 to wav using lame
                    using (Process MyProcLameMP3 = new Process())
                    {
                        try
                        {                            
                            MyProcLameMP3.StartInfo.UseShellExecute = false;
                            MyProcLameMP3.StartInfo.RedirectStandardOutput = true;
                            MyProcLameMP3.StartInfo.CreateNoWindow = true;
                            MyProcLameMP3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            MyProcLameMP3.StartInfo.FileName = app_path + lame_path;
                            Args = "--decode \"" + file_from_path + "\" \"" + file_name_temp + "\"";                        	                               
                            MyProcLameMP3.StartInfo.Arguments = Args;
                            MyProcLameMP3.Start();
                            MyProcLameMP3.WaitForExit();
                            Result = true;
                        }
                        catch (Exception ex)
                        {
                        	LogText("convertToVox> fileEncode mp3 to wav exception occured: " + ex);
                            Result = false;
                        }
                    }
                    // convert wav to vox using sox
                    if (Result)
                    {
                        try
                        {
                        	Result = WavToVox(file_name_temp, file_save_path, normalise, true);
                        }
                        catch (Exception ex)
                        {
                            LogText("convertToVox> fileEncode mp3 to vox exception occured: " + ex.Message);
                            Result = false;
                        }
                    }
                    break;
                    
                   case "AAC": case "AIFF": case "ALAC": case "FLAC": case "VORBIS": case "WMA": case "MONKEY'S AUDIO":
                    // convert others to wav using ffmpeg
                    using (Process MyProcFfmpeg = new Process())
                    {
                        try
                        {                            
                            MyProcFfmpeg.StartInfo.UseShellExecute = false;
                            MyProcFfmpeg.StartInfo.RedirectStandardOutput = true;
                            MyProcFfmpeg.StartInfo.CreateNoWindow = true;
                            MyProcFfmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            MyProcFfmpeg.StartInfo.FileName = app_path + ffmpeg_path;
                            Args = "-i \"" + file_from_path + "\" -vn -ar 8000 \"" + file_name_temp + "\"";
                            MyProcFfmpeg.StartInfo.Arguments = Args;
                            MyProcFfmpeg.Start();
                            MyProcFfmpeg.WaitForExit();
                            Result = true;
                        }
                        catch (Exception ex)
                        {
                        	LogText("convertToVox> fileEncode audio to wav exception occured: " + ex.Message);
                            Result = false;
                        }
                    }
                    // convert wav to vox using sox
                    if (Result)
                    {
                        try
                        {
                        	Result = WavToVox(file_name_temp, file_save_path, normalise, true);
                        }
                        catch (Exception ex)
                        {
                            LogText("convertToVox> fileEncode mp3 to vox exception occured: " + ex.Message);
                            Result = false;
                        }
                    }
					break;
                    
                    default:
                    LogText(string.Format(CultureInfo.InvariantCulture,"convertToVox> audio format: '{0}' for file: '{1}' not supported", file_format, file_from));
                    break;
                }
            }
            catch (Exception ex)
            {
            	LogText("convertToVox> An exception occured: " + ex.ToString());
            }
            return Result;
        }
		
		private static bool WavToVox (string file_from, string file_to, bool normalise, bool delete_from)
		{
			string sox_path = getSetting("soxpath", false), Args = string.Empty;
			bool Result = false;
			                
            using (Process MyProcSoxVox = new Process())
	        {
				try
				{
					MyProcSoxVox.StartInfo.RedirectStandardOutput = false;
					MyProcSoxVox.StartInfo.CreateNoWindow = true;
					MyProcSoxVox.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					MyProcSoxVox.StartInfo.FileName = app_path + sox_path;
					Args = (normalise) ? "--norm " : "";
					Args += "'" + file_from + "' -t raw -r 8000 -c 1 -e a-law -b 8 '" + file_to + "'";					
					MyProcSoxVox.StartInfo.Arguments = Args;
					MyProcSoxVox.Start();
					MyProcSoxVox.WaitForExit();
					LogText("convertToVox> WavToVox file '" + file_to + "' converted");
					Result = true;
				}
				catch (Exception ex)
				{
					LogText("convertToVox> WavToVox exception occured: " + ex.Message);
					Result = false;
				}
				finally
				{
					MyProcSoxVox.Close();
					MyProcSoxVox.Dispose();					
					if (delete_from) File.Delete(file_from);
				}
            }
			return Result;
		}
		
		public static string getSetting(string setting_name, bool is_user)
		{
		    string result = string.Empty;
		    
		    if(File.Exists(app_path + settings_path))
            {
		        XPathDocument xml_doc = new XPathDocument(app_path + settings_path);
		        XPathNavigator xml_nav = xml_doc.CreateNavigator();
		        xml_nav.MoveToChild("settings","");
		        if(is_user) xml_nav.MoveToChild("user",""); else xml_nav.MoveToChild("app","");
		        if(xml_nav.MoveToChild(setting_name,"")) result = xml_nav.Value;		        
            }
		    return result;
		}

		public static bool updateSetting(string setting_name, string new_value, bool is_user)
		{
		    bool result = false;
		    
		    try
		    {
    		    if(File.Exists(app_path + settings_path))
                {
                    try
                    {
                        XmlDocument xml_doc = new XmlDocument();
                        xml_doc.Load(app_path + settings_path);
                        XPathNavigator xml_nav = xml_doc.CreateNavigator();
                        xml_nav.MoveToChild("settings","");
                        if(is_user) xml_nav.MoveToChild("user",""); else xml_nav.MoveToChild("app","");
                        
                        // change this nodes value
                        if(xml_doc.GetElementsByTagName(setting_name) != null)
                        {
                            XmlNodeList node_list = xml_doc.GetElementsByTagName(setting_name);
                            XmlNode xml_node = node_list[0];
                            LogText("changing xml node '" + xml_node.Name + "' from '" + xml_node.InnerXml + "' to '" + new_value + "'");
                            xml_node.InnerXml = new_value;
                            xml_doc.Save(app_path + settings_path);                            
                            result = true;
                        }
                        else LogText("updatesSetting> xml_doc.GetElementById(" + setting_name + ") returned null");
                    }
                    catch (XmlException xmlex)
                    {
                        LogText("updateSetting> an XML exception occured: " + xmlex.ToString());
                    }
                }   
		    }
		    catch (IOException ioex)
		    {
		        LogText("updateSetting> an IO Exception occured: " + ioex.ToString());
		    }
            return result;
		}
		
		public static void LogText(String LogMsg)
        {
		    Mutex mx = new Mutex();
		    mx.WaitOne();
		    
		    string log_path = app_path + getSetting("logpath", false);
		    
		    DirectoryInfo dir_info = new DirectoryInfo(log_path);
		    if(!dir_info.Exists) Directory.CreateDirectory(log_path);
		    
            string file_path = log_path + "VoxSharp_" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".log";

            if (File.Exists(file_path))
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(file_path))
                    {
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.ff ", CultureInfo.InvariantCulture) + LogMsg);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = File.CreateText(file_path))
                    {
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.ff ", CultureInfo.InvariantCulture) + LogMsg);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            mx.Close();
        }
		
		public static void ShowLog()
		{					    
		    string log_path = app_path + getSetting("logpath", false);
		    
		    DirectoryInfo dir_info = new DirectoryInfo(log_path);
		    if(dir_info.Exists)
		    {		    
            	string file_path = log_path + "VoxSharp_" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".log";
            	
	            if (File.Exists(file_path))
	            {
	            	using (Process logproc = new Process())
	            	{
						logproc.StartInfo.RedirectStandardOutput = false;
						logproc.StartInfo.CreateNoWindow = false;
						logproc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
						logproc.StartInfo.FileName = file_path;
						logproc.Start();
						logproc.WaitForExit();	            		
	            	}
	            }		    	
		    } else {
		    	MessageBox.Show(string.Format(CultureInfo.InvariantCulture,"can't find log directory: {0}", log_path));
		    }
		}
	}
}
