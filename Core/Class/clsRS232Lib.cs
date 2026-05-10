using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Class
{
    public class SerialPortHelper : IDisposable
    {
        public static SerialPort _serialPort { get; private set; }

        // 이벤트: 데이터 수신
        public event Action<string> DataReceived;

        public SerialPortHelper(string portName, int baudRate = 9600, int dataBits = 8,
                                Parity parity = Parity.None, StopBits stopBits = StopBits.One)
        {
            _serialPort = new SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity   = parity,
                StopBits = stopBits,
                Encoding = Encoding.UTF8
            };

            _serialPort.DataReceived += OnDataReceived;
        }

        // 포트 열기
        public void Open()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowMessageBox.XtraShowWarning("포트가 이미 사용 중입니다: " + ex.Message);
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowWarning("포트 열기 실패: " + ex.Message);
            }
        }

        // 포트 닫기
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        // 데이터 전송
        public void Send(string data)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(data);
            }
            else
            {
                throw new InvalidOperationException("Serial port is not open.");
            }
        }

        // 내부 데이터 수신 처리
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadExisting();
                DataReceived?.Invoke(data.Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read error: " + ex.Message);
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen) _serialPort.Close();
                _serialPort.Dispose();
            }
        }
    }
}
