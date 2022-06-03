# This is a sample Python script.
import sys
import numpy as np
from scipy import signal


# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.

def get_butter(n, cutoff, btype):
    if btype == "high":
        b, a = signal.butter(n, cutoff, btype=btype)
    else:
        b, a = signal.butter(n, cutoff)

    return b, a


def get_lh_filtered_data(n, cutoff, btype, data):
    b, a = get_butter(n, cutoff, btype)
    y = signal.filtfilt(b, a, data, method='pad')  # LPF 적용 (100Hz 이상 버림)
    return y


def get_psd_filtered_data(n, cutoff, btype, data):
    b, a = get_butter(n, cutoff, btype)
    y = get_lh_filtered_data(n, cutoff, btype, data)
    f, p = signal.welch(y, window="boxcar", nperseg=500, noverlap=0, nfft=500)
    return f, p


def get_rms_filtered_data(n, cutoff, btype, data):
    f, p = get_psd_filtered_data(n, cutoff, btype, data)
    rms = np.sqrt(sum(p[:100]) * (f[1] - f[0]))
    fc = np.sqrt(sum(p[:100] * (f[:100] ** 2)) / sum(p[:100]))

    return rms, fc


def get_peak_filtered_data(n, cutoff, btype, data):
    b, a = get_butter(n, cutoff, btype)
    y = get_lh_filtered_data(n, cutoff, btype, data)
    idx_pv = np.diff(np.sign(np.diff(y))).nonzero()[0] + 1  # Index of peak/valley

    return y[idx_pv]


def get_zarray_filtered_data(n, cutoff, btype, data):
    b, a = get_butter(n, cutoff, btype)
    y = get_lh_filtered_data(n, cutoff, btype, data)
    idx_pv = np.diff(np.sign(np.diff(y))).nonzero()[0] + 1  # Index of peak/valley
    rms, fc = get_rms_filtered_data(n, cutoff, btype, data)
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
    arg3 = float(sys.argv[3])  # LPF/HPF의 경우 필터계수
    arg4 = float(sys.argv[4])  # LPF/HPF의 경우 필터계수
    arg5 = sys.argv[5]  # LOW / HIGH

    # for test
    # with open("/Users/sykim/PycharmProjects/pythonProject/x.txt") as f:
    #    arg2 = f.read()

    str_x = np.array(arg2.split(","))
    x = str_x.astype(np.float)

    result1 = ""
    result2 = ""
    if arg1 == "lpf":
        result1 = get_lh_filtered_data(arg3, arg4, "", x)
    elif arg1 == "hpf":
        result1 = get_lh_filtered_data(arg3, arg4, "high", x)
    elif arg1 == "psd":
        result1, result2 = get_psd_filtered_data(arg3, arg4, arg5, x)
    elif arg1 == "rms":
        result1, result2 = get_rms_filtered_data(arg3, arg4, arg5, x)
    elif arg1 == "peak":
        result1 = get_peak_filtered_data(arg3, arg4, arg5, x)
    elif arg1 == "zarray":
        result1 = get_zarray_filtered_data(arg3, arg4, arg5, x)

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
