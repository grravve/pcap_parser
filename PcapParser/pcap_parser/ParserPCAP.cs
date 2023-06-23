using SharpPcap;
using SharpPcap.LibPcap;

namespace PcapParser
{
    public class ParserPCAP : IParser
    {
        private ICaptureDevice _captureFileReader;
        
        private string _parsingFileFullName;
        private string _pcapFilter;

        private int _capturePacketCounter;

        private List<int> _packetsLength;
        private List<double> _packetsIntervals;

        public ParserPCAP(string[] args)
        {
            if (args.Length > 2)
            {
                throw new Exception("Input arguments count greater than 2");
            }

            _capturePacketCounter = 0;
            _parsingFileFullName = args[0];
            _pcapFilter = args[1];
            _packetsIntervals = new List<double>();
            _packetsLength = new List<int>();
            _captureFileReader = new CaptureFileReaderDevice(_parsingFileFullName);

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

        public void Parse()
        {
            if(_captureFileReader == null)
            {
                return;
            }

            Console.WriteLine($"Parsing from file {_parsingFileFullName}...");

            var startTime = DateTime.Now;

            _captureFileReader.Capture();
            _captureFileReader.Close();

            var durationTime = DateTime.Now - startTime;

            Console.WriteLine($"Read {_capturePacketCounter} packets in {durationTime.TotalSeconds} seconds");
        }

        private void OnPacketArrival_HandlePacket(object sender, PacketCapture e)
        {
            _capturePacketCounter++;
        }


    }
}
