using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
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
using DevExpress.XtraSplashScreen;
using Core.Class;

namespace HM_WAP_JEIL_DOSING
{
    public partial class frm_MilkReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        public frm_MilkReport()
        {
            InitializeComponent();
        }

        private string workNumMake(string WORK_START_DATE)
        {
            try
            {
                string SQL =
                "SELECT ISNULL(MAX(WORK_SEQ) + 1, 1) AS SEQ  " +
                "FROM MILK_REPORT WHERE WORK_NUMBER = '{0}' ";
                SQL = string.Format(SQL, WORK_START_DATE);

                using (DataSet Ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(Ds) == 0)
                    {
                        clsLog.logSave("작업순번 생성에러 / SQL : " + SQL, 0);
                        return string.Empty;
                    }

                    string return_seq = Dbconn.conn.getData(Ds, "SEQ", 0);
                    Ds.Dispose();

                    return return_seq;
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "workNumMake", ex);
                return string.Empty;
            }
        }

        private void XMain_Search()
        {
            try
            {
                resetControls();

                SQL =
                    "SELECT WORK_NUMBER, WORK_SEQ, SEQ, RESOURCE_NO, [DESCRIPTION], WEIGHT_ST, WEIGHT_ET, INPUT_ST, INPUT_ET,  " +
                    "DOS_ST, DOS_ET, PACK_ST, PACK_ET, CLEAN_ST, CELAN_ET, ETC_ST, ETC_ET, REMARK_ST, REMARK_ET,  " +
                    "DOS_Q, OR_QTY, PRO_QTY, F_Q, E_Q, PA_Q, DIFF, OEGWAN, DAN1, DAN2, DAN3, I_TIME, I_USER  " +
                    "FROM MILK_REPORT " +
                    "WHERE WORK_NUMBER = '{0}' " +
                    "ORDER BY WORK_NUMBER, WORK_SEQ ";

                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                    );
                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                for (int i = 0; i < Dbconn.conn.getRowCnt(ds); i++)
                {
                    string SEQ = Dbconn.conn.getData(ds, "SEQ", i);

                    findControlSet("txt_RESOURCE_NO" + SEQ, "tag", Dbconn.conn.getData(ds, "RESOURCE_NO", i));
                    findControlSet("txt_RESOURCE_NO" + SEQ, "text", Dbconn.conn.getData(ds, "DESCRIPTION", i));
                    findControlSet("txt_WEIGHT_ST" + SEQ, "text", Dbconn.conn.getData(ds, "WEIGHT_ST", i));
                    findControlSet("txt_WEIGHT_ET" + SEQ, "text", Dbconn.conn.getData(ds, "WEIGHT_ET", i));
                    findControlSet("txt_INPUT_ST" + SEQ, "text", Dbconn.conn.getData(ds, "INPUT_ST", i));
                    findControlSet("txt_INPUT_ET" + SEQ, "text", Dbconn.conn.getData(ds, "INPUT_ET", i));
                    findControlSet("txt_DOS_ST" + SEQ, "text", Dbconn.conn.getData(ds, "DOS_ST", i));
                    findControlSet("txt_DOS_ET" + SEQ, "text", Dbconn.conn.getData(ds, "DOS_ET", i));
                    findControlSet("txt_PACK_ST" + SEQ, "text", Dbconn.conn.getData(ds, "PACK_ST", i));
                    findControlSet("txt_PACK_ET" + SEQ, "text", Dbconn.conn.getData(ds, "PACK_ET", i));
                    findControlSet("txt_CLEAN_ST" + SEQ, "text", Dbconn.conn.getData(ds, "CLEAN_ST", i));
                    findControlSet("txt_CLEAN_ET" + SEQ, "text", Dbconn.conn.getData(ds, "CELAN_ET", i));
                    findControlSet("txt_ETC_ST" + SEQ, "text", Dbconn.conn.getData(ds, "ETC_ST", i));
                    findControlSet("txt_ETC_ET" + SEQ, "text", Dbconn.conn.getData(ds, "ETC_ET", i));
                    findControlSet("txt_REMARK_ST" + SEQ, "text", Dbconn.conn.getData(ds, "REMARK_ST", i));
                    findControlSet("txt_REMARK_ET" + SEQ, "text", Dbconn.conn.getData(ds, "REMARK_ET", i));

                    findControlSet("txt_DOS_Q" + SEQ, "text", Dbconn.conn.getData(ds, "DOS_Q", i));
                    findControlSet("txt_OR_QTY" + SEQ, "text", Dbconn.conn.getData(ds, "OR_QTY", i));
                    findControlSet("txt_PRO_QTY" + SEQ, "text", Dbconn.conn.getData(ds, "PRO_QTY", i));
                    findControlSet("txt_F_Q" + SEQ, "text", Dbconn.conn.getData(ds, "F_Q", i));
                    findControlSet("txt_E_Q" + SEQ, "text", Dbconn.conn.getData(ds, "E_Q", i));
                    findControlSet("txt_PA_Q" + SEQ, "text", Dbconn.conn.getData(ds, "PA_Q", i));
                    findControlSet("txt_DIFF" + SEQ, "text", Dbconn.conn.getData(ds, "DIFF", i));
                    findControlSet("txt_OEGWAN" + SEQ, "text", Dbconn.conn.getData(ds, "OEGWAN", i));
                    findControlSet("txt_DAN1_" + SEQ, "text", Dbconn.conn.getData(ds, "DAN1", i));
                    findControlSet("txt_DAN2_" + SEQ, "text", Dbconn.conn.getData(ds, "DAN2", i));
                    findControlSet("txt_DAN3_" + SEQ, "text", Dbconn.conn.getData(ds, "DAN3", i));

                }
                

                SQL =
                "SELECT CHK_LIST1, CHK_LIST2, CHK_LIST3, CHK_LIST4,  " +
                "CHK_LIST5, CHK_LIST6, CHK_LIST7, REMARK " +
                "FROM MILK_CHK_LIST " +
                "WHERE WORK_NUMBER = '{0}' ";

                SQL = string.Format(SQL,
                string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                );

                ds = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(ds) == 1)
                {
                    txt_CHK_MEMO1.Text = Dbconn.conn.getData(ds, "CHK_LIST1", 0);
                    txt_CHK_MEMO2.Text = Dbconn.conn.getData(ds, "CHK_LIST2", 0);
                    txt_CHK_MEMO3.Text = Dbconn.conn.getData(ds, "CHK_LIST3", 0);
                    txt_CHK_MEMO4.Text = Dbconn.conn.getData(ds, "CHK_LIST4", 0);
                    txt_CHK_MEMO5.Text = Dbconn.conn.getData(ds, "CHK_LIST5", 0);
                    txt_CHK_MEMO6.Text = Dbconn.conn.getData(ds, "CHK_LIST6", 0);
                    txt_CHK_MEMO7.Text = Dbconn.conn.getData(ds, "CHK_LIST7", 0);

                    memoEdit_REMARK.Text = Dbconn.conn.getData(ds, "REMARK", 0);
                }

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생하였습니다");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void layoutControl_CustomDraw(object sender, DevExpress.XtraLayout.ItemCustomDrawEventArgs e)
        {
            try
            {
                if (e.Item.Tag == "LINE")
                {
                    e.DefaultDraw();
                    Pen pen = new Pen(Brushes.DarkGray, 1);
                    e.Cache.DrawRectangle(pen, e.Bounds);
                    pen.Dispose();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "layoutControl_CustomDraw", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }
        private string findControl(string controlName, string type)
        {
            string rtnText = string.Empty;
            foreach (var item in layoutControl.Items)
            {
                if (item is LayoutControlItem && (item as LayoutControlItem).Control is TextEdit)
                {
                    if (typeof(DevExpress.XtraEditors.TextEdit) == (item as LayoutControlItem).Control.GetType() && (item as LayoutControlItem).Control.Name == controlName)
                    {
                        if (type == "text")
                        {
                            rtnText = (item as LayoutControlItem).Control.Text;
                            break;
                        }
                        else if (type == "tag")
                        {
                            if ((item as LayoutControlItem).Control.Tag == null)
                            {
                                break;
                            }

                            rtnText = (item as LayoutControlItem).Control.Tag.ToString();
                            break;
                        }
                        
                    }
                }
            }
            return rtnText;
        }

        private void findControlSet(string controlName, string type, string value)
        {
            string rtnText = string.Empty;
            foreach (var item in layoutControl.Items)
            {
                if (item is LayoutControlItem && (item as LayoutControlItem).Control is TextEdit)
                {
                    if (typeof(DevExpress.XtraEditors.TextEdit) == (item as LayoutControlItem).Control.GetType() && (item as LayoutControlItem).Control.Name == controlName)
                    {
                        if (type == "text")
                        {
                            (item as LayoutControlItem).Control.Text = value;
                            break;
                        }
                        else if (type == "tag")
                        {

                            (item as LayoutControlItem).Control.Tag = value;
                            break;
                        }

                    }
                }
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

        private void frm_MilkReport_Load(object sender, EventArgs e)
        {
            try
            {
                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;
                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_MilkReport_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != ShowMessageBox.Confirm("대용유 작업일지 정보를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {

                for (int i = 0; i < 5; i++)
                {
                    if (!string.IsNullOrEmpty(findControl("txt_RESOURCE_NO" + (i + 1), "tag")))
                    {
                        SQL = "DELETE FROM MILK_REPORT WHERE WORK_NUMBER = '{0}' AND SEQ = '{1}' ";
                        SQL = string.Format(SQL,
                            string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                            (i + 1).ToString()
                            );
                        Dbconn.conn.SQLrun(SQL);

                        SQL =
                        "INSERT INTO MILK_REPORT " +
                        "(WORK_NUMBER, WORK_SEQ, SEQ, RESOURCE_NO, DESCRIPTION, WEIGHT_ST, WEIGHT_ET, INPUT_ST, INPUT_ET, DOS_ST, DOS_ET,  " +
                        "PACK_ST, PACK_ET,CLEAN_ST, CELAN_ET, ETC_ST, ETC_ET, REMARK_ST, REMARK_ET, DOS_Q, OR_QTY, PRO_QTY,  " +
                        "F_Q, E_Q, PA_Q, DIFF,OEGWAN, DAN1, DAN2, DAN3, I_TIME, I_USER)  " +
                        "VALUES ('{0}', '{1}',  " +
                        "'{2}', '{3}', '{4}', '{5}', '{6}',  " +
                        "'{7}', '{8}', '{9}', '{10}',  " +
                        "'{11}', '{12}', '{13}', '{14}',  " +
                        "'{15}', '{16}', '{17}', '{18}',  " +
                        "'{19}', '{20}', '{21}', '{22}',  " +
                        "'{23}', '{24}', '{25}', '{26}',  " +
                        "'{27}', '{28}', '{29}', getdate(),  " +
                        "'{30}' ) ";

                        SQL = string.Format(SQL,
                            string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                            workNumMake(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)),
                            (i + 1).ToString(),
                            findControl("txt_RESOURCE_NO" + (i + 1), "tag"),
                            findControl("txt_RESOURCE_NO" + (i + 1), "text"),
                            findControl("txt_WEIGHT_ST" + (i + 1), "text"),
                            findControl("txt_WEIGHT_ET" + (i + 1), "text"),
                            findControl("txt_INPUT_ST" + (i + 1), "text"),
                            findControl("txt_INPUT_ET" + (i + 1), "text"),
                            findControl("txt_DOS_ST" + (i + 1), "text"),
                            findControl("txt_DOS_ET" + (i + 1), "text"),
                            findControl("txt_PACK_ST" + (i + 1), "text"),
                            findControl("txt_PACK_ET" + (i + 1), "text"),
                            findControl("txt_CLEAN_ST" + (i + 1), "text"),
                            findControl("txt_CLEAN_ET" + (i + 1), "text"),
                            findControl("txt_ETC_ST" + (i + 1), "text"),
                            findControl("txt_ETC_ET" + (i + 1), "text"),
                            findControl("txt_REMARK_ST" + (i + 1), "text"),
                            findControl("txt_REMARK_ET" + (i + 1), "text"),
                            findControl("txt_DOS_Q" + (i + 1), "text"),
                            findControl("txt_OR_QTY" + (i + 1), "text"),
                            findControl("txt_PRO_QTY" + (i + 1), "text"),
                            findControl("txt_F_Q" + (i + 1), "text"),
                            findControl("txt_E_Q" + (i + 1), "text"),
                            findControl("txt_PA_Q" + (i + 1), "text"),
                            findControl("txt_DIFF" + (i + 1), "text"),
                            findControl("txt_OEGWAN" + (i + 1), "text"),
                            findControl("txt_DAN1_" + (i + 1), "text"),
                            findControl("txt_DAN2_" + (i + 1), "text"),
                            findControl("txt_DAN3_" + (i + 1), "text"),
                            clsCommon._strUserId
                            );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패하였습니다");
                            return;
                        }
                    }
                } //for

                SQL = "DELETE FROM MILK_CHK_LIST WHERE WORK_NUMBER = '{0}' ";
                SQL = string.Format(SQL,
                        string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                        );
                Dbconn.conn.SQLrun(SQL);

                SQL =
                   "INSERT INTO MILK_CHK_LIST " +
                   "(WORK_NUMBER, CHK_LIST1, CHK_LIST2, CHK_LIST3, CHK_LIST4, CHK_LIST5,CHK_LIST6, CHK_LIST7, REMARK, I_TIME, I_USER)  " +
                   "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}' , getdate(), '{9}') ";

                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), //1
                    txt_CHK_MEMO1.Text,
                    txt_CHK_MEMO2.Text,
                    txt_CHK_MEMO3.Text,
                    txt_CHK_MEMO4.Text,
                    txt_CHK_MEMO5.Text,
                    txt_CHK_MEMO6.Text,
                    txt_CHK_MEMO7.Text,
                    memoEdit_REMARK.Text,
                    clsCommon._strUserId
                    );

                Dbconn.conn.SQLrun(SQL);


                XMain_Search();

                ShowMessageBox.XtraShowInformation("저장되었습니다");

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                clsLog.logSave(this, "btn_save_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생하였습니다");
            }
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                clsPrintReport.PrintMilkReport(workDate);
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                SQL = "DELETE FROM MILK_REPORT WHERE WORK_NUMBER = '{0}' ";
                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                    );

                Dbconn.conn.SQLrun(SQL);

                SQL = "DELETE FROM MILK_CHK_LIST WHERE WORK_NUMBER = '{0}' ";
                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                    );

                Dbconn.conn.SQLrun(SQL);

                resetControls();
            }
            catch (Exception ex)
            {

                
            }
        }

        private void btn_loadData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = ShowMessageBox.Confirm("알림", "대용유 작업내역을 불러오시겠습니까?");
                if (result != DialogResult.Yes)
                {
                    return;
                }


                resetControls();

                SQL =
                    "SELECT PO.RESOURCE_NO, ERP_ING.DESCRIPTION " +
                    "FROM PACK_ORDER PO LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING " +
                    "ON PO.RESOURCE_NO = ERP_ING.RESOURCE_NO " +
                    "WHERE PO.SBNO = '2' AND PO.WORK_START_DATE = '{0}'  " +
                    "AND ISNULL(PO.DEL_FLAG,'1') <> 'D' " +
                    "ORDER BY PO.WORK_SEQ ";

                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                    );
;

                DataSet milkWorkDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(milkWorkDs) > 0)
                {
                    int dbIndex = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        if (dbIndex > 5)
                        {
                            break;
                        }

                        if (dbIndex > Dbconn.conn.getRowCnt(milkWorkDs))
                        {
                            break;
                        }

                        findControlSet("txt_RESOURCE_NO" + (i + 1).ToString() , "tag", Dbconn.conn.getData(milkWorkDs, "RESOURCE_NO", dbIndex));
                        findControlSet("txt_RESOURCE_NO" + (i + 1).ToString(), "text", Dbconn.conn.getData(milkWorkDs, "DESCRIPTION", dbIndex));

                        dbIndex += 1;
                    }
                }else
                {
                    ShowMessageBox.XtraShowInformation("불러오기할 작업정보가 없습니다");
                    return;
                }
             
                ShowMessageBox.XtraShowInformation("작업정보 불러오기가 완료되었습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_loadData_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
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

            // 저장
            if (e.Control && e.KeyCode == Keys.S)
            {
                XMain_Save();
            }

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }
    }
}