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
    public partial class frm_EqTroubleResult : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_EqTroubleResult()
        {
            InitializeComponent();

            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {

                SQL = $@"
                SELECT 
                    SEQ, 
                    TO_CHAR(WORKDATE, 'YY-MM-DD') AS WORKDATE, 
                    EQ_CODE, 
                    EQ_NAME, 
                    EQ_TRB_MSG, 
                    EQ_TRB_TASK_MSG, 
                    REMARK, 
                    IN_GUBUN, 
                    I_TIME, 
                    I_USER
                FROM EQ_TROUBLE
                WHERE TO_CHAR(WORKDATE, 'YYYYMMDD') BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                ORDER BY SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "silo_XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_EqTroubleResult_Load(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

                dateEdit_workStartDate.EditValue = DateTime.Today;
                dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_EqTrouble_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }

        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gridView.ExportToXlsx(sfd.FileName);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_excelExport_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }

            //// 신규 행 추가
            //if (e.KeyCode == Keys.F3)
            //{
            //    btn_rowAdd_Click(sender, e);
            //}

            //// 행 삭제
            //if (e.KeyCode == Keys.Delete)
            //{
            //    btn_rowDel_Click(sender, e);
            //}

            //// 저장
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    XMain_Save();
            //}

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void dateEdit_workStartDate_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = dateEdit_workStartDate.EditValue as DateTime?;
            DateTime? endDate = dateEdit_workEndDate.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    dateEdit_workEndDate.EditValue = startDate.Value;
                }
            }
        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = dateEdit_workStartDate.EditValue as DateTime?;
            DateTime? endDate = dateEdit_workEndDate.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    dateEdit_workStartDate.EditValue = endDate.Value;
                }
            }
        }
    }
}