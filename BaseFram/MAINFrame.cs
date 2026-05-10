using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraTabbedMdi;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Core;
using Core.Class;
using Core.Extension;

namespace HM_WAP_JEIL_DOSING
{
    public partial class MAINFrame : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 실행 경로y
        /// </summary>
        private string _strExecutablePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public static string PlcConnChk = string.Empty;

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

        public static clsCommon login_info = new clsCommon();

        public MAINFrame()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Text = login_info.ProgramName;

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

                accordionControlElement_menu6.Visible = true;

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Screen_attr", ex);
                ShowMessageBox.XtraShowWarning("화면권한처리를 하는 도중 에러가 발생하였습니다");
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

                if (clsCommon._strUserId == "kfirst")
                {
                    accordionControlElement1_emp.Text = "사용자 관리";
                    accordionControlElement1_scAttr.Text = "사용자별 접근권한";

                    accordionControlElement2_ingred.Text = "원부재료관리";
                    accordionControlElement2_product.Text = "제품 및 배합비관리";

                    accordionControlElement3_erpStock.Text = "관내재고현황(ERP)";
                    accordionControlElement3_stockChange.Text = "계정대체관리";

                    accordionControlElement4_DosingResult.Text = "배치별 작업일지";

                    accordionControlElement5_bulkOrder.Text = "벌크 작업/상차관리";
                    accordionControlElement5_bagOrder.Text = "타이콘 작업/상차관리";
                    accordionControlElement5_weightIn.Text = "입차내역관리";
                    accordionControlElement5_weightOut.Text = "출차내역관리";
                    accordionControlElement5_weightEtc.Text = "계근기타내역관리";
                    accordionControlElement5_expCar.Text = "계근예외차량관리";
                    accordionControlElement5_wapCustMapping.Text = "공급사별 납품원료관리";
                }

                splashScreenManager_loding.ShowWaitForm();

                splashScreenManager_loding.SetWaitFormCaption("로그인 유저를 설정하는중입니다");
                LoginInfo_Reg(login_info.UserName);

