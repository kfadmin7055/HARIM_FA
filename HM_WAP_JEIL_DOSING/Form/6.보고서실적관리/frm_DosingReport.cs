using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
using DevExpress.XtraLayout;
using DevExpress.XtraSpreadsheet.Forms;
using Core.Class;
using System.Web.UI.WebControls;

namespace HARIM_FA_DOSING
{
    public partial class frm_DosingReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = string.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_DosingReport()
        {
            InitializeComponent();
        }

        private void frm_DosingReport2_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_DosingReport2_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void resetControls()
        {
            foreach (var item in layoutControl.Items)
            {
                if (item is LayoutControlItem && (item as LayoutControlItem).Control is TextEdit)
                {
                    if (typeof(DevExpress.XtraEditors.TextEdit) == (item as LayoutControlItem).Control.GetType() ||
                        typeof(DevExpress.XtraEditors.MemoEdit) == (item as LayoutControlItem).Control.GetType()
                        )
                    {
                        (item as LayoutControlItem).Control.Text = string.Empty;
                    }
                }
            }
        }

        private void XMain_Search(string WORK_START_DATE)
        {
            try
            {
                resetControls();

                SQL = $@"
                SELECT 
                    PLANT_CODE, PROCESS_KEY, L_CODE , 
                    WORK_START_DATE, 
                    DAY_WORKMAN, 
                    NIGHT_WORKMAN, 
                    DOS_START_TIME, 
                    DOS_END_TIME, 
                    DOS_PRO_Q, 
                    DOS_INCOM_Q,  
                    DOS_PRO_BATCH, 
                    DOS_LOT_QTY, 
                    CHK_WORK_BEFOR_CHK1,
                    CHK_WORK_BEFOR_CHK1_REMARK,
                    CHK_WORK_BEFOR_CHK2,
                    CHK_WORK_BEFOR_CHK2_REMARK,
                    CHK_WORK_BEFOR_CHK3,
                    CHK_WORK_BEFOR_CHK3_REMARK,
                    CHK_WORK_BEFOR_CHK4,
                    CHK_WORK_BEFOR_CHK4_REMARK,
                    CHK_WORK_BEFOR_CHK5,
                    CHK_WORK_BEFOR_CHK5_REMARK,
                    CHK_WORK_RUN_CHK1, 
                    CHK_WORK_RUN_CHK1_REMARK,  
                    CHK_WORK_RUN_CHK2, 
                    CHK_WORK_RUN_CHK2_REMARK, 
                    CHK_WORK_RUN_CHK3, 
                    CHK_WORK_RUN_CHK3_REMARK, 
                    CHK_WORK_END_CHK1, 
                    CHK_WORK_END_CHK1_REMARK,  
                    CHK_WORK_END_CHK2, 
                    CHK_WORK_END_CHK2_REMARK, 
                    CHK_WORK_END_CHK3, 
                    CHK_WORK_END_CHK3_REMARK, 
                    CHK_USE_TANK_1, 
                    CHK_USE_TANK_2,  
                    CHK_TANK1_TEMP1, 
                    CHK_TANK1_TEMP2, 
                    CHK_TANK1_TEMP3, 
                    CHK_TANK2_TEMP1, 
                    CHK_TANK2_TEMP2, 
                    CHK_TANK2_TEMP3, 
                    CHK_TANK3_TEMP1,  
                    CHK_TANK3_TEMP2, 
                    CHK_TANK3_TEMP3, 
                    DOS_LOAD_EQ1_NO_VALUE, 
                    DOS_LOAD_EQ1_FULL_VALUE, 
                    DOS_LOAD_EQ1_OVER_VALUE, 
                    DOS_LOAD_EQ2_NO_VALUE,  
                    DOS_LOAD_EQ2_FULL_VALUE, 
                    DOS_LOAD_EQ2_OVER_VALUE, 
                    DOS_LOAD_EQ3_NO_VALUE, 
                    DOS_LOAD_EQ3_FULL_VALUE, 
                    DOS_LOAD_EQ3_OVER_VALUE,
                    DOS_EXTEND_REMARK, 
                    DOS_MANUAL_INPUT_REMARK, 
                    DOS_WKMONTH_REMARK, 
                    DOS_REMARK, 
                    I_TIME, 
                    I_USER  
                FROM 
                    DOS_REPORT 
                WHERE 
                    PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_START_DATE = TO_DATE('{WORK_START_DATE}', 'YYYYMMDD')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) == 1)
                {

                    txt_DAY_WORKMAN.Text = Dbconn.conn.getData(ds, "DAY_WORKMAN", 0);
                    txt_NIGHT_WORKMAN.Text = Dbconn.conn.getData(ds, "NIGHT_WORKMAN", 0);
                    txt_DOS_START_TIME.EditValue = Convert.ToDateTime(Dbconn.conn.getData(ds, "DOS_START_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");
                    txt_DOS_END_TIME.EditValue = Convert.ToDateTime(Dbconn.conn.getData(ds, "DOS_END_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");
                    txt_DOS_PRO_Q.Text = Dbconn.conn.getData(ds, "DOS_PRO_Q", 0);
                    txt_DOS_INCOM_Q.Text = Dbconn.conn.getData(ds, "DOS_INCOM_Q", 0);
                    txt_DOS_PRO_BATCH.Text = Dbconn.conn.getData(ds, "DOS_PRO_BATCH", 0);
                    txt_DOS_LOT_QTY.Text = Dbconn.conn.getData(ds, "DOS_LOT_QTY", 0);

                    chK_CHK1_1.Checked = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK1", 0).ToString() == "Y" ? true : false;
                    chK_CHK1_1_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK1_REMARK", 0);
                    chK_CHK1_2.Checked = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK2", 0).ToString() == "Y" ? true : false;
                    chK_CHK1_2_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK2_REMARK", 0);
                    chK_CHK1_3.Checked = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK3", 0).ToString() == "Y" ? true : false;
                    chK_CHK1_3_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK3_REMARK", 0);
                    chK_CHK1_4.Checked = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK4", 0).ToString() == "Y" ? true : false;
                    chK_CHK1_4_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK4_REMARK", 0); ;
                    chK_CHK1_5.Checked = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK5", 0).ToString() == "Y" ? true : false;
                    chK_CHK1_5_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_BEFOR_CHK5_REMARK", 0);

                    chK_CHK2_1.Checked = Dbconn.conn.getData(ds, "CHK_WORK_RUN_CHK1", 0).ToString() == "Y" ? true : false;
                    chK_CHK2_1_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_RUN_CHK1_REMARK", 0);
                    chK_CHK2_2.Checked = Dbconn.conn.getData(ds, "CHK_WORK_RUN_CHK2", 0).ToString() == "Y" ? true : false;
                    chK_CHK2_2_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_RUN_CHK2_REMARK", 0);
                    chK_CHK2_3.Checked = Dbconn.conn.getData(ds, "CHK_WORK_RUN_CHK3", 0).ToString() == "Y" ? true : false;
                    chK_CHK2_3_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_RUN_CHK3_REMARK", 0);

                    chK_CHK3_1.Checked = Dbconn.conn.getData(ds, "CHK_WORK_END_CHK1", 0).ToString() == "Y" ? true : false;
                    chK_CHK3_1_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_END_CHK1_REMARK", 0);
                    chK_CHK3_2.Checked = Dbconn.conn.getData(ds, "CHK_WORK_END_CHK2", 0).ToString() == "Y" ? true : false;
                    chK_CHK3_2_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_END_CHK2_REMARK", 0);
                    chK_CHK3_3.Checked = Dbconn.conn.getData(ds, "CHK_WORK_END_CHK3", 0).ToString() == "Y" ? true : false;
                    chK_CHK3_3_REMARK.Text = Dbconn.conn.getData(ds, "CHK_WORK_END_CHK3_REMARK", 0);

                    txt_USE_TANK1.Text = Dbconn.conn.getData(ds, "CHK_USE_TANK_1", 0);
                    txt_USE_TANK2.Text = Dbconn.conn.getData(ds, "CHK_USE_TANK_2", 0);

                    txt_TANK1_TEMP1.Text = Dbconn.conn.getData(ds, "CHK_TANK1_TEMP1", 0);
                    txt_TANK1_TEMP2.Text = Dbconn.conn.getData(ds, "CHK_TANK1_TEMP2", 0);
                    txt_TANK1_TEMP3.Text = Dbconn.conn.getData(ds, "CHK_TANK1_TEMP3", 0);

                    txt_TANK2_TEMP1.Text = Dbconn.conn.getData(ds, "CHK_TANK2_TEMP1", 0);
                    txt_TANK2_TEMP2.Text = Dbconn.conn.getData(ds, "CHK_TANK2_TEMP2", 0);
                    txt_TANK2_TEMP3.Text = Dbconn.conn.getData(ds, "CHK_TANK2_TEMP3", 0);

                    txt_TANK3_TEMP1.Text = Dbconn.conn.getData(ds, "CHK_TANK3_TEMP1", 0);
                    txt_TANK3_TEMP2.Text = Dbconn.conn.getData(ds, "CHK_TANK3_TEMP2", 0);
                    txt_TANK3_TEMP3.Text = Dbconn.conn.getData(ds, "CHK_TANK3_TEMP3", 0);

                    txt_EQ1_1.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ1_NO_VALUE", 0);
                    txt_EQ1_2.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ1_FULL_VALUE", 0);
                    txt_EQ1_3.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ1_OVER_VALUE", 0);
                    txt_EQ2_1.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ2_NO_VALUE", 0);
                    txt_EQ2_2.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ2_FULL_VALUE", 0);
                    txt_EQ2_3.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ2_OVER_VALUE", 0);
                    txt_EQ3_1.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ3_NO_VALUE", 0);
                    txt_EQ3_2.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ3_FULL_VALUE", 0);
                    txt_EQ3_3.Text = Dbconn.conn.getData(ds, "DOS_LOAD_EQ3_OVER_VALUE", 0);

                    memoEdit_EXTEND.Text = Dbconn.conn.getData(ds, "DOS_EXTEND_REMARK", 0);
                    memoEdit_MANUAL.Text = Dbconn.conn.getData(ds, "DOS_MANUAL_INPUT_REMARK", 0);
                    memoEdit_WKMONTH.Text = Dbconn.conn.getData(ds, "DOS_WKMONTH_REMARK", 0);
                    memoEdit_REMARK.Text = Dbconn.conn.getData(ds, "DOS_REMARK", 0);
                }else
                {
                    txt_DOS_START_TIME.EditValue = DateTime.Today;
                    txt_DOS_END_TIME.EditValue = DateTime.Today.AddDays(1);
                }

                ds.Dispose();


            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
        }

        private void layoutControl_CustomDraw(object sender, DevExpress.XtraLayout.ItemCustomDrawEventArgs e)
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

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string sPlantCode = cboPlant_Code.EditValue?.ToString();
                string sProcessKey = cboProcessKey.EditValue?.ToString();
                string sLCode = cboL_Code.EditValue?.ToString();

                string sWorkDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);


