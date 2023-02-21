namespace HelmetRanker.Extensions;

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    // From: https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
    public static string ToSlug(this string slugit, int maxLength = 0)
    {
        string str = slugit.RemoveDiacritics().ToLower(); 
        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); 
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim(); 
        // cut and trim 
        if (maxLength > 0)
        {
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
        }
        str = Regex.Replace(str, @"\s", "-"); // hyphens   
        return str; 
    }
    
    // From: https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
    public static string RemoveDiacritics(this string text) 
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string IncrementingSuffix(this string text)
    {
        if (!char.IsDigit(text[text.Length - 1]))
        {
            return $"{text}1";
        }
        else
        {
            var pos = text.Length - 1;
            while (pos >= 0 && char.IsDigit(text[pos]))
                pos--;
            pos++;

            var number = int.Parse(text.Substring(pos));
            number++;
            return $"{text.Substring(0, pos)}{number}";
        }
    }
}