class PcapStats:
    def __init__(self, LengthStat, TimeStat):
        self.LengthStat = LengthStat
        self.TimeStat = TimeStat

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