﻿using System.Globalization;
using System.Linq;
using System.Text;

namespace PlantenApplicatie.Data
{
    internal static class PlantenParser
    {
        public static string ParseSearchText(string text)
        {
            return RemoveDiacriticsFromText(text.Trim().ToLower().Replace("'", ""));
        }

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
    }
}