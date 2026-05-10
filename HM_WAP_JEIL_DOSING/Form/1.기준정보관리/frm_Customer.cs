using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Customer : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = string.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_Customer()  
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Customer_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);
            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            clsDevexpressUtil.ItemSearchLookUpEditSetup(scboCustomer, clsCommon.GetCustomer(), "거래처를 선택 해주세요.", false);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 거래처 마스터
                SELECT 
                PARTNER, NAME_ORG1, RLTYP, 
                    TEL_NUMBER_1, MOD_NUMBER, J_1KFREPRE, 
                    ZZREPBPNM,
                    CASE WHEN CRDAT = '00000000' THEN NULL ELSE TO_CHAR(TO_DATE(CRDAT, 'YYYYMMDD'), 'YYYY-MM-DD') END CRDAT,
                    CASE WHEN CRTIM = '000000' THEN NULL ELSE TO_CHAR(TO_DATE(CRTIM, 'HH24MISS'), 'HH24:MI:SS') END CRTIM,
                    CASE WHEN CHDAT = '00000000' THEN NULL ELSE TO_CHAR(TO_DATE(CHDAT, 'YYYYMMDD'), 'YYYY-MM-DD') END CHDAT,
                    CASE WHEN CHTIM = '000000' THEN NULL ELSE TO_CHAR(TO_DATE(CHTIM, 'HH24MISS'), 'HH24:MI:SS') END CHTIM,
                    XDELE, I_TIME
                FROM SAP_DI_CUSTOMER
                WHERE ('{scboCustomer.EditValue}' IS NULL OR PARTNER = '{scboCustomer.EditValue}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                gridView.Columns["PARTNER"].Fixed = FixedStyle.Left;
                gridView.Columns["NAME_ORG1"].Fixed = FixedStyle.Left;

                sValid = new string[] { "PARTNER", "NAME_ORG1" };

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

            }catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        
        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            XMain_Save();   
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);

                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("저장하시겠습니까?"))
                {
                    return;
                }

                DataTable DT = (DataTable)gridControl.DataSource;

                if (DT == null)
                {
                    return;
                }

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                        if (!string.IsNullOrWhiteSpace(rValid))
                        {
                            gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                            gridView.ShowEditor(); // 편집 모드 진입 (선택)
                            Dbconn.conn.Rollback();
                            return;
                        }

                        SQL = $@"
                        INSERT INTO SAP_DI_CUSTOMER (
                             PARTNER           -- 01. 거래처 코드
                            , NAME_ORG1        -- 02. 거래처명
                            , RLTYP            -- 03. 관계유형
                            , TEL_NUMBER_1     -- 04. 전화번호
                            , MOD_NUMBER       -- 05. 변경번호
                            , J_1KFREPRE       -- 06. 담당자 코드
                            , ZZREPBPNM        -- 07. 담당자명
                            , CRDAT            -- 08. 생성일자
                            , CRTIM            -- 09. 생성시간
                            , CHDAT            -- 10. 수정일자
                            , CHTIM            -- 11. 수정시간
                            , XDELE            -- 12. 삭제여부
                            , I_TIME           -- 13. 입력시간
                        )
                        VALUES (
                             '{dr["PARTNER"]}'        -- 01
                            , '{dr["NAME_ORG1"]}'     -- 02
                            , '{dr["RLTYP"]}'         -- 03
                            , '{dr["TEL_NUMBER_1"]}'  -- 04
                            , '{dr["MOD_NUMBER"]}'    -- 05
                            , '{dr["J_1KFREPRE"]}'    -- 06
                            , '{dr["ZZREPBPNM"]}'     -- 07
                            , TO_CHAR(TO_DATE('{dr["CRDAT"]}', 'YYYY-MM-DD'), 'YYYYMMDD')         -- 08
                            , TO_CHAR(TO_DATE('{dr["CRTIM"]}', 'HH24:MI:SS'), 'HH24MISS')         -- 09
                            , TO_CHAR(TO_DATE('{dr["CHDAT"]}', 'YYYY-MM-DD'), 'YYYYMMDD')         -- 10
                            , TO_CHAR(TO_DATE('{dr["CHTIM"]}', 'HH24:MI:SS'), 'HH24MISS')         -- 11
                            , '{dr["XDELE"]}'         -- 12
                            , SYSDATE                 -- 13
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 입력에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                        if (!string.IsNullOrWhiteSpace(rValid))
                        {
                            gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                            gridView.ShowEditor(); // 편집 모드 진입 (선택)
                            Dbconn.conn.Rollback();
                            return;
                        }

                        SQL = $@"
                        UPDATE SAP_DI_CUSTOMER
                        SET
                             NAME_ORG1    = '{dr["NAME_ORG1"]}'     -- 01
                            , RLTYP        = '{dr["RLTYP"]}'         -- 02
                            , TEL_NUMBER_1 = '{dr["TEL_NUMBER_1"]}'  -- 03
                            , MOD_NUMBER   = '{dr["MOD_NUMBER"]}'    -- 04
                            , J_1KFREPRE   = '{dr["J_1KFREPRE"]}'    -- 05
                            , ZZREPBPNM    = '{dr["ZZREPBPNM"]}'     -- 06
                            , CRDAT        = TO_CHAR(TO_DATE('{dr["CRDAT"]}', 'YYYY-MM-DD'), 'YYYYMMDD')         -- 07
                            , CRTIM        = TO_CHAR(TO_DATE('{dr["CRTIM"]}', 'HH24:MI:SS'), 'HH24MISS')         -- 08
                            , CHDAT        = TO_CHAR(TO_DATE('{dr["CHDAT"]}', 'YYYY-MM-DD'), 'YYYYMMDD')         -- 09
                            , CHTIM        = TO_CHAR(TO_DATE('{dr["CHTIM"]}', 'HH24:MI:SS'), 'HH24MISS')         -- 10
                            , XDELE        = '{dr["XDELE"]}'         -- 11
                            , I_TIME       = SYSDATE                 -- 12
                        WHERE
                            PARTNER        = '{dr["PARTNER"]}'       -- 13
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                }

                ShowMessageBox.XtraShowInformation("거래처 정보를 저장 했습니다.");

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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 거래처 " + gridView.GetFocusedRowCellValue("NAME_ORG1") + " 를 삭제하시겠습니까?"))
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

                SQL = $"DELETE FROM SAP_DI_CUSTOMER WHERE PARTNER = '{gridView.GetFocusedRowCellValue("PARTNER")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("거래처를 삭제 했습니다.");

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

        private void scboCustomer_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}