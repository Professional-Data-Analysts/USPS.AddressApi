using System.Text.RegularExpressions;

namespace USPS.AddressApi.Extensions
{
    internal static class XMLExtensions
    {
        private const string STRIP_REGEX_PATTERN = "[*,.()\"\\:\\;\\'\\-\\@\\&<>]+";
        private static Regex regex = new Regex(STRIP_REGEX_PATTERN, RegexOptions.Compiled);

        internal static string Clean(this string input)
        {
            return string.IsNullOrEmpty(input) ? string.Empty : regex.Replace(input, string.Empty);
        }
    }
}