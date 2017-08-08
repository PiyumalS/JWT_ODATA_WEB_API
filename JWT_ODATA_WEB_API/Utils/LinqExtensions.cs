using System;
using System.Collections.Generic;
using System.Linq;

namespace JWT_ODATA_WEB_API.Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
        }

        public static string ToHtmlTable<T>(this List<T> listOfClassObjects)
        {
            var ret = string.Empty;

            return listOfClassObjects == null || !listOfClassObjects.Any()
                ? ret
                : "<table>" +
                  listOfClassObjects.First().GetType().GetProperties().Select(p => p.Name).ToList().ToColumnHeaders() +
                  listOfClassObjects.Aggregate(ret, (current, t) => current + t.ToHtmlTableRow()) +
                  "</table>";
        }

        public static string ToColumnHeaders<T>(this List<T> listOfProperties)
        {
            var ret = string.Empty;

            return listOfProperties == null || !listOfProperties.Any()
                ? ret
                : "<tr>" +
                  listOfProperties.Aggregate(ret,
                      (current, propValue) =>
                          current +
                          "<th style='font-size: 9pt; font-weight: bold; border: 0pt solid black;width: 150px' align='left'>" +
                          (Convert.ToString(propValue).Length <= 150
                              ? Convert.ToString(propValue)
                              : Convert.ToString(propValue).Substring(0, 150)) + "</th>") +
                  "</tr>";
        }

        public static string ToHtmlTableRow<T>(this T classObject)
        {
            var ret = string.Empty;

            return classObject == null
                ? ret
                : "<tr>" +
                  classObject.GetType()
                      .GetProperties()
                      .Aggregate(ret,
                          (current, prop) =>
                              current +
                              "<td style='font-size: 9pt; font-weight: normal; border: 0pt solid black;width: 150px'>" +
                              (Convert.ToString(prop.GetValue(classObject, null)).Length <= 150
                                  ? Convert.ToString(prop.GetValue(classObject, null))
                                  : Convert.ToString(prop.GetValue(classObject, null)).Substring(0, 150)
                                  ) + "</td>") + "</tr>";
        }
    }
}