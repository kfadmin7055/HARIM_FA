using Core.Class;
using DevExpress.Internal;
using DevExpress.XtraCharts;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;


namespace HARIM_FA_DOSING
{
    public partial class frm_BinInput : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_BinInput()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(gridView_Input, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridViewEnd, Properties.Settings.Default.FontSize);
        }

        private void frm_BinInput_Load(object sender, EventArgs e)
        {
            try
            {
                //// 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                LookupEdit_inputSel.Properties.NullText = "";
                clsDevexpressUtil.ItemLookUpEditSetup(LookupEdit_inputSel, clsCommon.GetGridProcess(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString()), "", false, 0);

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                XMain_Search();

                //tab2 작업일자
                dateEdit_workDateStart.EditValue = DateTime.Today;
                dateEdit_workDateEnd.EditValue = DateTime.Today.AddDays(1);

                binInput_sum_search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_BinInput_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                      MAX(a.PLANT_CODE)     AS PLANT_CODE, MAX(b.PROCESS_DESC)   AS PROCESS_DESC, a.LOCATION
                    , MAX(a.BIN_NAME)       AS BIN_NAME  , MAX(a.RESOURCE_NO)    AS RESOURCE_NO
                    , MAX(a.B_STOCK)        AS B_STOCK   , MAX(a.HI_Q)           AS HI_Q        , MAX(a.LO_Q)           AS LO_Q
                    , MAX(a.MAX_CAPA)       AS MAX_CAPA  , MAX(a.FAIL)           AS FAIL        , MAX(a.HZ_01)          AS HZ_01
                    , MAX(a.HZ_02)          AS HZ_02     , MAX(a.M_RATE)         AS M_RATE      , MAX(a.M_ON)           AS M_ON
                    , MAX(a.M_OFF)          AS M_OFF     , MAX(a.ROW_RATE)       AS ROW_RATE    , MAX(a.S_ON)           AS S_ON
                    , MAX(a.S_OFF)          AS S_OFF     , MAX(a.HZ_03)          AS HZ_03       , MAX(a.HL_ERROR)       AS HL_ERROR
                    , MAX(a.STOCK)          AS STOCK     , MAX(a.JOG_ON_TIME)    AS JOG_ON_TIME , MAX(a.DROP_SAFE_T)    AS DROP_SAFE_T
                    , MAX(a.SEQ)            AS SEQ       , MAX(a.SCALE_CODE)     AS SCALE_CODE  , MAX(a.BIN_GUBUN)      AS BIN_GUBUN
                    , MAX(a.BIN_SERIAL)     AS BIN_SERIAL, MAX(a.PLC_ADDRESS)    AS PLC_ADDRESS , MAX(a.ERP_LOCATION)   AS ERP_LOCATION
                    , MAX(a.I_TIME)         AS I_TIME
                FROM BIN a
                    JOIN SAP_PROCESS_DIVISION b ON a.PLANT_CODE = b.PLANT_CODE AND a.PROCESS_KEY = b.PROCESS_KEY
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND a.RESOURCE_NO IS NOT NULL
                    AND a.PROCESS_KEY = '{LookupEdit_inputSel.EditValue}'
                GROUP BY a.LOCATION
                ORDER BY SCALE_CODE, BIN_SERIAL
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

                sValid = new string[] { "" };


                repItemLkUpEdit_RESOURCE_NO.NullText = "";
                repItemLkUpEdit_RESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repItemLkUpEdit_RESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
                repItemLkUpEdit_RESOURCE_NO.PopupFormMinSize = new Size(400, 0);
                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "bin_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다" + SQL);
            }
        }

        private void binInput_detail_search(string res_no)
        {
            try
            {
                SQL = $@"
                SELECT a.PLANT_CODE, b.RESOURCE_NO,
                    a.SEQ, a.INPUT_DATE, a.JJO, a.INPUT_EA,
                    CASE WHEN a.INPUT_EA IS NULL 
                            THEN a.INPUT_QTY 
                            ELSE b.BAG_UNIT * a.INPUT_EA 
                        END INPUT_QTY, 
                    a.ERP_UP_YN, a.I_TIME, a.I_USER, 
                    a.LOCATION
                FROM BIN_INGRED_INPUT a
                    INNER JOIN BIN b ON b.PLANT_CODE = a.PLANT_CODE AND b.LOCATION = a.LOCATION
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND a.LOCATION = '{res_no}' AND a.INPUT_DATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                    AND b.PROCESS_KEY = '{LookupEdit_inputSel.EditValue}'
                ORDER BY a.SEQ, a.I_TIME
                ";

                SQL = string.Format(SQL, res_no, string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_Input, gridView_Input, ds.Tables[0], true, true);

                sValid = new string[] { "" };


                repositoryItemLookUpEdit_RESOURCE_NO.NullText = "";
                repositoryItemLookUpEdit_RESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                clsDevexpressGrid.ItemLookUpEditSetup(repositoryItemLookUpEdit_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "binInput_detail_search", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void binInput_sum_search()
        {

            try
            {
                SQL = $@"
                SELECT DISTINCT TO_CHAR(TO_DATE(B.INPUT_DATE, 'YYYYMMDD'), 'YYYY-MM-DD') AS INPUT_DATE,
                    B.LOCATION,
                    A.RESOURCE_NO,
                    C.DESCRIPTION,
                    B.INPUT_QTY,
                    B.I_TIME,
                    B.I_USER,
                    D.NAME
                FROM BIN A
                    INNER JOIN BIN_INGRED_INPUT B ON B.PLANT_CODE = A.PLANT_CODE AND B.LOCATION = A.LOCATION
                    LEFT JOIN SAP_DI_PRODUCT C ON C.PLANT_CODE = A.PLANT_CODE AND A.RESOURCE_NO = C.RESOURCE_NO
                    LEFT JOIN DO_INSA D ON B.I_USER = D.EMPLOYEE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND B.INPUT_DATE BETWEEN TO_DATE('{dateEdit_workDateStart.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                                        AND TO_DATE('{dateEdit_workDateEnd.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                    AND A.RESOURCE_NO IS NOT NULL
                ORDER BY B.I_TIME
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_End, gridViewEnd, ds.Tables[0], true, true);

                ds.Dispose();

                chartControl_binInput.Series.Clear();
                //title
                chartControl_binInput.Titles.Clear();
                DevExpress.XtraCharts.ChartTitle chartTitle = new DevExpress.XtraCharts.ChartTitle();
                chartTitle.Text = "빈원료 투입량(KG)";
                chartTitle.Font = new System.Drawing.Font("맑은 고딕", 15F, FontStyle.Bold);
                chartControl_binInput.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] { chartTitle });

                //chartControl_binInput.Legend.Visible = true;

                chartControl_binInput.Legend.MarkerMode = LegendMarkerMode.Marker;

                SQL = $@"
                SELECT 
                    TRIM(c.DESCRIPTION) AS DESCRIPTION,
                    SUM(b.INPUT_QTY) AS SUM_QTY
                FROM BIN A
                    INNER JOIN BIN_INGRED_INPUT B ON B.PLANT_CODE = A.PLANT_CODE AND B.LOCATION = A.LOCATION
                    LEFT JOIN SAP_DI_PRODUCT C ON A.RESOURCE_NO = C.RESOURCE_NO
                    LEFT JOIN DO_INSA D ON B.I_USER = D.EMPLOYEE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND B.INPUT_DATE BETWEEN TO_DATE('{dateEdit_workDateStart.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                                        AND TO_DATE('{dateEdit_workDateEnd.DateTime.ToString("yyyyMMdd")}', 'YYYYMMDD')
                    AND C.DESCRIPTION IS NOT NULL
                GROUP BY c.DESCRIPTION
                ORDER BY SUM_QTY DESC
                ";

                DataSet sumDs = Dbconn.conn.ExecutDataset(SQL);
                Series series = new Series("TOTAL", ViewType.Pie);
                chartControl_binInput.Series.Add(series);

                for (int i = 0; i < Dbconn.conn.getRowCnt(sumDs); i++)
                {
                    object obj = Dbconn.conn.getData(sumDs, "SUM_QTY", i);

                    if (obj.ToString() != "" && obj != null && obj != DBNull.Value)
                    {
                        series.Points.Add(new SeriesPoint(Dbconn.conn.getData(sumDs, "DESCRIPTION", i), Convert.ToDouble(obj)));
                    }
                }

                chartControl_binInput.Series.Add(series);
                //series.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                //series.PointOptions.ValueNumericOptions.Precision = 0;
                series.LegendTextPattern = "{A} ({V:n0} Kg)";
                series.Label.TextPattern = "{VP:p0} ({A})";

                chartControl_binInput.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
                chartControl_binInput.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
                chartControl_binInput.Legend.MaxHorizontalPercentage = 100;
                chartControl_binInput.Legend.MaxVerticalPercentage = 20;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "binInput_sum_search", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }

        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (gridView.RowCount < 1)
            {
                ShowMessageBox.XtraShowInformation("원료를 투입하실 빈을 선택해주세요");
                return;
            }

            if (string.IsNullOrEmpty(clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO").Trim()))
            {
                ShowMessageBox.XtraShowInformation("빈에 설정된 원료가 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView_Input);
            gridView_Input.SetRowCellValue(gridView_Input.FocusedRowHandle, gridView_Input.Columns["PLANT_CODE"], clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE"));
            gridView_Input.SetRowCellValue(gridView_Input.FocusedRowHandle, gridView_Input.Columns["RESOURCE_NO"], clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO"));
            gridView_Input.SetRowCellValue(gridView_Input.FocusedRowHandle, gridView_Input.Columns["INPUT_QTY"], 0);

        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                string selData = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION");
                if (!string.IsNullOrEmpty(selData))
                {
                    binInput_detail_search(selData);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != ShowMessageBox.Confirm("작업지시정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView_Input);
                DataTable DT = (DataTable)gridControl_Input.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();
                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    //input check
                    if (string.IsNullOrEmpty(dr["INPUT_QTY"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("INPUT_QTY", "투입량을 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("INPUT_QTY"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO BIN_INGRED_INPUT (
                           PLANT_CODE, SEQ, INPUT_DATE, INPUT_QTY, 
                           ERP_UP_YN, I_TIME, I_USER, 
                           LOCATION, RESOURCE_NO) 
                        VALUES ( 
                             '{dr["PLANT_CODE"]}',
                             (SELECT NVL(MAX(SEQ) + 1, 1) FROM BIN_INGRED_INPUT WHERE PLANT_CODE = '{dr["PLANT_CODE"]}'),
                             '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}',
                             '{dr["INPUT_QTY"]}',
                             'Y',
                             SYSDATE,
                             '{clsCommon.UserId}',
                             '{clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION")}',
                             '{clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO")}' )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        if (dr["RESOURCE_NO"].ToString() != "1920021133")
                        {
                            string from_loc = string.Empty;
                            string to_loc = string.Empty;

                            if (LookupEdit_inputSel.EditValue.Equals("M"))
                            {
                                from_loc = "R01";
                                to_loc = "M01";

                            }
                            else if (LookupEdit_inputSel.EditValue.Equals("S"))
                            {

                                from_loc = "R01";
                                to_loc = "P01";

                            }
                        }

                        if (clsCommon.PlantCode != "P101")
                        {
                            clsProcessDosing.BinStock(
                                clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION"),
                                Convert.ToDecimal(dr["INPUT_QTY"].ToString())
                             );
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                Dbconn.conn.Commit();

                gridView.RefreshData();
                XMain_Search();
                binInput_detail_search(clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION"));

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void LookupEdit_inputSel_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
            gridControl_Input.DataSource = null;
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
            gridControl_Input.DataSource = null;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();

        }

        private void XMain_Delete()
        {
            try
            {
                if (gridView_Input.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 투입내역를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView_Input.GetDataRow(gridView_Input.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView_Input.DeleteRow(gridView_Input.FocusedRowHandle);
                }
                else
                {
                    string SEQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_Input, "SEQ");
                    string RESOURCE_NO = clsDevexpressGrid.GetFocusedRowCellValue(gridView_Input, "RESOURCE_NO");
                    string Location = clsDevexpressGrid.GetFocusedRowCellValue(gridView_Input, "LOCATION");
                    string INPUT_QTY = clsDevexpressGrid.GetFocusedRowCellValue(gridView_Input, "INPUT_QTY");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 투입내역을 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {

                        Dbconn.conn.BeginTransaction();

                        //delete work num
                        SQL = $"DELETE FROM BIN_INGRED_INPUT WHERE SEQ = '{SEQ}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("투입내역 삭제에 실패했습니다");
                            return;
                        }

                        string from_loc = string.Empty;
                        string to_loc = string.Empty;

                        if (LookupEdit_inputSel.EditValue.Equals("M"))
                        {
                            from_loc = "M01";
                            to_loc = "R01";

                        }
                        else if (LookupEdit_inputSel.EditValue.Equals("S"))
                        {
                            if (RESOURCE_NO.Length == 6)
                            {
                                from_loc = "P01";
                                to_loc = "R01";
                            }
                            else
                            {
                                from_loc = "P01";
                                to_loc = "P01";
                            }
                        }

                        clsProcessDosing.BinStock(
                            Location,
                            -Convert.ToDecimal(INPUT_QTY)
                         );

                        Dbconn.conn.Commit();

                        binInput_detail_search(clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION"));

                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                string selData = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION");
                if (!string.IsNullOrEmpty(selData))
                {
                    binInput_detail_search(selData);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_FocusedRowChanged", ex);
            }
        }

        private void btn_searchEnd_Click(object sender, EventArgs e)
        {
            binInput_sum_search();
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gridViewEnd.ExportToXlsx(sfd.FileName);
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

            // 신규 행 추가
            if (e.KeyCode == Keys.F3)
            {
                e.Handled = true;
                btn_rowAdd_Click(sender, e);
            }

            // 행 삭제
            if (e.KeyCode == Keys.Delete)
            {
                btn_rowDel_Click(sender, e);
            }

            // 저장
            if (e.Control && e.KeyCode == Keys.S)
            {
                XMain_Save();
            }

            // 삭제
            if (e.Control && e.KeyCode == Keys.D)
            {
                XMain_Delete();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void tabbedControlGroup1_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            //// 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code1, clsCommon.GetPlant(), "", true, 0, false);
        }
    }
}