using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
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
    public partial class frm_ProductUseResult : DevExpress.XtraEditors.XtraForm
    {
        private string[] sValid = null;

        private string SQL = String.Empty;
        public frm_ProductUseResult()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewSub, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT
                    WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE
                    , b.P_TYPE,                   -- 자원 유형 설명
                    WO.RESOURCE_NO,                 -- 자원 번호
                    b.DESCRIPTION,
                    MAX(WO.NOTE) AS NOTE,                        -- 비고
                    SUM(WO.OR_Q) AS OR_Q,          -- 총 지시 수량
                    SUM(WO.PRO_Q) AS PRO_Q,        -- 총 생산 수량
                    (SUM(WO.OR_Q) - SUM(WO.PRO_Q)) * -1 AS DEV_Q, -- 수량 차이 (실적 - 지시)
                    ROUND(SUM((WO.END_TIME - WO.START_TIME) * 24), 2) AS DIFF_HOUR
                FROM WORK_ORDER WO
                LEFT OUTER JOIN SAP_DI_PRODUCT b ON WO.PLANT_CODE = b.PLANT_CODE AND WO.RESOURCE_NO = b.RESOURCE_NO
                WHERE
                    ('{cboPlant_Code.EditValue}' IS NULL OR WO.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{cboProcess_Key.EditValue}' IS NULL OR WO.PROCESS_KEY = '{cboProcess_Key.EditValue}')
                      AND WO.WORKDATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD')
                    AND NVL(WO.DEL_FLAG, ' ') != 'Y'
                    AND WO.ERP_OSTATUS = 'Y'
                GROUP BY
                  WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE,
                  b.P_TYPE,
                  WO.RESOURCE_NO,
                  b.DESCRIPTION
                HAVING
                  SUM(WO.PRO_Q) > 0
                ORDER BY
                  WO.RESOURCE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                sValid = new string[] { "" };

                //clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "ing_Result_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XSub_Search()
        {
            try
            {
                SQL = $@"
                SELECT
                    WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE
                    , b.P_TYPE,                   -- 자원 유형 설명
                    WO.RESOURCE_NO,                 -- 자원 번호
                    b.DESCRIPTION,
                    WO.NOTE,         -- 비고
                    SUM(WO.OR_Q) AS OR_Q,          -- 총 지시 수량
                    SUM(WO.PRO_Q) AS PRO_Q,        -- 총 생산 수량
                    (SUM(WO.OR_Q) - SUM(WO.PRO_Q)) * -1 AS DEV_Q, -- 수량 차이 (실적 - 지시)
                    ROUND(SUM((WO.END_TIME - WO.START_TIME) * 24), 2) AS DIFF_HOUR
                FROM WORK_ORDER WO
                LEFT OUTER JOIN SAP_DI_PRODUCT b ON WO.PLANT_CODE = b.PLANT_CODE AND WO.RESOURCE_NO = b.RESOURCE_NO
                WHERE
                    ('{cboPlant_Code.EditValue}' IS NULL OR WO.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{cboProcess_Key.EditValue}' IS NULL OR WO.PROCESS_KEY = '{cboProcess_Key.EditValue}')
                      AND WO.WORKDATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD')
                    AND NVL(WO.DEL_FLAG, 'N') != 'Y'
                    AND WO.ERP_ISTATUS = 'Y' AND WO.ERP_OSTATUS = 'Y'
                GROUP BY
                  WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE,
                  WO.NOTE, 
                  b.P_TYPE,
                  WO.RESOURCE_NO,
                  b.DESCRIPTION
                HAVING
                  SUM(WO.PRO_Q) > 0
                ORDER BY
                  WO.RESOURCE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSub, viewSub, ds.Tables[0], true);

                sValid = new string[] { "" };


                //clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "ing_Result_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_ProductUseResult_Load(object sender, EventArgs e)
        {
            DataTable gubun_dt = new DataTable();
            gubun_dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("NAME"),
                    new DataColumn("CODE"),
                });

            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStartDate.EditValue = DateTime.Today;
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);


            XMain_Search();
            XSub_Search();

            viewMain.ShowFindPanel();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
                XSub_Search();
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
            gridMain.Focus();
            viewMain.FocusedRowHandle = 0;
            viewMain.FocusedColumn = viewMain.VisibleColumns[0];
        }

        private void cboProcess_Key_EditValueChanged(object sender, EventArgs e)
        {
            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcess_Key, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("배합")), "", false, 0, false);

            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, false);

            XMain_Search();
            XSub_Search();
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();
            XSub_Search();
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