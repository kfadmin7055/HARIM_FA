using Core;
using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraSplashScreen;
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
    public partial class frm_PelletReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_PelletReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT a.PLANT_CODE, a.PROCESS_KEY, l.PROCESS_DESC AS L_CODE, 
                        a.WORKDATE, a.WORK_SEQ, a.P_TYPE, 
                        a.RESOURCE_NO || ' :  ' || p.DESCRIPTION AS DESCRIPTION, a.NOTE, a.HALT_TIME, 
                        a.RUN_ST, a.RUN_ET, a.BF_PROCESS_KEY, 
                        a.BF_WORKDATE, a.BF_NUM, a.BF_QTY, 
                        a.QTY, a.REMAIN_QTY, a.PROTY, 
                        a.TOTAL_OPER, a.DY_ST, a.DY_THICK, 
                        a.WORK_START_DATE, a.ICM_CODE, a.EMPLOYEE_NO, 
                        a.DY_SP, a.CURRENT_1, a.CURRENT_2, 
                        a.FEEDER_RATE, a.CRUMBLE_YN, a.HARDNESS, 
                        a.PDI, a.CLEAN_QTY, a.LOCATION_ST1, 
                        a.LOCATION_ST2, a.LOCATION_ED1, a.LOCATION_ED2, 
                        a.HZ, a.CD_TEMP, a.P_TEMP, 
                        a.COL_TEMP, a.REMARK, a.DEL_FLAG, 
                        a.ERP_UP_YN, a.ERP_TNUMBER, a.I_TIME
                FROM PELLET_REPORT a
                    LEFT JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = p.RESOURCE_NO
                    LEFT JOIN SAP_PROCESS_LDIVISION l ON l.PLANT_CODE = a.PLANT_CODE AND l.PROCESS_KEY = a.PROCESS_KEY AND l.L_CODE = a.L_CODE
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND a.PROCESS_KEY = '{cboProcessKey.EditValue}'
                    AND a.L_CODE = '{cboL_Code.EditValue}'
                    AND a.WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
                    AND NVL(a.DEL_FLAG, 'N') != 'Y'  
                ORDER BY a.L_CODE, a.WORKDATE, a.WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

                sValid = new string[] { "" };


            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                clsPrintReport.PrintPelletReport(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), cboL_Code.EditValue?.ToString(), workDate);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_report_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_PelletReport_Load(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                // 라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("펠렛")), "", false, 0, false);

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;
                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_PelletReport_Load", ex);
            }

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void layoutControl_CustomDraw(object sender, DevExpress.XtraLayout.ItemCustomDrawEventArgs e)
        {
            try
            {
                if (e.Item.Tag != null && e.Item.Tag.ToString() == "LINE")
                {
                    e.DefaultDraw();
                    Pen pen = new Pen(Brushes.DarkGray, 1);
                    e.Cache.DrawRectangle(pen, e.Bounds);
                    pen.Dispose();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "layoutControl_CustomDraw", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
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
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("펠렛")), "", false, 0, false);
            
            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl.DataSource = null;

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