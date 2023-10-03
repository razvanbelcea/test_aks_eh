using System.Collections.Generic;
using System.IO;

namespace eathappy.order.business.Helper
{
    public static class DetectCsvDelimiter
    {
        public static string DetectDelimiter(StreamReader reader)
        {
            var possibleDelimiters = new List<string> { ",", ";", "\t", "|" };

            var headerLine = reader.ReadLine();

            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();

            foreach (var possibleDelimiter in possibleDelimiters)
            {
                if (headerLine.Contains(possibleDelimiter))
                {
                    return possibleDelimiter;
                }
            }

            return possibleDelimiters[0];
        }
    }
}
