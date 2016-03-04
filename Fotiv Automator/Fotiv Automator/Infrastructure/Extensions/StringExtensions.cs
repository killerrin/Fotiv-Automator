using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fotiv_Automator.Infrastructure.Extensions
{
    public static class StringExtensions
    {

        public static string Slugify(this string rawString)
        {
            rawString = Regex.Replace(rawString, @"[^a-zA-Z0-9\s]", "");
            rawString = rawString.ToLower();
            rawString = Regex.Replace(rawString, @"\s", "-");
            return rawString;
        }
    }
}
