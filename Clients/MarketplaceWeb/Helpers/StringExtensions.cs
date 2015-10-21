using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Helpers
{
    public static class StringExtensions
    {
        public static string EscapeSearchTerm(this string term)
        {
            char[] specialCharcters =
            {
                '+',
                '-',
                '!',
                '(',
                ')',
                '{',
                '}',
                '[',
                ']',
                '^',
                '"',
                '~',
                '*',
                '?',
                ':',
                '\\'
            };
            var retVal = "";
            //'&&', '||',
            foreach (var ch in term)
            {
                if (specialCharcters.Any(x => x == ch))
                {
                    retVal += "\\";
                }
                retVal += ch;
            }
            retVal = retVal.Replace("&&", @"\&&");
            retVal = retVal.Replace("||", @"\||");
            retVal = retVal.Trim();

            return retVal;
        }
    }
}