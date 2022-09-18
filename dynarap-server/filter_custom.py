# This is a sample Python script.
import sys
import numpy as np
import os 
import csv
from scipy import signal


# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.

def get_butter(btype):
    with open("bacsvpath.conf") as f:
    	path = f.read().replace("\n", "")

    csv_file = open(path, 'r', encoding='utf-8')
    rdr = csv.reader(csv_file, delimiter=',')

    data = list()
    for line in rdr:
        data.append(line)

    csv_file.close()
    f.close()

    return data[0], data[1]


def get_lh_filtered_data(btype, data):
    b, a = get_butter(btype)
    y = signal.filtfilt(b, a, data, method='pad')  # LPF 적용 (100Hz 이상 버림)
    return y


def get_psd_filtered_data(btype, data):
    b, a = get_butter(btype)
    y = get_lh_filtered_data(btype, data)
    f, p = signal.welch(y, window="boxcar", nperseg=500, noverlap=0, nfft=500)
    return f, p


def get_rms_filtered_data(btype, data):
    f, p = get_psd_filtered_data(btype, data)
    rms = np.sqrt(sum(p[:100]) * (f[1] - f[0]))
    fc = np.sqrt(sum(p[:100] * (f[:100] ** 2)) / sum(p[:100]))

    return rms, fc


def get_peak_filtered_data(btype, data):
    b, a = get_butter(btype)
    y = get_lh_filtered_data(btype, data)
    idx_pv = np.diff(np.sign(np.diff(y))).nonzero()[0] + 1  # Index of peak/valley

    return y[idx_pv]


def get_zarray_filtered_data(btype, data):
    b, a = get_butter(btype)
    y = get_lh_filtered_data(btype, data)
    idx_pv = np.diff(np.sign(np.diff(y))).nonzero()[0] + 1  # Index of peak/valley
    rms, fc = get_rms_filtered_data(btype, data)
    y_pv = y[idx_pv]  # peak/valley 값

    z_array = y_pv / rms  # Normalization
    z_array = abs(z_array)  # Absolute Value
    z_array.sort()
    return z_array


# Press the green button in the gutter to run the script.
if __name__ == '__main__':

    if len(sys.argv) < 1:
        print("Usage: runpy cmd [args]")
        sys.exit(0)

    arg1 = sys.argv[1]  # 명령어 종류
    arg2 = sys.argv[2]  # 필터링할 데이터
    arg3 = sys.argv[3]  # LOW / HIGH

    with open(arg2) as f:
        data = f.read()

    os.remove(arg2)

    str_x = np.array(data.split(","))
    x = str_x.astype(np.float)

    result1 = ""
    result2 = ""
    if arg1 == "lpf":
        result1 = get_lh_filtered_data("", x)
    elif arg1 == "hpf":
        result1 = get_lh_filtered_data("high", x)
    elif arg1 == "psd":
        result1, result2 = get_psd_filtered_data(arg3, x)
    elif arg1 == "rms":
        result1, result2 = get_rms_filtered_data(arg3, x)
    elif arg1 == "peak":
        result1 = get_peak_filtered_data(arg3, x)
    elif arg1 == "zarray":
        result1 = get_zarray_filtered_data(arg3, x)

    if arg1 == "rms":
        print("result1=" + str(result1))
        print("result2=" + str(result2))
        sys.exit(0)

    print("result1=", end="")
    for item in result1:
        print(str(item) + ",", end="")

    print("")
    print("result2=", end="")
    for item in result2:
        print(str(item) + ",", end="")

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
