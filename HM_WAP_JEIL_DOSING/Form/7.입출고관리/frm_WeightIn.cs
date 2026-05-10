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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using DevExpress.XtraPrinting;
using Core.Class;

namespace HARIM_FA_DOSING
{
    public partial class frm_WeightIn : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_WeightIn()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView_Detail, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView_Pallet, Properties.Settings.Default.FontSize);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void XMain_Search()
        {
            try
            {

                ArrayList exRowList = new ArrayList();

                for (int i = 0; i < gridView.RowCount; i++)
                {
                    if (gridView.GetMasterRowExpanded(i))
                    {
                        exRowList.Add(i);
                    }
                }

                var topRowIndex = gridView.TopRowIndex;
                var focusedRowHandle = gridView.FocusedRowHandle;

                SQL = $@"
                SELECT 
                    DECAR.IS_NO,
                    DECAR.CUST_CODE,
                    DECAR.CUST_CODE AS CUST_NM,
                    DECAR.INCAR_NO,
                    DECAR.CHKIN_TIME,
                    CEIL(DECAR.IN_WEIGHT) AS IN_WEIGHT,
                    CEIL(DECAR.OUT_WEIGHT) AS OUT_WEIGHT,
                    DECAR.CAR_TDETAIL,
                    DECAR.D_NAME,
                    TO_CHAR(DECAR.INCAR_DATE, 'YYYY-MM-DD') AS INCAR_DATE,
                    DECAR.INCAR_TIME,
                    TO_CHAR(DECAR.OUTCAR_DATE, 'YYYY-MM-DD') AS OUTCAR_DATE,
                    DECAR.OUTCAR_TIME,
                    NVL(PAADD.SUM_PAADD, 0) AS SUM_PAADD,
                    CEIL(DECAR.IN_WEIGHT - DECAR.OUT_WEIGHT) AS WEIGHT,
                    CEIL(DECAR.IN_WEIGHT - DECAR.OUT_WEIGHT - NVL(PAADD.SUM_PAADD, 0) - NVL(P_W.P_WEIGHT, 0)) AS REAL_WEIGHT,
                    SONG.SONG_WEIGHT,
                    NVL(DECAR.PC_STATUS, 99) AS PC_STATUS,
                    NVL(P_W.P_WEIGHT, 0) AS P_WEIGHT,
                    CEIL(DECAR.IN_WEIGHT - DECAR.OUT_WEIGHT - NVL(PAADD.SUM_PAADD, 0) - NVL(P_W.P_WEIGHT, 0)) - SONG.SONG_WEIGHT AS CHA_WEIGHT,
                    SONG.ERP_UP_YN
                FROM 
                    WAP_DECAR DECAR
                    LEFT JOIN (
                        SELECT 
                            IN_ADD.IS_NO, 
                            SUM(PA_M.WEIGHT * IN_ADD.PD_QTY) AS SUM_PAADD
                        FROM 
                            WAP_IN_ADD IN_ADD
                            LEFT JOIN WAP_PA_MASTER PA_M ON IN_ADD.PTMCD = PA_M.PTMCD
                        GROUP BY 
                            IN_ADD.IS_NO
                    ) PAADD ON DECAR.IS_NO = PAADD.IS_NO
                    LEFT JOIN (
                        SELECT 
                            IS_NO, 
                            NVL(SUM(SPIV_CAR_WEIGHT), 0) AS SONG_WEIGHT,
                            MIN(ERP_UP_YN) AS ERP_UP_YN
                        FROM 
                            WAP_GOCAR
                        GROUP BY 
                            IS_NO
                    ) SONG ON DECAR.IS_NO = SONG.IS_NO
                    LEFT JOIN (
                        SELECT 
                            A.IS_NO,
                            SUM(A.P_WEIGHT) AS P_WEIGHT
                        FROM (
                            SELECT 
                                IS_NO,
                                CASE 
                                    WHEN EA = 1 THEN 0
                                    WHEN EA <> 1 AND (NVL(SPIV_CAR_WEIGHT, 0) / NVL(EA, 0)) >= 100 THEN NVL(3 * EA, 0)
                                    ELSE NVL(0.23 * EA, 0)
                                END AS P_WEIGHT
                            FROM 
                                WAP_GOCAR
                            WHERE 
                                EA > 0
                        ) A
                        GROUP BY 
                            A.IS_NO
                    ) P_W ON DECAR.IS_NO = P_W.IS_NO
                WHERE 
                    (
                        TO_CHAR(CHKIN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                        AND CAR_TDETAIL = '입고'
                    )
                    OR (
                        TO_CHAR(INCAR_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                        AND CAR_TDETAIL = '입고' 
                        AND PC_STATUS IS NULL
                    )
                ORDER BY IS_NO
                ";

                DataSet ds1 = Dbconn.conn.ExecutDataset(SQL, "in_table");

                SQL = $@"
                SELECT 
                    IS_NO,
                    IV_NO,
                    INGRED_CODE,
                    INGRED_CODE AS INGRED_NAME,
                    SPIV_CAR_WEIGHT,
                    EA,
                    UNIT,
                    SPCS,
                    N_WEIGHT,
                    I_TIME,
                    CASE TO_CHAR(ERP_UP_YN)
                        WHEN 'N' THEN '미전송'
                        WHEN 'Y' THEN '전송'
                        ELSE '미전송'
                    END AS ERP_UP_YN,
                    CASE 
                        WHEN EA = 1 THEN '0'
                        WHEN EA = 0 THEN '0 (피중량계산불가)'
                        WHEN EA <> 1 AND NVL(SPIV_CAR_WEIGHT, 0) / NVL(EA, 0) >= 100 THEN '3'
                        ELSE TO_CHAR(NVL(0.23, 0))
                    END AS P_WEIGHT,
                    CASE 
                        WHEN EA = 1 THEN '사이로'
                        WHEN EA <> 1 AND NVL(SPIV_CAR_WEIGHT, 0) / NVL(EA, 0) >= 100 THEN '톤백'
                        ELSE '지대'
                    END AS P_TYPE
                FROM 
                    WAP_GOCAR
                WHERE 
                    IS_NO IN (
                        SELECT IS_NO 
                        FROM WAP_DECAR
                        WHERE 
                            (
                                TO_CHAR(CHKIN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                                AND CAR_TDETAIL = '입고'
                            )
                            OR (
                                TO_CHAR(INCAR_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                                AND CAR_TDETAIL = '입고'
                                AND PC_STATUS IS NULL
                            )
                    )
                ";

                DataSet ds2 = Dbconn.conn.ExecutDataset(SQL, "in_detail");
                ds1.Merge(ds2);

                GridLevelNode gridLevelNode1 = new GridLevelNode();
                gridView_Detail.OptionsDetail.EnableMasterViewMode = true;
                gridLevelNode1.RelationName = "입고원료내역";
                gridLevelNode1.LevelTemplate = gridView_Detail;
                this.gridControl.LevelTree.Nodes.Add(gridLevelNode1);
                //ds1.Relations.Add("입고원료내역", ds1.Tables["in_table"].Columns["IS_NO"], ds1.Tables["in_detail"].Columns["IS_NO"]);

                SQL = $@"
                SELECT 
                    INADD.IS_NO, 
                    PA.PTMCDNM, 
                    PA.WEIGHT, 
                    INADD.PD_QTY, 
                    INADD.I_TIME
                FROM 
                    WAP_IN_ADD INADD
                LEFT OUTER JOIN 
                    WAP_PA_MASTER PA 
                    ON INADD.PTMCD = PA.PTMCD
                WHERE 
                    INADD.IS_NO IN (
                        SELECT IS_NO 
                        FROM WAP_DECAR 
                        WHERE 
                            (TO_CHAR(CHKIN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                             AND CAR_TDETAIL IN ('입고')) 
                            OR 
                            (TO_CHAR(INCAR_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                             AND CAR_TDETAIL = '입고' 
                             AND PC_STATUS IS NULL)
                    )
                ORDER BY 
                    INADD.IS_NO
                ";

                DataSet ds3 = Dbconn.conn.ExecutDataset(SQL, "pa_detail");

                ds1.Merge(ds3);


                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds1.Tables["in_table"], true, true);

                sValid = new string[] { "" };

                GridLevelNode gridLevelNode2 = new GridLevelNode();
                //gridView_Pallet.OptionsDetail.EnableMasterViewMode = true;
                gridLevelNode2.RelationName = "파레트상세";
                gridLevelNode2.LevelTemplate = gridView_Pallet;
                this.gridControl.LevelTree.Nodes.Add(gridLevelNode2);
                //ds1.Relations.Add("파레트상세", ds1.Tables["in_table"].Columns["IS_NO"], ds1.Tables["pa_detail"].Columns["IS_NO"]);

                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_VENDOR_NO, clsCommon.GetLocation(clsCommon.PlantCode));

                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource());

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("CODE"),
                    new DataColumn("NAME"),
                });

                dt.Rows.Add("99", "수기입력");
                dt.Rows.Add("0", "입차등록");
                dt.Rows.Add("1", "입차완료");
                dt.Rows.Add("2", "출차완료");
                dt.Rows.Add("9", "강제완료");

                repItemLkUpEdit_PC_STATUS.NullValuePrompt = "";
                repItemLkUpEdit_PC_STATUS.NullText = "";
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_PC_STATUS, dt);

                foreach (var item in exRowList)
                {
                    gridView.SetMasterRowExpanded(Convert.ToInt16(item), true);
                }

                gridView.FocusedRowHandle = focusedRowHandle;
                gridView.TopRowIndex = topRowIndex;

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_WeightIn_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView_Detail);

            dateEdit_workStDate.EditValue = DateTime.Today;
            dateEdit_workEdDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();


            if (
                clsCommon._strUserId == "AD0001" || 
                clsCommon._strUserId == "N20210101" || 
                clsCommon._strUserId == "N20191001" || 
                clsCommon._strUserId == "N20211001" || 
                clsCommon._strUserId == "kfirst"
                )
            {
                layoutControlItem_weightReset.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_weightPlc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_weightInput.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_weightVal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem_weightReset.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_weightPlc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_weightInput.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_weightVal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes != ShowMessageBox.Confirm("계근원료입고 정보를 수정하시겠습니까?"))
            {
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);
                DataTable DT = (DataTable)gridControl.DataSource;

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

                foreach (DataRow dr in DT.DataSet.Tables["in_table"].Rows)
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

                    if (dr.RowState == DataRowState.Modified)
                    {
                        string uIncarTIme = string.Empty;
                        string uOutcarTime = string.Empty;


                        if (!string.IsNullOrEmpty(dr["INCAR_TIME"].ToString()))
                        {

                            uIncarTIme = "'" + Convert.ToDateTime(dr["INCAR_TIME"]).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            uIncarTIme = "NULL";
                        }


                        if (!string.IsNullOrEmpty(dr["OUTCAR_TIME"].ToString()))
                        {

                            uOutcarTime = "'" + Convert.ToDateTime(dr["OUTCAR_TIME"]).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            uOutcarTime = "NULL";
                        }

                        SQL = $@"
                        UPDATE WAP_DECAR
                        SET    INCAR_NO         = '{dr["INCAR_NO"]}',
                               VEHICLEGROUPCODE = '{dr["VEHICLEGROUPCODE"]}',
                               WEIGHT_KG        = '{dr["WEIGHT_KG"]}',
                               IN_WEIGHT        = '{dr["IN_WEIGHT"]}',
                               OUT_WEIGHT       = '{dr["OUT_WEIGHT"]}',
                               TR_YN            = '{dr["TR_YN"]}',
                               TR_WEIGHT        = '{dr["TR_WEIGHT"]}',
                               USER_ID          = '{dr["USER_ID"]}',
                               INCAR_DATE       = '{uIncarTIme}',
                               OUTCAR_DATE      = '{uOutcarTime}',
                               PC_STATUS        = '{dr["PC_STATUS"]}',
                               ERP_UP_YN        = '{dr["ERP_UP_YN"]}',
                               ERP_TNUMBER      = '{dr["ERP_TNUMBER"]}',
                               DEL_FLAG         = '{dr["DEL_FLAG"]}',
                               TEM_TYPE         = '{dr["TEM_TYPE"]}',
                               I_TIME           = '{dr["I_TIME"]}',
                               I_USER           = '{clsCommon.UserId}'
                        WHERE  IS_NO            = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_update_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }

                    }

                    dr.AcceptChanges();

                } //foreach


                Dbconn.conn.Commit();

                XMain_Search();

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_update_Click", ex);
                clsLog.logSave(this, "btn_update_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (clsCommon._strUserId == "AD0001" ||
                    clsCommon._strUserId == "kfirst" ||
                    clsCommon._strUserId == "N20210101" ||  //음영준
                    clsCommon._strUserId == "N20191001" ||  //곽종면 차장
                    clsCommon._strUserId == "N20211001") //김미정
                {

                }else
                {
                    if (gridView.GetFocusedRowCellValue("PC_STATUS").ToString().Trim().Equals("2") || gridView.GetFocusedRowCellValue("PC_STATUS").ToString().Trim().Equals("99"))
                    {
                        e.Cancel = true;

                    }
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }

        private void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                string pc_status = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["PC_STATUS"]).ToString();

                string erp_up_yn = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["ERP_UP_YN"]).ToString();


                if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                {

                }
                
                if (erp_up_yn == "Y")
                {
                    if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                    {
                        e.Appearance.BackColor = Color.LightCyan;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }

                }
                else
                {

                    if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                    {
                        if (pc_status == "2" || pc_status == "99") //출차완료
                        {
                            e.Appearance.BackColor = Color.LightGray;
                            e.Appearance.ForeColor = Color.Black;
                            e.Appearance.FontStyleDelta = FontStyle.Bold;
                        }
   
                   }

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void checkEdit_reflashSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_reflashSearch.Checked)
            {
                reflash_timer.Interval = 5000;
                reflash_timer.Enabled = true;
            }
            else
            {
                reflash_timer.Enabled = false;
            }
        }

        private void reflash_timer_Tick(object sender, EventArgs e)
        {
            XMain_Search();
            gridView.MakeRowVisible(gridView.RowCount - 1);
        }

        //엑셀 내보내기(세부)
        private void btn_exportExcel_Click(object sender, EventArgs e)
        {
            gridView.OptionsPrint.ExpandAllDetails = true;
            gridView.OptionsPrint.PrintDetails = true;
            gridView.OptionsPrint.AutoWidth = true;
            XlsxExportOptionsEx o = new XlsxExportOptionsEx();
            o.ExportType = DevExpress.Export.ExportType.WYSIWYG;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                gridView.ExportToXlsx(sfd.FileName, o);
                System.Diagnostics.Process.Start(sfd.FileName);
            }
        }

        //계근량 초기화
        private void btn_weightReset_Click(object sender, EventArgs e)
        {
            try
            {
                int visibleDetailRelationIndex = gridView.GetVisibleDetailRelationIndex(gridView.FocusedRowHandle);

                GridView detailView = gridView.GetDetailView(gridView.FocusedRowHandle, visibleDetailRelationIndex) as GridView;

                if (detailView == null || string.IsNullOrEmpty(clsDevexpressGrid.GetFocusedRowCellValue(detailView, "DELIVERY_NO")))
                {
                    ShowMessageBox.XtraShowInformation("초기화 하실 계근세부사항을 선택하여 주세요");
                    return;
                }

                string is_no = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "IS_NO");
                string iv_no = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "IV_NO");
                string ingred_code = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "INGRED_CODE");
                string ingred_name = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "INGRED_NAME");
                string erp_up_yn = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "ERP_UP_YN");

                if (erp_up_yn == "전송")
                {
                    ShowMessageBox.XtraShowInformation("ERP에 업로드 처리된 주문서는 초기화 하지 못합니다");
                    return;
                }

                DialogResult result = ShowMessageBox.Confirm("선택하신 발급번호" + is_no + "-" + iv_no + "-" + ingred_name + " 계근사항을 초기화 하시겠습니까?");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                SQL =
                    "UPDATE WAP_GOCAR SET  BEFORE_WEIGHT = NULL, BEFORE_WEIGHT_TIME = NULL, WEIGHT = NULL, WEIGHT_TIME = NULL, N_WEIGHT = '0', PC_STATUS = '1' " +
                    "WHERE IS_NO = '{0}' AND IV_NO = '{1}' AND INGRED_CODE = '{2}' ";
                SQL = string.Format(SQL,
                    is_no,
                    iv_no,
                    ingred_code
                    );

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    ShowMessageBox.XtraShowWarning("계근량 초기화에 실패했습니다");
                    return;
                }

                XMain_Search();

                ShowMessageBox.XtraShowInformation("계근량 초기화가 완료되었습니다");
            }
            catch (Exception ex)
            {
                clsLog.logSave(ex.Message, 0);
                ShowMessageBox.XtraShowWarning("초기화를 하는 도중 에러가 발생했습니다");
            }
        }

        private void weightProcess(string gubun)
        {
            try
            {
                if (gridView.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("계근수동처리 하실 입차내역을 선택하여 주세요");
                    return;
                }

                string is_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "IS_NO");
                string incar_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "INCAR_NO");

                DialogResult result = ShowMessageBox.Confirm("선택하신 발급번호" + is_no + ", 차량번호" + incar_no + " 입차내역을 수동계량 하시겠습니까?");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                string scale_weight = string.Empty;

                //if (gubun == "PLC")
                //{
                //    string weight_end_chk = string.Empty;
                //    try
                //    {
                //        weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
                //    }
                //    catch
                //    {
                //        weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
                //    }


                //    if (weight_end_chk != "1")
                //    {
                //        ShowMessageBox.XtraShowInformation("계량스케일 안정상태가 아닙니다\r\n차량정차 후 실행해주세요");
                //        return;
                //    }

                //    string temp_weight1 = string.Empty;
                //    string temp_weight2 = string.Empty;
                //    try
                //    {
                //        temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
                //        clsUtil.Delay(200);
                //        temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
                //    }
                //    catch
                //    {
                //        temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
                //        clsUtil.Delay(200);
                //        temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
                //    }

                //    try
                //    {
                //        scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
                //    }
                //    catch (Exception)
                //    {
                //        scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
                //    }
                //}
                //else if (gubun == "INPUT")
                //{
                //    string input_weight = XtraInputBox.Show("", "계량값을 입력해주세요", "");

                //    if (string.IsNullOrEmpty(input_weight))
                //    {
                //        ShowMessageBox.XtraShowInformation("계량값을 입력하지 않아 취소되었습니다");
                //        return;
                //    }

                //    scale_weight = input_weight;
                //}

                int intScaleValue = 0;

                bool isIntChk = int.TryParse(scale_weight, out intScaleValue);

                if (!isIntChk)
                {
                    ShowMessageBox.XtraShowInformation("계량값을 정확하지 않습니다\r\n다시 실행 바랍니다");
                    return;
                }

                string return_status = string.Empty;

                mInoutSelect mInoutSelect = new mInoutSelect();
                mInoutSelect.StartPosition = FormStartPosition.CenterScreen;
                DialogResult dResult = mInoutSelect.ShowDialog();

                if (dResult == DialogResult.Yes)
                {
                    return_status = clsCarProcess.InWeightProcess(incar_no, scale_weight, is_no);
                }
                else if (dResult == DialogResult.No)
                {
                    return_status = clsCarProcess.outChkProcess(incar_no, scale_weight, is_no);
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("수동계량이 취소되었습니다\r\n입/출차 구분을 선택하여 주십쇼");
                    return;
                }


                if (return_status == "OK")
                {
                    if (gubun == "PLC")
                    {
   /*                     try
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0004", "1");
                        }
                        catch (Exception)
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0004", "1");
                        }

                        clsUtil.Delay(4000);

                        try
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");
                        }
                        catch (Exception)
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");
                        }

                        XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");*/
                    }

                    ShowMessageBox.XtraShowInformation("계근수동계량처리가 완료되었습니다");
                    XMain_Search();
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("계근수동계량처리가 실패했습니다\r\n실패사유: " + return_status);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "weightProcess", ex);
            }
        }

        private void btn_weightSelf_Click(object sender, EventArgs e)
        {
            weightProcess("PLC");
        }

        private void btn_weightInput_Click(object sender, EventArgs e)
        {
            weightProcess("INPUT");
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("프린터 출력을 하실 입차내역을 선택하여 주세요");
                return;
            }

            string is_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "IS_NO");

            mInOutPrintSel popForm = new mInOutPrintSel(is_no, false);
            popForm.StartPosition = FormStartPosition.CenterScreen;
            popForm.Show();
        }

        private void btn_weightTest_Click(object sender, EventArgs e)
        {
            string scale_weight = string.Empty;
            string temp_weight1 = string.Empty;
            string temp_weight2 = string.Empty;

            string weight_end_chk = string.Empty;
            //try
            //{
            //    weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
            //}
            //catch
            //{
            //    weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
            //}

            //try
            //{
            //    temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
            //    clsUtil.Delay(200);
            //    temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
            //}
            //catch
            //{
            //    temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
            //    clsUtil.Delay(200);
            //    temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
            //}

            try
            {
                scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
            }
            catch (Exception)
            {
                scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
            }

            string msgEndChk = string.Empty;
            if (weight_end_chk == "1")
            {
                msgEndChk = "(안정)";
            }
            else
            {
                msgEndChk = "(안정안됨)";
            }

            btn_weightTest.Text = scale_weight + "\r\n" + msgEndChk;
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void gridView_Detail_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView_Detail);
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

        private void dateEdit_workStDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workEdDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}