                //DB, PLC 접속상태 감시
                conn_witch_timer.Interval = 2000;
                conn_witch_timer.Enabled = true;



            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "MAIN_Load", ex);
                ShowMessageBox.XtraShowWarning("화면을 불러오는 도중 에러가 발생하였습니다");
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
                //AccordionControlElement accControl = sender as AccordionControlElement;
                //if ( accControl.Tag == null)
                //{
                //    return;
                //}

                //string nameSpace = "HM_WAP_JEIL_DOSING";
                //Assembly cuasm = Assembly.GetExecutingAssembly();
                //Form frm = (Form)cuasm.CreateInstance(string.Format("{0}.{1}", nameSpace, accControl.Tag.ToString()));
                //frm.Text = accControl.Text;

                //splashScreenManager_loding.ShowWaitForm();
                //splashScreenManager_loding.SetWaitFormCaption("페이지 로딩중입니다");

                //FormDisp(frm);

                ////SetMenuPath(activeUI.MENUID, ActiveMdiChild.Text);
                ///
                AccordionControlElement aceZeroLevel = null;// new AccordionControlElement();
                AccordionControlElement aceOneLevel = null;// new AccordionControlElement();
                AccordionControlElement aceTwoLevel = null;// new AccordionControlElement();

                DataRow[] dr = null;
                int iLevel = 0;

                DataTable dt = null; // GetMenuInfo();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Select($"PARENTMENU = '{dt.Rows[i]["MENUID"]}'");

                    iLevel = dt.Rows[i]["MENULEVEL"].ToString().Int32Parse();

                    switch (iLevel)
                    {
                        case 0:
                            aceZeroLevel = new AccordionControlElement();
                            break;
                        case 1:
                            aceOneLevel = new AccordionControlElement();
                            break;
                        default:
                            aceTwoLevel = new AccordionControlElement();
                            break;
                    }

                    if (dr.Length > 0)
                        SetAccordionElement(iLevel == 0 ? null : aceZeroLevel, iLevel == 0 ? aceZeroLevel : aceOneLevel, dt.Rows[i]["MENUID"].ToString(), dt.Rows[i]["MENUNAME"].ToString(), iLevel);
                    else
                    {
                        SetAccordionElement(iLevel == 1 ? aceZeroLevel : aceOneLevel, iLevel == 1 ? aceOneLevel : aceTwoLevel, dt.Rows[i]["MENUID"].ToString(), dt.Rows[i]["MENUNAME"].ToString(), dt.Rows[i]["MENULEVEL"].ToString().Int32Parse(), dt.Rows[i]["CLASSNAME"].ToString());
                    }

                    splashScreenManager_loding.ShowWaitForm();
                    splashScreenManager_loding.SetWaitFormCaption("페이지 로딩중입니다");
                }
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

        private void SetAccordionElement(AccordionControlElement aceParentMenu, AccordionControlElement aceChildMenu, string MenuId, string MenuName, int level, string ClassName = null)
        {
            aceChildMenu.Name = MenuId;
            aceChildMenu.Text = MenuName;

            if (ClassName.IsNullEmpty())
            {
                aceChildMenu.Expanded = true;
                aceChildMenu.ImageOptions.Image = global::Core.Properties.Resources.open_32x32;
            }
            else
            {
                aceChildMenu.Tag = ClassName;
                aceChildMenu.ImageOptions.Image = global::Core.Properties.Resources.apply_16x16;
                aceChildMenu.Style = ElementStyle.Item;
                aceChildMenu.Click += new System.EventHandler(this.menuClick);
            }

            if (aceParentMenu == null)
                this.acMenu.Elements.AddRange(new AccordionControlElement[] { aceChildMenu });
            else
                aceParentMenu.Elements.AddRange(new AccordionControlElement[] { aceChildMenu });
        }

        /// <summary>
        /// accordion 메뉴 도징,대용유 클릭 이벤트 (도징화면 공통 분할)
        /// </summary>
        private void menuDosingClick(object sender, EventArgs e)
        {
            try
            {
                AccordionControlElement accControl = sender as AccordionControlElement;

                string form_name = string.Empty;
                Form dosingForm = null;
                //if (accControl.Text.ToString().Equals("배합 작업지시관리"))
                //{
                //    form_name = "frm_dosing";
                //    dosingForm = new frm_Dosing(clsCommon.dosing_process_code);
                //}
                //else
                //{
                //    form_name = "frm_milk";
                //    dosingForm = new frm_Dosing(clsCommon.milk_process_code);
                //}

                dosingForm.Name = form_name;
                dosingForm.Text = accControl.Text;

                splashScreenManager_loding.ShowWaitForm();
                splashScreenManager_loding.SetWaitFormCaption("페이지 로딩중입니다");

                if (frmDupChk(form_name) == true)
                {
                    dosingForm.MdiParent = this;

                    xtraTabbedMdiManager.BeginUpdate();
                    dosingForm.Show();
                    dosingForm.Update();
                    xtraTabbedMdiManager.EndUpdate();
                }
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

        /// <summary>
        /// 메인폼 닫히기전 이벤트
        /// </summary>
        private void MAIN_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = ShowMessageBox.FlyoutConfirm(this, "프로그램 종료안내", "프로그램을 종료하시겠습니까?", "프로그램종료", "취소");

            if (result == DialogResult.Yes)
            {
                Dbconn.conn.Close();
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
            if (Dbconn.conn.getState().Equals("Open"))
            {
                barEditItem_dbConnSt.EditValue = "접속완료";
                repotxtEdit_DbConnSt.Appearance.BackColor = Color.SteelBlue;
            }
            else
            {
                barEditItem_dbConnSt.EditValue = "접속실패";
                repotxtEdit_DbConnSt.Appearance.BackColor = Color.Red;
            }

            if (clsCommon._strPlcConnYn != "Y")
            {
                barEditItem_plcConnSt.EditValue = "권한없음";
                repotxtEdit_PlcConnSt.Appearance.BackColor = Color.SteelBlue;
            }
            else
            {
                if (PlcConnChk == "Y")
                {
                    barEditItem_plcConnSt.EditValue = "접속완료";
                    repotxtEdit_PlcConnSt.Appearance.BackColor = Color.SteelBlue;
                }
                else if (PlcConnChk == "N")
                {
                    barEditItem_plcConnSt.EditValue = "접속실패";
                    repotxtEdit_PlcConnSt.Appearance.BackColor = Color.Red;
                }
                else if (PlcConnChk == "M")
                {
                    barEditItem_plcConnSt.EditValue = "접속대기";
                    repotxtEdit_PlcConnSt.Appearance.BackColor = Color.SteelBlue;
                }
            }
        }

        private void btn_topRemote_Click(object sender, EventArgs e)
        {
            DialogResult result = ShowMessageBox.Confirm("개발사에 원격제어를 요청하시겠습니까?", "원격제어를 받기위해서는 인터넷에 연결되어있어야 합니다");

            if (result == DialogResult.Yes)
            {
                try
                {

                    FileInfo fi = new FileInfo(Application.StartupPath  + "\\kf-2.exe");

                    if (fi.Exists)
                    {
                        string exe_name = Application.StartupPath + "\\kf-2.exe";
                        Process.Start(exe_name);
                    }
                    else
                    {
                        ShowMessageBox.XtraShowError("원격실행파일을 찾을 수 없습니다");
                    }

                }catch (Exception ex)
                {
                    ShowMessageBox.XtraShowErrorLog("원격을 실행하는도중 에러가 발생하였습니다", this, "btn_topRemote_Click", ex);
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
                LoginInfo_Reg(login_info.UserName);
                Screen_attr();
                this.Opacity = 1;
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog("로그아웃을 하는 도중 에러가 발생하였습니다", this, "btn_logout_Click", ex);
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

        private void barEditItem_plcConnSt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string result = XtraInputBox.Show("비밀번호를 입력하여 주세요", "확인", "");
            
            if (!string.IsNullOrEmpty(result))
            {
                if (result.Equals("KFIRST"))
                {
                    //mPlcConnSetting mPlcSetting = new mPlcConnSetting();
                    //mPlcSetting.StartPosition = FormStartPosition.CenterScreen;
                    //mPlcSetting.ShowDialog();

                }
                else
                {
                    ShowMessageBox.XtraShowInformation("비밀번호가 맞지않습니다");
                }
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
        internal void SetMenuPath(string menuid, string text)
        {
            //siMenuInfo.Caption = menuid == "" ? "" : String.Format("{0} : {1}", text, (ActiveMdiChild as UIFrame).CLASSNAME);
        }
    }
}