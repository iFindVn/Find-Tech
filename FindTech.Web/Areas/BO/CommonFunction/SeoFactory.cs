using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FindTech.Web.Areas.BO.CommonFunction
{
    public static class SeoFactory
    {
        public static string GenerateSeoTitle(this string title)
        {
            var normalizedString = title.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(title.Length);
            foreach (var c in normalizedString.ToLower().ToCharArray())
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    switch (c.ToString())
                    {
                        case "&":
                            stringBuilder.Append("va");
                            break;
                        case "$":
                            stringBuilder.Append("dola");
                            break;
                        case "%":
                            stringBuilder.Append("phan-tram");
                            break;
                        case ".":
                            stringBuilder.Append("-");
                            break;
                        case "/":
                            stringBuilder.Append("-");
                            break;
                        case "\\":
                            stringBuilder.Append("-");
                            break;
                        case "đ":
                            stringBuilder.Append("d");
                            break;
                        default:
                            stringBuilder.Append(c);
                            break;
                    }
                }
            }
            var seoTitle = stringBuilder.ToString();
            seoTitle = Regex.Replace(seoTitle, @"[^a-z0-9\s-]", "");
            seoTitle = Regex.Replace(seoTitle, @"\s+", " ").Trim();
            seoTitle = Regex.Replace(seoTitle, @"\s", "-");
            return seoTitle;
        }
    }
}