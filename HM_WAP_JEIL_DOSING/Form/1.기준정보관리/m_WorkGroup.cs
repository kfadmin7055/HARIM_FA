using Core.Class;
using DevExpress.Office.History;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraRichEdit.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_WorkGroup : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;

        string argWorkNumb = string.Empty;
        string argWorkSeq = string.Empty;
        string argSbno = string.Empty;
        public m_WorkGroup(string work_numb, string work_seq, string sbno)
        {
            argWorkNumb = work_numb;
            argWorkSeq = work_seq;
            argSbno = sbno;

            InitializeComponent();
        }

        public m_WorkGroup()
        {
            InitializeComponent();
        }

        private void m_WorkGroup_Load(object sender, EventArgs e)
        {
            ControlInit();
        }

        private void ControlInit()
        {
            DataSet ds;

            SQL = $@"
            SELECT COMM_DTCODE AS CODE, COMM_DTNM AS NAME
            FROM COMM_DTCODE 
            WHERE WK_DIVCODE = '03' AND COMM_CODE = '90' AND USE_YN = 'Y'
            ";

            ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressUtil.ItemLookUpEditSetup(cboWorkGroup, ds.Tables[0]);

            SQL = $@"
            SELECT COMM_DTCODE AS CODE, COMM_DTNM AS NAME
            FROM COMM_DTCODE 
            WHERE WK_DIVCODE = '03' AND COMM_CODE = '91' AND USE_YN = 'Y'
            ";

            ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressUtil.ItemLookUpEditSetup(cboWorkPlan, ds.Tables[0]);

            SQL = $@"
            SELECT COMM_DTCODE AS CODE, COMM_DTNM AS NAME
            FROM COMM_DTCODE 
            WHERE WK_DIVCODE = '03' AND COMM_CODE = '10' AND USE_YN = 'Y'
            ";

            ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressUtil.ItemLookUpEditSetup(cboWorkProcess, ds.Tables[0]);


            //처음로딩시 도징공정선택
            cboWorkProcess.EditValue = "3202";

            SQL = "SELECT EMPLOYEE_NO AS CODE, NAME FROM DO_INSA ";
            ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressUtil.ItemLookUpEditSetup(cboWorker, ds.Tables[0]);

            //처음로딩시 도징공정선택
            cboWorker.EditValue = "AD0001";

            WORK_START_DATE.EditValue = DateTime.Today;

            dtFromWorkTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            dtFromWorkTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtFromWorkTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            dtFromWorkTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtFromWorkTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            dtFromWorkTime.EditValue = DateTime.Now; // 현재 시간으로 설정
            dtFromWorkTime.Dock = DockStyle.Top;

            dtToWorkTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            dtToWorkTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtToWorkTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            dtToWorkTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtToWorkTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            dtToWorkTime.EditValue = DateTime.Now.AddHours(8); // 현재 시간으로 설정
            dtToWorkTime.Dock = DockStyle.Top;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            string workdate = WORK_START_DATE.DateTime.ToString("yyyy-MM-dd");
            string workstart = dtFromWorkTime.DateTime.ToString("yyyy-MM-dd HH:mm");
            string workend = dtToWorkTime.DateTime.ToString("yyyy-MM-dd HH:mm");
            string icmdesc = cboWorkGroup.EditValue.ToString();
            string icmactive = cboWorkPlan.EditValue.ToString();
            string processkey = cboWorkProcess.EditValue.ToString();
            string name = cboWorker.EditValue.ToString();
            string reasondesc = REASON_DESC.Text;


            try
            {
                Dbconn.conn.BeginTransaction();

                if (string.IsNullOrEmpty(workdate))
                {
                    ShowMessageBox.XtraShowWarning("작업일자를 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(workstart))
                {
                    ShowMessageBox.XtraShowWarning("근무시작시간을 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(workend))
                {
                    ShowMessageBox.XtraShowWarning("근무종료시간을 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(icmdesc))
                {
                    ShowMessageBox.XtraShowWarning("작업조를 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(icmactive))
                {
                    ShowMessageBox.XtraShowWarning("진행여부를 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(processkey))
                {
                    ShowMessageBox.XtraShowWarning("작업공정을 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(name))
                {
                    ShowMessageBox.XtraShowWarning("작업자를 입력하여 주세요");
                    return;
                }


                string SQL = $@"
                INSERT INTO SHIFT_DETAIL (
                   WORK_START_DATE, PROCESS_KEY, DAY_WORK_ST, 
                   DAY_WORK_ET, FIX_CH_YN, PROCESS_RUN_ST, 
                   REASON_DESC, REMARK, I_TIME, 
                   EMPLOYEE_NO, ICM_CODE) 
                VALUES ( 
                    '{workdate}', '{processkey}', (TO_DATE('{workstart}', 'YYYY-MM-DD HH24:MI:SS')), 
                   (TO_DATE('{workend}', 'YYYY-MM-DD HH24:MI:SS')), 'W', '{icmactive}', 
                   '{reasondesc}', '', SYSDATE, 
                   '{name}', '{icmdesc}')
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_input_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                //Dbconn.conn.BeginTransaction();
                
                //string updateSQL = $@"
                //UPDATE SHIFT_DETAIL SD
                //SET SD.ADD_WORK_TIME = (
                //(TO_NUMBER(TO_CHAR(SD.DAY_WORK_ET, 'DD')) * 24 * 60 +  
                //TO_NUMBER(TO_CHAR(SD.DAY_WORK_ET, 'HH24')) * 60 +     
                //TO_NUMBER(TO_CHAR(SD.DAY_WORK_ET, 'MI'))) -
                //(TO_NUMBER(TO_CHAR(SD.DAY_WORK_ST, 'DD')) * 24 * 60 +  
                //TO_NUMBER(TO_CHAR(SD.DAY_WORK_ST, 'HH24')) * 60 +     
                //TO_NUMBER(TO_CHAR(SD.DAY_WORK_ST, 'MI'))) -
                //(SELECT MAX(PD.STANDARD_WORK_HOUR) FROM V_MES_ATG_111_1 PD WHERE SD.PROCESS_KEY = '{processkey}')
                //)
                //WHERE SD.WORK_START_DATE = '{workdate}'
                //AND SD.EMPLOYEE_NO = (SELECT EMPLOYEE_NO FROM DO_INSA WHERE NAME = '{name}') ";

                //if (Dbconn.conn.SQLrun(updateSQL) < 1)
                //{
                //    Dbconn.conn.Rollback();
                //    clsLog.logSave(this.Text, "btn_input_Click", updateSQL);
                //    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                //    return;
                //}

                //Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("작업조를 추가 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_input_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
    }
}