using Core.Class;
using Core.Enum;
using DevExpress.Schedule;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_BagUseResult : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_BagUseResult()
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
                    BO.PLANT_CODE, BO.PROCESS_KEY, BO.L_CODE
                    , BO.RESOURCE_NO,
                    PRO.DESCRIPTION,
                    BO.NOTE,
                    SUM(BO.PRO_QTY) AS PRO_QTY
                FROM BAG_ORDER BO
                LEFT OUTER JOIN SAP_DI_PRODUCT PRO ON PRO.PLANT_CODE = BO.PLANT_CODE AND BO.RESOURCE_NO = PRO.RESOURCE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR BO.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND BO.PROCESS_KEY = '{clsCommon.GetProcessKey("타이콘")}'
                    AND BO.L_CODE = '{cboL_Code.EditValue}'
                    AND BO.WORK_START_DATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                                                AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                    AND BO.C_CONDITION = '{clsCommon.PcStatus.Completed}'
                GROUP BY 
                    BO.PLANT_CODE, BO.PROCESS_KEY, BO.L_CODE
                    ,BO.RESOURCE_NO,
                    PRO.DESCRIPTION,
                    BO.NOTE
                ORDER BY SUM(BO.PRO_QTY) DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "WORKDATE", "P_TYPE", "RESOURCE_NO", "NOTE", "ICM_CODE", "WORK_START_DATE", "LOCATION", "EMPLOYEE_NO" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_BagUseResult_Load(object sender, EventArgs e)
        {
            try
            {
                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("타이콘")), "", false, 0, false);

                dateEdit_workStartDate.EditValue = DateTime.Today;
                dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);
                XMain_Search();

                gridView.ShowFindPanel();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_BagUseResult_Load", ex);
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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

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