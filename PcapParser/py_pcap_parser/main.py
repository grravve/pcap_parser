import sys

from pcap_parser import PcapParser
from pcap_serializer import SerializerPcapToJson

def main():
    # input: pcap_path, json_path
    _input = sys.argv
    _input.pop(0)

    pcap_filepath = _input[0]
    json_filepath = _input[1]

    parser = PcapParser()
    serializer = SerializerPcapToJson()

    metadata = serializer.deserialize_from_file(json_filepath)
    new_metadata = parser.signal_analyze(pcap_filepath, metadata)
    
    serializer.serialize_to_file(new_metadata, json_filepath)

if(__name__ == "__main__"):
    main()
