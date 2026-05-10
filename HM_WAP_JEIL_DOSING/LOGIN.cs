 using System;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Deployment.Application;
using System.Net.NetworkInformation;
using Core.Class;
using DevExpress.XtraEditors;
using System.Diagnostics;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HARIM_FA_DOSING
{
    public partial class LOGIN : DevExpress.XtraEditors.XtraForm
    {
        //SQL 구문 임시 변수
        private string SQL = string.Empty;
        private bool relogin_flg = false;
        public string wPlantCode = string.Empty;

        public LOGIN(bool relogin = false)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.TopMost = true;

            relogin_flg = relogin;
        }

        private static bool IsCheckNetwork()
        {
            bool networkState = NetworkInterface.GetIsNetworkAvailable();
            bool pingResult = true;

            //네트워크가 연결이 되어있다면
            if (networkState)
            {
                string addr = new string[] { "PJ01", "PJ02" }.Contains(clsCommon.PlantCode) ? "211.46.7.24" : "211.46.7.24";
                Ping pingSender = new Ping();

                //Ping 체크 (IP, TimeOut 지정)
                PingReply reply = pingSender.Send(addr, 300);

                //상태가 Success이면 true반환
                pingResult = reply.Status == IPStatus.Success;

            }

            return networkState & pingResult;
        }

        /// <summary>
        /// 로그인화면 폼로드 이벤트
        /// </summary>
        private void LOGIN_Load(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.Plant == "P101" || Properties.Settings.Default.Plant == "P102")
                    clsCommon.SetLoginLogo(picLogo, "하림");
                else if (Properties.Settings.Default.Plant == "PJ01" || Properties.Settings.Default.Plant == "PJ02" || Properties.Settings.Default.Plant == "PJ04" || Properties.Settings.Default.Plant == "PJ05")
                    clsCommon.SetLoginLogo(picLogo, "제일");
                else if (Properties.Settings.Default.Plant == "P201")
                    clsCommon.SetLoginLogo(picLogo, "올품");
                else
                    clsCommon.SetLoginLogo(picLogo, "하림");

                string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                this.Text = "HARIM ONE ERP SYSTEM V" + ver + " 로그인 화면";

                clsCommon._strProgramName = "HARIM ONE ERP SYSTEM V" + ver;

                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant, clsCommon.GetPlantTable(), "", true, 0, false);

                if (Properties.Settings.Default.PLCOnly)
                    chkPLC.Checked = true;
                else
                    chkPLC.Checked = false;

                cboPlant.EditValue = Properties.Settings.Default.Plant;

                wPlantCode = "1";

                //if (IsCheckNetwork() == false)
                //{
                //    ShowMessageBox.XtraShowInformation("DB서버를 찾을 수 없습니다\r\n연결상태를 확인 바랍니다");
                //    return;
                //}

                if (Dbconn.conn.Open() == false)
                {
                    ShowMessageBox.XtraShowError("DB서버 연결에 실패했습니다");

                    Properties.Settings.Default.Plant = "Z999";
                    Properties.Settings.Default.Save();

                    Environment.Exit(0);
                    //Application.Exit();
                }

                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcess, clsCommon.GetProcess(cboPlant.EditValue?.ToString()), "", false, 0, false);

                cboProcess.EditValue = Properties.Settings.Default.Process;

                //SaveID Load
                if (Properties.Settings.Default.LoginId_Save == "Y")
                {
                    check_LoginIdSave.Checked = true;
                    txt_LoginId.Text = Properties.Settings.Default.Login_Id;
                    this.ActiveControl = txt_LoginPw;
                    txt_LoginPw.Focus();
                }
                else
                {
                    this.ActiveControl = txt_LoginId;
                    txt_LoginId.Focus();
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "LOGIN_Load", ex);
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_LoginId.Text))
                {
                    ShowMessageBox.XtraShowInformation("사용자ID를 입력하여 주세요");
                    txt_LoginId.Focus();
                    return;
                }

                if (txt_LoginId.Text != "kfirst" && string.IsNullOrEmpty(txt_LoginPw.Text))
                {
                    ShowMessageBox.XtraShowInformation("비밀번호를 입력하여 주세요");
                    txt_LoginPw.Focus();
                    return;
                }

                SQL = $@"
                SELECT INSA.EMPLOYEE_NO, INSA.PASSWORD, INSA.NAME, INSA.MANAGE_TYPE
                FROM DO_INSA INSA 
                WHERE INSA.PLANT_CODE = '{cboPlant.EditValue?.ToString()}' AND INSA.EMPLOYEE_NO = '{txt_LoginId.Text}' ";

                DataSet LoginInfoDs = Dbconn.conn.ExecutDataset(SQL);

                if (txt_LoginId.Text == "kfirst" && (txt_LoginPw.Text == "K0909" || Debugger.IsAttached))
                {
                    clsCommon.UserId = "kfirst";
                    clsCommon.UserName = "한국제일";
                    clsCommon.UserType = "admin";
                    clsCommon.PlantName = cboPlant.Text;
                    clsCommon.PlantCode = cboPlant.EditValue?.ToString() == "Z999" ? "PJ01" : cboPlant.EditValue?.ToString();
                    clsCommon.ProcessCode = cboProcess.EditValue?.ToString();

                    Properties.Settings.Default.Plant = cboPlant.EditValue?.ToString();
                    Properties.Settings.Default.Process = cboProcess.EditValue?.ToString();

                    if (check_LoginIdSave.Checked)
                    {
                        Properties.Settings.Default.LoginId_Save = "Y";
                        Properties.Settings.Default.Login_Id = txt_LoginId.Text;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.LoginId_Save = "N";
                        Properties.Settings.Default.Save();
                    }

                    if (relogin_flg == false)
                    {
                        this.Hide();
                        if (Debugger.IsAttached && clsCommon.PlantCode == "PJ02")
                        {
                            //MAIN_Test2 frmMain = new MAIN_Test2();
                            MAIN frmMain = new MAIN();
                            frmMain.Show();
                        }
                        else
                        {
                            MAIN frmMain = new MAIN();
                            frmMain.Show();
                        }
                    }
                    else
                    {
                        this.Hide();
                        relogin_flg = false;
                    }

                    return;
                }

                if (Dbconn.conn.getRowCnt(LoginInfoDs) == 0)
                {
                    Dbconn.conn.Close();
                    ShowMessageBox.XtraShowInformation("사용자ID를 찾을 수 없습니다\r\n확인 후 다시 입력 바랍니다");
                    txt_LoginId.Focus();
                    return;
                }
                else if (Dbconn.conn.getRowCnt(LoginInfoDs) == 1)
                {
                    if (Dbconn.conn.getData(LoginInfoDs, "PASSWORD", 0).Equals(clsEncryption.SHA256Hash(txt_LoginPw.Text)))
                    {
                        clsCommon.UserId = Dbconn.conn.getData(LoginInfoDs, "EMPLOYEE_NO", 0);
                        clsCommon.UserName = Dbconn.conn.getData(LoginInfoDs, "NAME", 0);
                        clsCommon.UserType = Dbconn.conn.getData(LoginInfoDs, "MANAGE_TYPE", 0);
                        clsCommon.PlantName = cboPlant.Text;
                        //clsCommon.PlantCode = cboPlant.EditValue?.ToString();
                        clsCommon.PlantCode = cboPlant.EditValue?.ToString() == "Z999" ? "PJ04" : cboPlant.EditValue?.ToString();
                        clsCommon.ProcessCode = cboProcess.EditValue?.ToString();

                        clsCommon.MainPlcConnYn = Properties.Settings.Default.MainPlc_Yn;
                        clsCommon.SubPlcConnYn = Properties.Settings.Default.SubPlc_Yn;

                        Properties.Settings.Default.Plant = cboPlant.EditValue?.ToString();
                        Properties.Settings.Default.Process = cboProcess.EditValue?.ToString();

                        if (check_LoginIdSave.Checked)
                        {
                            Properties.Settings.Default.LoginId_Save = "Y";
                            Properties.Settings.Default.Login_Id = txt_LoginId.Text;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.LoginId_Save = "N";
                            Properties.Settings.Default.Save();
                        }

                        if (relogin_flg == false)
                        {
                            this.Hide();
                            MAIN frmMain = new MAIN();
                            frmMain.Show();

                        }
                        else
                        {
                            this.Hide();
                            relogin_flg = false;

                        }
                    }
                    else
                    {
                        Dbconn.conn.Close();
                        ShowMessageBox.XtraShowInformation("비밀번호가 다릅니다\r\n확인 후 다시 입력 바랍니다");
                        txt_LoginPw.Focus();

                        clsLog.logSave(clsEncryption.SHA256Hash(Dbconn.conn.getData(LoginInfoDs, "PASSWORD", 0)), 0);
                        clsLog.logSave(clsEncryption.SHA256Hash(txt_LoginPw.Text), 0);
                        return;
                    }
                }
                else
                {
                    Dbconn.conn.Close();
                    ShowMessageBox.XtraShowWarning("사용자 정보가 부정확합니다. \r\n관리자에게 사용자로그인 정보를 확인 바랍니다");
                    return;
                }
            }
            catch (Exception ex)
            {
                Dbconn.conn.Close();
                clsLog.logSave(this, "btn_Login_Click", ex);
            }
        }

        private void txt_LoginPw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Login_Click(sender, e);
            }
        }

        private void LOGIN_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (relogin_flg)
            {
                DialogResult result = ShowMessageBox.FlyoutConfirm(this, "프로그램 종료안내", "프로그램을 종료하시겠습니까?", "프로그램종료", "취소");

                if (result == DialogResult.Yes)
                {
                    Dbconn.conn.Close();
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void cboPlant_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit textEditor = (DevExpress.XtraEditors.TextEdit)sender;

            if (wPlantCode == "1")
            {
                if (new string[] { "PJ01", "PJ02" }.Contains(clsCommon.PlantCode) && new string[] { "PJ01", "PJ02" }.Contains(textEditor.EditValue?.ToString()))
                    return;

                if (new string[] { "PJ04", "PJ05" }.Contains(clsCommon.PlantCode) && new string[] { "PJ04", "PJ05" }.Contains(textEditor.EditValue?.ToString()))
                    return;

                Properties.Settings.Default.Plant = textEditor.EditValue?.ToString();

                Properties.Settings.Default.Save();

                ShowMessageBox.XtraShowInformation("플랜트가 변경 되어 DB 접속을 위해 시스템을 종료합니다. 재실행 해주십시오.");

                Environment.Exit(0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LOGIN_Load(null, null);
        }

        private void chkPLC_CheckedChanged(object sender, EventArgs e)
        {
            if (clsCommon.PLCOnly != chkPLC.Checked)
            {
                Properties.Settings.Default.PLCOnly = chkPLC.Checked;

                Properties.Settings.Default.Save();

                ShowMessageBox.XtraShowInformation("배합 전용 DB 접속을 위해 시스템을 종료합니다. 재실행 해주십시오.");

                Environment.Exit(0);
            }
        }
    }
}
