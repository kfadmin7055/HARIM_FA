namespace HARIM_FA_DOSING
{
    partial class mWorkTimeInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mWorkTimeInput));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.txt_workseq = new DevExpress.XtraEditors.TextEdit();
            this.txt_workdate = new DevExpress.XtraEditors.TextEdit();
            this.btn_input = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.txt_resName = new DevExpress.XtraEditors.TextEdit();
            this.dateEdit_workStart = new DevExpress.XtraEditors.DateEdit();
            this.dateEdit_workEnd = new DevExpress.XtraEditors.DateEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem22 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem23 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_workseq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_workdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_resName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.txt_workseq);
            this.layoutControl.Controls.Add(this.txt_workdate);
            this.layoutControl.Controls.Add(this.btn_input);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.txt_resName);
            this.layoutControl.Controls.Add(this.dateEdit_workStart);
            this.layoutControl.Controls.Add(this.dateEdit_workEnd);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(503, 205);
            this.layoutControl.TabIndex = 2;
            this.layoutControl.Text = "layoutControl1";
            // 
            // txt_workseq
            // 
            this.txt_workseq.Location = new System.Drawing.Point(362, 35);
            this.txt_workseq.Name = "txt_workseq";
            this.txt_workseq.Properties.AllowFocused = false;
            this.txt_workseq.Properties.ReadOnly = true;
            this.txt_workseq.Size = new System.Drawing.Size(130, 24);
            this.txt_workseq.StyleController = this.layoutControl;
            this.txt_workseq.TabIndex = 4;
            // 
            // txt_workdate
            // 
            this.txt_workdate.EditValue = "";
            this.txt_workdate.Location = new System.Drawing.Point(119, 35);
            this.txt_workdate.Name = "txt_workdate";
            this.txt_workdate.Properties.AllowFocused = false;
            this.txt_workdate.Properties.ReadOnly = true;
            this.txt_workdate.Size = new System.Drawing.Size(130, 24);
            this.txt_workdate.StyleController = this.layoutControl;
            this.txt_workdate.TabIndex = 13;
            // 
            // btn_input
            // 
            this.btn_input.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_input.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_input.ImageOptions.Image")));
            this.btn_input.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_input.ImageOptions.ImageToTextIndent = 20;
            this.btn_input.Location = new System.Drawing.Point(11, 147);
            this.btn_input.Name = "btn_input";
            this.btn_input.Size = new System.Drawing.Size(226, 47);
            this.btn_input.StyleController = this.layoutControl;
            this.btn_input.TabIndex = 15;
            this.btn_input.Text = "입  력";
            this.btn_input.Click += new System.EventHandler(this.btn_input_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 20;
            this.btn_cancel.Location = new System.Drawing.Point(278, 147);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(214, 47);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 16;
            this.btn_cancel.Text = "취 소";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_resName
            // 
            this.txt_resName.EditValue = "";
            this.txt_resName.Location = new System.Drawing.Point(119, 63);
            this.txt_resName.Name = "txt_resName";
            this.txt_resName.Properties.AllowFocused = false;
            this.txt_resName.Properties.ReadOnly = true;
            this.txt_resName.Size = new System.Drawing.Size(373, 24);
            this.txt_resName.StyleController = this.layoutControl;
            this.txt_resName.TabIndex = 13;
            // 
            // dateEdit_workStart
            // 
            this.dateEdit_workStart.EditValue = null;
            this.dateEdit_workStart.Location = new System.Drawing.Point(124, 91);
            this.dateEdit_workStart.Name = "dateEdit_workStart";
            this.dateEdit_workStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit_workStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit_workStart.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit_workStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit_workStart.Properties.MaskSettings.Set("mask", "G");
            this.dateEdit_workStart.Size = new System.Drawing.Size(368, 24);
            this.dateEdit_workStart.StyleController = this.layoutControl;
            this.dateEdit_workStart.TabIndex = 17;
            // 
            // dateEdit_workEnd
            // 
            this.dateEdit_workEnd.EditValue = null;
            this.dateEdit_workEnd.Location = new System.Drawing.Point(124, 119);
            this.dateEdit_workEnd.Name = "dateEdit_workEnd";
            this.dateEdit_workEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit_workEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit_workEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit_workEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit_workEnd.Properties.MaskSettings.Set("mask", "G");
            this.dateEdit_workEnd.Size = new System.Drawing.Size(368, 24);
            this.dateEdit_workEnd.StyleController = this.layoutControl;
            this.dateEdit_workEnd.TabIndex = 18;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.Root.Size = new System.Drawing.Size(503, 205);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup1.CaptionImageOptions.Image")));
            this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem10,
            this.layoutControlItem15,
            this.layoutControlItem14,
            this.emptySpaceItem3,
            this.emptySpaceItem5,
            this.layoutControlItem17,
            this.layoutControlItem22,
            this.layoutControlItem23});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(499, 201);
            this.layoutControlGroup1.Text = "대용유배합 작업시간 입력";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txt_workseq;
            this.layoutControlItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem1.ImageOptions.Image")));
            this.layoutControlItem1.Location = new System.Drawing.Point(252, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(233, 28);
            this.layoutControlItem1.Text = "작업순번";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(69, 16);
            this.layoutControlItem1.TextToControlDistance = 30;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.txt_workdate;
            this.layoutControlItem10.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem10.ImageOptions.Image")));
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(242, 28);
            this.layoutControlItem10.Text = "작업지시일";
            this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(81, 16);
            this.layoutControlItem10.TextToControlDistance = 27;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btn_cancel;
            this.layoutControlItem15.Location = new System.Drawing.Point(267, 112);
            this.layoutControlItem15.MaxSize = new System.Drawing.Size(218, 51);
            this.layoutControlItem15.MinSize = new System.Drawing.Size(218, 51);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(218, 51);
            this.layoutControlItem15.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btn_input;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 112);
            this.layoutControlItem14.MaxSize = new System.Drawing.Size(230, 51);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(230, 51);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(230, 51);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(242, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(10, 28);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.Location = new System.Drawing.Point(230, 112);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(37, 51);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this.txt_resName;
            this.layoutControlItem17.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem17.CustomizationFormText = "작업지시일";
            this.layoutControlItem17.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem17.ImageOptions.Image")));
            this.layoutControlItem17.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(485, 28);
            this.layoutControlItem17.Text = "작업제품명";
            this.layoutControlItem17.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem17.TextSize = new System.Drawing.Size(81, 16);
            this.layoutControlItem17.TextToControlDistance = 27;
            // 
            // layoutControlItem22
            // 
            this.layoutControlItem22.Control = this.dateEdit_workStart;
            this.layoutControlItem22.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem22.ImageOptions.Image")));
            this.layoutControlItem22.Location = new System.Drawing.Point(0, 56);
            this.layoutControlItem22.Name = "layoutControlItem22";
            this.layoutControlItem22.Size = new System.Drawing.Size(485, 28);
            this.layoutControlItem22.Text = "작업시작시간";
            this.layoutControlItem22.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem22.TextSize = new System.Drawing.Size(93, 16);
            this.layoutControlItem22.TextToControlDistance = 20;
            // 
            // layoutControlItem23
            // 
            this.layoutControlItem23.Control = this.dateEdit_workEnd;
            this.layoutControlItem23.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem23.ImageOptions.Image")));
            this.layoutControlItem23.Location = new System.Drawing.Point(0, 84);
            this.layoutControlItem23.Name = "layoutControlItem23";
            this.layoutControlItem23.Size = new System.Drawing.Size(485, 28);
            this.layoutControlItem23.Text = "작업종료시간";
            this.layoutControlItem23.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem23.TextSize = new System.Drawing.Size(93, 16);
            this.layoutControlItem23.TextToControlDistance = 20;
            // 
            // mWorkTimeInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 205);
            this.Controls.Add(this.layoutControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("mWorkTimeInput.IconOptions.Image")));
            this.Name = "mWorkTimeInput";
            this.Text = "작업시간입력";
            this.Load += new System.EventHandler(this.mWorkTimeInput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_workseq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_workdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_resName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.TextEdit txt_workseq;
        private DevExpress.XtraEditors.TextEdit txt_workdate;
        private DevExpress.XtraEditors.SimpleButton btn_input;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.TextEdit txt_resName;
        private DevExpress.XtraEditors.DateEdit dateEdit_workStart;
        private DevExpress.XtraEditors.DateEdit dateEdit_workEnd;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem17;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem22;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem23;
    }
}