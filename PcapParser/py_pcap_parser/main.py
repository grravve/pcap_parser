import sys

from pcap_parser import PcapParser
from pcap_serializer import SerializerPcapToJson

def main():
    _input = sys.argv
    parser = PcapParser()
    serializerStatsToJson = SerializerPcapToJson()
    
    statistics = parser.parse(_input)
    serializerStatsToJson.serialize(statistics)

    print("Stats is serialized to JSON")

if(__name__ == "__main__"):
    main()
