using Core.Class;
using Core.Enum;
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
    public partial class frm_BulkReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_BulkReport()
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
                    PLANT_CODE, PROCESS_KEY, L_CODE , 
                    CASE TO_CHAR(PROCESS_KEY) 
                        WHEN 'P06' THEN '벌크'
                        WHEN 'P07' THEN '톤백'
                        ELSE NULL 
                    END AS OUT_TYPE,
                    CUST_NAME,
                    NVL(CAR_FULL_NUM, CAR_NO_REAL) AS VEHICLENO,
                    PART_NAME,
                    NVL(P_Q, 0) AS P_Q,
                    LOCATION,
                    START_TIME,
                    END_TIME,
                    REMARK
                FROM BULK_ORDER
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND PROCESS_KEY = '{clsCommon.GetProcessKey("벌크")}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_START_DATE = TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYYMMDD')
                  AND C_CONDITION IN ('{clsCommon.PcStatus.Completed}', '{clsCommon.PcStatus.ForceCompleted}')
                ORDER BY START_TIME
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_BulkReport_Load(object sender, EventArgs e)
        {
            try
            {
                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                // 라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("벌크")), "", false, 0, false);

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_BulkReport_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    e.Info.DisplayText = (1 + e.RowHandle).ToString();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_CustomDrawRowIndicator", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);
                cboProcessKey.EditValue = clsCommon.GetProcessKey("벌크");

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }
    }
}