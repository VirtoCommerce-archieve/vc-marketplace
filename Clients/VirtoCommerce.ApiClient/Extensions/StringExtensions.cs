using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class StringExtensions
    {

        public static string EscapeSearchTerm(this string term)
        {
            char[] specialCharcters = { '+', '-', '!', '(', ')', '{', '}', '[', ']', '^', '"', '~', '*', '?', ':', '\\' };
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

        /// <summary>
        /// Escapes the selector. Query requires special characters to be escaped in query
        /// http://api.jquery.com/category/selectors/
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public static string EscapeSelector(this string attribute)
        {
            return Regex.Replace(attribute, string.Format("([{0}])", "/[!\"#$%&'()*+,./:;<=>?@^`{|}~\\]"), @"\\$1");
        }

    }
}
