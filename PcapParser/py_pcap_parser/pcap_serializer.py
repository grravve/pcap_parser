import json

class PcapStatsEncoder(json.JSONEncoder):
    def default(self, o):
        return o.__dict__

class SerializerPcapToJson:
    def serialize(self, stats):
        with open('pcap_statistics.json', 'w') as f:
            json.dump(stats, f, indent=1, ensure_ascii=False, cls = PcapStatsEncoder)
