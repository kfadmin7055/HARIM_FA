using Core.Class;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Sql;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_BinSeqDupChack : DevExpress.XtraEditors.XtraForm
    {
        string SQL = String.Empty;
        string vPlantCode = string.Empty;
        string vProcessKey = string.Empty;
        string vLCode = string.Empty;
        DataSet authDs;

        public m_BinSeqDupChack(string sPlantCode, string sProcessKey, string sLCode)
        {
            vPlantCode = sPlantCode;
            vProcessKey = sProcessKey;
            vLCode = sLCode;
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Default;
            gridView.OptionsBehavior.Editable = true;
            gridView.OptionsCustomization.AllowSort = false;
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                    a.PLANT_CODE,
                    a.PROCESS_KEY,
                    a.L_CODE,
                    a.SCALE_CODE, 
                    b.RESOURCE_NO, 
                    b.DESCRIPTION,
                    a.LOCATION, 
                    a.SEQ
                FROM 
                    BIN a
                    JOIN SAP_DI_PRODUCT b  ON b.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = b.RESOURCE_NO
                WHERE 
                    a.RESOURCE_NO IN (
                        SELECT A.RESOURCE_NO
                        FROM BIN A
                            JOIN SCALE B ON B.PLANT_CODE = A.PLANT_CODE AND B.PROCESS_KEY = A.PROCESS_KEY AND B.L_CODE = A.L_CODE AND B.SCALE_CODE = A.SCALE_CODE
                        WHERE A.SCALE_CODE = B.SCALE_CODE AND A.PLANT_CODE = '{vPlantCode}' AND A.PROCESS_KEY = '{vProcessKey}' AND A.L_CODE = '{vLCode}'
                        GROUP BY A.RESOURCE_NO
                        HAVING COUNT(A.RESOURCE_NO) >= 2
                    )
                    AND a.PLANT_CODE = '{vPlantCode}' AND a.PROCESS_KEY = '{vProcessKey}' AND a.L_CODE = '{vLCode}'
                ORDER BY 
                    b.RESOURCE_NO, 
                    a.SCALE_CODE, 
                    a.LOCATION, 
                    a.SEQ
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
            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);

                splashScreenManager.ShowWaitForm();
                Dbconn.conn.BeginTransaction();

                for (int i = 0; i < gridView.RowCount; i++)
                {
                    SQL = $@"
                    UPDATE BIN
                    SET SEQ = '{gridView.GetRowCellValue(i, "SEQ")}'
                    WHERE PLANT_CODE = '{vPlantCode}' AND PROCESS_KEY = '{vProcessKey}' AND L_CODE = '{vLCode}'
                        AND LOCATION = '{gridView.GetRowCellValue(i, "LOCATION")}'
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

                SQL = $@"
                UPDATE BIN B
                   SET B.SEQ = 2
                 WHERE EXISTS
                       (
                           SELECT LOCATION,
                            RESOURCE_NO,
                            SEQ
                             FROM (
                                     SELECT LOCATION,
                                            RESOURCE_NO,
                                            SEQ,
                                            ROW_NUMBER() OVER (PARTITION BY RESOURCE_NO ORDER BY LOCATION) AS RN
                                       FROM BIN
                                      WHERE SCALE_CODE IS NOT NULL
                                        AND PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                                        AND PROCESS_KEY = '{gridView.GetFocusedRowCellValue("PROCESS_KEY")}'
                                        AND L_CODE = '{gridView.GetFocusedRowCellValue("L_CODE")}'
                                        AND SEQ = 1          -- SEQ=1 인 행만 체크
                                   )
                            WHERE RESOURCE_NO = B.RESOURCE_NO
                              AND LOCATION = B.LOCATION
                              AND RN > 1               -- 1번째만 남기고 나머지는 0으로 변경
                        )
                ";

                Dbconn.conn.SQLrun(SQL);

                XMain_Search();
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
                //DataTable dt = gridControl.

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
                    }
                }
            }
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            string condition = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["SEQ"]).ToString();

            if (condition == "1") //완료
            {
                e.Appearance.BackColor = Color.LightCyan;
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }
    }
}