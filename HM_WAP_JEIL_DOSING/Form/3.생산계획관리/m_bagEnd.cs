using Core.Class;
using System;
using System.Data;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_bagEnd : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;

        string argPlantCode = string.Empty;
        string argProcessKey = string.Empty;
        string argLCode = string.Empty;
        string argWorkNumb = string.Empty;
        string argWorkSeq = string.Empty;
        string argSbno = string.Empty;

        public m_bagEnd(string plantCode, string process_key, string lCode, string work_numb, string work_seq, string sbno)
        {
            argPlantCode = plantCode;
            argProcessKey = process_key;
            argLCode = lCode;
            argWorkNumb = work_numb;
            argWorkSeq = work_seq;
            argSbno = sbno;

            InitializeComponent();
        }

        private void m_bagEnd_Load(object sender, EventArgs e)
        {
            txt_workdate.Text = argWorkNumb;
            txt_workseq.Text = argWorkSeq;

            dateEdit_workStart.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dateEdit_workEnd.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            if (argSbno.Equals("1"))
            {
                txt_sbno.Text = "1호기";
            }
            else if (argSbno.Equals("2"))
            {
                txt_sbno.Text = "2호기";
            }
            else if (argSbno.Equals("3"))
            {
                txt_sbno.Text = "3호기";
            }
            else if (argSbno.Equals("B1"))
            {
                txt_sbno.Text = "벌크1호기";
            }
            else if (argSbno.Equals("B2"))
            {
                txt_sbno.Text = "벌크2호기";
            }
            else if (argSbno.Equals("B3"))
            {
                txt_sbno.Text = "벌크3호기";
            }

            SQL = $@"
            SELECT ERP_ING.DESCRIPTION                    -- 01
                 , TO_CHAR(BAG.RUN_ST, 'YYYY-MM-DD HH24:MI:SS') AS RUN_ST  -- 02
            FROM   BAG_ORDER BAG
            LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING
                   ON BAG.RESOURCE_NO = ERP_ING.RESOURCE_NO
            WHERE  BAG.PLANT_CODE = '{argPlantCode}'
                AND BAG.PROCESS_KEY = '{argProcessKey}' 
                AND BAG.L_CODE = '{argLCode}'
                AND BAG.WORKDATE = '{argWorkNumb}'
                AND BAG.WORK_SEQ = '{argWorkSeq}'
            ";

            using (DataSet resDs = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(resDs) > 0)
                {
                    txt_resName.Text = Dbconn.conn.getData(resDs, "DESCRIPTION", 0).Trim();

                    if (Dbconn.conn.getData(resDs, "RUN_ST", 0).Trim() != "")
                    {
                        dateEdit_workStart.EditValue = Convert.ToDateTime(Dbconn.conn.getData(resDs, "RUN_ST", 0).Trim());
                    }
                }                
            }


            //SQL = $"select 'NO' as CODE, '없음' as NAME\r\n union all \r\nselect error_no, description from ERP_DBLINK.{"clsCommon.erp_dosing_db_name"}.dbo.V_MES_ATG_110_1";
            //DataSet errorDs = Dbconn.conn.ExecutDataset(SQL);

            //clsDevexpressUtil.ItemLookUpEditSetup(le_badcode1, errorDs);
            //clsDevexpressUtil.ItemLookUpEditSetup(le_badcode2, errorDs);
            //clsDevexpressUtil.ItemLookUpEditSetup(le_badcode3, errorDs);
            //clsDevexpressUtil.ItemLookUpEditSetup(le_badcode4, errorDs);
            //clsDevexpressUtil.ItemLookUpEditSetup(le_badcode5, errorDs);

            //le_badcode1.ItemIndex = 0;
            //le_badcode2.ItemIndex = 0;
            //le_badcode3.ItemIndex = 0;
            //le_badcode4.ItemIndex = 0;
            //le_badcode5.ItemIndex = 0;

            txt_proqty.Focus();

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_proqty.Text))
            {
                ShowMessageBox.XtraShowInformation("포장생산수량을 입력해주세요");
                this.DialogResult = DialogResult.None;
                return;
            }

            int time_diff = Convert.ToDateTime(dateEdit_workEnd.EditValue).CompareTo(Convert.ToDateTime(dateEdit_workStart.EditValue));
            if (time_diff < 0)
            {
                ShowMessageBox.XtraShowInformation("종료시간이 시작시간보다 빠릅니다");
                this.DialogResult = DialogResult.None;
                return;
            }

            if (le_badcode1.EditValue != null && !le_badcode1.EditValue.Equals("NO"))
            {
                if (string.IsNullOrEmpty(txt_badqty1.Text.Trim()) || txt_badqty1.Text.Trim() == "0")
                {
                    ShowMessageBox.XtraShowInformation("불량수량1을 입력해주세요");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            if (le_badcode2.EditValue != null && !le_badcode2.EditValue.Equals("NO"))
            {
                if (string.IsNullOrEmpty(txt_badqty2.Text.Trim()) || txt_badqty2.Text.Trim() == "0")
                {
                    ShowMessageBox.XtraShowInformation("불량수량2을 입력해주세요");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            if (le_badcode3.EditValue != null && !le_badcode3.EditValue.Equals("NO"))
            {
                if (string.IsNullOrEmpty(txt_badqty3.Text.Trim()) || txt_badqty3.Text.Trim() == "0")
                {
                    ShowMessageBox.XtraShowInformation("불량수량3을 입력해주세요");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            if (le_badcode4.EditValue != null && !le_badcode4.EditValue.Equals("NO"))
            {

                if (string.IsNullOrEmpty(txt_badqty4.Text.Trim()) || txt_badqty4.Text.Trim() == "0")
                {
                    ShowMessageBox.XtraShowInformation("불량수량4을 입력해주세요");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            if (le_badcode5.EditValue != null && !le_badcode5.EditValue.Equals("NO"))
            {

                if (string.IsNullOrEmpty(txt_badqty5.Text.Trim()) || txt_badqty5.Text.Trim() == "0")
                {
                    ShowMessageBox.XtraShowInformation("불량수량5을 입력해주세요");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            SQL = $@"
            UPDATE BAG_ORDER SET PRO_QTY = '{txt_proqty.Text}', PRO_KG = '{txt_prokg.Text}'
                , BAD_CODE1 = '{le_badcode1.EditValue}', BAD_QTY1 = '{txt_badqty1.Text}'
                , BAD_CODE2 = '{le_badcode2.EditValue}', BAD_QTY2 = '{txt_badqty2.Text}'
                , BAD_CODE3 = '{le_badcode3.EditValue}', BAD_QTY3 = '{txt_badqty3.Text}'
                , BAD_CODE4 = '{le_badcode4.EditValue}', BAD_QTY4 = '{txt_badqty4.Text}'
                , BAD_CODE5 = '{le_badcode5.EditValue}', BAD_QTY5 = '{txt_badqty5.Text}'
                , F_Q = '{txt_badbeginqty.EditValue}', E_Q = '{txt_badendqty.EditValue}'
                , END_QTY = '{txt_badremantsqty.EditValue}', USE_END_QTY = '{txt_badremantsUseQty.EditValue}'
                , RUN_ST = TO_DATE('{Convert.ToDateTime(dateEdit_workStart.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                , RUN_ET = TO_DATE('{Convert.ToDateTime(dateEdit_workEnd.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
            WHERE  PLANT_CODE = '{argPlantCode}'
                AND PROCESS_KEY = '{argProcessKey}' 
                AND L_CODE = '{argLCode}'
                AND WORKDATE = '{txt_workdate.Text}' AND WORK_SEQ = '{txt_workseq.Text}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave(this, "btn_input_Click", SQL);
                ShowMessageBox.XtraShowInformation("강제완료를 실행을 실패했습니다");
                this.DialogResult = DialogResult.None;
                return;
            }

        }
    }
}