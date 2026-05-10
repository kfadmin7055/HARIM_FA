using Core.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new KoreanGridLocalizer();

            if (IsExistProcess(Process.GetCurrentProcess().ProcessName))
            {
                ShowMessageBox.XtraShowInformation("도징 프로그램이 이미 실행중입니다");
            }
            else
            {

                /*              // 관리자권한으로 재실행
                                if (!IsAdministrator())
                                {
                                    try
                                    {
                                        var pi = new ProcessStartInfo();
                                        pi.UseShellExecute = true;
                                        pi.FileName = Application.ExecutablePath;
                                        pi.WorkingDirectory = Environment.CurrentDirectory;
                                        pi.Verb = "runas";
                                        Process.Start(pi);
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowMessageBox.XtraShowInformation(ex.Message.ToString());
                                    }
                                    return;
                                }
                */
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ko-KR");

                clsCommon.PlantCode = Properties.Settings.Default.Plant;
                clsCommon.PLCOnly = Properties.Settings.Default.PLCOnly;

                Application.Run(new LOGIN());
            }
        }

        private static bool IsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            if (wi == null) return false;

            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }


        static bool IsExistProcess(string processName)
        {
            Process[] process = Process.GetProcesses();
            int cnt = 0;

            foreach (var p in process)
            {
                if (p.ProcessName == processName)
                    cnt++;
                if (cnt > 1)
                    return true;
            }
            return false;
        }
    }
}
