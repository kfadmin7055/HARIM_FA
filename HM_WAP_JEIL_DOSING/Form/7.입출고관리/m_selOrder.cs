using Core.Class;
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
    public partial class m_selOrder : DevExpress.XtraEditors.XtraForm
    {
        DataSet argDataSet;
        public m_selOrder(DataSet ds)
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
            argDataSet = ds;
        }

 
        private void m_selOrder_Load(object sender, EventArgs e)
        {
            try
            {

                clsDevexpressGrid.BindGridControl(gridControl, gridView, argDataSet.Tables[0], true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "m_selOrder_Load", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("입력하실 주문서정보를 선택하여 주세요");
                this.DialogResult = DialogResult.None;
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}