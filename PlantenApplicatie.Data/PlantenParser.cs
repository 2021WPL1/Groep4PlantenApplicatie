using System.Globalization;
using System.Linq;
using System.Text;

namespace PlantenApplicatie.Data
{
    internal static class PlantenParser
    {
        //Zet de tekst om naar kleine letters en verwijdert spaties in het begin en einde (Davy&Zakaria&Lily)
        public static string ParseSearchText(string text)
        {
            return text.Trim().TrimStart().ToLower();
            //return RemoveDiacriticsFromText(text.Trim().ToLower());
        }

        /*
        private static string RemoveDiacriticsFromText(string originalText)
        {
            var newText = string.Empty;
            
            foreach (var c in originalText.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                newText += c;
            }

            return newText;
        }
        */
    }
}