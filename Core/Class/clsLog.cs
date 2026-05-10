using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace Core.Class
{
    public class clsLog
    {
        public static string fPATH = @"C:\HarimFA\Log\";
        public static string fLogFileName = "LOG";
        public static int fSaveLevel = 0;	//저장할 로그의 iLevel설정
        public static bool fEnabled = true;

        private static readonly object logLock = new object();

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]   //API
        private static extern bool BitBlt(
            IntPtr hdcDest, // handle to destination DC 
            int nXDest,  // x-coord of destination upper-left corner
            int nYDest,  // y-coord of destination upper-left corner
            int nWidth,  // width of destination rectangle
            int nHeight, // height of destination rectangle
            IntPtr hdcSrc,  // handle to source DC
            int nXSrc,   // x-coordinate of source upper-left corner
            int nYSrc,   // y-coordinate of source upper-left corner
            System.Int32 dwRop  // raster operation code
            );


        //This structure shall be used to keep the size of the screen.
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

        public class PlatformInvokeUSER32
        {
            #region Class Variables
            public const int SM_CXSCREEN = 0;
            public const int SM_CYSCREEN = 1;
            #endregion

            #region Class Functions
            [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", EntryPoint = "GetDC")]
            public static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
            public static extern int GetSystemMetrics(int abc);

            [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);

            [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

            #endregion

        }

        public class PlatformInvokeGDI32
        {
            #region Class Variables
            public const int SRCCOPY = 13369376;
            #endregion

            #region Class Functions
            [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
            public static extern IntPtr DeleteDC(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
            public static extern IntPtr DeleteObject(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
            public static extern bool BitBlt(IntPtr hdcDest, int xDest,
                int yDest, int wDest, int hDest, IntPtr hdcSource,
                int xSrc, int ySrc, int RasterOp);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
                int nWidth, int nHeight);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
            #endregion
        }


        #region sDirectory : logSave Directory 속성
        public string sDirectory
        {
            get
            {
                return fPATH;
            }
            set
            {
                fPATH = value;
            }
        }
        #endregion

        #region sLogFileName : 속성
        public string sLogFileName
        {
            get
            {
                return fLogFileName;
            }
            set
            {
                fLogFileName = value;
            }
        }
        #endregion

        #region iSaveLogLevel : iSaveLogLevel 속성
        public int iSaveLogLevel
        {
            get
            {
                return fSaveLevel;
            }
            set
            {
                fSaveLevel = value;
            }

        }
        #endregion

        #region Enableded : logSave Enabled 상태설정
        public bool Enableded
        {
            get
            {
                return fEnabled;
            }
            set
            {
                fEnabled = value;
            }

        }
        #endregion

        #region fileExistCheck() : 로그파일이 존재하지 않으면 생성
        /// <summary>
        /// 로그파일이 존재하지 않으면 생성한다.
        /// sDirectory가 존재하지 않으면 생성한다.
        /// </summary>
        /// <returns></returns>
        private static string fileExistCheck(string sLogType)
        {
            string sFileName;
            string sDir;
            string sYYYY;
            string sMM;
            string sDD;

            //로그 sDirectory를 만들기 위하여 현재일자를 구한다.
            sYYYY = DateTime.Today.Year.ToString();
            sMM = DateTime.Today.Month.ToString();
            sDD = DateTime.Today.Day.ToString();
            sMM = sMM.PadLeft(2, '0');
            sDD = sDD.PadLeft(2, '0');

            //현재년월 sDirectory 만들기
            sDir = String.Concat(fPATH, "\\");		//현재년월 sDirectory
            sDir = String.Concat(sDir, sYYYY);
            sDir = String.Concat(sDir, sMM);

            //Path를 포함한 sLogFileName 만들기
            sFileName = String.Concat(sDir, "\\");
            sFileName = String.Concat(sFileName, fLogFileName);	//sLogFileName
            sFileName = String.Concat(sFileName, sDD);
            sFileName = String.Concat(sFileName, $"[{sLogType}]");
            sFileName = String.Concat(sFileName, ".TXT");

            //로그파일이 존재하면 Return
            FileInfo f = new FileInfo(sFileName);
            if (f.Exists) return sFileName;

            DirectoryInfo D = new DirectoryInfo(sDir);
            //sDirectory가 존재하지 않으면 생성한다.
            if (!D.Exists) D.Create();

            //로그파일을 생성한다.
            FileStream fs = f.OpenWrite();
            fs.Close();

            //타이틀 Write하기
            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);
                sw.WriteLine("LOG START [&레파지토리 이름, $:레파지토리 DATA, *:일반LOG, #:에러로그]");
                sw.Close();
            }

            return sFileName;
        }
        #endregion

        #region ScreenCapture(Form frm) : 화면 스크린샷 파일 생성
        public static Bitmap ScreenCapture(Form frm)// Capturing the Screen Image in C# By Agha Ali Raza
        {
            //In size variable we shall keep the size of the screen.
            SIZE size;

            //Variable to keep the handle to bitmap.
            IntPtr hBitmap;

            //Here we get the handle to the desktop device context.
            IntPtr hDC = PlatformInvokeUSER32.GetWindowDC(frm.Handle);//PlatformInvokeUSER32.GetDC(PlatformInvokeUSER32.GetDesktopWindow());

            //Here we make a compatible device context in memory for screen device context.
            IntPtr hMemDC = PlatformInvokeGDI32.CreateCompatibleDC(hDC);

            //We pass SM_CXSCREEN constant to GetSystemMetrics to get the X coordinates of the screen.
            size.cx = frm.Bounds.Width;//PlatformInvokeUSER32.GetSystemMetrics(PlatformInvokeUSER32.SM_CXSCREEN);

            //We pass SM_CYSCREEN constant to GetSystemMetrics to get the Y coordinates of the screen.
            size.cy = frm.Bounds.Height;//PlatformInvokeUSER32.GetSystemMetrics(PlatformInvokeUSER32.SM_CYSCREEN);

            //We create a compatible bitmap of the screen size and using the screen device context.
            hBitmap = PlatformInvokeGDI32.CreateCompatibleBitmap(hDC, size.cx, size.cy);//size.cx, size.cy);

            //As hBitmap is IntPtr, we cannot check it against null.
            //For this purpose, IntPtr.Zero is used.
            if (hBitmap != IntPtr.Zero)
            {
                //Here we select the compatible bitmap in the memeory device
                //context and keep the refrence to the old bitmap.
                IntPtr hOld = (IntPtr)PlatformInvokeGDI32.SelectObject(hMemDC, hBitmap);

                //We copy the Bitmap to the memory device context.
                //PlatformInvokeGDI32.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, PlatformInvokeGDI32.SRCCOPY);
                PlatformInvokeGDI32.BitBlt(hMemDC, 0, 0, SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height, hDC, 0, 0, PlatformInvokeGDI32.SRCCOPY);


                //We select the old bitmap back to the memory device context.
                PlatformInvokeGDI32.SelectObject(hMemDC, hOld);

                //We delete the memory device context.
                PlatformInvokeGDI32.DeleteDC(hMemDC);

                //We release the screen device context.
                PlatformInvokeUSER32.ReleaseDC(frm.Handle, hDC);//(PlatformInvokeUSER32.GetDesktopWindow(), hDC);

                //Image is created by Image bitmap handle and stored in local variable.
                Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);

                //Release the memory to avoid memory leaks.
                PlatformInvokeGDI32.DeleteObject(hBitmap);

                //This statement runs the garbage collector manually.
                GC.Collect();

                //Return the bitmap 
                return bmp;
            }
            //If hBitmap is null, retun null.
            return null;
        }
        #endregion

        #region logSave(char RUN_MODE, string sRepasitoryName, DataSet INDATA) : Reository Log
        /// <summary>
        /// Reository Log
        /// </summary>
        /// <param name="RUN_MODE"></param>
        /// <param name="sRepasitoryName"></param>
        /// <param name="INDATA"></param>
        public static void logSave(char RUN_MODE, string sRepasitoryName, DataSet INDATA)
        {
            string sFileName;
            string sToday;
            string sMessage;

            //사용안함 상태이면 Return;
            if (fEnabled == false) return;

            //존재하는 로그파일인지 검사
            sFileName = fileExistCheck("");
            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);

                //logSave
                sToday = DateTime.Now.ToString("yyyyMMddHHmmss");

                sw.WriteLine("&" + RUN_MODE + "|" + sToday + "|" + sRepasitoryName);

                for (int iRow = 0; iRow < INDATA.Tables[0].Rows.Count; iRow++)
                {
                    sMessage = INDATA.Tables[0].Rows[iRow][0].ToString();
                    for (int iCol = 1; iCol < INDATA.Tables[0].Columns.Count; iCol++)
                        sMessage += "|" + INDATA.Tables[0].Rows[iRow][iCol].ToString();
                    sw.WriteLine("$" + RUN_MODE + "|" + sMessage);
                }

                sw.Close();
            }
        }
        #endregion

        #region logSave(string sMsg, int iLevel) : 단순 메시지 로그, 예외가 없을 경우
        /// <summary>
        /// 단순 메시지 로그, 예외가 없을 경우
        /// </summary>
        /// <param name="sMsg"></param>
        /// <param name="iLevel"></param>
        /// /// <param name="sLogType"></param>
        public static void logSave(string sMsg, int iLevel, string sLogType = "SQL", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            string sFileName;
            string sToday;
            string sLEVEL;

            //사용안함 상태이면 Return;
            if (fEnabled == false) return;

            //설정된 저장iLevel보다 높으면...
            if (fSaveLevel > 0 && iLevel > fSaveLevel) return;

            //존재하는 로그파일인지 검사
            sFileName = fileExistCheck(sLogType);

            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);

                //logSave
                sToday = DateTime.Now.ToString("yyyyMMddHHmmss");
                sLEVEL = iLevel.ToString();

                sw.WriteLine("*" + sToday + "|" + sLEVEL + "|" + sMsg + $"  ({System.IO.Path.GetFileName(file)}:{line}, 함수:{caller})");
                sw.Close();
            }
        }
        #endregion

        #region logSave(System.Windows.Forms.Form frm, string FunctionName, Exception ex) : 예외가 있을 경우
        /// <summary>
        /// 예외가 있을 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="FunctionName"></param>
        /// <param name="ex"></param>
        public static void logSave(System.Windows.Forms.Form frm, string FunctionName, Exception ex)
        {
            string sFileName;
            string sToday;

            if (frm == null)
                return;

            //사용안함 상태이면 Return;
            if (fEnabled == false) return;

            //존재하는 로그파일인지 검사
            sFileName = fileExistCheck("");
            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);

                //logSave
                sToday = DateTime.Now.ToString("yyyyMMdd HHmmss : ");

                if (ex != null)
                {
                    sw.WriteLine("#" + sToday + "|" + frm.Name + "|" + FunctionName + "|" + ex.Message);
                }
                else
                {
                    sw.WriteLine("%" + sToday + "|" + frm.Name + "|" + FunctionName);
                }
                sw.Close();
            }
        }
        #endregion

        #region logSave(string sClassName, string FunctionName, Exception ex) : 예외가 있을 경우
        /// <summary>
        /// 예외가 있을 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="FunctionName"></param>
        /// <param name="ex"></param>
        public static void logSave(string sClassName, string FunctionName, Exception ex)
        {
            string sFileName;
            string sToday;

            //사용안함 상태이면 Return;
            if (fEnabled == false) return;

            //존재하는 로그파일인지 검사
            sFileName = fileExistCheck("Exception");
            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);

                //logSave
                sToday = DateTime.Now.ToString("yyyyMMdd HHmmss : ");

                if (ex != null)
                {
                    sw.WriteLine("#" + sToday + "|" + sClassName + "|" + FunctionName + "|" + ex.Message);
                }
                else
                {
                    sw.WriteLine("%" + sToday + "|" + sClassName + "|" + FunctionName);
                }
                sw.Close();
            }
        }
        #endregion

        #region logSave(string Form_Name, string FunctionName, string SQL) : SQL 실행 에러
        /// <summary>
        /// 예외가 있을 경우
        /// </summary>
        /// <param name="Form_Name"></param>
        /// <param name="FunctionName"></param>
        /// <param name="SQL"></param>
        public static void logSave(string Form_Name, string FunctionName, string SQL)
        {
            string sFileName;
            string sToday;

            //사용안함 상태이면 Return;
            if (fEnabled == false) return;

            //존재하는 로그파일인지 검사
            sFileName = fileExistCheck("");
            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);

                //logSave
                sToday = DateTime.Now.ToString("yyyyMMdd HHmmss : ");


                sw.WriteLine("SQL#" + sToday + "|" + Form_Name + "|" + FunctionName + "| SQL : " + SQL);

                sw.Close();
            }
        }
        #endregion

        public static void logSave(System.Windows.Forms.Form frm, string FunctionName, string msg)
        {
            string sFileName;
            string sToday;

            //사용안함 상태이면 Return;
            if (fEnabled == false) return;

            //존재하는 로그파일인지 검사
            sFileName = fileExistCheck("");
            lock (logLock)
            {
                StreamWriter sw = File.AppendText(sFileName);

                //logSave
                sToday = DateTime.Now.ToString("yyyyMMdd HHmmss : ");

                sw.WriteLine("#" + sToday + "|" + frm.Name + "|" + FunctionName + "|" + msg);

                sw.Close();
            }
        }
    }
}
