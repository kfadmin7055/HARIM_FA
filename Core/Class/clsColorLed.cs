using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Class
{
    public class clsColorLed
    {

        public clsColorLed()
        {


        }
        private static bool IsPing(string ipaddr)
        {
            bool networkState = NetworkInterface.GetIsNetworkAvailable();
            bool pingResult = true;

            //네트워크가 연결이 되어있다면
            if (networkState)
            {
                //"192.168.0.80";
                string addr = ipaddr;
                Ping pingSender = new Ping();

                PingReply reply = pingSender.Send(addr, 300);

                pingResult = reply.Status == IPStatus.Success;

            }

            return networkState & pingResult;
        }

        public static void ledOFF(string ip_addr)
        {
            try
            {

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var ep = new IPEndPoint(IPAddress.Parse(ip_addr), 5000);
                sock.Connect(ep);


                int len = 6;
                var frame = new List<byte>();
                //STX
                frame.Add((byte)0x02);
                //length
                frame.Add((byte)(len >> 8));
                frame.Add((byte)len);
                frame.Add((byte)0x35);

                int nSum = 0;
                for (int i = 0; i < (len - 2); i++)
                {
                    nSum += frame[i];
                }

                //buff[4] = sum;
                frame.Add((byte)nSum);

                //buff[5] = ETX;
                frame.Add(0x03);

                if (!sock.Connected)
                {
                    return;
                }

                // (3) 서버에 데이타 전송
                int st = sock.Send(frame.ToArray(), SocketFlags.None);


                byte[] receiverBuff = new byte[256];
                // (4) 서버에서 데이타 수신
                int n = sock.Receive(receiverBuff, 0, receiverBuff.Length, SocketFlags.None);

                string data = Encoding.UTF8.GetString(receiverBuff, 0, n);
                sock.Close();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsColorLed", "ledOFF", ex);
            }
        }

        public static void ledON(string ip_addr)
        {
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var ep = new IPEndPoint(IPAddress.Parse(ip_addr), 5000);
                sock.Connect(ep);


                int len = 6;
                var frame = new List<byte>();
                //STX
                frame.Add((byte)0x02);
                //length
                frame.Add((byte)(len >> 8));
                frame.Add((byte)len);
                frame.Add((byte)0x36);

                int nSum = 0;
                for (int i = 0; i < (len - 2); i++)
                {
                    nSum += frame[i];
                }

                //buff[4] = sum;
                frame.Add((byte)nSum);

                //buff[5] = ETX;
                frame.Add(0x03);

                if (!sock.Connected)
                {
                    return;
                }


                // (3) 서버에 데이타 전송
                int st = sock.Send(frame.ToArray(), SocketFlags.None);

                byte[] receiverBuff = new byte[256];
                // (4) 서버에서 데이타 수신
                int n = sock.Receive(receiverBuff, 0, receiverBuff.Length, SocketFlags.None);

                string data = Encoding.UTF8.GetString(receiverBuff, 0, n);
                sock.Close();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsColorLed", "ledON", ex);
            }
        }

        public static string LedColor(string color)
        {
            switch (color)
            {
                case "Red": return "00000000";
                case "White": return "10000001";
                case "Green": return "01000000";
                case "Yellow": return "10000000";
            }
            return "";
        }

        public static void sendLedMsg(string ip_addr, string msg, string color)
        {
            try
            {
                if (!IsPing(ip_addr))
                {
                    return;
                }

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.ReceiveTimeout = 1500;
                sock.SendTimeout = 1500;
                var ep = new IPEndPoint(IPAddress.Parse(ip_addr), 5000);
                sock.Connect(ep);

                int len = 551;
                var frame = new List<byte>();
                //STX
                frame.Add((byte)0x02);
                //length
                frame.Add((byte)(len >> 8));
                frame.Add((byte)len);
                //CMD
                frame.Add((byte)0x30);
                //방번호 0~9
                frame.Add(0);
                //1줄문장 flag
                frame.Add(0);
                //입장효과 0~18
                frame.Add(16);
                //유지시간 1~10
                frame.Add(5);
                //퇴장효과 0~15
                frame.Add(15);
                //속도 0~4
                frame.Add(0);

                //사용안함 10~36
                for (int i = 0; i < 27; i++)
                {
                    frame.Add(0);
                }
                 

                //문자열 256BYTE (37 ~ 292)
                frame.AddRange(Encoding.GetEncoding("euc-kr").GetBytes(msg));

                for (int i = Encoding.GetEncoding("euc-kr").GetByteCount(msg); i < 256; i++)
                {
                    frame.Add(0);
                }

                /* 글자 속성

                //bit 6~7
                //청색flag 0일 경우 0:빨강색, 1:초록색, 2:노랑색, 3:검정색
                //청색flag 1일 경우 0:마젠타, 1.시안, 2:화이트, 3:블루
                //bit 4~5
                //배경색 0:검정색, 1:빨강색, 2:초록색, 3:노랑색
                //bit 2~3
                //글꼴 0:굴림체, 1:가로형컨트롤러:굵은체,세로형 컨트롤러:궁서체, 2:옛체, 3:좁은폭체
                //bit 1 : 깜박임여부
                //bit 0 : 청색flag

                */

                string bitStr = LedColor(color);
                byte bAttr = Convert.ToByte(bitStr, 2);

                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);

                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);

                //속성 256BYTE (293 ~ 548)
                for (int i = 0; i < 240; i++)
                {
                    frame.Add(0);
                }

                int nSum = 0;
                for (int i = 0; i < (len - 12); i++)
                {
                    nSum += frame[i];
                }

                //buf[549] = sum;
                frame.Add((byte)nSum);

                //buf[550] = ETX;
                frame.Add(0x03);


                if (!sock.Connected)
                {
                    return;
                }

                // (3) 서버에 데이타 전송
                int st = sock.Send(frame.ToArray(), SocketFlags.None);

/*                byte[] receiverBuff = new byte[256];
                // (4) 서버에서 데이타 수신
                int n = sock.Receive(receiverBuff, 0, receiverBuff.Length, SocketFlags.None);

                string data = Encoding.UTF8.GetString(receiverBuff, 0, n);*/
                sock.Close();

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsColorLed", "sendLedMsg", ex);
            }
        }


        public static void sendLedMsg2(string ip_addr, string msg1, string color1, string msg2, string color2)
        {
            try
            {
                if (!IsPing(ip_addr))
                {
                    return;
                }
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var ep = new IPEndPoint(IPAddress.Parse(ip_addr), 5000);
                sock.ReceiveTimeout = 1500;
                sock.SendTimeout = 1500;
                sock.Connect(ep);

                int len = 1063;
                var frame = new List<byte>();
                //0:STX
                frame.Add((byte)0x02);
                //1:length
                frame.Add((byte)(len >> 8));
                //2
                frame.Add((byte)len);
                //3:CMD
                frame.Add((byte)0x30);
                //4:방번호 0~9
                frame.Add(0);
                //5: 2줄문장 flag
                frame.Add(1);
                //6~9 사용안함
                for (int i = 0; i < 4; i++)
                {
                    frame.Add(0);
                }

                //10: 상단입장효과 0~17
                frame.Add(16);
                //11: 상단유지시간 1~10
                frame.Add(5);
                //12: 상단퇴장효과 0~15
                frame.Add(15);
                //13: 속도 0~4
                frame.Add(0);

                //14: 하단입장효과 0~17
                frame.Add(16);
                //15: 하단유지시간 1~10
                frame.Add(5);
                //16: 하단퇴장효과 0~15
                frame.Add(15);

                //17: 사용안함

                //사용안함 17~36
                for (int i = 0; i < 20; i++)
                {
                    frame.Add(0);
                }


                //상단문자열 256BYTE (37 ~ 292)
                frame.AddRange(Encoding.GetEncoding("euc-kr").GetBytes(msg1));

                for (int i = Encoding.GetEncoding("euc-kr").GetByteCount(msg1); i < 256; i++)
                {
                    frame.Add(0);
                }


                /* 글자 속성

                //bit 6~7
                //청색flag 0일 경우 0:빨강색, 1:초록색, 2:노랑색, 3:검정색
                //청색flag 1일 경우 0:마젠타, 1.시안, 2:화이트, 3:블루
                //bit 4~5
                //배경색 0:검정색, 1:빨강색, 2:초록색, 3:노랑색
                //bit 2~3
                //글꼴 0:굴림체, 1:가로형컨트롤러:굵은체,세로형 컨트롤러:궁서체, 2:옛체, 3:좁은폭체
                //bit 1 : 깜박임여부
                //bit 0 : 청색flag

                */

                string bitStr = LedColor(color1);
                byte bAttr = Convert.ToByte(bitStr, 2);

                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);

                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);
                frame.Add(bAttr);

                //상단속성 256BYTE (293 ~ 548)
                for (int i = 0; i < 238; i++)
                {
                    frame.Add(0);
                }

                //하단문자열 256BYTE (549 ~ 804)
                frame.AddRange(Encoding.GetEncoding("euc-kr").GetBytes(msg2));

                for (int i = Encoding.GetEncoding("euc-kr").GetByteCount(msg2); i < 256; i++)
                {
                    frame.Add(0);
                }

                bitStr = LedColor(color2);
                byte bAttr2 = Convert.ToByte(bitStr, 2);

                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);

                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);
                frame.Add(bAttr2);

                //하단속성 256BYTE (805 ~ 1060)
                for (int i = 0; i < 238; i++)
                {
                    frame.Add(0);
                }


                int nSum = 0;
                for (int i = 0; i < (len - 2); i++)
                {
                    nSum += frame[i];
                }


                //buf[1061] = sum;

                frame.Add((byte)nSum);

                //buf[1062] = ETX;
                frame.Add(0x03);


                if (!sock.Connected)
                {
                    return;
                }

                // (3) 서버에 데이타 전송
                int st = sock.Send(frame.ToArray(), SocketFlags.None);


/*                byte[] receiverBuff = new byte[256];
                // (4) 서버에서 데이타 수신
                int n = sock.Receive(receiverBuff, 0, receiverBuff.Length, SocketFlags.None);

                string data = Encoding.UTF8.GetString(receiverBuff, 0, n);*/
                sock.Close();


            }
            catch (Exception ex)
            {
                clsLog.logSave("clsColorLed", "sendLedMsg2", ex);
            }
        }
    }
}
