using System;
using System.Collections.Generic;
using System.Linq;

namespace GetDateRanges
{
    public class DateRange
    {
        public DateRange(string start, string end)
            : this(DateTime.Parse(start), DateTime.Parse(end))
        {
        }

        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override string ToString()
        {
            return $"{Start} - {End}";
        }
    }


    /// <summary>
    /// Some helper methods
    /// </summary>
    public static class DateRangeExtensions
    {
        public static IEnumerable<DateRange> ConsolidateRange(this IEnumerable<DateRange> source)
        {
            var periods = new List<DateRange>();
            var dates = source.ToList(); // Take a copy as we mutate it (we remove ranges as we process)

            // Start with earliest start
            var range = dates.Next(DateTime.MinValue);

            while (range != null)
            {
                // Find a range that overlaps the current range. Get the one with the latest end date
                var overlappingRange = dates.LatestOverlappingByEndDate(range);

                // Didn't find one. End of range, store it and look for next range
                if (overlappingRange == null)
                {
                    periods.Add(range);
                    range = dates.Next(range.End);
                }

                // Found one: Expand our range to cover it.
                else
                {
                    // Remove this range from the source
                    dates.Remove(overlappingRange);

                    range.End = overlappingRange.End;
                }
            }

            return periods;
        }


        // Find any ovelapping
        // Get the source range that has:
        //  a start between currant dt start/end
        //  an end later than currant dt end
        private static DateRange LatestOverlappingByEndDate(this IEnumerable<DateRange> source, DateRange range)
        {
            return source
                .OrderByDescending(d => d.End)
                .FirstOrDefault(d =>
                    d != range
                    && d.Start >= range.Start
                    && d.Start <= range.End
                    && d.End > range.End);
        }


        /// <summary>
        /// Next range with a start greater than minStart
        /// </summary>
        /// <param name="source"></param>
        /// <param name="minStart"></param>
        /// <returns></returns>
        private static DateRange Next(this IEnumerable<DateRange> source, DateTime minStart)
        {
            return source
                .Where(d => d.Start > minStart)
                .OrderBy(d => d.Start)
                .FirstOrDefault();
        }
    }
}
