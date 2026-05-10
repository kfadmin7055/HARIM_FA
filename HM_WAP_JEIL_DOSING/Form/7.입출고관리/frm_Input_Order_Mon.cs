using Core.Class;
using Core.Extension;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Input_Order_Mon : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;


        public frm_Input_Order_Mon()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_SCR_MG_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //배송일자
            dtFromREQ_DATE.EditValue = DateTime.Now.AddDays(-7);
            dtToREQ_DATE.EditValue = DateTime.Now;

            clsDevexpressUtil.ItemLookUpEditSetup(cboTransFlag, clsCommon.GetResultType(), "", false, 0, true);

            XMain_Search();


            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
            {
                string fromDate = Convert.ToDateTime(dtFromREQ_DATE.EditValue).ToString("yyyyMMdd");
                string toDate = Convert.ToDateTime(dtToREQ_DATE.EditValue).ToString("yyyyMMdd");

                SQL = $@"
                SELECT
                     a.DOC_NO
                   , a.DOC_ITEM
                   , a.SALES_ORG
                   , a.SALES_ORG_DESC
                   , a.SALES_GROUP
                   , a.SALES_GROUP_DESC
                   , a.SALES_TYPE
                   , a.SALES_TYPE_DESC
                   , a.REQ_DATE
                   , a.CUSTOMER
                   , a.CUSTOMER_NAME
                   , a.SCHCUSTOMER
                   , a.SCHCUSTOMER_NAME
                   , a.POST_FLAG
                   , a.SHIPTO
                   , a.SHIPTO_NAME
                   , TO_NUMBER(a.MATERIAL) AS MATERIAL
                   , a.MATERIAL_DESC
                   , a.SO_QTY_UNIT
                   , a.SO_BOX
                   , a.SO_EACH
                   , a.CNF_BOX
                   , a.CNF_EACH
                   , a.BASIC_QTY_UNIT
                   , a.SO_QTY
                   , a.CNF_QTY
                   , a.WEIGHT_UNIT
                   , a.GWEIGHT
                   , a.NWEIGHT
                   , a.CURRENCY
                   , a.PRICE
                   , a.PLANT
                   , a.PLANT_DESC
                   , a.PACKING_CODE
                   , a.PACKING_DESC
                   , a.TON_FLAG
                   , a.SALESPERSON
                   , a.SALESPERSON_NAME
                   , a.CREATE_DATE
                   , a.CREATE_TIME
                   , a.UPDATE_FLAG
                   , a.SO_NO
                   , a.SO_ITEM
                   , a.XDATS
                   , a.SEND_YN
                FROM SAP_INPUT_ORDER_MON a
                WHERE PLANT = '{cboPlant_Code.EditValue}'
                    AND ('{txtSALES_TYPE_DESC.EditValue}' IS NULL OR a.SALES_TYPE_DESC LIKE '%{txtSALES_TYPE_DESC.EditValue}%')
                    AND ('{txtCUSTOMER_NAME.EditValue}' IS NULL OR a.CUSTOMER_NAME LIKE '%{txtCUSTOMER_NAME.EditValue}%')
                    AND ('{cboTransFlag.EditValue}' IS NULL OR a.UPDATE_FLAG LIKE '%{cboTransFlag.EditValue}%')
                    AND a.REQ_DATE BETWEEN '{fromDate}' AND '{toDate}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                gridView.SetFixCol(new string[] {  "UPDATE_FLAG"
                                                , "PROC_TYPE"
                                                , "XSEQNR_C"
                                                , "MATNR_NAME"
                                                , "UMMAT_NAME"});

                sValid = new string[] { "" };


                // 전송구분
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTRANS_FLAG, clsCommon.GetTransFlag());

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        #region 버튼 이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            gridView.AddNewRow();
            int newRowHandle = gridView.FocusedRowHandle;

            gridView.SetRowCellValue(newRowHandle, gridView.Columns["I_TIME"], DateTime.Now);

            gridView.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridControl.DataSource;

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

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_INPUT_ORDER_MON_CON (
                            DOC_NO, DOC_ITEM, SALES_ORG, 
                            SALES_ORG_DESC, SALES_GROUP, SALES_GROUP_DESC, 
                            SALES_TYPE, SALES_TYPE_DESC, REQ_DATE, 
                            CUSTOMER, CUSTOMER_NAME, SCHCUSTOMER, 
                            SCHCUSTOMER_NAME, POST_FLAG, SHIPTO, 
                            SHIPTO_NAME, MATERIAL, MATERIAL_DESC, 
                            SO_QTY_UNIT, SO_BOX, SO_EACH, 
                            CNF_BOX, CNF_EACH, BASIC_QTY_UNIT, 
                            SO_QTY, CNF_QTY, WEIGHT_UNIT, 
                            GWEIGHT, NWEIGHT, CURRENCY, 
                            PRICE, PLANT, PLANT_DESC, 
                            PACKING_CODE, PACKING_DESC, TON_FLAG, 
                            SALESPERSON, SAELSPERSON_NAME, CREATE_DATE, 
                            CREATE_TIME, UPDATE_FLAG, SO_NO, 
                            SO_ITEM, XDATS, SEND_YN) 
                        VALUES ( 
                              '{dr["DOC_NO"]}'
                            , '{dr["DOC_ITEM"]}'
                            , '{dr["SALES_ORG"]}'
                            , '{dr["SALES_ORG_DESC"]}'
                            , '{dr["SALES_GROUP"]}'
                            , '{dr["SALES_GROUP_DESC"]}'
                            , '{dr["SALES_TYPE"]}'
                            , '{dr["SALES_TYPE_DESC"]}'
                            , '{dr["REQ_DATE"]}'
                            , '{dr["CUSTOMER"]}'
                            , '{dr["CUSTOMER_NAME"]}'
                            , '{dr["SCHCUSTOMER"]}'
                            , '{dr["SCHCUSTOMER_NAME"]}'
                            , '{dr["POST_FLAG"]}'
                            , '{dr["SHIPTO"]}'
                            , '{dr["SHIPTO_NAME"]}'
                            , '{dr["MATERIAL"]}'
                            , '{dr["MATERIAL_DESC"]}'
                            , '{dr["SO_QTY_UNIT"]}'
                            , '{dr["SO_BOX"]}'
                            , '{dr["SO_EACH"]}'
                            , '{dr["CNF_BOX"]}'
                            , '{dr["CNF_EACH"]}'
                            , '{dr["BASIC_QTY_UNIT"]}'
                            , '{dr["SO_QTY"]}'
                            , '{dr["CNF_QTY"]}'
                            , '{dr["WEIGHT_UNIT"]}'
                            , '{dr["GWEIGHT"]}'
                            , '{dr["NWEIGHT"]}'
                            , '{dr["CURRENCY"]}'
                            , '{dr["PRICE"]}'
                            , '{dr["PLANT"]}'
                            , '{dr["PLANT_DESC"]}'
                            , '{dr["PACKING_CODE"]}'
                            , '{dr["PACKING_DESC"]}'
                            , '{dr["TON_FLAG"]}'
                            , '{dr["SALESPERSON"]}'
                            , '{dr["SAELSPERSON_NAME"]}'
                            , '{dr["CREATE_DATE"]}'
                            , '{dr["CREATE_TIME"]}'
                            , '{dr["UPDATE_FLAG"]}'
                            , '{dr["SO_NO"]}'
                            , '{dr["SO_ITEM"]}'
                            , '{dr["XDATS"]}'
                            , '{dr["SEND_YN"]}' )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SAP_INPUT_ORDER_MON_CON
                        SET DOC_NO           = '{dr["DOC_NO"]}',
                            DOC_ITEM         = '{dr["DOC_ITEM"]}',
                            SALES_ORG        = '{dr["SALES_ORG"]}',
                            SALES_ORG_DESC   = '{dr["SALES_ORG_DESC"]}',
                            SALES_GROUP      = '{dr["SALES_GROUP"]}',
                            SALES_GROUP_DESC = '{dr["SALES_GROUP_DESC"]}',
                            SALES_TYPE       = '{dr["SALES_TYPE"]}',
                            SALES_TYPE_DESC  = '{dr["SALES_TYPE_DESC"]}',
                            REQ_DATE         = '{dr["REQ_DATE"]}',
                            CUSTOMER         = '{dr["CUSTOMER"]}',
                            CUSTOMER_NAME    = '{dr["CUSTOMER_NAME"]}',
                            SCHCUSTOMER      = '{dr["SCHCUSTOMER"]}',
                            SCHCUSTOMER_NAME = '{dr["SCHCUSTOMER_NAME"]}',
                            POST_FLAG        = '{dr["POST_FLAG"]}',
                            SHIPTO           = '{dr["SHIPTO"]}',
                            SHIPTO_NAME      = '{dr["SHIPTO_NAME"]}',
                            MATERIAL         = '{dr["MATERIAL"]}',
                            MATERIAL_DESC    = '{dr["MATERIAL_DESC"]}',
                            SO_QTY_UNIT      = '{dr["SO_QTY_UNIT"]}',
                            SO_BOX           = '{dr["SO_BOX"]}',
                            SO_EACH          = '{dr["SO_EACH"]}',
                            CNF_BOX          = '{dr["CNF_BOX"]}',
                            CNF_EACH         = '{dr["CNF_EACH"]}',
                            BASIC_QTY_UNIT   = '{dr["BASIC_QTY_UNIT"]}',
                            SO_QTY           = '{dr["SO_QTY"]}',
                            CNF_QTY          = '{dr["CNF_QTY"]}',
                            WEIGHT_UNIT      = '{dr["WEIGHT_UNIT"]}',
                            GWEIGHT          = '{dr["GWEIGHT"]}',
                            NWEIGHT          = '{dr["NWEIGHT"]}',
                            CURRENCY         = '{dr["CURRENCY"]}',
                            PRICE            = '{dr["PRICE"]}',
                            PLANT            = '{dr["PLANT"]}',
                            PLANT_DESC       = '{dr["PLANT_DESC"]}',
                            PACKING_CODE     = '{dr["PACKING_CODE"]}',
                            PACKING_DESC     = '{dr["PACKING_DESC"]}',
                            TON_FLAG         = '{dr["TON_FLAG"]}',
                            SALESPERSON      = '{dr["SALESPERSON"]}',
                            SAELSPERSON_NAME = '{dr["SAELSPERSON_NAME"]}',
                            CREATE_DATE      = '{dr["CREATE_DATE"]}',
                            CREATE_TIME      = '{dr["CREATE_TIME"]}',
                            UPDATE_FLAG      = '{dr["UPDATE_FLAG"]}',
                            SO_NO            = '{dr["SO_NO"]}',
                            SO_ITEM          = '{dr["SO_ITEM"]}',
                            XDATS            = '{dr["XDATS"]}',
                            SEND_YN          = '{dr["SEND_YN"]}'
                        WHERE  DOC_NO           = '{dr["DOC_NO"]}'
                        AND    DOC_ITEM         = '{dr["DOC_ITEM"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("작업자를 저장 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(gridView);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 주문정보를 삭제하시겠습니까?"))
            {
                return;
            }

            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $"DELETE FROM TMS_INPUT_PLOADM_CON WHERE XSEQNR = '{gridView.GetFocusedRowCellValue("XSEQNR")}' AND INTERFACEID = '{gridView.GetFocusedRowCellValue("INTERFACEID")}' AND DISPATCHNO = '{gridView.GetFocusedRowCellValue("DISPATCHNO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("삭제를 실행하는 도중 에러가 발생했습니다");
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

        private void cboTransFlag_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}