using System.Text.Json;

namespace PcapParser
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if(!CheckInputArguments(args))
            {
                return;
            }

            IParser parser = new ParserPCAP(args);

            parser.Parse();

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            string jsonString = SerializePCAPStatsToJSON(parser.GetParserData(), jsonOptions);

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\output.json";

            File.WriteAllText(filePath, jsonString);

            Console.WriteLine(jsonString);
        }

        private static bool CheckInputArguments(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("The number of input arguments should be 2");
                
                return false;
            }

            return true;
        }

        private static string SerializePCAPStatsToJSON(Data data, JsonSerializerOptions options)
        {
            PcapStatisticsData pcapStats;

            try
            {
                pcapStats = (PcapStatisticsData)data;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return JsonSerializer.Serialize(pcapStats, options);
        }
    }
}
