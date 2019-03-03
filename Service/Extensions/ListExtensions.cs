using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Services.Extensions
{
    public static class Lists
    {
        public static List<SelectListItem> ToSelectList<T>(
                     this IEnumerable<T> enumerable,
                     Func<T, string> text,
                     Func<T, string> value,
                     String defaultOption)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();

            if (!(defaultOption == null))
            {
                items.Insert(0, new SelectListItem()
                {
                    Text = defaultOption,
                    Value = ""
                });
            }

            return items.ToList();
        }
        public static bool Like(this string s, string pattern)
        {
            //Find the pattern anywhere in the string
            pattern = ".*" + pattern + ".*";
            return Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);

        }
    }
}
