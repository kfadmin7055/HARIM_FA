using Core.Class;
using DevExpress.XtraGrid;
using System;
using System.Data;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_LoginPasswordChange : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string Emp_No = String.Empty;
        public m_LoginPasswordChange(string emp_no)
        {
            InitializeComponent();
            Emp_No = emp_no;
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

            //비번 암호화 SHA-256
            string  shaEncryPass = clsEncryption.SHA256Hash(txt_changePassword.Text);
            
             SQL = $"UPDATE DO_INSA SET PASSWORD = '{shaEncryPass}' WHERE PLANT_CODE = '{clsCommon.PlantCode}' AND EMPLOYEE_NO = '{Emp_No}' ";
            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "btn_save_Click", SQL);
                this.DialogResult = DialogResult.None;
                ShowMessageBox.XtraShowWarning("비밀번호 변경을 실패했습니다");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void m_PasswordChange_Load(object sender, EventArgs e)
        {
            SQL = "SELECT PASSWORD, NVL(NAME,' ') as NAME FROM DO_INSA WHERE EMPLOYEE_NO = '{0}' ";
            SQL = string.Format(SQL, Emp_No);

            DataSet emp_ds = Dbconn.conn.ExecutDataset(SQL);

            
            if (Dbconn.conn.getRowCnt(emp_ds) > 0)
            {
                LabelItem_userName.Text = Dbconn.conn.getData(emp_ds, "NAME", 0);
            }
        }
    }
}