using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GenTools
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            return String.Format("{0}{1}", input.Substring(0, 1).ToUpper(), input.Substring(1).ToLower());
        }

        public static string ConvertToAscii(this string input)
        {
            Encoding inEncoding = Encoding.UTF8;
            Encoding outEncoding = Encoding.GetEncoding(437); // IBM437 - ASCII
            byte[] byteOuput = Encoding.Convert(inEncoding, outEncoding, inEncoding.GetBytes(input));
            return outEncoding.GetString(byteOuput);
        }

        public static string ConvertToIso(this string input)
        {
            // using "ISO-8859-8" gives better results than ""ISO-8859-1"
            byte[] byteOuput = Encoding.GetEncoding("ISO-8859-8").GetBytes(input);
            return Encoding.GetEncoding("ISO-8859-8").GetString(byteOuput);
        }

        public static string ConvertToUtf8(this string input)
        {
            byte[] byteOuput = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(input));
            return Encoding.UTF8.GetString(byteOuput);
        }

        public static string DecodeFrom64(this string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string GetFileExtensionNoDot(this string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            return fi.Extension.Substring(1).ToLower();
        }

        public static bool IsMailAddress(this string input)
        {
            Regex rgx = new Regex(@"^([\w\-\._]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$");
            return rgx.IsMatch(input);
        }

        public static bool IsPhoneNumberFrench(this string input)
        {
            Regex rgx = new Regex(@"^(01|02|03|04|05|06|07|09)[0-9]{8}$");
            return rgx.IsMatch(input);
        }

        public static bool IsUrl(this string input)
        {
            Regex rgx = new Regex(@"((https?|www|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
            return rgx.IsMatch(input);
        }

        public static string ReplaceDiacritics(this string input)
        {
            var stFormD = input.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string ReplaceSpecialCharacters(this string value)
        {
            value = Regex.Replace(value, "[ÀÁÂÃÅÄĀĂĄǍǺ]", "A");
            value = Regex.Replace(value, "[ǻǎàáâãäåąāă]", "a");
            value = Regex.Replace(value, "[ÇĆĈĊČ]", "C");
            value = Regex.Replace(value, "[çčćĉċ]", "c");
            value = Regex.Replace(value, "[ĎĐ]", "D");
            value = Regex.Replace(value, "[ďđ]", "d");
            value = Regex.Replace(value, "[ÈÉÊËĒĔĖĘĚ]", "E");
            value = Regex.Replace(value, "[ěèéêëēĕėę]", "e");
            value = Regex.Replace(value, "[ĜĞĠĢ]", "G");
            value = Regex.Replace(value, "[ģĝğġ]", "g");
            value = Regex.Replace(value, "[Ĥ]", "H");
            value = Regex.Replace(value, "[ĥ]", "h");
            value = Regex.Replace(value, "[ÌÍÎÏĨĪĬĮİǏ]", "I");
            value = Regex.Replace(value, "[ǐıįĭīĩìíîï]", "i");
            value = Regex.Replace(value, "[Ĵ]", "J");
            value = Regex.Replace(value, "[ĵ]", "j");
            value = Regex.Replace(value, "[Ķ]", "K");
            value = Regex.Replace(value, "[ķĸ]", "k");
            value = Regex.Replace(value, "[ĹĻĽĿŁ]", "L");
            value = Regex.Replace(value, "[ŀľļĺł]", "l");
            value = Regex.Replace(value, "[ÑŃŅŇŊ]", "N");
            value = Regex.Replace(value, "[ñńņňŉŋ]", "n");
            value = Regex.Replace(value, "[ÒÓÔÖÕŌŎŐƠǑǾ]", "O");
            value = Regex.Replace(value, "[ǿǒơòóôõöøōŏő]", "o");
            value = Regex.Replace(value, "[ŔŖŘ]", "R");
            value = Regex.Replace(value, "[ŗŕř]", "r");
            value = Regex.Replace(value, "[ŚŜŞŠ]", "S");
            value = Regex.Replace(value, "[şŝśš]", "s");
            value = Regex.Replace(value, "[ŢŤ]", "T");
            value = Regex.Replace(value, "[ťţ]", "t");
            value = Regex.Replace(value, "[ÙÚÛÜŨŪŬŮŰŲƯǓǕǗǙǛ]", "U");
            value = Regex.Replace(value, "[ǜǚǘǖǔưũùúûūŭůűų]", "u");
            value = Regex.Replace(value, "[Ŵ]", "W");
            value = Regex.Replace(value, "[ŵ]", "w");
            value = Regex.Replace(value, "[ÝŶŸ]", "Y");
            value = Regex.Replace(value, "[ýÿŷ]", "y");
            value = Regex.Replace(value, "[ŹŻŽ]", "Z");
            value = Regex.Replace(value, "[žźż]", "z");
            value = Regex.Replace(value, "[œ]", "oe");
            value = Regex.Replace(value, "[Œ]", "Oe");
            value = Regex.Replace(value, "[°]", "o");
            value = Regex.Replace(value, "[¡]", "!");
            value = Regex.Replace(value, "[¿]", "?");
            value = Regex.Replace(value, "[«»\u201C\u201D\u201E\u201F\u2033\u2036]", "\"");
            value = Regex.Replace(value, "[\u2026]", "...");
            return value;

            //var oldChar = new[] { "À", "Á", "Â", "Ã", "Å", "Ä", "Ç", "È", "É", "Ê", "Ë", "Ì", "Í", "Î", "Ï", "Ñ", "Ò", "Ó", "Ô", "Ö", "Õ", "Ù", "Ú", "Û", "Ü", "Ý", "à", "á", "â", "ã", "ä", "å", "ç", "è", "é", "ê", "ë", "ì", "í", "î", "ï", "ñ", "ò", "ó", "ô", "õ", "ö", "ø", "ù", "ú", "û", "ý", "ÿ", "Ā", "ā", "Ă", "ă", "Ą", "ą", "Ć", "ć", "Ĉ", "ĉ", "Ċ", "ċ", "Č", "č", "Ď", "ď", "Đ", "đ", "Ē", "ē", "Ĕ", "ĕ", "Ė", "ė", "Ę", "ę", "Ě", "ě", "Ĝ", "ĝ", "Ğ", "ğ", "Ġ", "ġ", "Ģ", "ģ", "Ĥ", "ĥ", "Ĩ", "ĩ", "Ī", "ī", "Ĭ", "ĭ", "Į", "į", "İ", "ı", "Ĵ", "ĵ", "Ķ", "ķ", "ĸ", "Ĺ", "ĺ", "Ļ", "ļ", "Ľ", "ľ", "Ŀ", "ŀ", "Ł", "ł", "Ń", "ń", "Ņ", "ņ", "Ň", "ň", "ŉ", "Ŋ", "ŋ", "Ō", "ō", "Ŏ", "ŏ", "Ő", "ő", "Ŕ", "ŕ", "Ŗ", "ŗ", "Ř", "ř", "Ś", "ś", "Ŝ", "ŝ", "Ş", "ş", "Š", "š", "Ţ", "ţ", "Ť", "ť", "Ũ", "ũ", "Ū", "ū", "Ŭ", "ŭ", "Ů", "ů", "Ű", "ű", "Ų", "ų", "Ŵ", "ŵ", "Ŷ", "ŷ", "Ÿ", "Ź", "ź", "Ż", "ż", "Ž", "ž", "Ơ", "ơ", "Ư", "ư", "Ǎ", "ǎ", "Ǐ", "ǐ", "Ǒ", "ǒ", "Ǔ", "ǔ", "Ǖ", "ǖ", "Ǘ", "ǘ", "Ǚ", "ǚ", "Ǜ", "ǜ", "Ǻ", "ǻ", "Ǿ", "ǿ" };
            //var newChar = new[] { "A", "A", "A", "A", "A", "A", "C", "E", "E", "E", "E", "I", "I", "I", "I", "N", "O", "O", "O", "O", "O", "U", "U", "U", "U", "Y", "a", "a", "a", "a", "a", "a", "c", "e", "e", "e", "e", "i", "i", "i", "i", "n", "o", "o", "o", "o", "o", "o", "u", "u", "u", "y", "y", "A", "a", "A", "a", "A", "a", "C", "c", "C", "c", "C", "c", "C", "c", "D", "c", "D", "d", "E", "e", "E", "e", "E", "e", "E", "e", "E", "e", "G", "g", "G", "g", "G", "g", "G", "g", "H", "h", "I", "i", "I", "i", "I", "i", "I", "i", "I", "i", "J", "j", "K", "k", "k", "L", "l", "L", "l", "L", "l", "L", "l", "L", "l", "N", "n", "N", "n", "N", "n", "n", "N", "n", "O", "o", "O", "o", "O", "o", "R", "r", "R", "r", "R", "r", "S", "s", "S", "s", "S", "s", "S", "s", "T", "t", "T", "t", "U", "u", "U", "u", "U", "u", "U", "u", "U", "u", "U", "u", "W", "w", "Y", "y", "Y", "Z", "z", "Z", "z", "Z", "z", "O", "o", "U", "u", "A", "a", "I", "i", "O", "o", "U", "u", "U", "u", "U", "u", "U", "u", "U", "u", "A", "a", "O", "o" };

            //for (int i = 0; i < oldChar.Length; i++)
            //{
            //    value = value.Replace(oldChar[i], newChar[i]);
            //}
            //return value;
        }

        public static string RightStrip(this string s, int n)
        {
            return s.Substring(0, s.Length - n);
        }

        public static string Shorten(this string input, int sizeCut, bool addSuspensionPoints, bool cutOnlyOnSpaces)
        {
            if (input.Length < 5)
                return input;
            bool hasToBeCut = input.Length > sizeCut;
            string cutText = hasToBeCut ? input.Substring(0, sizeCut) : input;
            if (cutOnlyOnSpaces && hasToBeCut)
            {
                while (cutText[cutText.Length - 1] != ' ')
                {
                    cutText = cutText.RightStrip(1);
                }
                cutText = cutText.RightStrip(1);
            }
            return String.Format("{0}{1}", cutText.Replace("[...]", ""), addSuspensionPoints && hasToBeCut ? "..." : string.Empty);
        }

        public static List<string> SplitToList(this string input, params char[] separator)
        {
            string[] tokens = input.Split(separator);
            return tokens.ToList();
        }

        public static string StripHtml(this string inputString)
        {
            return Regex.Replace(inputString, @"<(.|\n)*?>", string.Empty);
        }

        public static string UrlFormat(this string input)
        {
            // remove ', &, punctuations, replace +, etc
            string result = input.Replace("'", "");
            result = result.Replace("+", "plus");
            result = result.Replace("&", "et");
            result = result.Replace("`", "");
            result = result.Replace("~", "");
            result = result.Replace("@", "at");
            result = result.Replace("#", "");
            result = result.Replace("\"", "");
            result = result.Replace("$", "");
            result = result.Replace("€", "");
            result = result.Replace("£", "");
            result = result.Replace("*", "");
            result = result.Replace(",", "");
            result = result.Replace(".", "");
            result = result.Replace(";", "");
            result = result.Replace(":", "");
            result = result.Replace("!", "");
            result = result.Replace("?", "");
            result = result.Replace("(", "");
            result = result.Replace(")", "");
            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("{", "");
            result = result.Replace("}", "");
            result = result.Replace("/", "");
            result = result.Replace(@"\", "");
            result = Regex.Replace(result, @"\s+", "-");

            // replace diacritics (Ç -> C, É -> E, é -> e, ç -> c, etc)
            result = ReplaceDiacritics(result);
            // lower
            result = result.ToLower();

            return result;
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            // ODLC artist, title, album character use allows these but not these
            // allow use of accents Über ñice \\p{L}
            // allow use of unicode punctuation \\p{P\\{S} not currently implimented
            // may need to be escaped \t\n\f\r#$()*+.?[\^{|  ... '-' needs to be escaped if not at the beginning or end of regex sequence
            // allow use of only these special characters \\-_ /&.:',!?()\"#
            // allow use of alphanumerics a-zA-Z0-9
            // tested and working ... Üuber!@#$%^&*()_+=-09{}][":';<>.,?/ñice

            Regex rgx = new Regex("[^a-zA-Z0-9\\-_ /&.:',!?()\"#%=; ]");
            var result = rgx.Replace(value, "");
            return result;
        }

        public static string RemoveLeadingNumbers(this string value)
        {
            Regex rgx = new Regex(@"^[\d]*\s*");
            var result = rgx.Replace(value, "");
            return result;
        }

        public static string RemoveLeadingSpecialCharacters(this string value)
        {
            Regex rgx = new Regex("^[^A-Za-z0-9]*");
            var result = rgx.Replace(value, "");
            return result;
        }

        public static string RemoveString(this string value, string stringToRemove)
        {
            var result = value.Replace(stringToRemove, "");
            return result;
        }

        public static string ReplaceString(this string value, string stringToReplace, string replacementValue = "")
        {
            var result = value.Replace(stringToReplace, replacementValue);
            return result;
        }

        public static string RemoveDiacritics(this string value)
        {
            // test string = "áéíóúç";
            var result = Regex.Replace(value.Normalize(NormalizationForm.FormD), "[^A-Za-z| ]", String.Empty);
            return result;
        }

        /// <summary>
        /// Removes the suffix from a string, otherwise leave string unchanged
        /// </summary>
        /// <param name="str"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string RemoveSuffix(this string str, string suffix)
        {
            if (str.EndsWith(suffix))
                str = str.Remove(str.Length - suffix.Length, suffix.Length);

            return str;
        }

        /// <summary>
        /// Removes the prefix from a string, otherwise leave string unchanged
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string RemovePrefix(this string str, string prefix)
        {
            if (str.StartsWith(prefix))
                str = str.Remove(0, prefix.Length);

            return str;
        }

        public static string GetStringInBetween(string strBegin, string strEnd, string strSource)
        {
            string result = "";
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    result = strSource.Substring(0, iEnd);
                }
            }
            return result;
        }

    }
}