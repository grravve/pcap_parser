using SharpPcap;
using SharpPcap.LibPcap;
using System.Text;

namespace PcapParser
{
    public class ParserPCAP : IParser
    {
        private PcapStatisticsData _statisticsData;

        private ICaptureDevice _captureFileReader;
        
        private string _parsingFileFullName;
        private string _pcapFilter;

        private List<int> _packetsLength;
        private List<double> _packetsIntervals;

        private PosixTimeval? _previousPacketTime;

        public ParserPCAP(string[] args)
        {
            _parsingFileFullName = args[0];

            _pcapFilter = args[1];

            _captureFileReader = new CaptureFileReaderDevice(_parsingFileFullName);
            
            InitializeCaptureReader();
                        
            _packetsIntervals = new List<double>();
            _packetsLength = new List<int>();
            _statisticsData = new PcapStatisticsData();
        }

        public Data GetParserData()
        {
            return _statisticsData;
        }

        public void Parse()
        {
            if(_captureFileReader == null)
            {
                return;
            }

            Console.WriteLine($"Parsing from file {_parsingFileFullName}...");

            _captureFileReader.Capture();
            _captureFileReader.Close();

            GenerateStats();            
        }

        private void OnPacketArrival_HandlePacket(object sender, PacketCapture e)
        {
            RawCapture capturedPacket = e.GetPacket();
            
            _packetsLength.Add(capturedPacket.PacketLength);

            if (_previousPacketTime == null)
            {
                _previousPacketTime = capturedPacket.Timeval;

                return;
            }

            var timeIntervalBetweenPackets = capturedPacket.Timeval.Date - _previousPacketTime.Date;
            _packetsIntervals.Add(timeIntervalBetweenPackets.TotalMilliseconds);

            _previousPacketTime = capturedPacket.Timeval;
        }

        private void GenerateStats()
        {
            _packetsLength.Sort();
            _packetsIntervals.Sort();

            int minLength = _packetsLength[0];
            int maxLenght = _packetsLength[_packetsLength.Count - 1];
            int averageLength = (int)_packetsLength.Average();
            int medianLength = _packetsLength[(_packetsLength.Count - 1) / 2];

            double minTime = _packetsIntervals[0];
            double maxTime = _packetsIntervals[_packetsIntervals.Count - 1];
            double averageTime = _packetsIntervals.Average();
            double medianTime = _packetsIntervals[(_packetsIntervals.Count - 1) / 2];

            _statisticsData.LengthStat = new PacketsLengthStat()
            {
                MinLength = minLength,
                MaxLength = maxLenght,
                AverageLength = averageLength,
                MedianLength = medianLength
            };

            _statisticsData.TimeIntervalStat = new PacketsTimeIntervalStat()
            {
                MinTime = minTime,
                MaxTime = maxTime,
                AverageTime = averageTime,
                MedianTime = medianTime
            };
        }

        private void InitializeCaptureReader()
        {
            try
            {
                _captureFileReader.Open();
                _captureFileReader.Filter = _pcapFilter;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when opening file.\n" + e.ToString());
                Environment.Exit(e.GetHashCode());
            }

            _captureFileReader.OnPacketArrival += OnPacketArrival_HandlePacket;
        }
    }
}
