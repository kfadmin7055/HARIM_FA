using ACTETHERLib;
using Core.Class;
using System;
using System.Text;
using System.Threading;

namespace Core.Extension
{
    public static class KfPLC
    {
        static int errCnt = 0;

        /// <summary>
        /// Q 타입 PLC 단독 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetQDeviceEx(this ActQJ71E71TCP actQJ71E71TCP, string deviceAddress, out int deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                // 읽기 수행
                result = actQJ71E71TCP.GetDevice(deviceAddress, out deviceValue);

                Console.WriteLine($"GetQDeviceEx(errCnt = {errCnt}) : {deviceAddress} | ReturnValue : [0]{deviceValue}", 1);

                clsLog.logSave($"GetQDeviceEx(errCnt = {errCnt}) : {deviceAddress} | ReturnValue : [0]{deviceValue}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 단독 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int SetQDeviceEx(this ActQJ71E71TCP actQJ71E71TCP, string deviceAddress, int deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                // 읽기 수행
                result = actQJ71E71TCP.SetDevice(deviceAddress, deviceValue);

                Console.WriteLine($"SetQDeviceEx(errCnt = {errCnt}) : {deviceAddress} SetValue : {deviceValue}", 1);
                clsLog.logSave($"SetQDeviceEx(errCnt = {errCnt}) : {deviceAddress} SetValue : {deviceValue}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="iAddSize"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        public static int ReadQDeviceAddBlockEx(this ActQJ71E71TCP actQJ71E71TCP, string deviceType, int address, int iAddSize, int AddCount, out int[] deviceValue, string errMSG = "")
        {
            // 읽은 값 반환
            return ReadQDeviceBlockEx(actQJ71E71TCP, deviceType + (address + iAddSize), AddCount, out deviceValue, errMSG = "");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ReadQDeviceBlockEx(this ActQJ71E71TCP actQJ71E71TCP, string deviceAddress, int AddCount, out int[] deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                StringBuilder sb = new StringBuilder();
                deviceValue = new int[15];

                // 읽기 수행
                result = actQJ71E71TCP.ReadDeviceBlock(deviceAddress, AddCount, out deviceValue[0]);

                for (int i = 0; i < deviceValue.Length; i++)
                {
                    sb.Append($"[{i}]:{deviceValue[i]}");

                    if (i < deviceValue.Length - 1)
                        sb.Append(", ");
                }

                Console.WriteLine($"ReadQDeviceBlockEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 1);

                clsLog.logSave($"ReadQDeviceBlockEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="iAddSize"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        public static int WriteQDeviceAddBlockEx(this ActQJ71E71TCP actQJ71E71TCP, string deviceType, int address, int iAddSize, int AddCount, ref int[] deviceValue, string errMSG = "")
        {
            // 읽은 값 반환
            return WriteQDeviceBlockEx(actQJ71E71TCP, deviceType + (address + iAddSize), AddCount, ref deviceValue, errMSG = "");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int WriteQDeviceBlockEx(this ActQJ71E71TCP actQJ71E71TCP, string deviceAddress, int AddCount, ref int[] deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                StringBuilder sb = new StringBuilder();

                // 쓰기 수행
                result = actQJ71E71TCP.WriteDeviceBlock(deviceAddress, AddCount, ref deviceValue[0]);

                for (int i = 0; i < deviceValue.Length; i++)
                {
                    sb.Append($"[{i}]:{deviceValue[i]}");

                    if (i < deviceValue.Length - 1)
                        sb.Append(", ");
                }

                Console.WriteLine($"WriteQDeviceBlockEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 1);

                clsLog.logSave($"WriteQDeviceBlockEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int WriteQDeviceBlock2Ex(this ActQJ71E71TCP actQJ71E71TCP, string deviceAddress, int AddCount, ref short[] deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                StringBuilder sb = new StringBuilder();

                // 쓰기 수행
                result = actQJ71E71TCP.WriteDeviceBlock2(deviceAddress, AddCount, ref deviceValue[0]);

                for (int i = 0; i < deviceValue.Length; i++)
                {
                    sb.Append($"[{i}]:{deviceValue[i]}");

                    if (i < deviceValue.Length - 1)
                        sb.Append(", ");
                }

                Console.WriteLine($"WriteQDeviceBlock2Ex(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 1);

                clsLog.logSave($"WriteQDeviceBlock2Ex(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 단독 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetADeviceEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceAddress, out int deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                // 읽기 수행
                result = actAJ71E71UDP.GetDevice(deviceAddress, out deviceValue);

                Console.WriteLine($"GetADeviceEx(errCnt = {errCnt}) : {deviceAddress} | deviceValue : {deviceValue}", 1);

                clsLog.logSave($"GetADeviceEx(errCnt = {errCnt}) : {deviceAddress} | deviceValue : {deviceValue}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 단독 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int SetADeviceEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceAddress, int deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                // 읽기 수행
                result = actAJ71E71UDP.SetDevice(deviceAddress, deviceValue);

                Console.WriteLine($"SetADeviceEx(errCnt = {errCnt}) : {deviceAddress} SetValue : {deviceValue}", 1);

                clsLog.logSave($"SetADeviceEx(errCnt = {errCnt}) : {deviceAddress} | deviceValue : {deviceValue}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        ///// <summary>
        ///// Q 타입 PLC 블록 통신
        ///// </summary>
        ///// <param name="actAJ71E71UDP"></param>
        ///// <param name="deviceType"></param>
        ///// <param name="address"></param>
        ///// <param name="AddCount"></param>
        ///// <param name="deviceValue"></param>
        ///// <param name="errMSG"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static int ReadDeviceBlockEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceType, int address, int AddCount, out int[] deviceValue, string errMSG = "")
        //{
        //    // 읽은 값 반환
        //    return ReadDeviceBlockEx(actAJ71E71UDP, deviceType + address, AddCount, out deviceValue, errMSG = "");
        //}

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="iAddSize"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        public static int ReadADeviceAddBlockEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceType, int address, int iAddSize, int AddCount, out int[] deviceValue, string errMSG = "")
        {
            // 읽은 값 반환
            return ReadADeviceBlockEx(actAJ71E71UDP, deviceType + (address + iAddSize), AddCount, out deviceValue, errMSG = "");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ReadADeviceBlockEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceAddress, int AddCount, out int[] deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                StringBuilder sb = new StringBuilder();
                deviceValue = new int[15];

                // 읽기 수행
                result = actAJ71E71UDP.ReadDeviceBlock(deviceAddress, AddCount, out deviceValue[0]);

                for (int i = 0; i < deviceValue.Length; i++)
                {
                    sb.Append($"[{i}]:{deviceValue[i]}");

                    if (i < deviceValue.Length - 1)
                        sb.Append(", ");
                }

                Console.WriteLine($"WriteQDeviceBlock2Ex(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 1);

                clsLog.logSave($"SetADeviceEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actQJ71E71TCP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="iAddSize"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        public static int WriteADeviceAddBlockEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceType, int address, int iAddSize, int AddCount, ref int[] deviceValue, string errMSG = "")
        {
            // 읽은 값 반환
            return WriteADeviceBlockEx(actAJ71E71UDP, deviceType + (address + iAddSize), AddCount, ref deviceValue, errMSG = "");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int WriteADeviceBlockEx(this ActAJ71E71UDP actAJ71E71UDP, string deviceAddress, int AddCount, ref int[] deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                StringBuilder sb = new StringBuilder();

                // 쓰기 수행
                result = actAJ71E71UDP.WriteDeviceBlock(deviceAddress, AddCount, ref deviceValue[0]);

                for (int i = 0; i < deviceValue.Length; i++)
                {
                    sb.Append($"[{i}]:{deviceValue[i]}");

                    if (i < deviceValue.Length - 1)
                        sb.Append(", ");
                }

                Console.WriteLine($"WriteADeviceBlockEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 1);

                clsLog.logSave($"WriteADeviceBlockEx(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }

        /// <summary>
        /// Q 타입 PLC 블록 통신
        /// </summary>
        /// <param name="actAJ71E71UDP"></param>
        /// <param name="deviceType"></param>
        /// <param name="address"></param>
        /// <param name="AddCount"></param>
        /// <param name="deviceValue"></param>
        /// <param name="errMSG"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int WriteADeviceBlock2Ex(this ActAJ71E71UDP actAJ71E71UDP, string deviceAddress, int AddCount, ref short[] deviceValue, string errMSG = "")
        {
            int result = 0;

            for (int errCnt = 0; errCnt < 5; errCnt++)
            {
                StringBuilder sb = new StringBuilder();

                // 쓰기 수행
                result = actAJ71E71UDP.WriteDeviceBlock2(deviceAddress, AddCount, ref deviceValue[0]);

                for (int i = 0; i < deviceValue.Length; i++)
                {
                    sb.Append($"[{i}]:{deviceValue[i]}");

                    if (i < deviceValue.Length - 1)
                        sb.Append(", ");
                }

                Console.WriteLine($"WriteQDeviceBlock2Ex(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 1);

                clsLog.logSave($"WriteQDeviceBlock2Ex(errCnt = {errCnt}) : {deviceAddress} | AddCount : {AddCount} | deviceValue : {sb}", 0, "PLC");

                if (result == 0)
                    return result;

                Thread.Sleep(500);
            }

            clsLog.logSave(errMSG, 1);
            throw new Exception($"PLC 읽기 실패: ErrCode={result} ErrorCode={errMSG}");
        }
    }
}
