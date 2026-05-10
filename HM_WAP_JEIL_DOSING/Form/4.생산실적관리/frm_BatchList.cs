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
    public partial class frm_BatchList : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_BatchList()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE
                    , SUBSTR(WO.WORKDATE, 1, 4) || '-' || SUBSTR(WO.WORKDATE, 5, 2) || '-' || SUBSTR(WO.WORKDATE, 7, 2) AS WORKDATE,
                    BT.BATCH,
                    BT.NUM,
                    WO.NOTE,
                    TRIM(PRO.DESCRIPTION) AS DESCRIPTION,
                    D.D1_TIME, D.D2_TIME, D.D3_TIME, D.D4_TIME, D.D5_TIME,
                    D.M1_TIME, D.M2_TIME, D.M3_TIME, D.M4_TIME,
                    D.L1_TIME,
                    BT.MIX_T, BT.LQ_T
                FROM BATCH BT
                LEFT OUTER JOIN WORK_ORDER WO  
                    ON BT.PLANT_CODE = WO.PLANT_CODE
                    AND BT.PROCESS_KEY = WO.PROCESS_KEY 
                    AND BT.L_CODE = WO.L_CODE
                    AND BT.WORKDATE = WO.WORKDATE 
                    AND BT.NUM = WO.NUM
                LEFT OUTER JOIN (
                    SELECT 
                        V.PROCESS_KEY, V.WORKDATE, V.NUM, V.BATCH,
                        NVL(MIN(V.D1_TIME), 0) AS D1_TIME,
                        NVL(MIN(V.D2_TIME), 0) AS D2_TIME,
                        NVL(MIN(V.D3_TIME), 0) AS D3_TIME,
                        NVL(MIN(V.D4_TIME), 0) AS D4_TIME,
                        NVL(MIN(V.D5_TIME), 0) AS D5_TIME,
                        NVL(MIN(V.M1_TIME), 0) AS M1_TIME,
                        NVL(MIN(V.M2_TIME), 0) AS M2_TIME,
                        NVL(MIN(V.M3_TIME), 0) AS M3_TIME,
                        NVL(MIN(V.M4_TIME), 0) AS M4_TIME,
                        NVL(MIN(V.L1_TIME), 0) AS L1_TIME
                    FROM (
                        SELECT 
                            WR.PLANT_CODE,
                            WR.PROCESS_KEY,
                            WR.L_CODE,
                            WR.WORKDATE,
                            WR.NUM,
                            WR.BATCH,
                            CASE WHEN B.SCALE_CODE = 'D1' THEN SUM(WR.P_Q_TIME) END AS D1_TIME,
                            CASE WHEN B.SCALE_CODE = 'D2' THEN SUM(WR.P_Q_TIME) END AS D2_TIME,
                            CASE WHEN B.SCALE_CODE = 'D3' THEN SUM(WR.P_Q_TIME) END AS D3_TIME,
                            CASE WHEN B.SCALE_CODE = 'D4' THEN SUM(WR.P_Q_TIME) END AS D4_TIME,
                            CASE WHEN B.SCALE_CODE = 'D5' THEN SUM(WR.P_Q_TIME) END AS D5_TIME,
                            CASE WHEN B.SCALE_CODE = 'M1' THEN SUM(WR.P_Q_TIME) END AS M1_TIME,
                            CASE WHEN B.SCALE_CODE = 'M2' THEN SUM(WR.P_Q_TIME) END AS M2_TIME,
                            CASE WHEN B.SCALE_CODE = 'M3' THEN SUM(WR.P_Q_TIME) END AS M3_TIME,
                            CASE WHEN B.SCALE_CODE = 'M4' THEN SUM(WR.P_Q_TIME) END AS M4_TIME,
                            CASE WHEN B.SCALE_CODE = 'L1' THEN SUM(WR.P_Q_TIME) END AS L1_TIME
                        FROM WORK_REMARK WR
                            LEFT OUTER JOIN BIN B ON WR.PLANT_CODE = B.PLANT_CODE
                                                AND WR.PROCESS_KEY = B.PROCESS_KEY 
                                                AND WR.L_CODE = B.L_CODE
                                                AND WR.LOCATION = B.LOCATION
                        WHERE WR.PLANT_CODE = '{cboPlant_Code.EditValue}'
                            AND WR.PROCESS_KEY = '{clsCommon.GetProcessKey("배합")}'
                            AND WR.L_CODE = '{cboL_Code.EditValue}'
                            AND WR.WORKDATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                        GROUP BY WR.PLANT_CODE,
                            WR.PROCESS_KEY,
                            WR.L_CODE, WR.WORKDATE, WR.NUM, WR.BATCH, B.SCALE_CODE
                    ) V
                    GROUP BY V.PROCESS_KEY, V.WORKDATE, V.NUM, V.BATCH
                ) D ON D.PROCESS_KEY = BT.PROCESS_KEY  
                   AND D.WORKDATE = BT.WORKDATE 
                   AND D.NUM = BT.NUM 
                   AND D.BATCH = BT.BATCH
                LEFT OUTER JOIN SAP_DI_PRODUCT PRO ON WO.PLANT_CODE = PRO.PLANT_CODE AND WO.RESOURCE_NO = PRO.RESOURCE_NO
                WHERE WO.PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND WO.PROCESS_KEY = '{clsCommon.GetProcessKey("배합")}'
                    AND WO.L_CODE = '{cboL_Code.EditValue}'
                    AND WO.WORK_START_DATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                ORDER BY WO.WORK_START_DATE, WO.NUM, BT.BATCH
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "" };


                ds.Dispose();

            }
             catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_BatchList_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            dateEdit_workStartDate.EditValue = DateTime.Today;
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
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