using CimonPlc.Enums;
using CimonPlc.PlcConnectors;
using CimonPlc.Sockets;
using Core.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Class
{

    public class clsCimonHandler
    {
        private static EthernetConnector Connector;

        public static void Init()
        {
            try
            {
                
            }
            catch (Exception e)
            {
                clsLog.logSave("clsCimonHandler", "Init", e.Message);
            }
        }

        public static void Finish()
        {
            try
            {
                if (Connector != null && Connector.IsConnected)
                {
                    Connector.Disconnect();
                }
            }
            catch (Exception e)
            {
                clsLog.logSave("clsCimonHandler", "Finish", e.Message);
            }
        }

        public static async Task<bool> Connect(string ip, int port)
        {
            try
            {
                Connector = new EthernetConnector(new TcpSocket(ip, port), false);
                Task<bool> returnTaskResult = Plc_conn(Connector);
                bool connResult = await returnTaskResult;

                if (!connResult)
                {
                    //MessageBox.Show("Plc connect 오류!!!");
                    return false;
                }
                else
                {
                    //MessageBox.Show("Plc connect 성공!!!");
                    return true;
                }
            }
            catch (Exception e)
            {
                clsLog.logSave("clsCimonHandler", "Connect", e.Message);
                return false;
            }
        }

        public static async Task<bool> Plc_conn(EthernetConnector conn)
        {
            try
            {
                var result = ConnectionStatus.DisConnected;

                result = await Task.Run(() =>
                {
                    return conn.Connect(300, 300, 3000);

                });

                if (!ConnectionStatus.Connected.Equals(result))
                {
                    //clsLog.logSave("clsPlcConnManager", "Plc_conn", "연결실패 / 에러내용 : " + result.ToString());
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //clsLog.logSave("clsPlcConnManager", "Plc_conn", ex);
                return false;
            }
        }

        public static async Task<bool> TryReadWord(string addr, int length, int[] data)
        {
            Task<int[]> returnTaskReadResult = clsCimonHandler.ReadWord(addr, length);
            data = await returnTaskReadResult;
            return data != null && data.Length > 0;
        }

        public static async Task<int[]> ReadWord(string addr, int length)
        {
            try
            {
                if (!Connector.IsConnected)
                {
                    await Plc_conn(Connector);
                }

                addr = addr.Replace("D", "");

                if (addr.Length > 6)
                {
                    addr = addr.Substring(addr.Length - 6);
                }

                var (responseCode, data) = await Connector.ReadWordAsync(MemoryType.D, addr, length);

                if (ResponseCode.Success.Equals(responseCode))
                {
                    return data;
                }
                else
                {
                    //clsLog.logSave("clsPlcConnManager", "ReadWord", "읽기실패 : " + responseCode.ToString()); 
                    data = new int[length];
                    Array.Clear(data, 0, length);
                    return data;
                }
            }
            catch (Exception e)
            {
                int[] data = new int[length];
                Array.Clear(data, 0, length);
                return data;
            }
        }

        public static async Task<bool> TryWriteWord(string addr, int[] data)
        {
            Task<bool> returnTaskSendResult = WriteWord(addr, data);
            return await returnTaskSendResult;
        }

        public static async Task<bool> WriteWord(string addr, int[] data)
        {
            try
            {
                if (!Connector.IsConnected)
                {
                    await Plc_conn(Connector);
                }

                addr = addr.Replace("D", "");

                if (addr.Length > 6)
                {
                    addr = addr.Substring(addr.Length - 6);
                }

                var responseCode = await Connector.WriteWordAsync(MemoryType.D, addr, data);

                if (ResponseCode.Success.Equals(responseCode))
                {
                    return true;
                }
                else
                {
                    //clsLog.logSave("clsPlcConnManager", "WriteWord", "쓰기실패 : " + addr + " / " + responseCode.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                //clsLog.logSave("clsPlcConnManager", "WriteWord", addr + " / " + ex);
                return false;
            }
        }

        //public static async Task<string> WritePlc(string address ,int val)
        //{
        //    EthernetConnector connector = new EthernetConnector(new TcpSocket(clsCommon.plc_dosing_ip, 10260), false);

        //    try
        //    {
        //        Task<bool> returnTaskResult = clsPlcConnManager.Plc_conn(connector);
        //        bool connResult = await returnTaskResult;

        //        if (!connResult)
        //        {
        //            //clsLog.logSave("clsPlcConnManager", "WritePlc", "PLC연결실패 ");
        //            return "PLC 연결에 실패하였습니다";

        //        }

        //        address = address.Replace("D", "");

        //        int[] plcVal = { val };
        //        Task<bool> returnTaskSendResult = clsPlcConnManager.WriteWord(connector, address, plcVal);
        //        bool sendResult = await returnTaskSendResult;
        //        if (!sendResult)
        //        {
        //            //clsLog.logSave("clsPlcConnManager", "WritePlc", "PLC전송실패 / D90 ");
        //            return "PLC 전송에 실패하였습니다 / D151";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //clsLog.logSave("clsPlcConnManager", "WritePlc", ex);
        //    }
        //    finally
        //    {
        //        if (connector.IsConnected)
        //        {
        //            connector.Disconnect();
        //        }
        //    }

        //    return "OK";
        //}

    }
}
