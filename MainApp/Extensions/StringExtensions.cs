using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainApp.Extensions
{
    public static class StringExtensionMethods
    {
        public static int? ParseNullableInt(this string str)
        {
            if (int.TryParse(str, out int tmpValue))
                return tmpValue;
            return null;
        }

        public static long? ParseNullableLong(this string str)
        {
            if (long.TryParse(str, out long tmpValue))
                return tmpValue;
            return null;
        }

        public static float? ParseNullableFloat(this string str)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            // if the first regex matches, the number string is in us culture
            if (Regex.IsMatch(str, @"^(:?[\d,]+\.)*\d+$"))
            {
                cultureInfo = new CultureInfo("en-US");
            }
            // if the second regex matches, the number string is in de culture
            else if (Regex.IsMatch(str, @"^(:?[\d.]+,)*\d+$"))
            {
                cultureInfo = new CultureInfo("de-DE");
            }
            NumberStyles styles = NumberStyles.Number;

            if (float.TryParse(str, styles, cultureInfo, out float tmpValue))
                return tmpValue;
            return null;
        }
        public static double? ParseNullableDouble(this string str)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            // if the first regex matches, the number string is in us culture
            if (Regex.IsMatch(str, @"^(:?[\d,]+\.)*\d+$"))
            {
                cultureInfo = new CultureInfo("en-US");
            }
            // if the second regex matches, the number string is in de culture
            else if (Regex.IsMatch(str, @"^(:?[\d.]+,)*\d+$"))
            {
                cultureInfo = new CultureInfo("de-DE");
            }
            NumberStyles styles = NumberStyles.Number;

            if (double.TryParse(str, styles, cultureInfo, out double tmpValue))
                return tmpValue;
            return null;
        }
    }
}
