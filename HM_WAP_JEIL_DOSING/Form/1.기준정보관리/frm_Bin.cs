using Core.Class;
using Core.Extension;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace HARIM_FA_DOSING
{
    public partial class frm_Bin : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;
        private bool bChangeValue = false;

        private string vPlant_Code = string.Empty;
        private string vProcess_Code = string.Empty;

        // GBG
        private int[] Buff = new int[640];
        private int[] Buff_100 = new int[100];
        private int[] Buff_20 = new int[20];
        private int[] Buff1 = new int[220];
        // GBG -

        public frm_Bin(string plant_code = "", string process_key = "")
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            gridView.OptionsView.ShowGroupPanel = false;

            vPlant_Code = plant_code;
            vProcess_Code = process_key;
        }

        public frm_Bin()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            gridView.OptionsView.ShowGroupPanel = false;
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        #region 폼로드 이벤트
        private void frm_Bin_Load(object sender, EventArgs e)
        {
            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //그리드 바로 첫클릭시 동작
            gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridControl.Focus();

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }
        #endregion

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@" 
                -- 빈 마스터
                SELECT 'N' AS CHK
                    , a.PLANT_CODE
                    , a.PROCESS_KEY
                    , a.L_CODE
                    , a.LOCATION
                    , a.RESOURCE_NO
                    , a.BIN_NAME
                    , a.BIN_SERIAL
                    , a.SCALE_CODE
                    , a.HI_Q
                    , a.LO_Q
                    , a.MAX_CAPA
                    , a.FAIL
                    , a.P1_FAIL
                    , a.P2_FAIL
                    , a.P3_FAIL
                    , a.P4_FAIL
                    , a.P5_FAIL
                    , a.HZ_01
                    , a.HZ_02
                    , a.M_RATE
                    , a.M_ON
                    , a.M_OFF
                    , a.ROW_RATE
                    , a.S_ON
                    , a.S_OFF
                    , a.HZ_03
                    , a.HL_ERROR
                    , a.LO_ERROR
                    , a.B_STOCK
                    , a.STOCK
                    , a.JOG_ON_TIME
                    , a.DROP_SAFE_T
                    , a.BAG_FAIL
                    , a.BAG_ROW_RATE
                    , a.BAG_S_ON
                    , a.BAG_S_OFF
                    , a.SEQ
                    , a.BIN_GUBUN
                    , a.PLC_ADDRESS
                    , a.STD
                    , a.SDD
                    , b.IN_SCALE
                    , a.ERP_LOCATION
                    , a.EMPLOYEE_NO
                    , a.I_TIME
                FROM BIN a
                    LEFT JOIN SCALE b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE AND b.SCALE_CODE = a.SCALE_CODE
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{cboProcessKey.EditValue}' IS NULL OR a.PROCESS_KEY = '{cboProcessKey.EditValue}')
                    AND ('{cboL_Code.EditValue}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue?.ToString()}')
                    AND ('{txtBinName.EditValue}' IS NULL OR a.BIN_NAME LIKE '%{txtBinName.EditValue}%')
                ORDER BY a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.SCALE_CODE, a.LOCATION, TO_NUMBER(a.BIN_SERIAL), a.RESOURCE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

                gridView.SetFixCol(new string[] {  "CHK"
                    , "PLANT_CODE"
                    , "PROCESS_KEY"
                    , "L_CODE"
                    , "LOCATION"
                    , "RESOURCE_NO"
                    , "BIN_NAME"
                    , "BIN_SERIAL"
                    , "SCALE_CODE"});

                sValid = new string[] { "LOCATION" };

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

                gridCHK.ValueChecked = "Y";
                gridCHK.ValueUnchecked = "N";
                gridCHK.NullStyle = StyleIndeterminate.Unchecked;
                gridCHK.CheckStyle = CheckStyles.Standard;

                GridColumn colck = gridView.Columns["CHK"];
                colck.ColumnEdit = gridCHK;
                colck.OptionsColumn.AllowSort = DefaultBoolean.False; // 정렬 막기 (선택)
                colck.OptionsColumn.AllowEdit = true;

                string argResType = string.Empty;
                string argProFilter = string.Empty;

                // 플랜트
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPlant, clsCommon.GetPlant("", true));

                // 공정
                gridcboPROCESS_KEY.NullText = "";
                gridcboPROCESS_KEY.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROCESS_KEY, clsCommon.GetGridProcess(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString()));

                // 라인
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString()));

                // 원료
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString(), argResType, "", 0, true));

                // 스케일
                repItemLkUpEdit_SCALE_CODE.NullText = "";
                repItemLkUpEdit_SCALE_CODE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_SCALE_CODE, clsCommon.GetScale(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString()), "", false, false);

                // 빈 타입
                repItemLkUpEdit_BIN_GUBUN.NullText = "";
                repItemLkUpEdit_BIN_GUBUN.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_BIN_GUBUN, clsCommon.GetBinType());

                // PLC IP
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridsCboPLC_ADDRESS, clsCommon.GetPLCIP("빈"), "", false);

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다" + SQL);
            }
        }


        #endregion

        #region 버튼 이벤트

        private void btn_search_Click(object sender, EventArgs e)
        {
            bChangeValue = false;

            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDefault()) return;

            clsDevexpressGrid.GridViewAddRow(gridView);

            gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
            gridView.SetFocusedRowCellValue("PROCESS_KEY", cboProcessKey.EditValue);
            gridView.SetFocusedRowCellValue("L_CODE", cboL_Code.EditValue);
        }

        private bool CheckDefault()
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                return false;
            }

            if (cboProcessKey.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("공정을 먼저 조회 해주세요.");
                cboProcessKey.Focus();
                return false;
            }

            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("라인을 먼저 조회 해주세요.");
                cboL_Code.Focus();
                return false;
            }

            return true;
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        #region 저장하기 버튼클릭 이벤트

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("빈정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            bChangeValue = false;

            XMain_Save();
        }

        private void XMain_Save()
        {
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

                splashScreenManager.ShowWaitForm();

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

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO BIN (
                              PLANT_CODE, PROCESS_KEY, L_CODE 
                            , LOCATION, RESOURCE_NO, BIN_NAME
                            , B_STOCK, HI_Q, LO_Q
                            , MAX_CAPA, FAIL, P1_FAIL, P2_FAIL, P3_FAIL, P4_FAIL, P5_FAIL
                            , HZ_01, HZ_02, M_RATE, M_ON, M_OFF, ROW_RATE
                            , S_ON, S_OFF, HZ_03
                            , HL_ERROR, LO_ERROR, STOCK, JOG_ON_TIME, DROP_SAFE_T
                            , SEQ, SCALE_CODE, BIN_GUBUN, BIN_SERIAL
                            , PLC_ADDRESS, STD, SDD, ERP_LOCATION, I_TIME) 
                        VALUES ( 
                             '{dr["PLANT_CODE"]}', '{dr["PROCESS_KEY"]}', '{dr["L_CODE"]}'
                            , '{dr["LOCATION"]}', '{dr["RESOURCE_NO"]}', '{dr["BIN_NAME"]}'
                           , '{dr["B_STOCK"]}', '{dr["HI_Q"]}', '{dr["LO_Q"]}'
                           , '{dr["MAX_CAPA"]}', '{dr["FAIL"]}', '{dr["P1_FAIL"]}', '{dr["P2_FAIL"]}', '{dr["P3_FAIL"]}'
                           , '{dr["P4_FAIL"]}', '{dr["P5_FAIL"]}', '{dr["HZ_01"]}', '{dr["HZ_02"]}'
                           , '{dr["M_RATE"]}', '{dr["M_ON"]}', '{dr["M_OFF"]}', '{dr["ROW_RATE"]}'
                           , '{dr["S_ON"]}', '{dr["S_OFF"]}', '{dr["HZ_03"]}'
                           , '{dr["HL_ERROR"]}', '{dr["LO_ERROR"]}', '{dr["STOCK"]}', '{dr["JOG_ON_TIME"]}', '{dr["DROP_SAFE_T"]}'
                           , '{dr["SEQ"]}', '{dr["SCALE_CODE"]}', '{dr["BIN_GUBUN"]}', '{dr["BIN_SERIAL"]}'
                           , '{dr["PLC_ADDRESS"]}', '{dr["STD"]}', '{dr["SDD"]}', '{dr["ERP_LOCATION"]}', SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(BIN)데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                         string bin_stock = string.Empty;

                        //빈원료가 바뀌었을 경우 재고 초기화
                        SQL = $"SELECT RESOURCE_NO FROM BIN WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND LOCATION = '{dr["LOCATION"]}' AND RESOURCE_NO IS NOT NULL";

                        using (DataSet binResDs = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(binResDs) > 0)
                            {
                                if (!dr["RESOURCE_NO"].ToString().Equals(Dbconn.conn.getData(binResDs, "RESOURCE_NO", 0)))
                                {
                                    bin_stock = "0";
                                }
                                else
                                {
                                    bin_stock = dr["STOCK"].ToString();
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(bin_stock))
                        {
                            bin_stock = dr["STOCK"].ToString();
                        }


                        SQL = $@"
                        UPDATE BIN
                        SET RESOURCE_NO     = '{dr["RESOURCE_NO"]}',
                            BIN_NAME        = '{dr["BIN_NAME"]}',
                            BIN_SERIAL      = '{dr["BIN_SERIAL"]}',
                            HI_Q            = '{dr["HI_Q"]}',
                            LO_Q            = '{dr["LO_Q"]}',
                            MAX_CAPA        = '{dr["MAX_CAPA"]}',
                            FAIL            = '{dr["FAIL"]}',
                            P1_FAIL         = '{dr["P1_FAIL"]}',
                            P2_FAIL         = '{dr["P2_FAIL"]}',
                            P3_FAIL         = '{dr["P3_FAIL"]}',
                            P4_FAIL         = '{dr["P4_FAIL"]}',
                            P5_FAIL         = '{dr["P5_FAIL"]}',
                            HZ_01           = '{dr["HZ_01"]}',
                            HZ_02           = '{dr["HZ_02"]}',
                            EMPLOYEE_NO     = '{clsCommon.UserId}',
                            M_RATE          = '{dr["M_RATE"]}',
                            M_ON            = '{dr["M_ON"]}',
                            M_OFF           = '{dr["M_OFF"]}',
                            ROW_RATE        = '{dr["ROW_RATE"]}',
                            S_ON            = '{dr["S_ON"]}',
                            S_OFF           = '{dr["S_OFF"]}',
                            HZ_03           = '{dr["HZ_03"]}',
                            HL_ERROR        = '{dr["HL_ERROR"]}',
                            LO_ERROR        = '{dr["LO_ERROR"]}',
                            B_STOCK         = '{dr["B_STOCK"]}',
                            STOCK           = '{bin_stock}',
                            JOG_ON_TIME     = '{dr["JOG_ON_TIME"]}',
                            DROP_SAFE_T     = '{dr["DROP_SAFE_T"]}',
                            PROCESS_KEY     = '{dr["PROCESS_KEY"]}',
                            SEQ             = CASE WHEN EXISTS (
                                                          SELECT 1
                                                          FROM BIN x
                                                          WHERE x.PLANT_CODE  = '{dr["PLANT_CODE"]}'
                                                            AND x.PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                                                            AND x.L_CODE      = '{dr["L_CODE"]}'
                                                            AND x.RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                                                            AND x.LOCATION   <> '{dr["LOCATION"]}'
                                                            AND x.SEQ = '1'
                                                     )
                                                     THEN '2'
                                                     ELSE '1'
                                                   END,
                            BIN_GUBUN       = '{dr["BIN_GUBUN"]}',
                            SCALE_CODE      = '{dr["SCALE_CODE"]}',
                            PLC_ADDRESS     = '{dr["PLC_ADDRESS"]}',
                            STD             = '{dr["STD"]}', 
                            SDD             = '{dr["SDD"]}', 
                            ERP_LOCATION    = '{dr["ERP_LOCATION"]}',
                            I_TIME          = SYSDATE
                        WHERE PLANT_CODE    = '{dr["PLANT_CODE"]}'
                            AND LOCATION    =  '{dr["LOCATION"]}'
                            AND PROCESS_KEY =  '{dr["PROCESS_KEY"]}'
                            AND L_CODE      =  '{dr["L_CODE"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning(dr.RowError);
                            return;
                        }

                        SQL = $@"
                        UPDATE BIN
                        SET RESOURCE_NO     = '{dr["RESOURCE_NO"]}',
                            HI_Q            = '{dr["HI_Q"]}',
                            LO_Q            = '{dr["LO_Q"]}',
                            MAX_CAPA        = '{dr["MAX_CAPA"]}',
                            STOCK           = '{bin_stock}'
                        WHERE PLANT_CODE    = '{dr["PLANT_CODE"]}'
                            AND LOCATION    =  '{dr["LOCATION"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning(dr.RowError);
                            return;
                        }

                        int bin_seq = 0;
                        if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                        {
                            bin_seq = 0;

                        }
                        else
                        {
                            SQL = $@"
                            SELECT SEQ
                            FROM BIN
                            WHERE PLANT_CODE    = '{dr["PLANT_CODE"]}'
                                AND PROCESS_KEY =  '{dr["PROCESS_KEY"]}'
                                AND L_CODE      =  '{dr["L_CODE"]}'
                                AND LOCATION    =  '{dr["LOCATION"]}'
                                AND SCALE_CODE  = '{dr["SCALE_CODE"]}' AND RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                            ";

                            bin_seq = Dbconn.conn.getRowCnt(Dbconn.conn.ExecutDataset(SQL));
                        }

                        //SQL = $@"
                        //UPDATE BIN SET SEQ = '2'
                        //WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY =  '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                        //    AND LOCATION != '{dr["LOCATION"]}' AND RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                        //";

                        //Dbconn.conn.SQLrun(SQL);

                    }
                    dr.AcceptChanges();
                    gridView.RefreshData();
                }

                if (clsCommon._strMainPlcConnYn == "Y" && clsCommon._strSubPlcConnYn == "Y")
                {
                    //PLC변경신호 플래그 D90
                    //EthernetConnector connector = new EthernetConnector(new TcpSocket(clsCommon.plc_dosing_ip, 10260), false);

                    try
                    {
                        //Task<bool> returnTaskResult = clsPlcConnManager.Plc_conn(connector);
                        //bool connResult = await returnTaskResult;

                        //if (!connResult)
                        //{
                        //    ShowMessageBox.XtraShowInformation("PLC 연결에 실패했습니다");
                        //    return;
                        //}

                        ////PLC D70~D80 까지 1플래그 전송
                        //int[] reflashFlag = { 1,1,1,1,1,1,1,1,1,1 };
                        //Task<bool> returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "70", reflashFlag);
                        //bool sendResult = await returnTaskSendResult;
                        //if (!sendResult)
                        //{
                        //    clsLog.logSave(this, "btn_save_Click", "PLC전송실패 / D90 ");
                        //    ShowMessageBox.XtraShowWarning("PLC 전송에 실패했습니다 / " + "D151");
                        //    return;
                        //}
                    }
                    finally
                    {
                        //if (connector.IsConnected)
                        //{
                        //    connector.Disconnect();
                        //}
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowInformation("빈을 저장 했습니다.");

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }
        #endregion

        /// <summary>
        /// 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (gridView.SelectedRowsCount == 0)
                {
                    XtraMessageBox.Show("삭제하실 빈을 선택하여 주세요");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(gridView);

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 빈을 삭제하시겠습니까?"))
                {
                    return;
                }

                DataTable dt = (gridControl.DataSource as DataTable);

                if (dt == null)
                    return;

                DataTable chkDt = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("CHK") == "Y")
                    .CopyToDataTable();

                for (int i = 0; i < chkDt.Rows.Count; i++)
                {
                    var dr = chkDt.Rows[i];

                    dr.ClearErrors();

                    SQL = $@"
                    DELETE FROM BIN
                    WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND L_CODE = '{dr["L_CODE"]}'
                        AND LOCATION = '{dr["LOCATION"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                        return;
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_plcUpload_Click", ex.Message + "/" + ex.StackTrace);
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            ShowMessageBox.XtraShowInformation("데이터를 삭제 했습니다.");
        }

        #region 새로고침 버튼클릭 이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }
        #endregion

        /// <summary>
        /// 체크 PLC 업로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPLCUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (bChangeValue == true)
            {
                ShowMessageBox.XtraShowWarning("변경된 데이터가 있습니다. 조회하여 초기화 하거나 저장 후 진행 해주세요.");
                return;
            }

            DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            List<string> plcIP = new List<string>();
            List<string> plcType = new List<string>();

            if (dtPlc != null && dtPlc.Rows.Count > 0)
            {
                for (int i = 0; i < dtPlc.Rows.Count; i++)
                {
                    plcIP.Add(dtPlc.Rows[i]["IP"]?.ToString());
                    plcType.Add(dtPlc.Rows[i]["PLC_TYPE"]?.ToString());
                }
            }

            if (!GetPlcCon(plcIP, plcType)) return;

            if (DialogResult.Yes != MessageBox.Show("빈설정 데이타를 PLC에 전송하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
            {
                return;
            }

            try
            {
                string Dev = string.Empty;
                int i = 0;

                short[] Sdata = new short[10];

                int in_scale = 1;

                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("빈을 PLC에 전송중입니다");

                // 공정별로 빈 파라미터의 데이타를 조회 한다. 

                // 그리드의 편집 상태 먼저 취소
                gridView1.CloseEditor();
                gridView1.CancelUpdateCurrentRow();

                clsDevexpressGrid.GridEndEdit(gridView);

                DataTable dt = gridControl.DataSource as DataTable;

                //if (dt != null)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (dr.RowState != DataRowState.Unchanged)
                //        {
                //            // CHK 값을 임시 저장
                //            object chkValue = dr["CHK"];

                //            // 원래 값으로 롤백
                //            dr.RejectChanges();

                //            // 롤백 후 CHK 값을 다시 설정
                //            dr["CHK"] = chkValue;
                //        }
                //    }
                //}

                DataTable chkDt = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("CHK") == "Y")
                    .OrderBy(row => row.Field<string>("PLC_ADDRESS"))
                    .CopyToDataTable();

                if (chkDt == null || chkDt.Rows.Count == 0)
                {
                    // 데이터가 없음 → 처리 로직
                    MessageBox.Show("체크된 빈이 없습니다.");
                    return;
                }

                if (!UploadSetDevice(chkDt, dtPlc))
                {
                    ShowMessageBox.XtraShowWarning("빈 정보를 PLC로 Uplaod가 실패 했습니다.");
                    return;
                }

                ShowMessageBox.XtraShowInformation("빈 정보를 PLC로 Uplaod 완료하였습니다");
            }
            catch (COMException exx)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, exx);
                ShowMessageBox.XtraShowWarning("MELSEC PLC 연결모듈 불러오기에 실패하였습니다");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                // GBG
                //clsMelsec.plc_scale_dosing.Close();
                // GBG -

                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private static bool GetPlcCon(List<string> plcIP, List<string> plcType)
        {
            switch (plcIP.Count)
            {
                case 1:
                    if ((plcType[0] == "Q" && MAIN.MainPlcConnChk != "Y"))
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        return false;
                    }

                    if (plcType[0] == "A" && MAIN.SubPlcConnChk != "Y")
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                        return false;
                    }

                    // GBG
                    if (plcType[0] == "XGI" && MAIN.MainPlcConnChk != "Y")
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        return false;
                    }

                    if (plcType[0] == "CM" && MAIN.MainPlcConnChk != "Y")
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        return false;
                    }
                    // GBG -

                    break;
                case 2:
                    if ((MAIN.MainPlcConnChk != "Y") || (MAIN.SubPlcConnChk != "Y"))
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                        return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        private bool UploadSetDevice(DataTable chkDt, DataTable dtPlc)
        {
            string Dev = string.Empty;
            string plcIP = string.Empty;

            // GBG
            short[] Sdata = new short[10];
            int[] Sdata2 = new int[20];
            int[] sAddr = new int[5] { 0, 220, 440, 500, 580 };

            Array.Clear(Buff, 0, Buff.Length);

            // GBG -

            int in_scale = 1;

            List<PlcUdpClient> clients = new List<PlcUdpClient>();

            string[] plcAddresses = chkDt.AsEnumerable()
            .Select(row => row.Field<string>("PLC_ADDRESS"))
            .Where(addr => !string.IsNullOrWhiteSpace(addr)) // 공백 또는 null 제거 (선택)
            .Distinct()
            .ToArray();

            if (plcAddresses.Length == 0)
                return false;

            // 연결
            foreach (var ip in plcAddresses)
            {
                DataRow[] resultRows = chkDt.AsEnumerable()
                   .Where(row => row.Field<string>("PLC_ADDRESS") == ip)
                   .ToArray();

                for (int i = 0; i < resultRows.Length; i++)
                {
                    DataRow dr = resultRows[i];

                    Dev = dr["STD"]?.ToString();

                    // GBG
                    if (string.IsNullOrEmpty(Dev.Trim())) continue;
                    // GBG -

                    //ShowMessageBox.XtraShowInformation("STD : " + dr["STD"]?.ToString());

                    plcIP = dr["PLC_ADDRESS"]?.ToString();

                    //ShowMessageBox.XtraShowInformation("PLC_ADDRESS : " + dr["PLC_ADDRESS"]?.ToString());

                    in_scale = Convert.ToInt16(dr["IN_SCALE"]);

                    //ShowMessageBox.XtraShowInformation("IN_SCALE : " + dr["IN_SCALE"]?.ToString());

                    // 빈 파라미터를 스케일별로 쏴준다. 
                    Array.Clear(Sdata, 0, Sdata.Length);
                    Array.Clear(Sdata2, 0, Sdata2.Length);

                    //double temp = Convert.ToDouble(chkDt.Rows[i]["MAX_CAPA"]) * in_scale;

                    //if (temp > short.MaxValue)
                    //{
                    //    ShowMessageBox.XtraShowWarning($"(최대용량, 잔량 * 스케일배율)은 SCALE ({short.MaxValue})용량 최대값을 초과 할 수 없습니다.");
                    //    return false;
                    //}

                    /*
                        D2300   빈번호        LOCATION
                        D2301   낙차          FAIL
                        D2302   편차+         HL_ERROR
                        D2303   편차-         LO_ERROR
                        D2304   소투입        ROW_RATE       
                        D2305   소HZ(ONT)     S_ON
                        D2306   OFFT          S_OFF
                        D2307   안정T         DROP_SAFE_T
                        D2308   빈재고        STOCK
                    */

                    short location = (dr["LOCATION"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["LOCATION"].ToString()))
                                ? (short)0
                                : Convert.ToInt16(Convert.ToDouble(dr["LOCATION"]));

                    short fail = (dr["FAIL"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["FAIL"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["FAIL"]) * in_scale, 0);

                    short hlError = (dr["HL_ERROR"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["HL_ERROR"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["HL_ERROR"]) * in_scale, 0);

                    short loError = (dr["LO_ERROR"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["LO_ERROR"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["LO_ERROR"]) * in_scale, 0);

                    short rowRate = (dr["ROW_RATE"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["ROW_RATE"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["ROW_RATE"]) * in_scale, 0);

                    short hz03 = (dr["HZ_03"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["HZ_03"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["HZ_03"]) * 10, 0);

                    short sOff = (dr["S_OFF"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["S_OFF"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["S_OFF"]) * 10, 0);

                    short dropSafeT = (dr["DROP_SAFE_T"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["DROP_SAFE_T"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["DROP_SAFE_T"]) * 10, 0);

                    short stock = (dr["STOCK"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["STOCK"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["STOCK"]) * in_scale, 0);

                    short fail1 = (dr["P1_FAIL"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["P1_FAIL"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["P1_FAIL"]) * in_scale, 0);

                    short fail2 = (dr["P2_FAIL"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["P2_FAIL"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["P2_FAIL"]) * in_scale, 0);

                    short fail3 = (dr["P3_FAIL"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["P3_FAIL"].ToString()))
                                ? (short)0
                                : (short)Math.Round(Convert.ToDouble(dr["P3_FAIL"]) * in_scale, 0);

                    Sdata[0] = location;
                    Sdata[1] = fail;
                    Sdata[2] = hlError;
                    Sdata[3] = loError;
                    Sdata[4] = rowRate;
                    Sdata[5] = hz03;
                    Sdata[6] = sOff;
                    Sdata[7] = dropSafeT;
                    Sdata[8] = stock;

                    Sdata2[0] = location;
                    Sdata2[1] = fail;
                    Sdata2[2] = hlError;
                    Sdata2[3] = loError;
                    Sdata2[4] = rowRate;
                    Sdata2[5] = hz03;
                    Sdata2[6] = sOff;
                    Sdata2[7] = dropSafeT;
                    Sdata2[8] = stock;

                    Sdata2[10] = fail1;
                    Sdata2[11] = fail2;
                    Sdata2[12] = fail3;

                    DataRow[] drPlc = dtPlc.Select($"IP = '{plcIP}'");

                    if (drPlc == null || drPlc.Length == 0)
                        continue;

                    if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {

                        if (MAIN.qPlc1.WriteDeviceBlock2(Dev, 10, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    else if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc2.WriteDeviceBlock2(Dev, 10, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    // GBG2
                    else if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc1.WriteDeviceBlock2(Dev, 10, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    else if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc2.WriteDeviceBlock2(Dev, 10, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    // GBG2 -
                    else if (drPlc[0]["PLC_TYPE"].ToString() == "XGI")
                    {
                        if (!Dev.StartsWith("%")) continue;

                        if (clsXgiHandler.Write(2, Dev, 15, Sdata2) == 0)
                        {
                            if (clsXgiHandler.Write(2, Dev, 15, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                                return false;
                            }
                        }
                    }
                    else if (drPlc[0]["PLC_TYPE"].ToString() == "CM")
                    {
                        if (!Dev.StartsWith("D")) continue;

                        if (clsCimonHandler2.Write(2, Dev, 10, Sdata2) == 0)
                        {
                            clsUtil.Delay(500);
                            if (clsCimonHandler2.Write(2, Dev, 10, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                                return false;
                            }
                        }
                        clsUtil.Delay(100);
                        // GBG -
                    }
                    // GBG -
                }
            }

            return true;
        }

        /// <summary>
        /// 체크 PLC 다운로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPLCDownLoad_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            string platCode = string.Empty;
            string processKey = string.Empty;
            string lCode = string.Empty;
            string Dev = string.Empty;
            int in_scale = 0;
            string sPlcIP = string.Empty;

            string SQL = null;

            DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            List<string> plcIP = new List<string>();
            List<string> plcType = new List<string>();
            if (dtPlc != null && dtPlc.Rows.Count > 0)
            {
                for (int i = 0; i < dtPlc.Rows.Count; i++)
                {
                    plcIP.Add(dtPlc.Rows[i]["IP"]?.ToString());
                    plcType.Add(dtPlc.Rows[i]["PLC_TYPE"]?.ToString());
                }
            }

            if (!GetPlcCon(plcIP, plcType)) return;

            if (DialogResult.Yes != MessageBox.Show("PLC에서 빈설정 데이타를 읽어오시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
            {
                return;
            }

            try
            {
                short[] Sdata = new short[15];
                // GBG
                int[] Sdata2 = new int[15];
                // GBG -

                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("PLC 빈 정보를 읽어오는중입니다.");

                // 그리드의 편집 상태 먼저 취소
                gridView1.CloseEditor();               // 현재 셀 에디터 닫기
                gridView1.CancelUpdateCurrentRow();   // 현재 행의 수정 내용 취소

                clsDevexpressGrid.GridEndEdit(gridView);

                DataTable dt = gridControl.DataSource as DataTable;

                //if (dt != null)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (dr.RowState != DataRowState.Unchanged)
                //        {
                //            // CHK 값을 임시 저장
                //            object chkValue = dr["CHK"];

                //            // 원래 값으로 롤백
                //            dr.RejectChanges();

                //            // 롤백 후 CHK 값을 다시 설정
                //            dr["CHK"] = chkValue;
                //        }
                //    }
                //}

                DataTable chkDt = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("CHK") == "Y")
                    .OrderBy(row => row.Field<string>("PLC_ADDRESS"))
                    .CopyToDataTable();

                if (chkDt == null || chkDt.Rows.Count == 0)
                {
                    // 데이터가 없음 → 처리 로직
                    MessageBox.Show("체크된 빈 이 없습니다.");
                    return;
                }

                for (int i = 0; i < chkDt.Rows.Count; i++)
                {
                    platCode = chkDt.Rows[i]["PLANT_CODE"]?.ToString();
                    processKey = chkDt.Rows[i]["PROCESS_KEY"]?.ToString();
                    lCode = chkDt.Rows[i]["L_CODE"]?.ToString();
                    Dev = chkDt.Rows[i]["STD"]?.ToString();

                    if (chkDt.Rows[i].Table.Columns.Contains("IN_SCALE") &&
                        chkDt.Rows[i]["IN_SCALE"] != DBNull.Value &&
                        !string.IsNullOrWhiteSpace(chkDt.Rows[i]["IN_SCALE"].ToString()))
                    {
                        in_scale = Convert.ToInt16(chkDt.Rows[i]["IN_SCALE"]);
                    }

                    if (string.IsNullOrEmpty(Dev))
                    {
                        continue;
                    }

                    sPlcIP = chkDt.Rows[i]["PLC_ADDRESS"]?.ToString();

                    Array.Clear(Sdata, 0, Sdata.Length);
                    Array.Clear(Sdata2, 0, Sdata2.Length);

                    DataRow[] drPlc = dtPlc.Select($"IP = '{sPlcIP}'");

                    if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc1.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc2.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    // GBG
                    else if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc1.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    else if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc2.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    else if (plcType[0] == "XGI")
                    {
                        if (clsXgiHandler.Read(0, Dev, 10, Sdata2) == 0)
                        {
                            if (clsXgiHandler.Read(0, Dev, 10, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                                return;
                            }
                        }
                    }
                    else if (plcType[0] == "CM")
                    {
                        //_ = clsCimonHandler.TryReadWord(Dev, 10, Sdata2);
                        if (clsCimonHandler2.Read(0, Dev, 10, Sdata2) == 0)
                        {
                            clsUtil.Delay(500);
                            if (clsCimonHandler2.Read(0, Dev, 10, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("빈 파라미터 전송을 실패하였습니다.", "알림");
                                return;
                            }
                        }
                    }
                    // GBG -

                    /*
                        D2300   빈번호        LOCATION
                        D2301   낙차          FAIL
                        D2302   편차+         HL_ERROR
                        D2303   편차-         LO_ERROR
                        D2304   소투입        HZ_03        ?       ROW_RATE        ?
                        D2305   소HZ(ONT)     S_ON
                        D2306   OFFT          S_OFF
                        D2307   안정T         DROP_SAFE_T
                        D2308   빈재고        STOCK
                    */

                    SQL = $@"
                    UPDATE BIN
                       SET FAIL          = '{(ushort)Sdata[1] / in_scale}'
                         , HL_ERROR      = '{(ushort)Sdata[2] / in_scale}'
                         , LO_ERROR      = '{(ushort)Sdata[3] / in_scale}'
                         , ROW_RATE         = '{(ushort)Sdata[4]}'
                         , HZ_03          = '{(ushort)Sdata[5] / 10}'
                         , S_OFF         = '{(ushort)Sdata[6] / 10}'
                         , DROP_SAFE_T   = '{(ushort)Sdata[7] / 10}'
                         , STOCK         = '{(ushort)Sdata[8] / 10}'
                     WHERE PLANT_CODE    = '{platCode}'
                        AND PROCESS_KEY  = '{processKey}'
                        AND L_CODE       = '{lCode}'
                        AND LOCATION     = '{Sdata[0]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        ShowMessageBox.XtraShowError("DB에서 데이타쓰기를 실패하였습니다");
                        break;
                    }
                }

                XMain_Search();

                ShowMessageBox.XtraShowInformation("PLC에서 빈정보를 읽기 완료하였습니다");

            }
            catch (COMException exx)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, exx);
                ShowMessageBox.XtraShowWarning("MELSEC PLC 연결모듈 불러오기에 실패하였습니다");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex);
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

        #region 빈우선순위 버튼 클릭 이벤트
        private void btn_dupSeqBin_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                return;
            }

            if (cboProcessKey.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("공정을 먼저 조회 해주세요.");
                cboProcessKey.Focus();
                return;
            }

            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("라인을 먼저 조회 해주세요.");
                cboL_Code.Focus();
                return;
            }

            m_BinSeqDupChack mBinSeqDup = new m_BinSeqDupChack(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), cboL_Code.EditValue?.ToString());
            mBinSeqDup.StartPosition = FormStartPosition.CenterScreen;
            mBinSeqDup.Show();
        }

        /// <summary>
        /// 빈 원료 없음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_binNameReset_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("빈원료를 지우실 빈을 선택하여 주세요");
                return;
            }

            string sel_LOCATION = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION");

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 " + sel_LOCATION + " 빈의 원료를 없음으로 변경하시겠습니까?"))
            {
                return;
            }

            SQL = $@"
            UPDATE BIN
            SET RESOURCE_NO = NULL,  SEQ = '0'
            WHERE PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' 
                AND PROCESS_KEY =  '{gridView.GetFocusedRowCellValue("PROCESS_KEY")}' 
                AND L_CODE = '{gridView.GetFocusedRowCellValue("L_CODE")}'
                AND LOCATION = '{sel_LOCATION}' ";

            Dbconn.conn.SQLrun(SQL);


            //스케일에 원료 1개인 빈은 순번 1로 조정
            SQL = $@"
            UPDATE BIN B
                SET B.SEQ = 1
                WHERE (B.LOCATION, B.RESOURCE_NO) IN
            (
                SELECT LOCATION, RESOURCE_NO
                FROM
                    (
                        SELECT LOCATION,
                                RESOURCE_NO,
                                SEQ,
                                ROW_NUMBER() OVER(PARTITION BY RESOURCE_NO ORDER BY LOCATION) AS RN,
                                SUM(CASE WHEN SEQ = 1 THEN 1 ELSE 0 END) 
                                    OVER(PARTITION BY RESOURCE_NO) AS CNT_SEQ1
                            FROM BIN
                        WHERE SCALE_CODE IS NOT NULL
                            AND PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' 
                            AND PROCESS_KEY =  '{gridView.GetFocusedRowCellValue("PROCESS_KEY")}' 
                            AND L_CODE = '{gridView.GetFocusedRowCellValue("L_CODE")}'
                    )
                WHERE CNT_SEQ1 = 0
                    AND RN = 1
            )
            ";

            Dbconn.conn.SQLrun(SQL);

            XMain_Search();
        }
        #endregion

        #endregion

        #region 그리드 값변경후 이벤트
        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView.UpdateCurrentRow();

            if (e.Column.FieldName == "PROCESS_KEY")  // 수량이 변경된 경우
            {
                //라인
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString()), "", false);
            }

            if (e.Column.FieldName != "CHK")
            {
                bChangeValue = true;
            }
        }
        #endregion

        #region 빈원료 변경전 이벤트
        private void repItemLkUpEdit_RESOURCE_NO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            gridView.SetRowCellValue(gridView.FocusedRowHandle, "RESOURCE_CODE", e.NewValue);
        }
        #endregion



        #region 그리드 로우넘버 그리기
        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }
        #endregion

        private void gridView_KeyUp(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;

            if (clsDevexpressGrid.GetSelectRowCount(view) > 0)
            {
                if (view.FocusedColumn.Name == "RESOURCE_NO")
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["RESOURCE_CODE"], "");
                        view.SetRowCellValue(view.FocusedRowHandle, view.Columns["RESOURCE_NO"], "");
                    }
                }
            }
        }

        private void gridView_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "SCALE_CODE")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.GetScale(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), cboL_Code.EditValue?.ToString()), "", false, 0, false);
                edit.EditValue = gridView.GetFocusedRowCellValue("SCALE_CODE");
                edit.Properties.PopupFormMinSize = new Size(200, 650);

                edit.KeyDown += Edit_KeyDown;
            }

            //if (view.FocusedColumn.FieldName == "RESOURCE_NO")
            //{
            //    SearchLookUpEdit edit = (SearchLookUpEdit)view.ActiveEditor;
            //    edit.ShowPopup();
            //    edit.ClosePopup();

            //    // 여기
            //    clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
            //    edit.Properties.PopupFormMinSize = new Size(200, 300);
            //}
        }

        private void Edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                (sender as LookUpEdit).EditValue = null;
                e.Handled = true;
            }
        }

        private void gridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn == null)
                return;

            if (e.FocusedColumn.FieldName == "BIN_GUBUN")
            {
                gridView.ShowEditor();

                if (((LookUpEdit)gridView.ActiveEditor) == null)
                    return;

                ((LookUpEdit)gridView.ActiveEditor).ShowPopup();
            }

            if (e.FocusedColumn.FieldName == "RESOURCE_NO")
            {
                gridView.ShowEditor();

                if (((SearchLookUpEdit)gridView.ActiveEditor) == null)
                    return;

                ((SearchLookUpEdit)gridView.ActiveEditor).ShowPopup();
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;

            if (view.FocusedColumn.FieldName == "BIN_GUBUN")
            {
                view.ShowEditor();

                if (((LookUpEdit)view.ActiveEditor) == null)
                    return;

                ((LookUpEdit)view.ActiveEditor).ShowPopup();
            }

            if (view.FocusedColumn.FieldName == "RESOURCE_NO")
            {
                view.ShowEditor();

                if (((SearchLookUpEdit)view.ActiveEditor) == null)
                    return;

                ((SearchLookUpEdit)view.ActiveEditor).ShowPopup();
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
            gridView1.FocusedRowHandle = 0;
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
        }

        //private void GridView_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        //{
        //    if (e.Column.FieldName == "L_CODE")
        //    {
        //        string category = gridView1.GetRowCellValue(e.RowHandle, "PROCESS_KEY")?.ToString();

        //        gridView.SetRowCellValue(gridView.FocusedRowHandle, "PROCESS_KEY", category);
        //    }
        //}

        private void gridcboPROCESS_KEY_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "PROCESS_KEY", textEditor.EditValue?.ToString());
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl.DataSource = null;

                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);

                cboProcessKey.EditValue = vProcess_Code;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
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

        private void gridsCboPLC_ADDRESS_EditValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    TextEdit textEditor = (TextEdit)sender;

            //    DataTable dt = clsCommon.GetPLCIP("빈", textEditor.EditValue.ToString());

            //    // 선택된 row에서 type 가져오기
            //    DataRow[] rows = dt.Select($"CODE = '{textEditor.EditValue.ToString()}'");
            //    if (rows.Length > 0)
            //    {
            //        string typeValue = rows[0]["PLCIP"].ToString();

            //        // 현재 행의 type 컬럼에 값 세팅
            //        gridView.SetFocusedRowCellValue("PLC_ADDRESS", typeValue);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
            //    ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            //}
        }

        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

            // 헤더 클릭 && 대상 컬럼이 CHK일 때
            if (hitInfo.InColumn && hitInfo.Column.FieldName == "CHK")
            {
                // 현재 체크 상태 확인
                bool foundUnchecked = false;

                for (int i = 0; i < view.RowCount; i++)
                {
                    object val = view.GetRowCellValue(i, "CHK");

                    view.SetRowCellValue(i, "CHK", val.ToString() == "N" ? "Y" : "N");
                }

                // 강제로 헤더 다시 그림
                view.InvalidateColumnHeader(view.Columns["CHK"]);
            }
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                gridView.ExcelPasteClipboard("LOCATION"); // CODE 기준
                e.Handled = true;
            }
        }

        private void gridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;

            // 현재 포커스된 로우인지 확인
            if (e.RowHandle == view.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.LightPink;   // 배경색
                e.Appearance.ForeColor = Color.Black;       // 글자색
            }
        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (gridView.GetFocusedRowCellValue("STOCK")?.ToString() == "0")
            //{
            //    e.Cancel = false;
            //    return;
            //}

            //SQL = $@"
            //SELECT 1
            //FROM WORK_ORDER A
            //    LEFT JOIN WORK_DETAIL B ON B.PLANT_CODE = A.PLANT_CODE AND B.PROCESS_KEY = A.PROCESS_KEY AND B.L_CODE = A.L_CODE
            //                AND B.WORKDATE = A.WORKDATE AND B.NUM = A.NUM
            //WHERE A.C_CONDITION = '{clsCommon.GetPcStatusCode("진행")}'
            //            AND A.PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
            //            AND (B.LOCATION = '{gridView.GetFocusedRowCellValue("LOCATION")}' 
            //                OR A.LOCATION_ED = '{gridView.GetFocusedRowCellValue("LOCATION")}')
            //            AND (A.RESOURCE_NO = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}'
            //                OR B.INGRED_CODE = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}')
            //";

            //DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            //if (Dbconn.conn.getRowCnt(ds) > 0)
            //{
            //    ShowMessageBox.XtraShowInformation("현재 작업중인 제품/원료/재고는 수정 할 수 없습니다.");

            //    switch (gridView.FocusedColumn.FieldName)
            //    {
            //        case "RESOURCE_NO":
            //        case "STOCK":
            //            e.Cancel = true;
            //            break;

            //        default:
            //            e.Cancel = false;
            //            break;
            //    }
            //}
        }

        private void txtBinName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }                
        }
    }
}