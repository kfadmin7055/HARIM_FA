using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Car : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_Car()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        /// <summary>
        /// 조회 쿼리
        /// </summary>
        private void XMain_Search()
        {
            try
            {
                DataSet ds = clsCarCommon.GetCarMaster(txtCarNum.Text, txtName.Text);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                //gridView.SetFixCol(new string[] {  "CHK"
                //    , "PLANT_CODE"
                //    , "PROCESS_KEY"
                //    , "L_CODE"
                //    , "LOCATION"
                //    , "RESOURCE_NO"
                //    , "BIN_NAME" });

                sValid = new string[] { "VEHICLECODE" };

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

                gridView.Columns["TRANSACTIONFLAG"].OptionsColumn.AllowEdit = false;

                // 서명, 봉인 필요
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "N", false);

                // 사용안함
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboUse, clsCommon.GetYn(null, new string[] {"사용", "사용안함"}), "N", false);

                // 트랜잭션 플라그
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTransFlag, clsCommon.GetTransFlag(), "N", false);

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

        private void frm_Car_Load(object sender, EventArgs e)
        {
            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
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
            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(gridView);

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
                        INSERT INTO TMS_INPUT_CARMASTER_CON (
                           VEHICLECODE
                         , TMSDIVISIONCODE
                         , TMSLOGISTICGROUP
                         , VEHICLENO
                         , VEHICLETONCODE
                         , VEHICLETONNAME
                         , VEHICLEGROUPCODE
                         , VEHICLEGROUPNAME
                         , DRIVERNAME
                         , DRIVERMOBILE
                         , ISREQUIRESIGNATURE
                         , ISREQUIRESEAL
                         , LIVESTOCKVEHICLENO
                         , CARRIERCODE
                         , CARRIERNAME
                         , TRANSACTIONFLAG
                         , USE_YN
                         , REGISTERAT
                         , REGISTERBY
                        )
                        VALUES (
                           '{dr["VEHICLECODE"]}'
                         , '{dr["TMSDIVISIONCODE"]}'
                         , '{dr["TMSLOGISTICGROUP"]}'
                         , '{dr["VEHICLENO"]}'
                         , '{dr["VEHICLETONCODE"]}'
                         , '{dr["VEHICLETONNAME"]}'
                         , '{dr["VEHICLEGROUPCODE"]}'
                         , '{dr["VEHICLEGROUPNAME"]}'
                         , '{dr["DRIVERNAME"]}'
                         , '{dr["DRIVERMOBILE"]}'
                         , '{dr["ISREQUIRESIGNATURE"]}'
                         , '{dr["ISREQUIRESEAL"]}'
                         , '{dr["LIVESTOCKVEHICLENO"]}'
                         , '{dr["CARRIERCODE"]}'
                         , '{dr["CARRIERNAME"]}'
                         , '{dr["TRANSACTIONFLAG"]}'
                         , '{dr["USE_YN"]}'
                         , '{dr["REGISTERAT"]}'
                         , '{dr["REGISTERBY"]}'
                        )
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
                        UPDATE TMS_INPUT_CARMASTER_CON
                        SET TMSDIVISIONCODE    = '{dr["TMSDIVISIONCODE"]}'
                            , TMSLOGISTICGROUP   = '{dr["TMSLOGISTICGROUP"]}'
                            , VEHICLENO          = '{dr["VEHICLENO"]}'
                            , VEHICLETONCODE     = '{dr["VEHICLETONCODE"]}'
                            , VEHICLETONNAME     = '{dr["VEHICLETONNAME"]}'
                            , VEHICLEGROUPCODE   = '{dr["VEHICLEGROUPCODE"]}'
                            , VEHICLEGROUPNAME   = '{dr["VEHICLEGROUPNAME"]}'
                            , DRIVERNAME         = '{dr["DRIVERNAME"]}'
                            , DRIVERMOBILE       = '{dr["DRIVERMOBILE"]}'
                            , ISREQUIRESIGNATURE = '{dr["ISREQUIRESIGNATURE"]}'
                            , ISREQUIRESEAL      = '{dr["ISREQUIRESEAL"]}'
                            , LIVESTOCKVEHICLENO = '{dr["LIVESTOCKVEHICLENO"]}'
                            , CARRIERCODE        = '{dr["CARRIERCODE"]}'
                            , CARRIERNAME        = '{dr["CARRIERNAME"]}'
                            , TRANSACTIONFLAG    = '{dr["TRANSACTIONFLAG"]}'
                            , USE_YN             = '{dr["USE_YN"]}'
                            , REGISTERAT         = '{dr["REGISTERAT"]}'
                            , REGISTERBY         = '{dr["REGISTERBY"]}'
                        WHERE  VEHICLECODE        = '{dr["VEHICLECODE"]}'
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

                ShowMessageBox.XtraShowWarning("차량을 저장 했습니다");

                XMain_Search();
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 차량 " + gridView.GetFocusedRowCellValue("VEHICLENO") + " 번을 삭제하시겠습니까?"))
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

                SQL = $"DELETE FROM TMS_INPUT_CARMASTER_CON WHERE VEHICLECODE = '{gridView.GetFocusedRowCellValue("VEHICLECODE")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("차량 정보를 삭제 했습니다.");

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
    }
}