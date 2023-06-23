namespace PcapParser
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ParserPCAP parserPCAP = new ParserPCAP(args);

            StartParse(parserPCAP);
        }

        public static void StartParse(IParser parser)
        {
            parser.Parse();
        }
    }
}
