namespace PcapParser
{
    public interface IParser
    {
        public void Parse();
        public Data GetParserData();
    }
}
