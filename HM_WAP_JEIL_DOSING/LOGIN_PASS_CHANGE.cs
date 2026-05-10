using Core.Class;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class LOGIN_PASS_CHANGE : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        public LOGIN_PASS_CHANGE()
        {
            InitializeComponent();

            txt_password.Properties.UseSystemPasswordChar = true;
            txt_password.Properties.PasswordChar = '*';
            txt_changePassword.Properties.UseSystemPasswordChar = true;
            txt_changePassword.Properties.PasswordChar = '*';
            txt_changeRePassword.Properties.UseSystemPasswordChar = true;
            txt_changeRePassword.Properties.PasswordChar = '*';
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_password.Text))
            {
                ShowMessageBox.XtraShowInformation("기존 비밀번호를 입력하여 주세요");
                this.DialogResult = DialogResult.None;
                txt_password.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txt_changePassword.Text))
            {
                ShowMessageBox.XtraShowInformation("변경할 비밀번호를 입력하여 주세요");
                this.DialogResult = DialogResult.None;
                txt_changePassword.Focus();
                return;
            }

            if (!txt_changePassword.Text.Equals(txt_changeRePassword.Text))
            {
                ShowMessageBox.XtraShowInformation("입력한 새비밀번호와 비밀번호확인에 입력한 내용이 틀립니다");
                this.DialogResult = DialogResult.None;
                txt_changePassword.Focus();
                return;
            }

            SQL = "SELECT PASSWORD FROM DO_INSA WHERE EMPLOYEE_NO = '{0}' ";
            SQL = string.Format(SQL, clsCommon._strUserId);

            DataSet passDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(passDs) > 0)
            {
                string befor_password = Dbconn.conn.getData(passDs, "PASSWORD", 0);

                string shaEncryBeforPass = clsEncryption.SHA256Hash(txt_password.Text);

                if (!befor_password.Equals(shaEncryBeforPass))
                {
                    ShowMessageBox.XtraShowInformation("입력된 기존 비밀번호가 저장된 비밀번호와 틀립니다");
                    this.DialogResult = DialogResult.None;
                    txt_password.Focus();
                    return;
                }

            }
            else
            {
                ShowMessageBox.XtraShowInformation("계정정보를 찾을 수 없습니다");
                this.DialogResult = DialogResult.None;
                return;
            }

            //비번 암호화 SHA-256
            string shaEncryPass = clsEncryption.SHA256Hash(txt_changePassword.Text);

            SQL = "UPDATE DO_INSA SET PASSWORD = '{1}' WHERE EMPLOYEE_NO = '{0}' ";
            SQL = string.Format(SQL,
                clsCommon._strUserId,
                shaEncryPass
                );

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "btn_save_Click", SQL);
                this.DialogResult = DialogResult.None;
                ShowMessageBox.XtraShowWarning("비밀번호 변경을 실패했습니다");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}