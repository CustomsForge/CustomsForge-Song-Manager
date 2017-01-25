using System;

/* general usage:
 * 
 * declare something like this in app Constants
     public static string OFDXmlFilter
     {
         get
         {
             var xmlDescExtList = new FileDescExt[]
             {
                 new FileDescExt{FileDescription = "SongData Xml File", FileExtension = "xml"}
             };

             return new OfdFilterBuilder().GetOfdFilter(xmlDescExtList);
         }
     }
 * 
 * 
 * call method like this
     sfd.Filter = Constants.OFDXmlFilter;
 * 
 */

namespace CustomControls
{
    public class OfdFilterBuilder
    {

        public string GetOfdFilter(dynamic fileDescExts) // file description and file extension as string array
        {
            // "All Supported Files|*.wem;*.ogg;*.wav|Wwise 2013 audio files (*.wem)|*.wem|Ogg Vorbis audio files (*.ogg)|*.ogg|Wave audio files (*.wav)|*.wav" 
            var ofdFilter = "All Supported Files";
            ofdFilter += "|";

            // get all supported extensions
            for (int i = 0; i < fileDescExts.GetLength(0); i++)
                ofdFilter += String.Format("*.{0};", fileDescExts[i].FileExtension);

            // get file descriptions and extensions
            for (int i = 0; i < fileDescExts.GetLength(0); i++)
                ofdFilter += String.Format("|{0} (*.{1})|*.{1}", fileDescExts[i].FileDescription, fileDescExts[i].FileExtension);

            ofdFilter += ";";

            return ofdFilter;
        }
    }

    public class FileDescExt
    {
        public string FileDescription { get; set; }
        public string FileExtension { get; set; }
    }

}
