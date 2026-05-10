using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Core.Class
{
    public sealed class ShowMessageBox
    {

        public static DialogResult XtraShowError(string strMessage)
        {
            return XtraMessageBox.Show(new Form { TopMost = true }, strMessage, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

        }

        public static DialogResult XtraShowError(string strMessage, string strSubMessage)
        {
            return XtraMessageBox.Show(new Form { TopMost = true }, "<size=" + clsCommon.strHtmlFontSize_M + ">" + strMessage + "</size><br/><size="+ clsCommon.strHtmlFontSize_S + "><color=red>" + strSubMessage + "</color></size>", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

        }

        public static DialogResult XtraShowErrorLog(string strMessage, Form frm, string ClassNm, Exception ex)
        {
            clsLog.logSave(frm, ClassNm, ex);
            return XtraMessageBox.Show(new Form { TopMost = true }, strMessage, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult XtraShowWarning(string strMessage)
        {

            return XtraMessageBox.Show(new Form { TopMost = true }, strMessage, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

        }
        public static DialogResult XtraShowWarning(string strMessage, string strSubMessage)
        {

            return XtraMessageBox.Show(new Form { TopMost = true }, strMessage, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

        }
        public static DialogResult XtraShowInformation(string strMessage)
        {

            return XtraMessageBox.Show(new Form { TopMost = true }, strMessage, "알림", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        }

        public static DialogResult XtraShowInformation(string strMessage, string strSubMessage)
        {

            return XtraMessageBox.Show(new Form { TopMost = true }, "<size=" + clsCommon.strHtmlFontSize_M + ">" + strMessage + "</size><br/><size="+ clsCommon.strHtmlFontSize_S + "><color=red>" + strSubMessage + "</color></size>", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        }

        public static DialogResult Confirm(string strMessage)
        {
            return XtraMessageBox.Show(new Form { TopMost = true }, strMessage, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, DevExpress.Utils.DefaultBoolean.True);
        }

        public static DialogResult Confirm(string strMessage, string strSubMessage)
        {
            return XtraMessageBox.Show(new Form { TopMost = true }, "<size=" + clsCommon.strHtmlFontSize_M + ">" + strMessage + "</size><br/><size="+ clsCommon.strHtmlFontSize_S + "><color=red>"+ strSubMessage + "</color></size>", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, DevExpress.Utils.DefaultBoolean.True);
        }

        private static bool canCloseFunc(DialogResult parameter)
        {
            return parameter != DialogResult.Cancel;
        }

        public static DialogResult FlyoutConfirm(DevExpress.XtraEditors.XtraForm frm, string caption,  string strMessage, string button1, string button2)
        {
            DevExpress.XtraBars.Docking2010.Views.WindowsUI.FlyoutAction action = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.FlyoutAction() { Caption = caption, Description = strMessage, Image = Properties.Resources.close_32x32 };
            Predicate<DialogResult> predicate = canCloseFunc;
            DevExpress.XtraBars.Docking2010.Views.WindowsUI.FlyoutCommand command1 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.FlyoutCommand() { Text = button1, Result = System.Windows.Forms.DialogResult.Yes};
            DevExpress.XtraBars.Docking2010.Views.WindowsUI.FlyoutCommand command2 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.FlyoutCommand() { Text = button2, Result = System.Windows.Forms.DialogResult.No };
            action.Commands.Add(command1);
            action.Commands.Add(command2);

            FlyoutProperties properties = new FlyoutProperties();
            properties.ButtonSize = new Size(100, 50);
            properties.Style = FlyoutStyle.MessageBox;
            return FlyoutDialog.Show(frm, action, properties, predicate);
        }
    }
}
