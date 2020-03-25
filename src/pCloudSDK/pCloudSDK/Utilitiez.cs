using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pCloudSDK
{
    public class Utilitiez
    {

        public static string AsQueryString(Dictionary<string, string> parameters)
        {
            if (!parameters.Any()) { return string.Empty; }

            var builder = new StringBuilder("?");
            var separator = string.Empty;
            foreach (var kvp in parameters.Where(P => !string.IsNullOrEmpty(P.Value)))
            {
                builder.AppendFormat("{0}{1}={2}", separator, System.Net.WebUtility.UrlEncode(kvp.Key), System.Net.WebUtility.UrlEncode(kvp.Value.ToString()));
                separator = "&";
            }
            return builder.ToString();
        }

        public static string RandomString(int length)
        {
            Random RandomCharactar = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[RandomCharactar.Next(s.Length)]).ToArray());
        }

        public enum SentType
        {
            filepath,
            bytesArray,
            memorystream
        }
        public enum FileFolderEnum
        {
            file,
            folder
        }
        public enum IfExistsEnum
        {
            rename,
            overwrite,
            skip
        }
        public enum IconEnum
        {
            image = 1,
            video = 2,
            document = 4,
            archive = 5,
            executable = 0,
            file = 0,
            web = 0,
            folder
        }
        public enum AudiobitrateEnum
        {
            Extra = 1411,
            High = 320
        }
        public enum PlaylistTypeEnum
        {
            AudioOnly = 1,
            All = 0
        }
        public enum OutputEnum
        {
            PlaylistsWithoutFiles,
            PlaylistsWithFiles
        }
        public enum ExtEnum
        {
            jpeg = 0,
            png = 1
        }


        public class PreviewSize
        {
            public const string Default = null;
            public const string S_150 = "S";
            public const string S_300 = "M";
            public const string S_500 = "L";
            public const string S_800 = "XL";
            public const string S_1024 = "XXL";
            public const string S_1280 = "XXXL";
            /// <summary>
            ///The width for example, "120" or "120x"
            ///</summary>
            public const string Width = "120";
            /// <summary>
            ///The height for example, "x145"
            ///</summary>
            public const string Height = "x110";
            /// <summary>
            ///The widthxheight.. The exact size (in the widthxheight format, for example "120x240").
            ///</summary>
            public const string widthXheight = "120x240";
        }






    }


    //public class ByteFormatter
    //{
    //    private const long KB = 1024;
    //    private const long MB = KB * 1024;
    //    private const long GB = MB * 1024;

    //    private const string BFormatPattern = "{0} b";
    //    private const string KBFormatPattern = "{0:0} KB";
    //    private const string MBFormatPattern = "{0:0,###} MB";
    //    private const string GBFormatPattern = "{0:0,###.###} GB";


    //    public new static string ToString(long size)
    //    {
    //        if (size < KB)
    //            return string.Format(BFormatPattern, size);
    //        else if ((size >= KB && size < MB))
    //            return string.Format(KBFormatPattern, size / (double)1024.0F);
    //        else if ((size >= MB && size < GB))
    //            return string.Format(MBFormatPattern, size / (double)1024.0F);
    //        else
    //            return string.Format(GBFormatPattern, size / (double)1024.0F);
    //    }
    //}

    //public class TimeSpanFormatter
    //{
    //    public new static string ToString(TimeSpan ts)
    //    {
    //        string str1;
    //        if ((!(ts == TimeSpan.MaxValue)))
    //        {
    //            string str = ts.ToString();
    //            int index = str.LastIndexOf('.');
    //            str1 = index <= 0 ? str : str.Remove(index);
    //        }
    //        else
    //            str1 = "?";
    //        return str1;
    //    }
    //}


}



