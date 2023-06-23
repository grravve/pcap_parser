namespace PcapParser
{
    public class PcapStatisticsData: Data
    {
        public PacketsLengthStat LengthStat { get; set; }
        public PacketsTimeIntervalStat TimeIntervalStat { get; set; }
    }
}
