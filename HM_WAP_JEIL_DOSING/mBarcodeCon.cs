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
    public partial class mBarcodeCon : DevExpress.XtraEditors.XtraForm
    {
        private string vPlcType = string.Empty;

        public mBarcodeCon()
        {
            InitializeComponent();
        }

        private void mBarcodeCon_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.BarcodeYn == "Y")
            {
                toggleSwitch_BarYn.IsOn = true;
            }
            else
            {
                toggleSwitch_BarYn.IsOn = false;
            }

            clsDevexpressUtil.ItemLookUpEditSetup(cboComPort, clsCommon.GetBarcodeCom());
            cboComPort.EditValue = Properties.Settings.Default.Com;

            clsDevexpressUtil.ItemLookUpEditSetup(cboBaud, clsCommon.GetBaudRate());
            cboBaud.EditValue = Properties.Settings.Default.BaudRate;

            clsDevexpressUtil.ItemLookUpEditSetup(cboStopBit, clsCommon.GetStopBit());
            cboStopBit.EditValue = Properties.Settings.Default.StopBit;

            clsDevexpressUtil.ItemLookUpEditSetup(cboDataBit, clsCommon.GetDataBit());
            cboDataBit.EditValue= Properties.Settings.Default.DataBit;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (toggleSwitch_BarYn.IsOn)
            {
                clsCommon.BarcodeConnYn = "Y";
                Properties.Settings.Default.BarcodeYn = "Y";
            }
            else
            {
                clsCommon.BarcodeConnYn = "N";
                Properties.Settings.Default.BarcodeYn = "N";
            }

            Properties.Settings.Default.Com = cboComPort.EditValue?.ToString();
            Properties.Settings.Default.BaudRate = int.Parse(cboBaud.EditValue?.ToString());
            Properties.Settings.Default.StopBit = int.Parse(cboStopBit.EditValue?.ToString());
            Properties.Settings.Default.DataBit = int.Parse(cboDataBit.EditValue?.ToString());
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void mBarcodeCon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(null, null);
            } 
        }
    }
}