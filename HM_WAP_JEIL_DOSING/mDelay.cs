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
    public partial class mDelay : DevExpress.XtraEditors.XtraForm
    {
        private string vPlcType = string.Empty;

        public mDelay()
        {
            InitializeComponent();
        }

        private void mDelay_Load(object sender, EventArgs e)
        {
             txtDelay.Text = Properties.Settings.Default.FontSize.ToString();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FontSize = int.Parse(txtDelay.EditValue?.ToString());
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void mDelay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(null, null);
            } 
        }
    }
}