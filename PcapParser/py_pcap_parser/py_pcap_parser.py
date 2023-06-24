from statistics import median, mean
import pyshark
import sys
import json
import os.path

class LengthStat:
    def __init__(self, min_length, max_length, average_length, median_length):
        self.min_length = min_length
        self.max_length = max_length
        self.average_length = average_length
        self.median_length = median_length

class TimeStat:
    def __init__(self, min_time, max_time, average_time, median_time):
        self.min_time = min_time
        self.max_time = max_time
        self.average_time = average_time
        self.median_time = median_time

class PcapStats:
    def __init__(self, LengthStat, TimeStat):
        self.LengthStat = LengthStat
        self.TimeStat = TimeStat

class PcapStatsEncoder(json.JSONEncoder):
    def default(self, o):
        return o.__dict__

class PcapParser:
    def __init__(self):
        self.__time_list = []
        self.__length_list = []
        self.__previous_packet_time = -1

    def check_input(self, input_args):
        input_args.pop(0)
        if(len(input_args) != 2):
            return
    
        filepath = input_args[0]
        file_exist = os.path.isfile(filepath)

        if(file_exist == False):
            print("Input file doesn't exist")
            exit(-1)

        path_split = os.path.splitext(filepath)

        if(path_split[1] != ".pcap" and path_split[1] != ".pcapng"):
            print("Input file extension non .pcap or .pcapng")
            exit(-1)

        return input_args
    
    def parse(self, input_list):
        args = self.check_input(input_list)
        
        filepath = args[0]
        packet_filter = args[1]

        self.start_pcap_filereader(filepath, packet_filter)
        
        if(len(self.__length_list) == 0 or len(self.__time_list) == 0):
            print("No such packet with input filter")
            exit(1)

        return self.generate_pcap_stats()
        
    def start_pcap_filereader(self, filepath, packet_filter):
        capture = pyshark.FileCapture(input_file = filepath, display_filter = packet_filter)
        capture.apply_on_packets(self.handle_packet)

    def handle_packet(self, packet):
        self.__length_list.append(packet.__len__())

        if(self.__previous_packet_time == -1):
            self.__previous_packet_time = packet.sniff_time
            return

        time_interval = packet.sniff_time - self.__previous_packet_time
        interval_milliseconds = time_interval.total_seconds() * 1000
        interval_milliseconds = round(interval_milliseconds, 5)
    
        self.__time_list.append(interval_milliseconds)

        self.__previous_packet_time = packet.sniff_time

    def generate_pcap_stats(self):
        self.__length_list.sort()
        self.__time_list.sort()

        length_stat = LengthStat(self.__length_list[0], self.__length_list[-1], 
                                 round(mean(self.__length_list), 2), round(median(self.__length_list), 2))
        time_stat = TimeStat(self.__time_list[0], self.__time_list[-1], 
                                 round(mean(self.__time_list), 2), round(median(self.__time_list), 2))

        return PcapStats(length_stat, time_stat)

def main():
    _input = sys.argv
    parser = PcapParser()
    statistics = parser.parse(_input)

    with open('pcap_statistics.json', 'w') as f:
        json.dump(statistics, f, indent=1, ensure_ascii=False, cls = PcapStatsEncoder)
    
if(__name__ == "__main__"):
    main()
