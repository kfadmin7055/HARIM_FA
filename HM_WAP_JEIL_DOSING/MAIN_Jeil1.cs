using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Core.Class;
using Core.Extension;
using ACTETHERLib;
using System.Linq;
using Core.Enum;
using System.Deployment.Application;
using DevExpress.XtraBars;
using HARIM_FA_DOSING.Class;
using System.Threading;

namespace HARIM_FA_DOSING
{
    public partial class MAIN_Jeil1 : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>        /// 실행 경로y
        /// </summary>
        private string _strExecutablePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public static string MainPlcConnChk = string.Empty;
        public static string SubPlcConnChk = string.Empty;
        public static string BarcodeConnChk = string.Empty;
        private static string vLCode = string.Empty;

        private static string vWorkDate = string.Empty;
        private static string vWorkNum = string.Empty;
        private static string vBatchNum = string.Empty;
        private static string vResourceNo = string.Empty;
        private static string vResourceName = string.Empty;

        public static ActAJ71E71UDP aPlc1 = null;
        public static ActAJ71E71UDP aPlc2 = null;
        public static ActQJ71E71TCP qPlc1 = null;
        public static ActQJ71E71TCP qPlc2 = null;

        private int iMainPlcCon = 0;
        private int iSubPlcCon = 0;

        private static string SQL = string.Empty;
        private static string workDEV = string.Empty;
        private static int processGubun = 0;

        public static string vPLCAddress;
        public static int vPLCUnit;
        public static int vPLCDataCount;

        private static bool vBatchComplete = false;
        private static bool vMixComplete = false;
        private static bool vWorkComplete = false;
        private static int vCompleteCnt = 0;
        private static bool vWorkMode = false;
        private static string vProcessKey = string.Empty;

        private string sPlantCode = "PJ04";

        // GBG
        // for Cimon PLC
        private static int[] Cimon_Job_Data = new int[150];
        private static int[] Cimon_Result_Data = new int[500];
        // GBG
        private static int[] Cimon_Result_Data_Scale = new int[100];
        //private static int[] Cimon_Result_Data_2 = new int[100];
        //private static int[] Cimon_Result_Data_3 = new int[100];
        //private static int[] Cimon_Result_Data_4 = new int[100];
        //private static int[] Cimon_Result_Data_5 = new int[100];
        // GBG -

        const string CIMON_PLC_JOB_ADDR = "D30000";
        const int CIMON_PLC_JOB_READE_SIZE = 120;
        const int CIMON_PLC_RESULT_READ_SIZE = 500;

        const int WORK_YYYY = 10;
        const int WORK_MMDD = 11;
        const int WORK_SEQ = 12;
        const int BATCH_PV = 22;
        const int IS_WORKABLE = 50;
        const int BATCH_COMPLETE = 51;
        const int JOB_COMPLETE = 52;
        const int IS_BIN_CHANGABLE = 53;
        const int IS_BIN_HOLDABLE = 54;
        const int MIXING_COMPLETE = 55;
        const int MIXING_WORK_YYYY = 100;
        const int MIXING_WORK_MMDD = 101;
        const int MIXING_WORK_SEQ = 102;
        const int MIXING_TO_BIN1 = 104;
        const int MIXING_BATCH = 112;
        const int MIXING_TIME_PV = 113;

        bool isVisible = true;

        static int vPLC_Location = 0;

        public static string sErrMsg = string.Empty;

        public static bool mainTimer = true;

        public static bool wStatus = true;

        public static string fPATH = string.Empty;

        // GBG -

        #region 실행 경로 - ExecutablePath

        /// <summary>
        /// 실행 경로
        /// </summary>
        public string ExecutablePath
        {
            get
            {
                return _strExecutablePath;
            }
        }

        #endregion

        public MAIN_Jeil1()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = clsCommon.ProgramName;

            clsDevexpressUtil.ButtonSkinChange(btn_topRemote, "Blueprint");
            clsDevexpressUtil.ButtonSkinChange(btn_logout, "Metropolis");
            clsDevexpressUtil.ButtonSkinChange(btn_passChange, "Metropolis");


