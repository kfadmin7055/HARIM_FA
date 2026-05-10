using Core.Class;
using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HARIM_FA_DOSING
{
    public class clsXgiHandler
    {
        //[DllImport("PLComm.dll")]
        //extern private static IntPtr CreateECP();
        //[DllImport("PLComm.dll")]
        //extern private static int ECP_InitPLC(IntPtr ecp, int plcKind, StringBuilder ip, int sPort, int ePort, int timeout, int flagAddr);
        //[DllImport("PLComm.dll")]
        //extern private static void ECP_ClearAllPLC(IntPtr ecp);
        //[DllImport("PLComm.dll")]
        //extern private static int ECP_ReadWrite(IntPtr ecp, int command, IntPtr block, int addr, int count, char domain);
        //[DllImport("PLComm.dll")]
        //extern private static void DeleteECP(IntPtr ecp);

        // GBG
        [DllImport(@"C:\PLC_Comm\PLComm.dll")]
        extern private static IntPtr CreateECP();
        [DllImport(@"C:\PLC_Comm\PLComm.dll")]
        extern private static int ECP_InitPLC(IntPtr ecp, int plcKind, StringBuilder ip, int sPort, int ePort, int timeout, int flagAddr);
        [DllImport(@"C:\PLC_Comm\PLComm.dll")]
        extern private static void ECP_ClearAllPLC(IntPtr ecp);
        [DllImport(@"C:\PLC_Comm\PLComm.dll")]
        extern private static int ECP_ReadWrite(IntPtr ecp, int command, IntPtr block, int addr, int count, char domain);
        [DllImport(@"C:\PLC_Comm\PLComm.dll")]
        extern private static void DeleteECP(IntPtr ecp);
        // GBG -

        private const int RET_OK = 1;
        private const int RET_NOK = 0;

        //enum PLC_RW_MODE { READ_WORD, READ_BIT, WRITE_WORD, WRITE_BIT };

        private static IntPtr PlcController = IntPtr.Zero;
        private static IntPtr ReadPtr;
        private static int[] ReadBuff;
        private static IntPtr WritePtr;
        private static int[] WriteBuff;
        public static bool IsConnected;


        /// <summary>
        ///  XGI PLC 핸들러 초기화
        /// </summary>
        /// <returns></returns>
        public static void Init()
        {
            try
            {
                ReadBuff = new int[512];
                WriteBuff = new int[512];
                IsConnected = false;

                PlcController = CreateECP();
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Init", e.Message);
                //clsLog.logSave("clsXgiHandler", "Init", e.StackTrace);
                //clsLog.logSave("clsXgiHandler", "Init", e.Source);
            }
        }

        /// <summary>
        ///  XGI PLC 핸들러 종료
        /// </summary>
        /// <returns></returns>
        public static void Deinit()
        {
            try
            {
                ECP_ClearAllPLC(PlcController);
                DeleteECP(PlcController);
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Deinit", e.Message);
            }
        }

        /// <summary>
        ///  PLC 연결
        /// </summary>
        /// <param name="ip"></param>
        /// /// <param name="port"></param>
        /// <returns></returns>
        public static bool Connect(string ip, int port)
        {
            try
            {
                StringBuilder ipAddr = new StringBuilder(ip);
                IsConnected = ECP_InitPLC(PlcController, 3, ipAddr, port, port, 2, 0) == RET_OK;
                return IsConnected;
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Connect", e.Message);
                return false;
            }
        }

        /// <summary>
        ///  PLC Read
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int Read(int mode, int addr, int count, int[] result, string errMSG = "")
        {
            try
            {
                //result = new int[count];
                System.Array.Clear(ReadBuff, 0, ReadBuff.Length);
                ReadPtr = Marshal.UnsafeAddrOfPinnedArrayElement(ReadBuff, 0);
                int ret = ECP_ReadWrite(PlcController, mode, ReadPtr, addr, count, 'W');

                if (ret == RET_OK)
                {
                    Array.Copy(ReadBuff, 0, result, 0, count);
                }
                else
                {
                    clsLog.logSave("clsXgiHandler", "Read", errMSG);
                }

                return ret;
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Read", e.Message);
                return 0;
            }
        }

        /// <summary>
        ///  PLC Read
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int Read(int mode, string addr, int count, int[] result, string errMSG = "")
        {
            try
            {
                string ad = addr.Substring(3);
                int.TryParse(ad, out int iAddr);

                if (iAddr <= 0) return 0;

                System.Array.Clear(ReadBuff, 0, ReadBuff.Length);
                ReadPtr = Marshal.UnsafeAddrOfPinnedArrayElement(ReadBuff, 0);
                int ret = ECP_ReadWrite(PlcController, mode, ReadPtr, iAddr, count, 'W');

                if (ret == RET_OK)
                {
                    Array.Copy(ReadBuff, 0, result, 0, count);
                }
                else
                {
                    clsLog.logSave("clsXgiHandler", "Read", errMSG);
                }
                return ret;
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Read", e.Message);
                return 0;
            }
        }

        /// <summary>
        ///  PLC Write
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static int Write(int mode, int addr, int count, int[] buff, string errMSG = "")
        {
            try
            {
                WritePtr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
                int ret = ECP_ReadWrite(PlcController, mode, WritePtr, addr, count, 'W');

                if (ret != RET_OK)
                {
                    clsLog.logSave("clsXgiHandler", "Write", errMSG);
                }

                return ret;
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Write", e.Message);
                return 0;
            }
        }

        /// <summary>
        ///  PLC Write
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static int Write(int mode, string addr, int count, int[] buff, string errMSG = "")
        {
            try
            {
                string ad = addr.Substring(3);
                int.TryParse(ad, out int iAddr);

                if (iAddr <= 0) return 0;

                WritePtr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
                int ret = ECP_ReadWrite(PlcController, mode, WritePtr, iAddr, count, 'W');

                if (ret != RET_OK)
                {
                    clsLog.logSave("clsXgiHandler", "Write", errMSG);
                }

                return ret;
            }
            catch (Exception e)
            {
                clsLog.logSave("clsXgiHandler", "Write", e.Message);
                return 0;
            }
        }
    }
}
