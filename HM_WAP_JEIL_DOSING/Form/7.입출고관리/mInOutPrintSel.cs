using Core.Class;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class mInOutPrintSel : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = string.Empty;
        string argIs_no = string.Empty;
        bool argF_print = false;
        public mInOutPrintSel(string is_no, bool f_print)
        {
            InitializeComponent();

            argIs_no = is_no;
            argF_print = f_print;
        }

        private void mInOutPrintSel_Load(object sender, EventArgs e)
        {
            SQL = "SELECT IS_NO, INCAR_NO FROM WAP_DECAR WHERE IS_NO = '{0}' ";
            SQL = string.Format(SQL,
                argIs_no
                );

            DataSet infoDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(infoDs) > 0)
            {
                label_sel1.Text = Dbconn.conn.getData(infoDs, "IS_NO", 0).Trim();
                label_sel2.Text = Dbconn.conn.getData(infoDs, "INCAR_NO", 0).Trim();
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {

            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("프린터물을 인쇄중입니다");

                Application.DoEvents();
                if (chkEd_print1.Checked)
                {
                    //clsPrintExcel.PrintDeliverySheet(argIs_no);
                }

                Application.DoEvents();
                if (chkEd_print2.Checked)
                {
                    clsPrintExcel.PrintChulgoSheet(argIs_no);
                }

                Application.DoEvents();
                if (chkEd_print3.Checked)
                {
                    clsPrintExcel.PrintWeighingSheet(argIs_no, argF_print);
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
            }finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}