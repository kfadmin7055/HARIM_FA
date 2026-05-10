using Core.Class;
using Core.Enum;
using DevExpress.XtraEditors;
using System;
using System.Data;

namespace HARIM_FA_DOSING
{
    public partial class m_WapInAdd : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        string sIS_NO = string.Empty;

        public m_WapInAdd(string argPcCdoe)
        {
            sIS_NO = argPcCdoe;
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Default;
            gridView.OptionsBehavior.Editable = true;
        }

        private void XMain_Search(string IS_NO)
        {
            try
            {
                SQL = $@"
                SELECT IS_NO, PTMCD, WEIGHT, 
                   PD_QTY, I_TIME
                FROM WAP_IN_ADD
                WHERE IS_NO = '{IS_NO}'
                ";

                DataSet XMain_SearchDs = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, XMain_SearchDs.Tables[0], false);

                XMain_SearchDs.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void m_BinSeqDupChack_Load(object sender, EventArgs e)
        {
            XMain_Search(sIS_NO);
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search(sIS_NO);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = string.Empty;

                clsDevexpressGrid.GridEndEdit(gridView);

                splashScreenManager.ShowWaitForm();
                Dbconn.conn.BeginTransaction();
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    SQL = $@"
                    UPDATE BIN
                    SET SEQ = '{gridView.GetRowCellValue(i, "SEQ")}'
                    WHERE LOCATION = '{gridView.GetRowCellValue(i, "LOCATION")}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        Dbconn.conn.Rollback();
                        return;
                    }
                }
                Dbconn.conn.Commit();

                //원료 1개인 빈은 순번 1로 조정
                SQL = $@"
                UPDATE BIN
                   SET SEQ = 1
                WHERE RESOURCE_NO IN (
                    SELECT a.RESOURCE_NO
                        FROM (
                            SELECT RESOURCE_NO
                                FROM BIN
                            WHERE SCALE_CODE IS NOT NULL
                                AND PROCESS_KEY IN ('{clsCommon.PcStatus.Plan}', 'P05')
                                AND LOCATION <> '315'
                            GROUP BY RESOURCE_NO
                            HAVING COUNT(RESOURCE_NO) = 1
                                AND MAX(SEQ) > 1
                            ) a
                )";

                Dbconn.conn.SQLrun(SQL);

                XMain_Search(sIS_NO);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            ShowMessageBox.XtraShowInformation("빈우선순위가 변경되었습니다");
        }

        private void reptemCheckEdit_SEQ_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit edit = sender as CheckEdit;
            if (edit.Checked)
            {
                string sc_cd = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "SCALE_CODE");
                string res_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO");
                string loc = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION");

                for (int i = 0; i < gridView.RowCount; i++)
                {
                    string f_sc_cd = gridView.GetRowCellValue(i, "SCALE_CODE").ToString();
                    string f_res_no = gridView.GetRowCellValue(i, "RESOURCE_NO").ToString();

                    if (f_res_no.Equals(res_no) && !loc.Equals(gridView.GetRowCellValue(i, "LOCATION"))
                        )
                    {
                        gridView.SetRowCellValue(i, "SEQ", "2");
                        break;
                    }
                }
            }
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(gridView);
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }
    }
}