namespace HARIM_FA_DOSING
{
    partial class m_WorkDateCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_WorkDateCopy));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_input = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.dtAWorkDate = new DevExpress.XtraEditors.DateEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtAWorkDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAWorkDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_input);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.dtAWorkDate);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(434, 99);
            this.layoutControl.TabIndex = 1;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_input
            // 
            this.btn_input.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_input.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_input.ImageOptions.Image")));
            this.btn_input.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_input.ImageOptions.ImageToTextIndent = 20;
            this.btn_input.Location = new System.Drawing.Point(11, 64);
            this.btn_input.Name = "btn_input";
            this.btn_input.Size = new System.Drawing.Size(216, 23);
            this.btn_input.StyleController = this.layoutControl;
            this.btn_input.TabIndex = 15;
            this.btn_input.Text = "복  사";
            this.btn_input.Click += new System.EventHandler(this.btn_input_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 20;
            this.btn_cancel.Location = new System.Drawing.Point(231, 64);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(192, 23);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 16;
            this.btn_cancel.Text = "취  소";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // dtAWorkDate
            // 
            this.dtAWorkDate.EditValue = "";
            this.dtAWorkDate.Location = new System.Drawing.Point(107, 36);
            this.dtAWorkDate.Name = "dtAWorkDate";
            this.dtAWorkDate.Properties.AllowFocused = false;
            this.dtAWorkDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtAWorkDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtAWorkDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Classic;
            this.dtAWorkDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dtAWorkDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtAWorkDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dtAWorkDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtAWorkDate.Properties.MaskSettings.Set("mask", "yyyy-MM-dd");
            this.dtAWorkDate.Properties.UseMaskAsDisplayFormat = true;
            this.dtAWorkDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView;
            this.dtAWorkDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            this.dtAWorkDate.Size = new System.Drawing.Size(116, 24);
            this.dtAWorkDate.StyleController = this.layoutControl;
            this.dtAWorkDate.TabIndex = 13;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.Root.Size = new System.Drawing.Size(434, 99);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup1.CaptionImageOptions.Image")));
            this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem10,
            this.layoutControlItem15,
            this.layoutControlItem14,
            this.emptySpaceItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 6, 6);
            this.layoutControlGroup1.Size = new System.Drawing.Size(430, 95);
            this.layoutControlGroup1.Text = "복사 할 작업일자 선택";
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem10.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem10.Control = this.dtAWorkDate;
            this.layoutControlItem10.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem10.ImageOptions.Image")));
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem10.MaxSize = new System.Drawing.Size(216, 28);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(216, 28);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(216, 28);
            this.layoutControlItem10.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem10.Text = "작업일자";
            this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(69, 16);
            this.layoutControlItem10.TextToControlDistance = 27;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btn_cancel;
            this.layoutControlItem15.Location = new System.Drawing.Point(220, 28);
            this.layoutControlItem15.MinSize = new System.Drawing.Size(75, 26);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(196, 27);
            this.layoutControlItem15.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btn_input;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(79, 26);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(220, 27);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(216, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(200, 28);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // m_WorkDateCopy
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 99);
            this.Controls.Add(this.layoutControl);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_WorkDateCopy";
            this.Text = "복사 할 작업일자 선택";
            this.Load += new System.EventHandler(this.m_WorkGroup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtAWorkDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAWorkDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_input;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraEditors.DateEdit dtAWorkDate;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}