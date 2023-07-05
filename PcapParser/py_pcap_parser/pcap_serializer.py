import json

class PcapStatsEncoder(json.JSONEncoder):
    def default(self, o):
        return o.__dict__

class SerializerPcapToJson:
    def serialize_to_file(self, data, json_filepath):
        with open(json_filepath, 'w') as f:
            json.dump(data, f, indent = 2, cls = PcapStatsEncoder)
    
    def deserialize_from_file(self, json_filepath):
        with open(json_filepath) as f:
            data = json.load(f)
        return data