                clsPrintReport.PrintDosingReport(sPlantCode, sProcessKey, sLCode, sWorkDate);
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
                if (DialogResult.Yes != ShowMessageBox.Confirm("배합작업일지 정보를 저장하시겠습니까?"))
                {
                    return;
                }

                SQL = "DELETE FROM DOS_REPORT WHERE WORK_START_DATE = '{0}' ";
                SQL = string.Format(SQL, string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
                Dbconn.conn.SQLrun(SQL);


                SQL = $@"
                INSERT INTO HARIMOERP.DOS_REPORT (
                   PLANT_CODE, PROCESS_KEY, L_CODE ,                                                             -- 1
                   WORK_START_DATE, DAY_WORKMAN, NIGHT_WORKMAN,                                                 -- 2
                   DOS_START_TIME, DOS_END_TIME, DOS_PRO_Q,                                                     -- 3
                   DOS_INCOM_Q, DOS_PRO_BATCH, DOS_LOT_QTY,                                                     -- 4
                   CHK_WORK_BEFOR_CHK1, CHK_WORK_BEFOR_CHK1_REMARK, CHK_WORK_BEFOR_CHK2,                        -- 5
                   CHK_WORK_BEFOR_CHK2_REMARK, CHK_WORK_BEFOR_CHK3, CHK_WORK_BEFOR_CHK3_REMARK,                 -- 6
                   CHK_WORK_BEFOR_CHK4, CHK_WORK_BEFOR_CHK4_REMARK, CHK_WORK_BEFOR_CHK5,                        -- 7
                   CHK_WORK_BEFOR_CHK5_REMARK, CHK_WORK_RUN_CHK1, CHK_WORK_RUN_CHK1_REMARK,                     -- 8
                   CHK_WORK_RUN_CHK2, CHK_WORK_RUN_CHK2_REMARK, CHK_WORK_RUN_CHK3,                              -- 9
                   CHK_WORK_RUN_CHK3_REMARK, CHK_WORK_END_CHK1, CHK_WORK_END_CHK1_REMARK,                       -- 10
                   CHK_WORK_END_CHK2, CHK_WORK_END_CHK2_REMARK, CHK_WORK_END_CHK3,                              -- 11
                   CHK_WORK_END_CHK3_REMARK, CHK_USE_TANK_1, CHK_USE_TANK_2,                                    -- 12
                   CHK_TANK1_TEMP1, CHK_TANK1_TEMP2, CHK_TANK1_TEMP3,                                           -- 13
                   CHK_TANK2_TEMP1, CHK_TANK2_TEMP2, CHK_TANK2_TEMP3,                                           -- 14
                   CHK_TANK3_TEMP1, CHK_TANK3_TEMP2, CHK_TANK3_TEMP3,                                           -- 15
                   DOS_LOAD_EQ1_NO_VALUE, DOS_LOAD_EQ1_FULL_VALUE, DOS_LOAD_EQ1_OVER_VALUE,                     -- 16
                   DOS_LOAD_EQ2_NO_VALUE, DOS_LOAD_EQ2_FULL_VALUE, DOS_LOAD_EQ2_OVER_VALUE,                     -- 17
                   DOS_LOAD_EQ3_NO_VALUE, DOS_LOAD_EQ3_FULL_VALUE, DOS_LOAD_EQ3_OVER_VALUE,                     -- 18
                   DOS_REMARK, DOS_EXTEND_REMARK, DOS_MANUAL_INPUT_REMARK,                                      -- 19
                   DOS_WKMONTH_REMARK, I_TIME, I_USER) 
                VALUES ( 
                    '{cboPlant_Code.EditValue}', '{clsCommon.GetProcessKey("배합")}', '{cboL_Code.EditValue}',
                    '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', '{txt_DAY_WORKMAN.Text}', '{txt_NIGHT_WORKMAN.Text}',
                    TO_DATE('{Convert.ToDateTime(txt_DOS_START_TIME.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), TO_DATE('{Convert.ToDateTime(txt_DOS_END_TIME.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), '{txt_DOS_PRO_Q.Text}',
                    '{txt_DOS_INCOM_Q.Text}', '{txt_DOS_PRO_BATCH.Text}', '{txt_DOS_LOT_QTY.Text}',            -- 4
                     '{chK_CHK1_1.EditValue}', '{chK_CHK1_1_REMARK.Text}', '{chK_CHK1_2.EditValue}',           -- 5
                    '{chK_CHK1_2_REMARK.Text}', '{chK_CHK1_3.EditValue}', '{chK_CHK1_3_REMARK.Text}',          -- 6
                    '{chK_CHK1_4.EditValue}', '{chK_CHK1_4_REMARK.Text}', '{chK_CHK1_5.EditValue}',            -- 7
                    '{chK_CHK1_5_REMARK.Text}', '{chK_CHK2_1.EditValue}', '{chK_CHK2_1_REMARK.Text}',          -- 8
                    '{chK_CHK2_2.EditValue}', '{chK_CHK2_2_REMARK.Text}', '{chK_CHK2_3.EditValue}',            -- 9
                    '{chK_CHK2_3_REMARK.Text}', '{chK_CHK3_1.EditValue}', '{chK_CHK3_1_REMARK.Text}',          -- 10
                    '{chK_CHK3_2.EditValue}', '{chK_CHK3_2_REMARK.Text}', '{chK_CHK3_3.EditValue}',            -- 11
                    '{chK_CHK3_3_REMARK.Text}', '{txt_USE_TANK1.Text}', '{txt_USE_TANK2.Text}',                -- 12
                    '{txt_TANK1_TEMP1.Text}', '{txt_TANK1_TEMP2.Text}', '{txt_TANK1_TEMP3.Text}',              -- 13
                    '{txt_TANK2_TEMP1.Text}', '{txt_TANK2_TEMP2.Text}', '{txt_TANK2_TEMP3.Text}',              -- 14
                    '{txt_TANK3_TEMP1.Text}', '{txt_TANK3_TEMP2.Text}', '{txt_TANK3_TEMP3.Text}',              -- 15
                    '{txt_EQ1_1.Text}', '{txt_EQ1_2.Text}', '{txt_EQ1_3.Text}',                                -- 16
                    '{txt_EQ2_1.Text}', '{txt_EQ2_2.Text}', '{txt_EQ2_3.Text}',                                -- 17
                    '{txt_EQ3_1.Text}', '{txt_EQ3_2.Text}', '{txt_EQ3_3.Text}',                                -- 18
                    '{memoEdit_REMARK.Text}', '{memoEdit_EXTEND.Text}', '{memoEdit_MANUAL.Text}',              -- 19
                    '{memoEdit_WKMONTH.Text}', SYSDATE, '{clsCommon._strUserId}')                              -- 20
                ";

                Dbconn.conn.SQLrun(SQL);

                XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));

                ShowMessageBox.XtraShowInformation("저장되었습니다");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
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

        /// <summary>
        /// 공정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboProcess_EditValueChanged(object sender, EventArgs e)
        {
            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);

            XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
        }

        /// <summary>
        /// 플랜트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("배합")), "", false, 0, false);

            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);

            XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
        }
    }
}