            //화면메뉴초기화
            foreach (AccordionControlElement item in acMenu.Elements)
            {
                foreach (AccordionControlElement subitem in item.Elements)
                {
                    subitem.Visible = false;
                }

                item.Visible = false;
            }
        }

        private void LoginInfo_Reg(string name)
        {
            labelControl_LoginInfo.Text = string.Format("<size=12><b>{0}</b></size><size=11> 님이 로그인하셨습니다</size>", name);

            //화면권한 설정
            Screen_attr();
        }

        private void Screen_attr()
        {
            try
            {
                if (clsCommon._strUserType == "admin")
                {
                    //화면메뉴초기화
                    foreach (AccordionControlElement item in acMenu.Elements)
                    {
                        foreach (AccordionControlElement subitem in item.Elements)
                        {
                            subitem.Visible = true;
                        }
                        item.Visible = true;
                    }

                    return;
                }

                //화면메뉴초기화
                foreach (AccordionControlElement item in acMenu.Elements)
                {
                    foreach (AccordionControlElement subitem in item.Elements)
                    {
                        subitem.Visible = false;
                    }
                    item.Visible = false;
                }


                //화면권한 설정 (Read : 메뉴표시)
                DataSet authDs = clsSql.GetAuthDataSet();
                DataTable dt = authDs.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (AccordionControlElement item in acMenu.Elements)
                    {
                        int visit_cnt = 0;
                        foreach (AccordionControlElement subitem in item.Elements)
                        {
                            if (subitem.Tag != null)
                            {

                                if (subitem.Tag.ToString().Trim().Equals(dr["FORM_NAME"].ToString().Trim()))
                                {
                                    if (dr["READ_ATT"].ToString().Trim() == "Y")
                                    {
                                        subitem.Visible = true;
                                        visit_cnt = visit_cnt + 1;
                                    }
                                    else
                                    {
                                        subitem.Visible = false;
                                    }
                                    break;
                                }
                            }
                        }

                        if (visit_cnt > 0)
                        {
                            item.Visible = true;
                        }
                    }
                }

                authDs.Dispose();
                accordionControlElement_menu6.Visible = true;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Screen_attr", ex);
                ShowMessageBox.XtraShowWarning("화면권한처리를 하는 도중 에러가 발생했습니다");
            }
        }

        public static string GetInternalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        /// <summary>
        /// 메인폼 로드 이벤트
        /// </summary>
        private void MAIN_Load(object sender, EventArgs e)
        {
            try
            {
                /*                
                if (clsCommon._strPlcConnYn != "Y")
                {
                    string externalip = new WebClient().DownloadString("http://ipinfo.io/ip").Trim();

                    if (String.IsNullOrWhiteSpace(externalip))
                    {d
                        externalip = GetInternalIPAddress();//null경우 Get Internal IP를 가져오게 한다.
                    }

                    if (externalip == "172.43.218.12")
                    {
                        Properties.Settings.Default.Plc_Yn = "Y";
                        Properties.Settings.Default.Save();
                        clsCommon._strPlcConnYn = "Y";
                    }

                }
                */

                //if (clsCommon._strUserId == "kfirst")
                //{
                //    accordionControlElement1_emp.Text = "사용자 관리";
                //    accordionControlElement1_scAttr.Text = "사용자별 접근권한";

                //    accordionControlElement2_ingred.Text = "원부재료관리";
                //    accordionControlElement2_Mix.Text = "제품 및 배합비관리";

                //    accordionControlElement4_DosingResult.Text = "배치별 작업일지";

                //    accordionControlElement5_bulkOrder.Text = "벌크 작업/상차관리";
                //    accordionControlElement5_bagOrder.Text = "타이콘 작업/상차관리";
                //    accordionControlElement5_weightIn.Text = "입차내역관리";
                //    accordionControlElement5_weightOut.Text = "출차내역관리";
                //    accordionControlElement5_weightEtc.Text = "계근기타내역관리";
                //    accordionControlElement5_expCar.Text = "계근예외차량관리";
                //    accordionControlElement5_wapCustMapping.Text = "공급사별 납품원료관리";
                //}

                if (clsCommon.PlantCode == "P101" || clsCommon.PlantCode == "P102")
                    clsCommon.SetLogo(picLogo, "하림");
                else if (clsCommon.PlantCode == "PJ01" || clsCommon.PlantCode == "PJ02" || clsCommon.PlantCode == "PJ04" || clsCommon.PlantCode == "PJ05")
                    clsCommon.SetLogo(picLogo, "제일");
                else if (clsCommon.PlantCode == "P201")
                    clsCommon.SetLogo(picLogo, "올품");

                fPATH = $@"C:\HarimFA\Bag\{clsCommon.PlantCode}\{DateTime.Now.ToString("yyyyMM")}";

                ShowSplahScreenManager("로그인 유저를 설정하는중입니다");

                LoginInfo_Reg(clsCommon.UserName);

                //DB, PLC 접속상태 감시
                conn_witch_timer.Interval = 2000;
                conn_witch_timer.Enabled = true;

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "MAIN_Load", ex);
                ShowMessageBox.XtraShowWarning("화면을 불러오는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager_loding.IsSplashFormVisible)
                {
                    splashScreenManager_loding.CloseWaitForm();
                }
            }
        }

        /// <summary>
        /// MDI 중복창 검사
        /// </summary>
        /// <param name="formname">폼이름</param>
        /// <returns>중복창 여부</returns>
        public bool frmDupChk(string formname)
        {
            foreach (Form openForm in this.MdiChildren)
            {
                if (formname.Equals(openForm.Name))
                {
                    openForm.Activate();
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 폼 MDI Show 이벤트
        /// </summary>
        /// <param name="argFrm">폼이름</param>
        public void FormDisp(Form argFrm)
        {
            if (frmDupChk(argFrm.Name) == true)
            {
                argFrm.MdiParent = this;

                xtraTabbedMdiManager.BeginUpdate();
                argFrm.Show();
                argFrm.Update();
                xtraTabbedMdiManager.EndUpdate();
                bar_bottom.Visible = false;
                bar_bottom.Visible = true;
            }
            else
            {
                argFrm.Dispose();
            }
        }

        /// <summary>
        /// accordion 메뉴 클릭 이벤트
        /// </summary>
        private void menuClick(object sender, EventArgs e)
        {
            try
            {
                AccordionControlElement accControl = sender as AccordionControlElement;
                if (accControl.Tag == null)
                {
                    return;
                }

                string nameSpace = "HARIM_FA_DOSING";
                Assembly cuasm = Assembly.GetExecutingAssembly();
                Form frm = (Form)cuasm.CreateInstance(string.Format("{0}.{1}", nameSpace, accControl.Tag.ToString()));
                frm.Text = accControl.Text;

                ShowSplahScreenManager("페이지 로딩중입니다");

                FormDisp(frm);

                SetMenuPath(accControl.Text, accControl.Tag.ToString(), "");
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog(ex.Message.ToString(), this, "menuClick()", ex);
            }
            finally
            {
                if (splashScreenManager_loding.IsSplashFormVisible)
                {
                    splashScreenManager_loding.CloseWaitForm();
                }
            }
        }

        /// <summary>
        /// accordion 메뉴 도징,대용유 클릭 이벤트 (도징화면 공통 분할)
        /// </summary>
        private void MultiMenuClick(object sender, EventArgs e)
        {
            string[] arrResult = null;

            try
            {
                AccordionControlElement accControl = sender as AccordionControlElement;

                string form_name = string.Empty;
                Form Form = null;

                form_name = accControl.Tag.ToString();

                arrResult = accControl.Hint.Split(',');

                DataTable dt = clsCommon.GetProcessLine(clsCommon.PlantCode, arrResult[0], arrResult[1]);

                if (dt != null && dt.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowWarning("현재 메뉴에 접근할수 없습니다. 관리자에게 문의 바랍니다.");
                    return;
                }

                if (dt.Rows[0]["PROCESS_TYPE"]?.ToString() == clsCommon.GetProcessTypeCode("배합") || dt.Rows[0]["PROCESS_TYPE"]?.ToString() == clsCommon.GetProcessTypeCode("갓돈배합"))
                    Form = new frm_Dosing(dt.Rows[0]["PLANT_CODE"]?.ToString(), dt.Rows[0]["PROCESS_KEY"]?.ToString(), dt.Rows[0]["L_CODE"]?.ToString(), form_name, arrResult[0]);
                else if (dt.Rows[0]["PROCESS_TYPE"]?.ToString() == clsCommon.GetProcessTypeCode("포장"))
                    Form = new frm_Pack(dt.Rows[0]["PLANT_CODE"]?.ToString(), dt.Rows[0]["PROCESS_KEY"]?.ToString(), dt.Rows[0]["L_CODE"]?.ToString(), form_name, arrResult[0]);
                else if (dt.Rows[0]["PROCESS_TYPE"]?.ToString() == clsCommon.GetProcessTypeCode("펠렛"))
                    //if (Debugger.IsAttached)
                    //    Form = new frm_Pellet2(dt.Rows[0]["PLANT_CODE"]?.ToString(), dt.Rows[0]["PROCESS_KEY"]?.ToString(), dt.Rows[0]["L_CODE"]?.ToString(), form_name, arrResult[0]);
                    //else
                        Form = new frm_Pellet(dt.Rows[0]["PLANT_CODE"]?.ToString(), dt.Rows[0]["PROCESS_KEY"]?.ToString(), dt.Rows[0]["L_CODE"]?.ToString(), form_name, arrResult[0]);
                else
                {
                    string nameSpace = "HARIM_FA_DOSING";
                    Assembly cuasm = Assembly.GetExecutingAssembly();
                    Form = (Form)cuasm.CreateInstance(string.Format("{0}.{1}", nameSpace, form_name));
                }

                Form.Name = form_name;
                Form.Text = accControl.Text;

                ShowSplahScreenManager("페이지 로딩중입니다");

                if (frmDupChk(form_name) == true)
                {
                    Form.MdiParent = this;

                    xtraTabbedMdiManager.BeginUpdate();
                    Form.Show();
                    Form.Update();
                    xtraTabbedMdiManager.EndUpdate();

                    bar_bottom.Visible = false;
                    bar_bottom.Visible = true;
                }

                SetMenuPath(accControl.Text, accControl.Tag.ToString(), accControl.Hint);
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog(ex.Message.ToString(), this, "menuDosingClick()", ex);
            }
            finally
            {
                if (splashScreenManager_loding.IsSplashFormVisible)
                {
                    splashScreenManager_loding.CloseWaitForm();
                }
            }
        }

        ///// <summary>
        ///// accordion 메뉴 펠렛 클릭 이벤트 (도징화면 공통 분할)
        ///// </summary>
        //private void menuPelletClick(object sender, EventArgs e)
        //{
        //    string[] arrResult = null;

        //    try
        //    {
        //        AccordionControlElement accControl = sender as AccordionControlElement;

        //        string form_name = string.Empty;
        //        Form Form = null;

        //        form_name = accControl.Tag.ToString();

        //        arrResult = accControl.Hint.Split(',');

        //        if (Debugger.IsAttached && clsCommon.PlantCode == "Z999")
        //            clsCommon.PlantCode = "PJ01";

        //        DataTable dt = clsCommon.GetProcessLine(clsCommon.PlantCode, arrResult[0], arrResult[1]);

        //        if (dt != null && dt.Rows.Count == 0)
        //        {
        //            ShowMessageBox.XtraShowWarning("현재 메뉴에 접근할수 없습니다. 관리자에게 문의 바랍니다.");
        //            return;
        //        }

        //        Form = new frm_Dosing(dt.Rows[0]["PLANT_CODE"]?.ToString(), dt.Rows[0]["PROCESS_KEY"]?.ToString(), dt.Rows[0]["L_CODE"]?.ToString(), form_name);

        //        Form.Name = form_name;
        //        Form.Text = accControl.Text;

        //        splashScreenManager_loding.ShowWaitForm();
        //        splashScreenManager_loding.SetWaitFormCaption("페이지 로딩중입니다");

        //        if (frmDupChk(form_name) == true)
        //        {
        //            Form.MdiParent = this;

        //            xtraTabbedMdiManager.BeginUpdate();
        //            Form.Show();
        //            Form.Update();
        //            xtraTabbedMdiManager.EndUpdate();

        //            bar_bottom.Visible = false;
        //            bar_bottom.Visible = true;
        //        }

        //        SetMenuPath(accControl.Text, accControl.Tag.ToString(), accControl.Hint);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessageBox.XtraShowErrorLog(ex.Message.ToString(), this, "menuDosingClick()", ex);
        //    }
        //    finally
        //    {
        //        if (splashScreenManager_loding.IsSplashFormVisible)
        //        {
        //            splashScreenManager_loding.CloseWaitForm();
        //        }
        //    }
        //}

        ///// <summary>
        ///// accordion 메뉴 포장, 소포장 클릭 이벤트 (도징화면 공통 분할)
        ///// </summary>
        //private void menuPackClick(object sender, EventArgs e)
        //{
        //    string[] arrResult = null;

        //    try
        //    {
        //        AccordionControlElement accControl = sender as AccordionControlElement;

        //        string form_name = string.Empty;
        //        Form packForm = null;

        //        form_name = accControl.Tag.ToString();

        //        arrResult = accControl.Hint.Split(',');

        //        if (Debugger.IsAttached && clsCommon.PlantCode == "Z999")
        //            clsCommon.PlantCode = "PJ01";

        //        DataTable dt = clsCommon.GetProcessLine(clsCommon.PlantCode, arrResult[0], arrResult[1]);

        //        packForm = new frm_Pack(dt.Rows[0]["PLANT_CODE"]?.ToString(), dt.Rows[0]["PROCESS_KEY"]?.ToString(), dt.Rows[0]["L_CODE"]?.ToString());

        //        packForm.Name = form_name;
        //        packForm.Text = accControl.Text;

        //        splashScreenManager_loding.ShowWaitForm();
        //        splashScreenManager_loding.SetWaitFormCaption("페이지 로딩중입니다");

        //        if (frmDupChk(form_name) == true)
        //        {
        //            packForm.MdiParent = this;

        //            xtraTabbedMdiManager.BeginUpdate();
        //            packForm.Show();
        //            packForm.Update();
        //            xtraTabbedMdiManager.EndUpdate();

        //            bar_bottom.Visible = false;
        //            bar_bottom.Visible = true;
        //        }

        //        SetMenuPath(accControl.Text, accControl.Tag.ToString(), accControl.Hint);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessageBox.XtraShowErrorLog(ex.Message.ToString(), this, "menuDosingClick()", ex);
        //    }
        //    finally
        //    {
        //        if (splashScreenManager_loding.IsSplashFormVisible)
        //        {
        //            splashScreenManager_loding.CloseWaitForm();
        //        }
        //    }
        //}

        /// <summary>
        /// 메인폼 닫히기전 이벤트
        /// </summary>
        private void MAIN_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = ShowMessageBox.FlyoutConfirm(this, "프로그램 종료안내", "프로그램을 종료하시겠습니까?", "프로그램종료", "취소");

            if (result == DialogResult.Yes)
            {
                Dbconn.conn.Close();

                // GBG
                if (clsCommon.PlantCode == "P101")
                {
                    if (clsXgiHandler.IsConnected)
                    {
                        clsXgiHandler.Deinit();
                    }
                }
                if (clsCommon.PlantCode == "P102")
                {
                    //clsCimonHandler.Finish();
                    if (clsCimonHandler2.IsConnected)
                    {
                        clsCimonHandler2.Deinit();
                    }
                }
                // GBG -

                System.Windows.Forms.Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// DB, PLC 연결상태 상태바 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void conn_witch_timer_Tick(object sender, EventArgs e)
        {
            barEditItem_Delay.EditValue = Properties.Settings.Default.FontSize;

            if (Dbconn.conn.getState().Equals("Open"))
            {
                barStaticItem_DBLocation.Caption = "접속 DB : ".Merge(clsCommon.DbLocation);

                barEditItem_dbConnSt.EditValue = "접속완료";
                repotxtEdit_DbConnSt.Appearance.BackColor = Color.SeaGreen;
            }
            else
            {
                barEditItem_dbConnSt.EditValue = "접속실패";
                repotxtEdit_DbConnSt.Appearance.BackColor = Color.Red;
            }

            barStaticItem_CheckPLCCon.Caption = sErrMsg;

            if (Properties.Settings.Default.MainPlc_Yn == "Y")
            {
                clsCommon._strMainPlcConnYn = "Y";
                //SetMainPlcConn();
            }

            if (clsCommon._strMainPlcConnYn != "Y")
            {
                barEditItem_plcDosConnSt.EditValue = "권한없음";
                repotxtEdit_MainPlcConnSt.Appearance.BackColor = Color.Black;
            }
            else
            {
                if (MainPlcConnChk == "Y")
                {
                    barEditItem_plcDosConnSt.EditValue = "접속완료";
                    repotxtEdit_MainPlcConnSt.Appearance.BackColor = Color.SeaGreen;

                }
                else if (MainPlcConnChk == "N")
                {
                    barEditItem_plcDosConnSt.EditValue = "접속실패";
                    repotxtEdit_MainPlcConnSt.Appearance.BackColor = Color.Red;

                    iMainPlcCon++;

                    //if (iMainPlcCon <= 5)
                    //{
                    //    SetMainPlcConn();
                    //}
                }
                else if (MainPlcConnChk == "M")
                {
                    barEditItem_plcDosConnSt.EditValue = "접속대기";
                    repotxtEdit_MainPlcConnSt.Appearance.BackColor = Color.SteelBlue;
                }
            }

            if (Properties.Settings.Default.SubPlc_Yn == "Y")
            {
                clsCommon._strSubPlcConnYn = "Y";
                //SetSubPlcConn();
            }

            if (clsCommon._strSubPlcConnYn != "Y")
            {
                barEditItem_plcMicConnSt.EditValue = "권한없음";
                repotxtEdit_SubPlcConnSt.Appearance.BackColor = Color.Black;
            }
            else
            {
                if (SubPlcConnChk == "Y")
                {
                    barEditItem_plcMicConnSt.EditValue = "접속완료";
                    repotxtEdit_SubPlcConnSt.Appearance.BackColor = Color.SeaGreen;

                }
                else if (SubPlcConnChk == "N")
                {
                    barEditItem_plcMicConnSt.EditValue = "접속실패";
                    repotxtEdit_SubPlcConnSt.Appearance.BackColor = Color.Red;

                    //iSubPlcCon++;

                    //if (iSubPlcCon <= 5)
                    //{
                    //    SetSubPlcConn();
                    //}

                    //SetSubPlcConn();
                }
                else if (SubPlcConnChk == "M")
                {
                    barEditItem_plcMicConnSt.EditValue = "접속대기";
                    repotxtEdit_SubPlcConnSt.Appearance.BackColor = Color.SteelBlue;
                }
            }

            if (MainPlcConnChk == "Y" && SubPlcConnChk == "Y")
            {
                if (work_watch_timer != null && !work_watch_timer.Enabled)
                {
                    vBatchComplete = false;
                    vMixComplete = false;
                    vWorkComplete = false;
                }
            }

            if (Properties.Settings.Default.BarcodeYn == "Y")
            {
                clsCommon.BarcodeConnYn = "Y";
                BarcodeConnChk = "Y";
            }

            if (clsCommon.BarcodeConnYn != "Y")
            {
                BtnEdit_Barcode.EditValue = "권한없음";
                repoText_Barcode.Appearance.BackColor = Color.Black;
            }
            else
            {
                if (BarcodeConnChk == "Y")
                {
                    BtnEdit_Barcode.EditValue = "접속완료";
                    repoText_Barcode.Appearance.BackColor = Color.SeaGreen;
                }
                else if (BarcodeConnChk == "N")
                {
                    BtnEdit_Barcode.EditValue = "접속실패";
                    repoText_Barcode.Appearance.BackColor = Color.Red;
                }
                else if (BarcodeConnChk == "M")
                {
                    BtnEdit_Barcode.EditValue = "접속대기";
                    repoText_Barcode.Appearance.BackColor = Color.SteelBlue;
                }
            }

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string version;

                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                else
                {
                    version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }

                try
                {
                    if (ad.CheckForUpdate())
                    {
                        var color = isVisible ? Color.Blue : Color.Red;

                        barStaticItem_CheckVersion.ItemAppearance.Normal.ForeColor = color;
                        barStaticItem_CheckVersion.ItemAppearance.Normal.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
                        barStaticItem_CheckVersion.ItemAppearance.Normal.Options.UseFont = true;

                        isVisible = !isVisible;

                        barStaticItem_CheckVersion.Caption = $"새로운 버전이 있습니다. (버전 : {version})";
                    }
                    else
                    {
                        barStaticItem_CheckVersion.ItemAppearance.Normal.ForeColor = Color.Black;

                        barStaticItem_CheckVersion.Caption = $"현재 최신 버전입니다. (버전 : {version})";
                    }
                }
                catch (DeploymentDownloadException dde)
                {
                    barStaticItem_CheckVersion.Caption = "업데이트 확인 중 오류: " + dde.Message;
                }
            }
            else
            {
                barStaticItem_CheckVersion.Caption = "ClickOnce 배포가 아닙니다.";
            }
        }

        private void btn_topRemote_Click(object sender, EventArgs e)
        {
            DialogResult result = ShowMessageBox.Confirm("개발사에 원격제어를 요청하시겠습니까?", "원격제어를 받기위해서는 인터넷에 연결되어있어야 합니다");

            if (result == DialogResult.Yes)
            {
                try
                {

                    FileInfo fi = new FileInfo(Application.StartupPath + "\\KF2.exe");

                    if (fi.Exists)
                    {
                        string exe_name = Application.StartupPath + "\\KF2.exe";
                        Process.Start(exe_name);
                    }
                    else
                    {
                        ShowMessageBox.XtraShowError("원격실행파일을 찾을 수 없습니다");
                    }

                }
                catch (Exception ex)
                {
                    ShowMessageBox.XtraShowErrorLog("원격을 실행하는도중 에러가 발생했습니다", this, "btn_topRemote_Click", ex);
                }
            }
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            try
            {
                this.Opacity = 0.8;
                LOGIN login = new LOGIN(true);
                login.TopMost = true;
                login.ShowDialog();
                LoginInfo_Reg(clsCommon.UserName);
                Screen_attr();
                this.Opacity = 1;
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog("로그아웃을 하는 도중 에러가 발생했습니다", this, "btn_logout_Click", ex);
            }
        }

        private void btn_passChange_Click(object sender, EventArgs e)
        {
            LOGIN_PASS_CHANGE loginPassForm = new LOGIN_PASS_CHANGE();
            loginPassForm.StartPosition = FormStartPosition.CenterScreen;
            DialogResult rlt = loginPassForm.ShowDialog();

            if (rlt == DialogResult.OK)
            {
                ShowMessageBox.XtraShowInformation("비밀번호 변경이 완료되었습니다");
            }
        }

        /// <summary>
        /// PLC 1번 접속
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem_MainPlcConnSt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string result = XtraInputBox.Show("비밀번호를 입력하여 주세요", "확인", "").ToUpper();

            //if (!string.IsNullOrEmpty(result))
            //{
            //    if (Debugger.IsAttached && result.Equals("KFIRST"))
            //    {
            try
            {
                mPlcConnSetting mPlcSetting = new mPlcConnSetting("Dos");
                mPlcSetting.StartPosition = FormStartPosition.CenterScreen;
                mPlcSetting.ShowDialog();

                if (clsCommon._strMainPlcConnYn == "Y")
                {
                    iMainPlcCon = 0;
                    SetMainPlcConn();
                }
                else
                {
                    work_watch_timer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                barStaticItem_CheckPLCCon.Caption = "PLC 1번 접속을 실패하였습니다";
            }
            //}
            //else
            //{
            //    ShowMessageBox.XtraShowInformation("비밀번호가 맞지않습니다");
            //}
            //}
        }

        private void SetMainPlcConn()
        {
            DataTable dt = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            DataRow dr = dt.AsEnumerable()
                        .FirstOrDefault(r => r.Field<Int16>("PLC_NO") == 1);

            if (dr == null || dr[0]?.ToString() == "")
                return;

            bool connOk = false;
            switch (dr["PLC_TYPE"]?.ToString())
            {
                case "Q":
                    {
                        qPlc1 = new ActQJ71E71TCP
                        {
                            ActHostAddress = dr["IP"]?.ToString(),
                            ActStationNumber = int.Parse(dr["N_NO"]?.ToString()),
                            //ActPortNumber = 5002,
                            //ActDestinationPortNumber = 6001,
                            ActSourceStationNumber = int.Parse(dr["N_NO"]?.ToString()) + 1,
                            ActTimeOut = int.Parse(dr["T_OUT"]?.ToString())
                        };

                        if (qPlc1.Open() == 0)
                        {
                            connOk = true;
                        }
                        else
                        {
                            qPlc1.Close();
                        }
                    }
                    break;
                case "A":
                    {
                        aPlc1 = new ActAJ71E71UDP
                        {
                            ActHostAddress = dr["IP"]?.ToString(),
                            //ActStationNumber = int.Parse(dr["N_NO"]?.ToString()),
                            //ActPortNumber = 5002,
                            ActPortNumber = int.Parse(dr["PORT"]?.ToString()),
                            //ActCpuType = 262,
                            ActDestinationPortNumber = int.Parse(dr["PORT"]?.ToString()),
                            //ActSourceStationNumber = int.Parse(dr["N_NO"]?.ToString()) + 1,
                            ActTimeOut = int.Parse(dr["T_OUT"]?.ToString())
                        };

                        if (aPlc1.Open() != 0)
                        {
                            connOk = true;
                        }
                        else
                        {
                            aPlc1.Close();
                        }
                    }
                    break;
                case "XGI":
                    {
                        clsXgiHandler.Init();
                        if (clsXgiHandler.Connect(dr["IP"]?.ToString(), int.Parse(dr["PORT"]?.ToString())))
                        {
                            connOk = true;
                        }
                    }
                    break;
                case "CM":
                    {
                        clsCimonHandler2.Init();
                        if (clsCimonHandler2.Connect(dr["IP"]?.ToString(), int.Parse(dr["PORT"]?.ToString())))
                        {
                            connOk = true;
                        }
                    }
                    break;
            }

            if (connOk)
            {
                Properties.Settings.Default.MainPlc_Yn = "Y";
                Properties.Settings.Default.Save();
                MainPlcConnChk = "Y";

                if (clsCommon.PlantCode == "PJ02")
                    work_watch_timer.Interval = 10000;
                else
                    work_watch_timer.Interval = 10000;
                // GBG -
                work_watch_timer.Enabled = true;
            }
            else
            {
                Properties.Settings.Default.MainPlc_Yn = "N";
                Properties.Settings.Default.Save();
                MainPlcConnChk = "N";
                work_watch_timer.Enabled = false;
                ShowMessageBox.XtraShowWarning("PLC 1번 접속을 실패하였습니다");
                //barStaticItem_CheckPLCCon.Caption = "PLC 1번 접속을 실패하였습니다";
            }
        }

        /// <summary>
        /// 마이크로 PLC 접속
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem_SubPlcConnSt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string plcPw = string.Empty;
            //string result = XtraInputBox.Show("비밀번호를 입력하여 주세요", "확인", "").ToUpper();

            //if (!string.IsNullOrEmpty(result))
            //{
            //    DataTable pwDT = clsCommon.GetPlcPw();

            //    plcPw = pwDT.Rows[0]["NAME"]?.ToString();

            //    if (Debugger.IsAttached && result.Equals(plcPw))
            //    {
            try
            {
                mPlcConnSetting mPlcSetting = new mPlcConnSetting("Mic");
                mPlcSetting.StartPosition = FormStartPosition.CenterScreen;
                mPlcSetting.ShowDialog();

                if (clsCommon._strSubPlcConnYn == "Y")
                {
                    iSubPlcCon = 0;
                    SetSubPlcConn();
                }
                else
                {
                    work_watch_timer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //ShowMessageBox.XtraShowWarning("PLC 1번 접속을 실패하였습니다");.
                barStaticItem_CheckPLCCon.Caption = "PLC 1번 접속을 실패하였습니다";
            }
            //}
            //else
            //{
            //    ShowMessageBox.XtraShowInformation("비밀번호가 맞지않습니다");
            //}
            //}
        }

        private void SetSubPlcConn()
        {
            DataTable dt = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            DataRow dr = dt.AsEnumerable()
                        .FirstOrDefault(r => r.Field<Int16>("PLC_NO") == 2);

            if (dr == null || dr[0]?.ToString() == "")
                return;

            bool connOk = false;

            switch (dr["PLC_TYPE"]?.ToString())
            {
                case "Q":
                    {
                        qPlc2 = new ActQJ71E71TCP
                        {
                            ActHostAddress = dr["IP"]?.ToString(),
                            ActStationNumber = int.Parse(dr["N_NO"]?.ToString()),
                            //ActPortNumber = 5002,
                            //ActDestinationPortNumber = 6001,
                            ActSourceStationNumber = int.Parse(dr["N_NO"]?.ToString()) + 2,
                            ActTimeOut = int.Parse(dr["T_OUT"]?.ToString())
                        };

                        if (qPlc2.Open() == 0)
                        {
                            connOk = true;
                        }
                        else
                        {
                            qPlc2.Close();
                        }

                    }
                    break;
                case "A":
                    {
                        aPlc2 = new ActAJ71E71UDP
                        {
                            ActHostAddress = dr["IP"]?.ToString(),
                            //ActStationNumber = int.Parse(dr["N_NO"]?.ToString()),
                            //ActPortNumber = 5002,
                            ActPortNumber = int.Parse(dr["PORT"]?.ToString()),
                            //ActCpuType = 262,
                            ActDestinationPortNumber = int.Parse(dr["PORT"]?.ToString()),
                            //ActSourceStationNumber = int.Parse(dr["N_NO"]?.ToString()) + 1,
                            ActTimeOut = int.Parse(dr["T_OUT"]?.ToString())
                        };

                        // GBG
                        //if (aPlc.Open() != 0)
                        //{
                        //    connOk = true;
                        //}
                        if (aPlc2.Open() == 0)
                        {
                            connOk = true;
                        }
                        else
                        {
                            aPlc2.Close();
                        }
                        // GBG -
                    }
                    break;
                case "XGI":
                    {
                        clsXgiHandler.Init();
                        if (clsXgiHandler.Connect(dr["IP"]?.ToString(), int.Parse(dr["PORT"]?.ToString())))
                        {
                            connOk = true;
                        }
                    }
                    break;
                case "CM":
                    {
                        clsCimonHandler2.Init();
                        if (clsCimonHandler2.Connect(dr["IP"]?.ToString(), int.Parse(dr["PORT"]?.ToString())))
                        {
                            connOk = true;
                        }
                    }
                    break;
            }

            if (connOk)
            {
                Properties.Settings.Default.SubPlc_Yn = "Y";
                Properties.Settings.Default.Save();
                SubPlcConnChk = "Y";

                if (clsCommon.PlantCode == "PJ02")
                    work_watch_timer.Interval = 10000;
                else
                    work_watch_timer.Interval = 10000;

                work_watch_timer.Enabled = true;
            }
            else
            {
                Properties.Settings.Default.SubPlc_Yn = "N";
                Properties.Settings.Default.Save();
                SubPlcConnChk = "N";
                work_watch_timer.Enabled = false;
                ShowMessageBox.XtraShowWarning("PLC 2번 접속을 실패하였습니다");
                //barStaticItem_CheckPLCCon.Caption = "PLC 2번 접속을 실패하였습니다";
            }
        }

        /// <summary>
        /// 시리얼 포트 접속
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repoText_Barcode_Click(object sender, EventArgs e)
        {
            try
            {
                mBarcodeCon barcodeCon = new mBarcodeCon();
                barcodeCon.StartPosition = FormStartPosition.CenterScreen;
                barcodeCon.ShowDialog();
            }
            catch (Exception ex)
            {
                BarcodeConnChk = "N";
                ShowMessageBox.XtraShowWarning("바코드 리더기 접속을 실패하였습니다");
            }
        }

        private void xtraTabbedMdiManager_PageAdded(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            pictureEdit_mdiLogo.SendToBack();
        }

        private void xtraTabbedMdiManager_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            if (xtraTabbedMdiManager.Pages.Count == 0)
            {
                pictureEdit_mdiLogo.BringToFront();
            }
        }

        /// <summary>
        /// 메뉴 정보를 Status Bar에 보여줍니다.
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="text"></param>
        internal void SetMenuPath(string menuid, string text, string hint)
        {
            try
            {
                if (Debugger.IsAttached)
                    siMenuInfo.Caption = menuid == "" ? "" : String.Format("{0} : {1}({2})", menuid, text, hint);
                else
                    siMenuInfo.Caption = "";
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog(ex.Message.ToString(), this, "SetMenuPath()", ex);
            }
            finally
            {
                if (splashScreenManager_loding.IsSplashFormVisible)
                {
                    splashScreenManager_loding.CloseWaitForm();
                }
            }
        }

        private bool isTimerRunning = false;
        private void SafeTimerTick(System.Windows.Forms.Timer timer, Action work)
        {
            if (isTimerRunning) return;

            try
            {
                isTimerRunning = true;
                work_watch_timer.Stop();
                work.Invoke();
            }
            finally
            {
                work_watch_timer.Start();
                isTimerRunning = false;
            }
        }

        private void work_watch_timer_Tick(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            vCompleteCnt = 0;

            //int[] sData = new int[10]; // plc 전송 패킷  
            //int[] rData = new int[10]; // plc->pc 메모리 

            // GBG
            //int[] tmpDatas = new int[5];
            int[] tmpDatas = new int[20];
            // GBG -

            //int[] tmpScr = new int[5];

            // GBG
            // for CIMON PLC
            if (clsCommon.PlantCode == "P102")
            {
                Array.Clear(Cimon_Job_Data, 0, Cimon_Job_Data.Length);
                //Task<int[]> returnTaskReadResult = clsCimonHandler.ReadWord(CIMON_PLC_JOB_ADDR, CIMON_PLC_JOB_READE_SIZE);
                //Cimon_Job_Data = await returnTaskReadResult;

                if (clsCimonHandler2.Read(0, CIMON_PLC_JOB_ADDR, CIMON_PLC_JOB_READE_SIZE, Cimon_Job_Data) == 0)
                {
                    Thread.Sleep(500);
                    if (clsCimonHandler2.Read(0, CIMON_PLC_JOB_ADDR, CIMON_PLC_JOB_READE_SIZE, Cimon_Job_Data) == 0)
                    {
                    }
                }

                if (Cimon_Job_Data != null)
                    clsLog.logSave($" - CIMON PLC - MAP [{Cimon_Job_Data[10]}, {Cimon_Job_Data[11]}, {Cimon_Job_Data[12]}, 50 : {Cimon_Job_Data[50]}, 98 : {Cimon_Job_Data[98]} ]", 0, "PLC");
            }
            // GBG -
            //work_watch_timer.Stop();

            //xDosingResult(tmpDatas);

            SafeTimerTick(work_watch_timer, () =>
            {
                xDosingResult(tmpDatas);
                clsLog.logSave("::::::::::::::::PLCWorker PLCRun:::::::::::::::: " + sw.Elapsed.TotalSeconds / 60, 0, "PLC");
                Console.WriteLine("::::::::::::::::PLCWorker PLCRun:::::::::::::::: " + sw.Elapsed.TotalSeconds / 60);
            });

            sw.Stop();

            clsLog.logSave("::::::::::::::::PLCWorker:::::::::::::::: " + sw.Elapsed.TotalSeconds / 60, 0, "PLC");
        }

        /// <summary>
        /// 실적 받기
        /// </summary>
        /// <param name="tmpDatas">PLC 리턴값</param>
        private void xDosingResult(int[] tmpDatas)
        {
            try
            {
                /*
                * 
                *  [ 작업PLC 영역감시 루틴 ]
                *  작업지시는 배치별로 스케일별 계량완료 신호가 모두 취합된후 배치계량완료시 믹서기로 방출신호가 온다. 이때 생산량,진행배치, 
                *  그외 필요한 배합정보(이송시간,배치량,분사시간 등)을 DB에 입력 취합한다
                *  
                *  배합작업순서 : 작업시간 -> 각 스케일별 완료신호 -> 스케일별 완료신호가 모두 완료되면 배치계량이 완료된걸로 보고 믹서방출을 한다.
                *  이때 믹서방출신호도 받는다 -> 배치수만큼 각 스케일별 완료신호 <-> 믹서방출 루틴을 반복한다 -> 마지막 배치가 믹서방출까지 완료될 시 작업완료 신호가 보내진다
                *  
                */

                #region  작지번호,배치 가져오기

                // GBG2
                DataTable dt = new DataTable();
                if (clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == clsCommon.ProcessCode)
                {
                    dt = clsCommon.GetProcess(clsCommon.PlantCode, clsCommon.GetProcessTypeCode("배합"));
                }
                else if (clsCommon.GetProcessKey("갓돈배합", clsCommon.PlantCode) == clsCommon.ProcessCode)
                {
                    dt = clsCommon.GetProcess(clsCommon.PlantCode, "", clsCommon.GetProcessKey("갓돈배합", clsCommon.PlantCode));
                }

                string[] aProcessCode = dt.AsEnumerable()
                   .Select(r => r.Field<string>("CODE"))
                   .ToArray();

                for (int p = 0; p < aProcessCode.Length; p++)
                {
                    // GBG4
                    //if (clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == clsCommon.ProcessCode && aProcessCode[p] == clsCommon.GetProcessKey(clsCommon.PlantCode, "갓돈배합"))
                    if (clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == clsCommon.ProcessCode && aProcessCode[p] == clsCommon.GetProcessKey("갓돈배합", clsCommon.PlantCode))
                    {
                        continue;
                    }
                    // GBG4 -

                    DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, aProcessCode[p]);

                    for (int i = 0; i < dtPlc.Rows.Count; i++)
                    {
                        // GBG2
                        if (clsCommon.PlantCode == "PJ01" && clsCommon.ProcessCode == clsCommon.GetProcessKey("배합", clsCommon.PlantCode))
                        {
                            if (i > 0) continue;
                        }
                        // GBG2

                        vProcessKey = dtPlc.Rows[i]["USE_PROCESS_KEY"]?.ToString();

                        vPLC_Location = Convert.ToInt32(dtPlc.Rows[i]["PLC_NO"]);

                        if (!PlcFunc.GetPlcCon(dtPlc, out sErrMsg)) return;

                        if (sErrMsg != "")
                            barStaticItem_CheckPLCCon.Caption = sErrMsg;

                        Array.Clear(tmpDatas, 0, tmpDatas.Length);

                        // GBG
                        DataTable dtLine = clsCommon.GetLine(clsCommon.PlantCode, vProcessKey);
                        // GBG -

                        // GBG
                        for (int lineNo = 1; lineNo < dtLine.Rows.Count + 1; lineNo++)
                        {
                            // GBG
                            if (vPLC_Location == 0)
                            {
                                continue;
                            }
                            else if (vPLC_Location == 1)
                            {
                                if (MainPlcConnChk != "Y")
                                    continue;
                            }
                            else if (vPLC_Location == 2)
                            {
                                if (SubPlcConnChk != "Y")
                                    continue;
                            }
                            // GBG -

                            // GBG
                            //vLCode = (iPlcNo).ToString();
                            vLCode = (lineNo).ToString();
                            // GBG -

                            clsCommon.GetPLCAddress(clsCommon.PlantCode
                                                    , vProcessKey
                                                    , vLCode
                                                    //, dtPlc.Rows[0]["PLC_TYPE"]?.ToString() == "Q" ? 1 : 2
                                                    , vPLC_Location
                                                    , PlcAddressType.WORKINFO.GetDesc()
                                                    , 1
                                                    , out vPLCAddress
                                                    , out vPLCUnit
                                                    , out vPLCDataCount);

                            Thread.Sleep(500);


                            PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "배합 작지 PLC 읽기 실패", Cimon_Job_Data, WORK_YYYY);

                            vWorkDate = tmpDatas[0].ToString().PadLeft(4, '0') + tmpDatas[1].ToString().PadLeft(4, '0');

                            vWorkNum = tmpDatas[2].ToString();

                            //if (aPlc.ReadDeviceBlock(workDEV, 5, out tmpDatas[0]) != 0)
                            //{
                            //    SubPlcConnChk = "N";
                            //    clsLog.logSave(vWorkDate + "/" + vWorkNum + ":" + vBatchNum + "배합 작지 PLC 읽기 실패", 1);
                            //}
                            //else
                            //{
                            //    SubPlcConnChk = "Y";
                            //}

                            if (vWorkDate != tmpDatas[0].ToString().PadLeft(4, '0') + tmpDatas[1].ToString().PadLeft(4, '0') && vWorkNum != tmpDatas[2].ToString())
                            {
                                //if (qPlc.WriteDeviceBlock("D" + DosDev.ToString(), 5, ref tmpDatas[0]) != 0)
                                //{
                                //    ShowMessageBox.XtraShowWarning("배합비전송을 실패하였습니다. 케이퍼스트에 문의해 주십시오", "알림");
                                //    return;
                                //}

                                //ShowMessageBox.XtraShowWarning("주배합과 마이크로 작지 정보가 상이 합니다.");
                                //return;
                            }

                            Array.Clear(tmpDatas, 0, tmpDatas.Length);

                            // 현 배치번호
                            //if (processGubun == "1") // 일반 배합
                            //    workDEV = "D" + (DosDev + 12);

                            clsCommon.GetPLCAddress(clsCommon.PlantCode
                                                , vProcessKey
                                                , vLCode
                                                , vPLC_Location
                                                , PlcAddressType.CURRENTBATCH.GetDesc()
                                                , 1
                                                , out vPLCAddress
                                                , out vPLCUnit
                                                , out vPLCDataCount);

                            PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "현재 배치 수 읽기 실패", Cimon_Job_Data, BATCH_PV);

                            vBatchNum = tmpDatas[0].ToString();

                            //if (aPlc.ReadDeviceBlock(workDEV, 1, out tmpDatas[0]) != 0)
                            //{
                            //    clsLog.logSave(vWorkDate + "/" + vWorkNum + ":" + vBatchNum + "배합 작지 PLC 읽기 실패", 1);
                            //}

                            if (vBatchNum != tmpDatas[0].ToString())
                            {
                                //if (qPlc.WriteDeviceBlock("D" + (DosDev + 112), 1, ref tmpDatas[0]) != 0)
                                //{
                                //    ShowMessageBox.XtraShowWarning("배합비전송을 실패하였습니다. 케이퍼스트에 문의해 주십시오", "알림");
                                //    return;
                                //}

                                //ShowMessageBox.XtraShowWarning("주배합과 마이크로 배치 정보가 상이 합니다.");
                                //return;
                            }

                            //제품명 가져오기
                            if (vWorkNum != "0")
                            {
                                SQL = $@"
                            SELECT WO.PROCESS_KEY, WO.RESOURCE_NO, P.DESCRIPTION
                            FROM WORK_ORDER WO
                                JOIN SAP_DI_PRODUCT P ON WO.RESOURCE_NO = P.RESOURCE_NO
                            WHERE WO.PLANT_CODE   = '{clsCommon.PlantCode}'
                                AND WO.PROCESS_KEY  IN ('{vProcessKey}')
                                AND WO.L_CODE       = '{vProcessKey.Merge(vLCode)}'
                                AND WO.WORKDATE     = '{vWorkDate}'
                                AND WO.NUM          = '{vWorkNum}'
                            ";

                                using (DataSet pSearchDs = Dbconn.conn.ExecutDataset(SQL))
                                {
                                    if (Dbconn.conn.getRowCnt(pSearchDs) > 0)
                                    {
                                        vResourceName = Dbconn.conn.getData(pSearchDs, "DESCRIPTION", 0);
                                        vResourceNo = Dbconn.conn.getData(pSearchDs, "RESOURCE_NO", 0);
                                    }
                                    else
                                    {
                                        vResourceName = string.Empty;
                                    }
                                }
                            }

                            //frm_Dosing frm = Application.OpenForms["frm_Dosing"] as frm_Dosing;
                            //if (frm != null && !frm.IsDisposed)
                            //{
                            //    frm.txtEdt_runWorkNum.Text = vWorkDate;
                            //    frm.txtEdt_runNum.Text = vWorkNum;
                            //    frm.txtEdt_runWorkBatch.Text = vBatchNum;
                            //    frm.txtEdt_runWorkProduct.Text = vResourceName;

                            //    frm.btn_workSearch.PerformClick();
                            //}

                            if (sPlantCode != "PJ01")
                            {
                                foreach (Form frm in Application.OpenForms)
                                {
                                    if (frm == null || frm.IsDisposed || !wStatus)
                                        continue;

                                    //재귀 탐색으로 컨트롤 찾기
                                    Control txtEdt_runWorkNum = FindControlRecursive(frm, "txtEdt_runWorkNum");
                                    Control txtEdt_runNum = FindControlRecursive(frm, "txtEdt_runNum");
                                    Control txtEdt_runWorkBatch = FindControlRecursive(frm, "txtEdt_runWorkBatch");
                                    Control txtEdt_runWorkProduct = FindControlRecursive(frm, "txtEdt_runWorkProduct");

                                    if ((string.IsNullOrWhiteSpace(vWorkDate) || vWorkDate == "00000000") || (string.IsNullOrWhiteSpace(vWorkNum) || vWorkDate == "0"))
                                        break;

                                    if (txtEdt_runWorkNum != null)
                                    {
                                        (txtEdt_runWorkNum as TextEdit).Text = vWorkDate;
                                    }

                                    if (txtEdt_runNum != null)
                                    {
                                        (txtEdt_runNum as TextEdit).Text = vWorkNum;
                                    }

                                    if (txtEdt_runWorkBatch != null)
                                    {
                                        (txtEdt_runWorkBatch as TextEdit).Text = vBatchNum;
                                    }

                                    if (txtEdt_runWorkProduct != null)
                                    {
                                        (txtEdt_runWorkProduct as TextEdit).Text = vResourceName;
                                    }
                                }
                            }

                            if (vBatchComplete == true || vMixComplete == true || vWorkComplete == true)
                            {
                                vBatchComplete = false;
                                vMixComplete = false;
                                vWorkComplete = false;
                                return;
                            }

                            // 배치 완료 처리
                            XSetBatchComplete(dtPlc, vProcessKey, vLCode, vPLC_Location);

                            // 믹서방출시 처리
                            XSetMixerOutput(dtPlc, vProcessKey, vLCode, vPLC_Location);

                            // 작업완료 플래그 처리
                            XSetWorkEnd(dtPlc, vProcessKey, vLCode, vPLC_Location);
                        } // for (int lineNo = 1; lineNo < dtLine.Rows.Count + 1; lineNo++)          
                    } // for (int i = 0; i < aProcessCode.Length; i++)
                }
                #endregion
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowWarning("PLC 1번 접속을 실패하였습니다");
                MAIN_Jeil1.MainPlcConnChk = "N";
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.Source);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.InnerException);
            }
            finally
            {
                work_watch_timer.Start();
            }
        }

        /// <summary>
        /// 배치 완료 체크
        /// </summary>
        /// <param name="tmpDatas"></param>
        /// <param name="vDosArg"></param>
        private void XSetBatchComplete(DataTable dtPlc, string sProcessKey, string sLCode, int iPlcNo)
        {
            try
            {
                int[] sData = new int[10]; // plc 전송 패킷  
                int[] tmpDatas = new int[10];

                int TmpDev = 0;

                string tmpScName = null; // 스케일명 
                string RstDev = null; // 리셋디바이스 주소 


                // PLC 스케일별 배치 완료 신호 Check
                Array.Clear(sData, 0, sData.Length);


                clsCommon.GetPLCAddress(clsCommon.PlantCode
                                        , sProcessKey
                                        , sLCode
                                        // GBG
                                        , iPlcNo
                                        // GBG -
                                        , PlcAddressType.BATCHCOMPLETE.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, ref sData, "배치완료 PLC 읽기 실패", Cimon_Job_Data, BATCH_COMPLETE);

                if (sData[0] == 1 && !vBatchComplete)
                {
                    vBatchComplete = true;

                    if (int.Parse(vWorkDate) == 0)
                    {
                        clsLog.logSave("********************배치 완료는 받았지만 작업일자가 없습니다.********************", 0, "PLC");
                        vBatchComplete = false;
                        return;
                    }

                    if (int.Parse(vWorkNum) == 0)
                    {
                        clsLog.logSave("********************배치 완료는 받았지만 작업번호가 없습니다.********************", 0, "PLC");
                        vBatchComplete = false;
                        return;
                    }

                    if (int.Parse(vBatchNum) == 0)
                    {
                        clsLog.logSave("********************배치 완료는 받았지만 배치번호가 없습니다.********************", 0, "PLC");
                        vBatchComplete = false;
                        return;
                    }

                    // GBG
                    clsLog.logSave($"배치 완료 - 작업일자 [{vWorkDate}] 순번 [{vWorkNum}] 배치 [{vBatchNum}]", 0, "PLC");
                    // GBG -

                    // GBG
                    if (clsCommon.PlantCode == "P102")
                    {
                        ReadResult();
                    }
                    // GBG -

                    // gubun : 1 배치 2 믹싱
                    PlcSetBatchResult(1, int.Parse(vBatchNum), dtPlc, iPlcNo, sProcessKey, sLCode);

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                        , sProcessKey
                                        , sLCode
                                        // GBG
                                        , iPlcNo
                                        // GBG -
                                        , PlcAddressType.BATCHCOMPLETE.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);
                    //SetQDeviceEx

                    sData[0] = 2;
                    PlcFunc.PlcSetQDeviceEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, null, 0, sData, "배치완료 PLC 읽기 실패");

                    Thread.Sleep(500);
                    // GBG -

                    //if (processGubun == "1") // 일반 배합
                    //    workDEV = "D" + (141 + vDosArg);

                    //qPlc.SetDevice(workDEV, 2);
                    if (sPlantCode != "PJ01")
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm == null || frm.IsDisposed || !wStatus)
                                continue;

                            Control btn = FindControlRecursive(frm, "btn_workSearch");

                            if (btn != null && btn is SimpleButton simpleBtn)
                            {
                                // DevExpress SimpleButton인 경우
                                simpleBtn.PerformClick();
                            }
                            else if (btn != null && btn is Button normalBtn)
                            {
                                // 일반 WinForms Button인 경우
                                normalBtn.PerformClick();
                            }
                        }
                    }
                }
            }
            finally
            {
                vBatchComplete = false;
            }
        }

        /// <summary>
        /// 배치실적
        /// </summary>
        /// <param name="vDosArg">PLC ADD 변수</param>
        /// <param name="plcType"></param>
        /// <param name="ScaleVal"></param>
        /// <param name="gubun"></param>
        /// <returns></returns>
        private void PlcSetBatchResult(int gubun, int batchNum, DataTable dtPlc, int iPlcNo, string sProcessKey, string sLCode)
        {
            DataSet ds;
            DataRow dr;

            int[] rData = new int[10]; // plc->pc 메모리 
            int Dev = 0; //디바이스 주소 
            string sPlcType = string.Empty;
            // GBG
            int iPlcType = 1;
            // GBG -
            double ScaleVal = 0; // 계량값

            // DS103	170.170.100.83	D2020	10	1
            SQL = $@"
            SELECT a.SCALE_CODE, COUNT(a.LOCATION) AS LOCNT, b.IN_SCALE
            FROM WORK_DETAIL a
                INNER JOIN SCALE b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE AND b.SCALE_CODE = a.SCALE_CODE
            WHERE a.PLANT_CODE = '{clsCommon.PlantCode}' AND a.PROCESS_KEY = '{sProcessKey}' AND a.L_CODE = '{sProcessKey.Merge(sLCode)}'
                AND a.WORKDATE = '{vWorkDate}' AND a.NUM = '{vWorkNum}'
            GROUP BY a.SCALE_CODE, b.IN_SCALE
            ORDER BY a.SCALE_CODE
            ";

            ds = Dbconn.conn.ExecutDataset(SQL);
            int[] valData = new int[10]; // plc->pc 메모리 
            Array.Clear(valData, 0, valData.Length);
            int iSeq = 0;
            int vDev = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = ds.Tables[0].Rows[i];

                clsLog.logSave($"스케일({dr["SCALE_CODE"]}) : 빈 개수({dr["LOCNT"]})", 0, "PLC");

                if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("배합") == sProcessKey)
                {
                    if (sLCode == "1")
                    {
                        switch (dr["SCALE_CODE"])
                        {
                            // GBG
                            //case "D11": Dev = 1; break;
                            //case "D12": Dev = 2; break;
                            //case "D13": Dev = 3; break;
                            //case "D14": Dev = 4; break;
                            //case "D15": Dev = 5; break;
                            //case "D16": Dev = 1; break;
                            //case "D17": Dev = 2; break;
                            //case "D18": Dev = 3; break;
                            //case "D19": Dev = 4; break;
                            case "DS101": Dev = 1; iPlcType = 1; break;
                            case "DS102": Dev = 2; iPlcType = 1; break;
                            case "DS103": Dev = 3; iPlcType = 1; break;
                            case "DS104": Dev = 4; iPlcType = 1; break;
                            case "DS105": Dev = 5; iPlcType = 1; break;
                            case "DS106": Dev = 1; iPlcType = 2; break;
                            case "DS107": Dev = 2; iPlcType = 2; break;
                            case "DS108": Dev = 3; iPlcType = 2; break;
                            case "DS109": Dev = 4; iPlcType = 2; break;
                                // GBG -
                        }
                    }
                    else if (sLCode == "2" && gubun == 1)
                    {
                        switch (dr["SCALE_CODE"])
                        {
                            // GBG
                            case "DS201": Dev = 1; iPlcType = 1; break;
                            case "DS202": Dev = 2; iPlcType = 1; break;
                            case "DS203": continue;
                            case "DS204": Dev = 1; iPlcType = 2; break;
                            case "DS205": Dev = 2; iPlcType = 2; break;
                                // GBG -
                        }
                    }
                    else if (sLCode == "2" && gubun == 2)
                    {
                        Dev = 3; iPlcType = 1;
                        //Dev = 3200; sPlcType = "Q";
                    }
                    else if (sLCode == "3")
                    {
                        Dev = 1; iPlcType = 1;
                        //Dev = 3500; sPlcType = "Q";
                    }
                }
                // GBG
                else if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("PF배합") == sProcessKey)
                {
                    Dev = 1;
                    iPlcType = 1;
                }
                // GBG -
                else if (clsCommon.PlantCode == "PJ04" && clsCommon.GetProcessKey("배합") == sProcessKey)
                {
                    if (sLCode == "1")
                    {
                        switch (dr["SCALE_CODE"])
                        {
                            case "D1": Dev = 1; break;
                            case "D2": Dev = 2; break;
                            case "D3": Dev = 3; break;
                            case "D4": Dev = 4; break;
                            case "D5": Dev = 5; break;
                            case "D6": Dev = 6; break;
                            case "L1": Dev = 7; break;
                            case "L2": Dev = 8; break;
                            case "L3": Dev = 9; break;
                        }
                    }
                    else if (sLCode == "2" && gubun == 1)
                    {
                        switch (dr["SCALE_CODE"])
                        {
                            case "P1": Dev = 1; break;
                        }
                    }
                }
                else if (clsCommon.PlantCode == "PJ04" && clsCommon.GetProcessKey("PF배합") == sProcessKey)
                {
                    if (sLCode == "1")
                    {
                        switch (dr["SCALE_CODE"])
                        {
                            case "P1": Dev = 1; iPlcType = 2; break;
                        }
                    }
                }
                else if (clsCommon.PlantCode == "PJ05" && clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == sProcessKey)
                {
                    switch (dr["SCALE_CODE"])
                    {
                        case "SD1": Dev = 1; break;
                        case "SD2": Dev = 2; break;
                    }
                }
                else if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("갓돈배합") == sProcessKey)
                {
                    switch (dr["SCALE_CODE"])
                    {
                        case "PS001": Dev = 1; break;
                        case "PS002": Dev = 2; break;
                        case "PS003": Dev = 3; break;
                    }

                    if (gubun == 2)
                        Dev = 4;

                    // GBG
                    iPlcType = 2;
                    // GBG -
                }
                else if (clsCommon.PlantCode == "PJ02" && clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == sProcessKey)
                {
                    switch (dr["SCALE_CODE"])
                    {
                        case "ES001": Dev = 1; break;
                        case "ES002": Dev = 2; break;
                        case "ES101": Dev = 3; break;
                    }
                }
                else if (clsCommon.PlantCode == "P201")
                {
                    switch (dr["SCALE_CODE"])
                    {
                        case "D1": Dev = 1; break;
                        case "D2": Dev = 2; break;
                        case "D3": Dev = 3; break;
                        case "D4": Dev = 4; break;
                        case "D5": Dev = 5; break;
                        case "D6": Dev = 6; break;
                        case "L1": Dev = 7; break;
                        case "L2": Dev = 8; break;
                    }
                }
                // GBG
                else if (clsCommon.PlantCode == "P101")
                {
                    switch (dr["SCALE_CODE"])
                    {
                        case "D1": Dev = 1; break;
                        case "D2": Dev = 2; break;
                        case "D3": Dev = 3; break;
                        case "D4": Dev = 6; break;
                        case "D5": Dev = 7; break;
                        case "L1": Dev = 4; break;
                        case "L2": Dev = 5; break;
                        // GBG
                        case "L5": Dev = 8; break;
                            // GBG -
                    }
                }
                else if (clsCommon.PlantCode == "P102")
                {
                    switch (dr["SCALE_CODE"])
                    {
                        case "D1": Dev = 1; break;
                        case "D2": Dev = 2; break;
                        case "D3": Dev = 3; break;
                        case "L1": Dev = 4; break;
                        case "L2": Dev = 5; break;
                    }
                }

                // GBG -

                for (int j = 0; j < int.Parse(dr["LOCNT"].ToString()); j++)
                {
                    vDev = Dev + (5 * j);

                    clsLog.logSave($"스케일내 빈 순서({j}) : [빈 개수:{dr["LOCNT"]}] {vDev}", 0, "PLC");

                    try
                    {
                        clsCommon.GetPLCAddress(clsCommon.PlantCode
                                        , sProcessKey
                                        , sLCode
                                        // GBG
                                        //, iPlcNo
                                        , iPlcType
                                        // GBG -
                                        , PlcAddressType.RESULT.GetDesc()
                                        , Dev
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                        var (device, number) = vPLCAddress.SplitDeviceAndNumber();

                        //ReadQDeviceAddBlockEx
                        // GBG4
                        //if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("갓돈배합") == sProcessKey)
                        //{
                        //    iPlcType = 1;
                        //}
                        // GBG4-

                        valData = PlcFunc.PlcGetReadDeviceAddBlock(dtPlc, iPlcType, vPLCAddress, vPLCUnit, vPLCDataCount, dr, Dev, j, device, number, "스케일 계량값을 가져오지 못했습니다", Cimon_Result_Data, vWorkDate, vWorkNum);

                        if (valData[0] == 0 || valData[1] == 0)
                            valData = PlcFunc.PlcGetReadDeviceAddBlock(dtPlc, iPlcType, vPLCAddress, vPLCUnit, vPLCDataCount, dr, Dev, j, device, number, "스케일 계량값을 가져오지 못했습니다", Cimon_Result_Data, vWorkDate, vWorkNum);
                        // GBG -
                    }
                    catch (OverflowException ex)
                    {
                        clsLog.logSave(vWorkDate + "/" + vWorkNum + ":" + batchNum + " 스케일 자료형식의 범위를 넘었습니다 // " + ex.Message.ToString(), 1, "PLC");
                    }
                    catch (Exception exx)
                    {
                        clsLog.logSave(vWorkDate + "/" + vWorkNum + ":" + batchNum + " 스케일계량값 Error //" + exx.Message.ToString(), 1, "PLC");
                    }

                    // GBG
                    clsLog.logSave($"실적 데이터 - 작업일자 [{vWorkDate}] 순번 [{vWorkNum}] 배치 [{batchNum}] - Data [{valData[0]}, {valData[1]}, {valData[2]}, {valData[3]}, {valData[4]} ] ", 1, "PLC");
                    // GBG -


                    //마스터 빈정보를 가져온다(원재료 및 스케일정보)
                    SQL = $@"
                    SELECT B.RESOURCE_NO
                        , P.DESCRIPTION
                        , B.LOCATION
                        , B.SCALE_CODE
                        , NVL(B.STOCK, 0) AS STOCK
                    FROM WORK_DETAIL A
                        JOIN BIN B ON B.PLANT_CODE = A.PLANT_CODE AND B.PROCESS_KEY = A.PROCESS_KEY AND B.L_CODE = A.L_CODE AND B.LOCATION = A.LOCATION
                        JOIN SAP_DI_PRODUCT P ON P.PLANT_CODE = B.PLANT_CODE AND A.INGRED_CODE = P.RESOURCE_NO
                        JOIN SCALE S ON S.PLANT_CODE = B.PLANT_CODE AND S.PROCESS_KEY = B.PROCESS_KEY AND S.L_CODE = B.L_CODE AND S.SCALE_CODE = B.SCALE_CODE
                    WHERE A.PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND A.PROCESS_KEY = '{sProcessKey}'
                        AND A.L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND A.WORKDATE = '{vWorkDate}'
                        AND A.NUM = '{vWorkNum}
                        AND A.LOCATION   = '209'
                        AND A.SCALE_CODE = 'FS001'
                    ";

                    SQL = $@"
                    SELECT B.RESOURCE_NO
                        , P.DESCRIPTION
                        , B.LOCATION
                        , B.SCALE_CODE
                        , NVL(B.STOCK, 0) AS STOCK
                    FROM BIN B
                        JOIN SAP_DI_PRODUCT P ON P.PLANT_CODE = B.PLANT_CODE AND B.RESOURCE_NO = P.RESOURCE_NO
                        JOIN SCALE S ON S.PLANT_CODE = B.PLANT_CODE AND S.PROCESS_KEY = B.PROCESS_KEY AND S.L_CODE = B.L_CODE AND S.SCALE_CODE = B.SCALE_CODE
                    WHERE B.PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND B.PROCESS_KEY = '{sProcessKey}'
                        AND B.L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND B.LOCATION   = '{valData[0]}'
                        AND B.SCALE_CODE = '{dr["SCALE_CODE"]}'
                    ";

                    DataSet binDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(binDs) > 0)
                    {
                        //P_TYPE(I: 원재료 P: 제품)
                        ScaleVal = (short)valData[1] / double.Parse(dr["IN_SCALE"]?.ToString());

                        SQL = $@"
                        SELECT 1
                        FROM WORK_REMARK 
                        WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                            AND PROCESS_KEY = '{sProcessKey}'
                            AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                            AND WORKDATE    = '{vWorkDate}'
                            AND NUM         = '{vWorkNum}'
                            AND BATCH       = '{batchNum}'
                            AND LOCATION    = '{valData[0]}'
                        ";

                        DataSet remarkDukChkDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(remarkDukChkDs) == 0)
                        {

                            string stock_locDcode = string.Empty;
                            SQL = $@"
                            SELECT 
                            WERKS, BKLAS, SEQ, 
                                MATNR, LGORT, CHARG, 
                                LABST, INSME, SPEME
                            FROM SAP_STOCK_MASTER
                            WHERE WERKS = '{clsCommon.PlantCode}'
                                    AND MATNR = '{Dbconn.conn.getData(binDs, "RESOURCE_NO", 0)}'
                                    AND LGORT = '{Dbconn.conn.getData(binDs, "LOCATION", 0)}'
                            ";

                            DataSet searchLocDs = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(searchLocDs) > 0)
                            {
                                stock_locDcode = Dbconn.conn.getData(searchLocDs, "LGORT", 0);
                            }

                            clsLog.logSave(valData[0] + "/" + ScaleVal, 0);
                            bool isStockYn = false;

                            //SQL = $@"
                            //INSERT INTO WORK_REMARK
                            //(PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM
                            //, BATCH, SEQ, IO_GUBUN, LOCATION, P_Q, IO_DATE
                            //, RESOURCE_NO, INGRED_LOT, P_TYPE, I_TIME, P_Q_TIME) 
                            //VALUES (
                            //'{clsCommon.PlantCode}', '{sProcessKey}', '{sProcessKey.Merge(sLCode)}', '{vWorkDate}', '{vWorkNum}'
                            //, '{batchNum}', (SELECT NVL(MAX(SEQ) + 1, 1) 
                            //                FROM WORK_REMARK a WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                            //                    AND a.PROCESS_KEY = '{sProcessKey}'
                            //                    AND a.L_CODE = '{sProcessKey.Merge(sLCode)}'
                            //                    AND a.WORKDATE = '{vWorkDate}' AND a.NUM = '{vWorkNum}' AND BATCH = '{batchNum}')
                            //, 'I', '{Dbconn.conn.getData(binDs, "LOCATION", 0)}', '{ScaleVal}', SYSDATE
                            //, '{vResourceNo}', '{Dbconn.conn.getData(binDs, "RESOURCE_NO", 0)}', 'I', SYSDATE, '{valData[3]}' )
                            //";

                            //if (Dbconn.conn.SQLrun(SQL) > 0)
                            //{
                            //    clsLog.logSave(SQL, 0);
                            //    isStockYn = true;
                            //}

                            if (isStockYn)
                            {
                                // GBG
                                //SQL = $@"
                                //UPDATE BIN
                                //    SET STOCK = STOCK + ( {(ScaleVal * -1)} * 1)
                                //WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                                //    AND PROCESS_KEY = '{sProcessKey}'
                                //    AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                                //    AND BIN_CODE = '{valData[0]}'
                                //";
                                //SQL = $@"
                                //UPDATE BIN
                                //    SET STOCK = STOCK + ( {(ScaleVal * -1)} * 1)
                                //WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                                //    AND PROCESS_KEY = '{sProcessKey}'
                                //    AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                                //    AND LOCATION = '{valData[0]}'
                                //";
                                // GBG -

                                Dbconn.conn.SQLrun(SQL);
                            }
                        }
                    } // Rs1 Chk
                }   // for j
            }   // for i

            // 완료 상태 체크
            Array.Clear(rData, 0, rData.Length);

            if (gubun == 1)
            {
                //SQL = $@"
                //INSERT INTO WORK_REMARK
                //SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM
                //    , '{batchNum}', ROWNUM, 'I', LOCATION, SET_VAL, 0, SYSDATE, INGRED_CODE, '{vResourceNo}'
                //    , '', 2, NULL, NULL, NULL, SYSDATE, NULL
                //FROM WORK_DETAIL
                //WHERE   PLANT_CODE  = '{clsCommon.PlantCode}'
                //    AND PROCESS_KEY = '{sProcessKey}'
                //    AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                //    AND WORKDATE    = '{vWorkDate}'
                //    AND NUM         = '{vWorkNum}'
                //    AND SCALE_CODE  = 'H'
                //";

                //Dbconn.conn.SQLrun(SQL);
            }

            SQL = $@"
            SELECT SUM(P_Q) AS SUM_PQ
            FROM WORK_REMARK
            WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                AND PROCESS_KEY = '{sProcessKey}'
                AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                AND WORKDATE     = '{vWorkDate}'
                AND NUM          = '{vWorkNum}'
            ";

            DataSet sumPqDs = Dbconn.conn.ExecutDataset(SQL);

            //// 생산지시에 배치 및 생산량을 업데이트 한다. 
            //SQL = $@"
            //UPDATE WORK_ORDER
            //    SET R_BATCH = '{batchNum}',
            //        PRO_Q   = '{Dbconn.conn.getData(sumPqDs, "SUM_PQ", 0)}'
            //WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
            //    AND PROCESS_KEY = '{sProcessKey}'
            //    AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
            //    AND WORKDATE    = '{vWorkDate}'
            //    AND NUM         = '{vWorkNum}'
            //";

            //Dbconn.conn.SQLrun(SQL);

            //SQL = $@"
            //UPDATE PELLET_REPORT
            //SET BF_QTY = '{Dbconn.conn.getData(sumPqDs, "SUM_PQ", 0)}'
            //    , QTY = '{Dbconn.conn.getData(sumPqDs, "SUM_PQ", 0)}'
            //WHERE BF_PLANT_CODE = '{clsCommon.PlantCode}'
            //        AND BF_PROCESS_KEY = '{sProcessKey}' AND BF_L_CODE = '{sProcessKey.Merge(sLCode)}'
            //        AND BF_WORKDATE = '{vWorkDate}' AND BF_NUM = '{vWorkNum}'
            //";

            //if (Dbconn.conn.SQLrun(SQL) < 1)
            //{
            //    clsLog.logSave(SQL, 0);
            //}

            // 비율 완료
            Array.Clear(rData, 0, rData.Length);

            clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.RATIO.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

            if (vPLCAddress != "")
            {
                PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, ref rData, "");
            }
            else
                rData[0] = 0;


            clsLog.logSave("작지생성 시작", 0, "PLC");

            //// 작지 SAP 실적 생성
            //if (!clsProcessDosing.SetSAPWorkRemark(clsCommon.PlantCode, sProcessKey, sProcessKey.Merge(sLCode), vWorkDate, vWorkNum, batchNum.ToString(), rData[0]))
            //{
            //    if (sPlantCode != "PJ01")
            //    {
            //        ShowMessageBox.XtraShowWarning("SAP 실적 생성에 실패했습니다");
            //    }
            //}

            clsLog.logSave("작지생성 완료", 0, "PLC");
            if (ds != null) ds.Dispose();
        }

        /// <summary>
        /// 믹서방출처리
        /// </summary>
        /// <param name="tmpDatas"></param>
        private void XSetMixerOutput(DataTable dtPlc, string sProcessKey, string sLCode, int iPlcNo)
        {
            try
            {
                vMixComplete = true;
                DataSet Rs1 = null;
                int[] tmpDatas = new int[10];

                Array.Clear(tmpDatas, 0, tmpDatas.Length);

                clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.MIXINGCOMPLETE.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "", Cimon_Job_Data, MIXING_COMPLETE);

                if (tmpDatas[0] == 1)
                {
                    clsLog.logSave("믹서방출완료", 0, "PLC");

                    //진행중 작지번호 읽기
                    Array.Clear(tmpDatas, 0, tmpDatas.Length);

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.MIXINGINFO.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "", Cimon_Job_Data, MIXING_WORK_YYYY);

                    if (int.Parse(tmpDatas[0].ToString()) == 0)
                    {
                        clsLog.logSave("********************믹서 완료는 받았지만 작업일자가 없습니다.********************", 0, "PLC");
                        vMixComplete = false;
                        return;
                    }

                    vWorkDate = tmpDatas[0].ToString().PadLeft(4, '0') + tmpDatas[1].ToString().PadLeft(4, '0');

                    if (int.Parse(tmpDatas[2].ToString()) == 0)
                    {
                        clsLog.logSave("********************믹서 완료는 받았지만 작업번호가 없습니다.********************", 0, "PLC");
                        vMixComplete = false;
                        return;
                    }

                    vWorkNum = tmpDatas[2].ToString();

                    clsLog.logSave($"믹서작업일자 {vWorkDate}, 작업순번 {vWorkNum} ", 0, "PLC");

                    Array.Clear(tmpDatas, 0, tmpDatas.Length);

                    //배치수 
                    int batch = 0;

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.MIXINGCURRBATCH.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    batch = PlcFunc.PlcGetDevice(dtPlc, iPlcNo, vPLCAddress, vPLCDataCount, Cimon_Job_Data, MIXING_BATCH);

                    if (int.Parse(batch.ToString()) == 0)
                    {
                        clsLog.logSave("********************믹서 완료는 받았지만 배치번호가 없습니다.********************", 0, "PLC");
                        vMixComplete = false;
                        return;
                    }

                    //clsLog.logSave($"믹서배치수 {tmpDatas[0]}", 0);
                    // GBG
                    clsLog.logSave($"믹서 완료 - 작업일자 [{vWorkDate}] 순번 [{vWorkNum}] 배치 [{tmpDatas[0]}]", 1, "PLC");
                    // GBG -


                    //제품명 가져오기
                    if (vWorkNum != "0")
                    {
                        SQL = $@"
                            SELECT WO.PROCESS_KEY, WO.RESOURCE_NO, P.DESCRIPTION
                            FROM WORK_ORDER WO
                                JOIN SAP_DI_PRODUCT P ON WO.RESOURCE_NO = P.RESOURCE_NO
                            WHERE WO.PLANT_CODE   = '{clsCommon.PlantCode}'
                                AND WO.PROCESS_KEY  IN ('{sProcessKey}')
                                AND WO.L_CODE       = '{sProcessKey.Merge(sLCode)}'
                                AND WO.WORKDATE     = '{vWorkDate}'
                                AND WO.NUM          = '{vWorkNum}'
                            ";

                        using (DataSet pSearchDs = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(pSearchDs) > 0)
                            {
                                vResourceName = Dbconn.conn.getData(pSearchDs, "DESCRIPTION", 0);
                                vResourceNo = Dbconn.conn.getData(pSearchDs, "RESOURCE_NO", 0);
                            }
                            else
                            {
                                vResourceName = string.Empty;
                            }
                        }
                    }

                    if (sProcessKey == clsCommon.GetProcessKey("배합") && sLCode.Replace(sProcessKey, "") == "2" || sProcessKey == clsCommon.GetProcessKey("갓돈배합") || processGubun == 2)
                    {
                        // gubun : 1 배치 2 믹싱
                        PlcSetBatchResult(2, batch, dtPlc, iPlcNo, sProcessKey, sLCode);
                    }

                    int tbin = 0;

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.WORKINFO.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "", Cimon_Job_Data, MIXING_WORK_YYYY);

                    tbin = tmpDatas[4];

                    clsLog.logSave($"믹서목적빈 {tbin}", 0, "PLC");

                    SQL = $@"
                    SELECT RESOURCE_NO, LOCATION_ED
                    FROM WORK_ORDER
                    WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND WORKDATE     = '{vWorkDate}'
                        AND NUM          = '{vWorkNum}'
                    ";

                    DataSet pInfoDs = Dbconn.conn.ExecutDataset(SQL);

                    string t_p_code = string.Empty;
                    string t_bin_code = string.Empty;
                    if (Dbconn.conn.getRowCnt(pInfoDs) > 0)
                    {
                        t_p_code = Dbconn.conn.getData(pInfoDs, "RESOURCE_NO", 0);
                        t_bin_code = Dbconn.conn.getData(pInfoDs, "LOCATION_ED", 0);
                    }
                    else
                    {
                        clsLog.logSave("frm_Dosing", "믹싱타이머", "작업지시번호 찾기 실패 " + SQL);
                    }

                    clsLog.logSave(SQL, 0);

                    SQL = $@"
                    SELECT NVL(SUM(P_Q), 0) AS SM_PQ
                    FROM WORK_REMARK
                    WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND WORKDATE     = '{vWorkDate}'
                        AND NUM          = '{vWorkNum}'
                        AND BATCH        = '{batch}'
                    ";

                    DataSet tbinSumDs = Dbconn.conn.ExecutDataset(SQL);

                    clsLog.logSave(SQL, 0);

                    decimal t_sum_pq = 0;
                    if (Dbconn.conn.getRowCnt(tbinSumDs) == 1)
                    {
                        t_sum_pq = Convert.ToDecimal(Dbconn.conn.getData(tbinSumDs, "SM_PQ", 0));
                    }

                    // 품목코드 , 품목명 조회 
                    SQL = $@"
                     SELECT 
                         WO.PROCESS_KEY
                       , P.RESOURCE_NO
                       , P.DESCRIPTION
                       , WO.LOCATION_ED
                    FROM WORK_ORDER WO
                        JOIN SAP_DI_PRODUCT P ON P.PLANT_CODE = wo.PLANT_CODE AND WO.RESOURCE_NO = P.RESOURCE_NO
                    WHERE WO.PLANT_CODE  = '{clsCommon.PlantCode}'
                      AND WO.PROCESS_KEY = '{sProcessKey}'
                      AND WO.L_CODE      = '{sProcessKey.Merge(sLCode)}'
                      AND WO.WORKDATE    = '{vWorkDate}'
                      AND WO.NUM         = '{vWorkNum}'
                    ";

                    Rs1 = Dbconn.conn.ExecutDataset(SQL);
                    string p_code = Dbconn.conn.getData(Rs1, "RESOURCE_NO", 0);
                    string p_name = Dbconn.conn.getData(Rs1, "DESCRIPTION", 0);

                    SQL = $@"
                    SELECT 
                         SUM(BATCH_Q) AS BATCH_Q
                       , SUM(PRO_Q) AS PRO_Q
                    FROM (
                        SELECT 
                             NVL(SUM(P_Q), 0) AS BATCH_Q
                           , 0 AS PRO_Q
                        FROM WORK_REMARK
                        WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                            AND PROCESS_KEY = '{sProcessKey}'
                            AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                            AND WORKDATE = '{vWorkDate}'
                            AND NUM = '{vWorkNum}'
                            AND BATCH = '{batch}'
                        UNION ALL
                        SELECT 
                             0 AS BATCH_Q
                           , NVL(SUM(P_Q), 0) AS PRO_Q
                        FROM WORK_REMARK
                        WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                            AND PROCESS_KEY = '{sProcessKey}'
                            AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                            AND WORKDATE = '{vWorkDate}'
                            AND NUM = '{vWorkNum}'
                    )";

                    Rs1 = Dbconn.conn.ExecutDataset(SQL);

                    string batch_q = Dbconn.conn.getData(Rs1, "BATCH_Q", 0);
                    string pro_q = Dbconn.conn.getData(Rs1, "PRO_Q", 0);

                    // 빈에 따른 재고 가져오기 
                    SQL = $@"
                    SELECT NVL(STOCK, 0) AS STOCK
                    FROM BIN
                    WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND LOCATION = '{tbin}'
                    ";

                    Rs1 = Dbconn.conn.ExecutDataset(SQL);

                    // 믹싱시간
                    int mix_time = 0;

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.MIXINGTIME.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    mix_time = PlcFunc.PlcGetDevice(dtPlc, iPlcNo, vPLCAddress, vPLCDataCount, Cimon_Job_Data, MIXING_TIME_PV);

                    clsLog.logSave($"믹서 믹싱시간 {mix_time}", 0, "PLC");

                    //SQL = $@"
                    //MERGE INTO BATCH d
                    //USING (
                    //        SELECT '{clsCommon.PlantCode}'           AS PLANT_CODE
                    //             , '{sProcessKey}'                  AS PROCESS_KEY
                    //             , '{sProcessKey.Merge(sLCode)}'    AS L_CODE
                    //             , '{vWorkDate}'                    AS WORKDATE
                    //             , '{vWorkNum}'                     AS NUM
                    //             , '{batch}'                        AS BATCH
                    //             , '{batch_q}'                      AS BA_Q
                    //             , '{mix_time}'                     AS MIX_T
                    //             , '0'                              AS DRY_T
                    //             , '0'                              AS RMIX_T
                    //             , SYSDATE                          AS ADDTM
                    //             , SYSDATE                          AS I_TIME
                    //             , '0'                              AS SC3_TR_TIME
                    //             , '0'                              AS SC4_TR_TIME
                    //             , '0'                              AS LQ_T
                    //             , '0'                              AS MIX_DOWN_TR_TIME
                    //        FROM DUAL
                    //      ) s
                    //ON (
                    //       d.PLANT_CODE  = s.PLANT_CODE
                    //   AND d.PROCESS_KEY = s.PROCESS_KEY
                    //   AND d.L_CODE      = s.L_CODE
                    //   AND d.WORKDATE    = s.WORKDATE
                    //   AND d.NUM         = s.NUM
                    //   AND d.BATCH       = s.BATCH
                    //)
                    //WHEN NOT MATCHED THEN
                    //    INSERT (
                    //          PLANT_CODE
                    //        , PROCESS_KEY
                    //        , L_CODE
                    //        , WORKDATE
                    //        , NUM
                    //        , BATCH
                    //        , BA_Q
                    //        , MIX_T
                    //        , DRY_T
                    //        , RMIX_T
                    //        , ADDTM
                    //        , I_TIME
                    //        , SC3_TR_TIME
                    //        , SC4_TR_TIME
                    //        , LQ_T
                    //        , MIX_DOWN_TR_TIME
                    //    )
                    //    VALUES (
                    //          s.PLANT_CODE
                    //        , s.PROCESS_KEY
                    //        , s.L_CODE
                    //        , s.WORKDATE
                    //        , s.NUM
                    //        , s.BATCH
                    //        , s.BA_Q
                    //        , s.MIX_T
                    //        , s.DRY_T
                    //        , s.RMIX_T
                    //        , s.ADDTM
                    //        , SYSDATE
                    //        , s.SC3_TR_TIME
                    //        , s.SC4_TR_TIME
                    //        , s.LQ_T
                    //        , s.MIX_DOWN_TR_TIME
                    //    )
                    //";

                    Dbconn.conn.SQLrun(SQL);

                    Dbconn.conn.Commit();

                    clsLog.logSave(vWorkDate + ":" + vWorkNum + " 믹싱 완료", 1, "PLC");

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                            , sProcessKey
                                            , sLCode
                                            // GBG
                                            , iPlcNo
                                            // GBG -
                                            , PlcAddressType.MIXINGCOMPLETE.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    int[] temp = new int[2];
                    temp[0] = 2;

                    PlcFunc.PlcSetQDeviceEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, null, 0, temp, "믹싱 완료를 실패 했습니다.");

                    Thread.Sleep(500);

                    Rs1.Dispose();

                } //믹서방출 END
            }
            finally
            {
                vMixComplete = false;
            }
        }

        /// <summary>
        /// 작업완료
        /// </summary>
        /// <param name="rData"></param>
        /// <param name="tmpDatas"></param>
        /// <param name="SQL"></param>
        /// <returns></returns>
        private string XSetWorkEnd(DataTable dtPlc, string sProcessKey, string sLCode, int iPlcNo)
        {
            try
            {
                vWorkComplete = true;
                // GBG
                //int[] tmpDatas = new int[5];
                int[] tmpDatas = new int[10];
                // GBG -
                int[] rData = new int[10];

                //작업완료 PLC가 플래그 읽기
                Array.Clear(rData, 0, rData.Length);

                clsCommon.GetPLCAddress(clsCommon.PlantCode
                                        , sProcessKey
                                        , sLCode
                                        // GBG
                                        , iPlcNo
                                        // GBG -
                                        , PlcAddressType.WORKCOMPLETE.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                rData[0] = PlcFunc.PlcGetDevice(dtPlc, iPlcNo, vPLCAddress, vPLCDataCount, Cimon_Job_Data, JOB_COMPLETE);

                //작업완료 PLC 플래그가 ON일 경우
                if (rData[0] == 1)
                {
                    //진행중인 작업지시번호 PLC에서 읽기
                    Array.Clear(tmpDatas, 0, tmpDatas.Length);

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                        , sProcessKey
                                        , sLCode
                                        // GBG
                                        , iPlcNo
                                        // GBG -
                                        , PlcAddressType.WORKINFO.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                    PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, vWorkDate + "/" + vWorkNum + ":" + vBatchNum + "작지 PLC 읽기 실패", Cimon_Job_Data, WORK_YYYY);

                    //작업지시 형식으로 재구성
                    string workdate = tmpDatas[0].ToString().PadLeft(4, '0') + tmpDatas[1].ToString().PadLeft(4, '0');

                    if (int.Parse(workdate) == 0)
                    {
                        clsLog.logSave("********************작업 완료는 받았지만 작업일자가 없습니다.********************", 0, "PLC");
                        vWorkComplete = false;
                        return "";
                    }

                    string worknum = tmpDatas[2].ToString();

                    if (int.Parse(worknum) == 0)
                    {
                        clsLog.logSave("********************작업 완료는 받았지만 작업번호가 없습니다.********************", 0, "PLC");
                        vWorkComplete = false;
                        return "";
                    }

                    // GBG
                    clsLog.logSave($"작업 완료 - 작업일자 [{workdate}] 순번 [{worknum}] ", 0, "PLC");
                    // GBG -

                    SQL = $@"
                    SELECT WO.RESOURCE_NO, WO.LOCATION_ED, P.DESCRIPTION
                    FROM WORK_ORDER WO
                        LEFT OUTER JOIN SAP_DI_PRODUCT P ON P.PLANT_CODE = WO.PLANT_CODE AND WO.RESOURCE_NO = P.RESOURCE_NO
                    WHERE WO.PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND WO.PROCESS_KEY = '{sProcessKey}'
                        AND WO.L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND WO.WORKDATE    = '{workdate}'
                        AND WO.NUM         = '{worknum}'
                    ";

                    DataSet pInfoDs = Dbconn.conn.ExecutDataset(SQL);

                    string t_p_code = string.Empty;
                    string t_bin_code = string.Empty;
                    string t_p_desc = string.Empty;

                    if (Dbconn.conn.getRowCnt(pInfoDs) > 0)
                    {
                        t_p_code = Dbconn.conn.getData(pInfoDs, "RESOURCE_NO", 0);
                        t_bin_code = Dbconn.conn.getData(pInfoDs, "LOCATION_ED", 0);
                        t_p_desc = Dbconn.conn.getData(pInfoDs, "DESCRIPTION", 0);
                    }
                    else
                    {
                        clsLog.logSave("frm_Dosing", "완료신호", "작업지시번호 찾기 실패 " + SQL);
                    }

                    SQL = $@"
                    SELECT NVL(SUM(P_Q), 0) AS SM_PQ
                    FROM WORK_REMARK
                    WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                        AND WORKDATE     = '{workdate}'
                        AND NUM          = '{worknum}'
                    ";

                    DataSet totalSumDs = Dbconn.conn.ExecutDataset(SQL);

                    decimal t_sum_pq = 0;

                    if (Dbconn.conn.getRowCnt(totalSumDs) == 1)
                    {
                        t_sum_pq = Convert.ToDecimal(Dbconn.conn.getData(totalSumDs, "SM_PQ", 0));
                    }

                    //SQL = $@"
                    //UPDATE BIN
                    //SET STOCK = STOCK + ('{t_sum_pq}' * 1),
                    //    I_TIME     = SYSDATE
                    //WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                    //    AND LOCATION = '{t_bin_code}'
                    //";
                    //// GBG -

                    //Dbconn.conn.SQLrun(SQL);

                    // 작업지시 진행상태값 및 종료시간 업데이트 
                    // GBG 
                    //SQL = $@"
                    //UPDATE WORK_ORDER
                    //SET PRO_Q        = '{t_sum_pq}',
                    //    c_condition  = '{(int)enumPcStatus.Completed}',
                    //    END_TIME     = SYSDATE
                    //WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                    //AND PROCESS_KEY = '{sProcessKey}'
                    //AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                    //AND WORKDATE     = '{workdate}'
                    //AND NUM          = '{worknum}'
                    //";
                    //SQL = $@"
                    //UPDATE WORK_ORDER
                    //SET PRO_Q        = '{t_sum_pq}',
                    //    c_condition  = '{((int)enumPcStatus.Completed).ToString().PadLeft(6, '0')}',
                    //    END_TIME     = SYSDATE
                    //WHERE PLANT_CODE  = '{clsCommon.PlantCode}'
                    //AND PROCESS_KEY = '{sProcessKey}'
                    //AND L_CODE      = '{sProcessKey.Merge(sLCode)}'
                    //AND WORKDATE     = '{workdate}'
                    //AND NUM          = '{worknum}'
                    //";
                    //// GBG -

                    //Dbconn.conn.SQLrun(SQL);

                    // plc 작업지시 영역 초기화 
                    clsLog.logSave(workdate + worknum + " 작업완료", 1);

                    //작업진행 LOG
                    string logMsg = "도징 작업이 완료되었습니다";
                    if (!clsProcessDosing.InsertLog(clsCommon.PlantCode, sProcessKey, sProcessKey.Merge(sLCode), workdate, worknum, "0", "031102", logMsg))
                    {
                        clsLog.logSave(this, "work_watch_timer_Tick", "작업로그 입력에 실패하였습니다/ " + workdate + "/" + worknum + "/" + "0");
                    }

                    Thread.Sleep(500);

                    SetDosingWorkInfo(vWorkDate, vWorkNum, vResourceName, vBatchNum);

                    clsCommon.GetPLCAddress(clsCommon.PlantCode
                                        , sProcessKey
                                        , sLCode
                                        // GBG
                                        , iPlcNo
                                        // GBG -
                                        , PlcAddressType.WORKCOMPLETE.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                    tmpDatas[0] = 2;

                    PlcFunc.PlcSetQDeviceEx(dtPlc, iPlcNo, vPLCAddress, vPLCUnit, vPLCDataCount, null, 0, tmpDatas, "작업완료가 실패 했습니다.");

                    if (clsCommon.GetAutoTrans() == "Y")
                    {
                        string pellet = string.Empty;

                        clsCommon.GetAutoPellet(clsCommon.PlantCode, sProcessKey, t_p_code, out pellet);

                        SQL = $@"
                        SELECT 1
                        FROM WORK_ORDER
                         WHERE PLANT_CODE = '{clsCommon.PlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sProcessKey.Merge(sLCode)}'
                                    AND WORKDATE = TO_CHAR(TO_DATE('{workdate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{worknum}'
                            AND (ERP_OSTATUS IN ('N') OR ERP_ISTATUS IN ('N'))
                            AND DEL_FLAG != 'Y'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(ds) > 0)
                        {
                            //SQL = $@"
                            //UPDATE WORK_ORDER
                            //SET   ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) 
                            //                        WHEN 'N' THEN 'F'
                            //                    ELSE TO_CHAR(ERP_OSTATUS) END
                            //    , ERP_OERR_CNT = 0
                            //    , ERP_ISTATUS = CASE WHEN '{pellet}' IS NULL 
                            //                        THEN CASE TO_CHAR(ERP_ISTATUS) 
                            //                                WHEN 'N' THEN 'F'
                            //                            ELSE TO_CHAR(ERP_ISTATUS) END 
                            //                    ELSE TO_CHAR(ERP_ISTATUS)
                            //                    END
                            //    , ERP_IERR_CNT = 0
                            //WHERE PLANT_CODE = '{clsCommon.PlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sProcessKey.Merge(sLCode)}'
                            //        AND WORKDATE = TO_CHAR(TO_DATE('{workdate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{worknum}'
                            //        AND DEL_FLAG != 'Y'
                            //";

                            //if (Dbconn.conn.SQLrun(SQL) < 1)
                            //{
                            //    clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            //    ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            //}
                        }
                    }

                    if (sPlantCode != "PJ01")
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm == null || frm.IsDisposed || !wStatus)
                                continue;

                            Control btn = FindControlRecursive(frm, "btn_workSearch");

                            if (btn != null && btn is SimpleButton simpleBtn)
                            {
                                // DevExpress SimpleButton인 경우
                                simpleBtn.PerformClick();
                            }
                            else if (btn != null && btn is Button normalBtn)
                            {
                                // 일반 WinForms Button인 경우
                                normalBtn.PerformClick();
                            }
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            finally
            {
                vWorkComplete = false;
            }

            return SQL;
        }
        // GBG
        //private static int[] PlcGetReadQDeviceBlockEx(DataTable dt, string sPLCAddress, int iPLCDataCount, int sourceIndex = 0, string sMsg = "")


        // GBG
        //private static void ReadResult()
        //{
        //    int[] scaleSize = new int[5] { 100, 100, 60, 60, 60 };

        //    if (Cimon_Result_Data != null)
        //    {
        //        Thread.Sleep(500);
        //        if (Cimon_Result_Data_1 != null)
        //        {
        //            Array.Clear(Cimon_Result_Data_1, 0, Cimon_Result_Data_1.Length);
        //            if (clsCimonHandler2.Read(0, "D27000", scaleSize[0], Cimon_Result_Data_1) == 0)
        //            {
        //                Thread.Sleep(500);
        //                if (clsCimonHandler2.Read(0, "D27000", scaleSize[0], Cimon_Result_Data_1) == 0)
        //                {
        //                }
        //            }
        //        }

        //        Array.Copy(Cimon_Result_Data_1, 0, Cimon_Result_Data, 0, 100);

        //        Thread.Sleep(500);
        //        if (Cimon_Result_Data_2 != null)
        //        {
        //            Array.Clear(Cimon_Result_Data_2, 0, Cimon_Result_Data_2.Length);
        //            if (clsCimonHandler2.Read(0, "D27100", scaleSize[1], Cimon_Result_Data_2) == 0)
        //            {
        //                Thread.Sleep(500);
        //                if (clsCimonHandler2.Read(0, "D27100", scaleSize[1], Cimon_Result_Data_2) == 0)
        //                {
        //                }
        //            }
        //        }

        //        Array.Copy(Cimon_Result_Data_2, 0, Cimon_Result_Data, 100, 100);

        //        Thread.Sleep(500);
        //        if (Cimon_Result_Data_3 != null)
        //        {
        //            Array.Clear(Cimon_Result_Data_3, 0, Cimon_Result_Data_3.Length);
        //            if (clsCimonHandler2.Read(0, "D27200", scaleSize[2], Cimon_Result_Data_3) == 0)
        //            {
        //                Thread.Sleep(500);
        //                if (clsCimonHandler2.Read(0, "D27200", scaleSize[2], Cimon_Result_Data_3) == 0)
        //                {
        //                }
        //            }
        //        }

        //        Array.Copy(Cimon_Result_Data_3, 0, Cimon_Result_Data, 200, 100);

        //        Thread.Sleep(500);
        //        if (Cimon_Result_Data_4 != null)
        //        {
        //            Array.Clear(Cimon_Result_Data_4, 0, Cimon_Result_Data_4.Length);
        //            if (clsCimonHandler2.Read(0, "D27300", scaleSize[3], Cimon_Result_Data_4) == 0)
        //            {
        //                Thread.Sleep(500);
        //                if (clsCimonHandler2.Read(0, "D27300", scaleSize[3], Cimon_Result_Data_4) == 0)
        //                {
        //                }
        //            }
        //        }

        //        Array.Copy(Cimon_Result_Data_4, 0, Cimon_Result_Data, 300, 100);

        //        Thread.Sleep(500);
        //        if (Cimon_Result_Data_5 != null)
        //        {
        //            Array.Clear(Cimon_Result_Data_5, 0, Cimon_Result_Data_5.Length);
        //            if (clsCimonHandler2.Read(0, "D27400", scaleSize[4], Cimon_Result_Data_5) == 0)
        //            {
        //                Thread.Sleep(500);
        //                if (clsCimonHandler2.Read(0, "D27400", scaleSize[4], Cimon_Result_Data_5) == 0)
        //                {
        //                }
        //            }
        //        }

        //        Array.Copy(Cimon_Result_Data_5, 0, Cimon_Result_Data, 400, 100);
        //    }
        //}
        // GBG -

        // GBG
        private static void ReadResult()
        {
            int[] scaleSize = new int[5] { 100, 100, 60, 60, 60 };
            int addr = 27000;

            if (Cimon_Result_Data != null)
            {
                for (int i = 0; i < scaleSize.Length; i++)
                {
                    if (Cimon_Result_Data_Scale != null)
                    {
                        Array.Clear(Cimon_Result_Data_Scale, 0, Cimon_Result_Data_Scale.Length);
                        if (clsCimonHandler2.Read(0, $"D{addr + (i * 100)}", scaleSize[i], Cimon_Result_Data_Scale) == 0)
                        {
                            Thread.Sleep(500);
                            if (clsCimonHandler2.Read(0, $"D{addr + (i * 100)}", scaleSize[i], Cimon_Result_Data_Scale) == 0)
                            {
                            }
                        }
                    }

                    Array.Copy(Cimon_Result_Data_Scale, 0, Cimon_Result_Data, (i * 100), 100);
                    Thread.Sleep(500);
                }
            }
        }
        // GBG -

        private static void SetDosingWorkInfo(string vWorkDate, string vWorkNum, string vResourceName, string vBatchNum)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f.Focused || f.ContainsFocus)   // 현재 포커스를 가진 폼
                {
                    Control[] ctrls = f.Controls.Find("txtEdt_runWorkNum", true);
                    if (ctrls.Length > 0)
                    {
                        ctrls[0].Text = vWorkDate;
                    }

                    ctrls = f.Controls.Find("txtEdt_runNum", true);
                    if (ctrls.Length > 0)
                    {
                        ctrls[0].Text = vWorkNum;
                    }

                    ctrls = f.Controls.Find("txtEdt_runWorkProduct", true);
                    if (ctrls.Length > 0)
                    {
                        ctrls[0].Text = vResourceName;
                    }

                    ctrls = f.Controls.Find("txtEdt_runWorkBatch", true);
                    if (ctrls.Length > 0)
                    {
                        ctrls[0].Text = vBatchNum;
                    }

                    ctrls = f.Controls.Find("comboBoxEdit_workMode", true);
                    if (ctrls.Length > 0)
                    {
                        vWorkMode = ctrls[0].Text == "Y" ? true : false;
                    }

                    break;
                }
            }
        }

        static Form GetActiveTopForm()
        {
            // 1) 우선 현재 ActiveForm
            var f = Form.ActiveForm;

            // 2) MDI Parent면 Child를 우선 사용
            if (f?.ActiveMdiChild is Form child) f = child;

            // 3) 그래도 못 찾으면 포커스 가진 폼
            if (f == null)
                f = Application.OpenForms.Cast<Form>()
                     .FirstOrDefault(x => x.ContainsFocus)
                    ?? Application.OpenForms.Cast<Form>().FirstOrDefault();

            return f;
        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            Screen_attr();
        }

        Control FindControlRecursive(Control parent, string controlName)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Name == controlName)
                    return ctrl;

                Control found = FindControlRecursive(ctrl, controlName);
                if (found != null)
                    return found;
            }
            return null;
        }

        private void barEditItem_Delay_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                mDelay delay = new mDelay();
                delay.StartPosition = FormStartPosition.CenterScreen;
                delay.ShowDialog();

                barEditItem_Delay.EditValue = Properties.Settings.Default.Delay;
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowWarning("딜레이 수정에 실패 했습니다.");
            }
        }

        private void ShowSplahScreenManager(string sMsg)
        {
            if (splashScreenManager_loding.IsSplashFormVisible)
            {
                splashScreenManager_loding.CloseWaitForm();
            }

            splashScreenManager_loding.ShowWaitForm();

            splashScreenManager_loding.SetWaitFormCaption(sMsg);
        }
    }
}