using System;
using System.Collections.Generic;
using System.Linq;

namespace GetDateRanges
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var dates = Mock();
            dates.Dump("Source data");

            dates = dates.ConsolidateRange();

            Console.WriteLine();
            dates.Dump("Periods");
        }


        static void Dump(this IEnumerable<DateRange> source, string title = null)
        {
            if(title != null)
            {
                Console.WriteLine(title);
            }

            foreach (var r in source.OrderBy(x => x.Start))
            {
                Console.WriteLine(r);
            }
        }


        static IEnumerable<DateRange> Mock()
        {
            return new[] {
                new DateRange("01 feb 2022 10:00", "02 feb 2022 15:00"),

                new DateRange("03 feb 2022 10:00", "10 feb 2022 15:00"),
                new DateRange("05 feb 2022 18:00", "06 feb 2022 09:00"),
                new DateRange("09 feb 2022 10:00", "15 feb 2022 19:00"),

                // Couple of dups
                new DateRange("09 feb 2022 10:00", "15 feb 2022 19:00"),
                new DateRange("09 feb 2022 10:00", "15 feb 2022 19:00"),

                new DateRange("20 feb 2022 08:00", "23 feb 2022 11:00"),
                new DateRange("21 feb 2022 08:00", "25 feb 2022 20:00")
            };
        }
    }
}
