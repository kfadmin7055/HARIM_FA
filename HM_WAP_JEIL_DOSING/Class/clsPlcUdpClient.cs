using ACTETHERLib;
using System;

public class PlcUdpClient
{
    private ActQJ71E71UDP plc;

    public string IP { get; }
    public int Port { get; }
    public int Station { get; }

    public bool IsConnected { get; private set; }

    public PlcUdpClient(string ip, int port = 5002, int station = 1)
    {
        IP = ip;
        Port = port;
        Station = station;

        plc = new ActQJ71E71UDP();
        plc.ActHostAddress = ip;
        plc.ActPortNumber = port;
        plc.ActStationNumber = station;
        plc.ActTimeOut = 3000;
    }

    public bool Connect()
    {
        int result = plc.Open();
        IsConnected = result != 0;
        Console.WriteLine(IsConnected ? $"[{IP}] 연결 성공" : $"[{IP}] 연결 실패 - 코드: {result}");
        return IsConnected;
    }

    public bool WriteDevice(string device, short value)
    {
        if (!IsConnected) return false;
        int result = plc.WriteDeviceBlock2(device, 1, ref value);
        Console.WriteLine($"[{IP}] WriteDevice 결과: {result}");
        return result != 0;
    }

    public bool WriteDeviceBlock(string startDevice, short[] values)
    {
        if (!IsConnected || values == null || values.Length == 0) return false;
        int result = plc.WriteDeviceBlock2(startDevice, values.Length, ref values[0]);
        Console.WriteLine($"[{IP}] WriteDeviceBlock 결과: {result}");
        return result != 0;
    }

    public void Disconnect()
    {
        if (IsConnected)
        {
            plc.Close();
            IsConnected = false;
        }
    }
}
