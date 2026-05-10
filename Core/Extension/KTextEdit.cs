using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Extension
{
    public static class KTextEdit
    {
        public static bool Validation(this TextEdit txtValue, LayoutControlItem lciValue)
        {
            if (txtValue.Text == "")
            {
                XtraMessageBox.Show(new Form { TopMost = true }, $"[{lciValue.Text}] 항목은 필수 입력 항목입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                txtValue.Focus();
                return true;
            }

            return false;
        }
    }
}
