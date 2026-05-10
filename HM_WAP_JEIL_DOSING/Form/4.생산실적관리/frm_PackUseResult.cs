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
    public partial class frm_PackUseResult : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_PackUseResult()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_PackUseResult_Load(object sender, EventArgs e)
        {
            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStartDate.EditValue = DateTime.Today;
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);
            XMain_Search();

            gridView.ShowFindPanel();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
            {
                //var quotedArray = lookUpEdit_pcSel.EditValue.ToString()
                //.Split(',')
                //.Select(x => $"'{x.Trim()}'")
                //.ToArray();

                //string process = string.Join(", ", quotedArray);

                SQL = $@"
                WITH BOM AS (
                    SELECT DISTINCT b.PLANT_CODE, b.MENGE, b.RESOURCE_NO, b.NOTE, a.BMENG
                    FROM SAP_IN_BOM_CONM a
                        JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                                                    AND b.NOTE = a.NOTE
                    WHERE SUBSTR(NVL(b.IDNRK, '1'), 1, 1) = '1'  
                            AND SUBSTR(b.RESOURCE_NO, 1, 1) = '1'
                            AND b.P_TYPE = '2'
                )

                SELECT 
                    PO.PLANT_CODE, PO.PROCESS_KEY, PO.L_CODE,
                    PO.RESOURCE_NO, b.DESCRIPTION,
                    SUM(PO.OR_QTY) AS OR_QTY,
                    SUM(PO.PRO_QTY + NVL(PO.F_Q, 0) + NVL(PO.E_Q, 0) + NVL(PO.PA_Q, 0) + NVL(PO.USE_PA_Q, 0) + NVL(PO.BAD_QTY1, 0) + NVL(PO.BAD_QTY2, 0)) AS PRO_QTY,
                    SUM(c.MENGE * (PO.PRO_QTY+ NVL(PO.F_Q, 0) + NVL(PO.E_Q, 0) + NVL(PO.PA_Q, 0) + NVL(PO.USE_PA_Q, 0) + NVL(PO.BAD_QTY1, 0) + NVL(PO.BAD_QTY2, 0)) / c.BMENG) AS PRO_KG
                FROM PACK_ORDER PO
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = PO.PLANT_CODE AND b.RESOURCE_NO = PO.RESOURCE_NO
                    LEFT OUTER JOIN BOM c ON c.PLANT_CODE = PO.PLANT_CODE AND c.RESOURCE_NO = PO.RESOURCE_NO AND c.NOTE = PO.NOTE
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PO.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND PO.PROCESS_KEY = '{cboProcessKey.EditValue}'
                    AND ('{cboL_Code.EditValue}' IS NULL OR PO.L_CODE = '{cboL_Code.EditValue}')
                    AND PO.WORK_START_DATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                    AND PO.ERP_ISTATUS = 'Y' AND PO.ERP_OSTATUS = 'Y'
                GROUP BY PO.PLANT_CODE, PO.PROCESS_KEY, PO.L_CODE
                    ,PO.RESOURCE_NO, b.DESCRIPTION
                ORDER BY PRO_QTY DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "" };


                //clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "ing_Result_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
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
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("포장")), "", false, 0, false);

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);

            XMain_Search();
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);

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