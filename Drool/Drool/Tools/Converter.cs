using System.Text;

namespace Drool.Tools
{
    public static class Converter
    {
        /// <summary>
        /// Convert utf to iso 8859-2.
        /// </summary>
        public static string ToIsoString(this string utf)
        {
            var iso = Encoding.GetEncoding("ISO-8859-2");
            var utf8 = Encoding.UTF8;
            var utfBytes = utf8.GetBytes(utf);
            var isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            var msg = iso.GetString(isoBytes);
            return msg;
        }
    }
}
