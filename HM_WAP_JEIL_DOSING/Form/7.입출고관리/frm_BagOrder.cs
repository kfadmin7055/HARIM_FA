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
    public partial class frm_BagOrder : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;


        public frm_BagOrder()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView_Detail, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL =
                "SELECT PLANT_CODE, PROCESS_KEY, L_CODE , WO_NUMBER, WORK_SEQ, DELIVERY_NO, ORDER_NO, ORDER_LINE_SKEY, CUST_NAME, VEHICLENO, PART_NO,  " +
                "PART_NO AS PART_NAME, WORK_START_DATE, BATCH, BATCH_Q, isnull(P_Q,0) as P_Q, LOCATION, LOCATION AS SCALE, AUTO_YN, START_TIME, END_TIME, " +
                "C_CONDITION, REMARK, I_TIME  " +
                "FROM BULK_ORDER " +
                "WHERE PROCESS_KEY = '{0}' AND WORK_START_DATE = '{1}' AND VEHICLENO IN ( " +
                $" SELECT CAR_NO FROM CARS  where VEHICLEGROUPCODE = '카고'  )" +
                "ORDER BY C_CONDITION, LOCATION, ORDER_NO, WO_NUMBER, WORK_SEQ ";

                SQL = string.Format(SQL, "clsCommon.bag_process_code", string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));

                DataSet searchDs1 = Dbconn.conn.ExecutDataset(SQL, "bulkTable");

                clsDevexpressGrid.BindGridControl(gridControl, gridView, searchDs1.Tables["bulkTable"], true, true);

                sValid = new string[] { "" };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", "", 2), "품목을 선택 해주세요.", false);

                //차량정보
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_CAR, clsCommon.GetCarMaster());

                //작업계획
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_C_CONDITION, "03", "10");

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {

            XMain_Search();
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_BagOrder_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //작업일자
            dateEdit_workDate.EditValue = DateTime.Today;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                  e.Cancel = true;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                string condition = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["C_CONDITION"]).ToString();

                if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                {
                    if (condition == clsCommon.PcStatus.Completed) //완료
                    {
                        e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
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
    }
}