namespace HARIM_FA_DOSING
{ 
    partial class m_SAP_INPUT_POORDERM_CON
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_SAP_INPUT_POORDERM_CON));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridPoor = new DevExpress.XtraGrid.GridControl();
            this.viewPoor = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.EBELN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BSART = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LIFNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboLIFNR = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.NAME1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BEDAT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LIFN2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NAME2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboMTYPE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.MESSAGE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnInput = new DevExpress.XtraEditors.SimpleButton();
            this.dtDate = new DevExpress.XtraEditors.DateEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPoor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPoor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboLIFNR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboMTYPE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridPoor);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.btnInput);
            this.layoutControl.Controls.Add(this.dtDate);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(759, 420);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridPoor
            // 
            this.gridPoor.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridPoor.Location = new System.Drawing.Point(8, 60);
            this.gridPoor.MainView = this.viewPoor;
            this.gridPoor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridPoor.Name = "gridPoor";
            this.gridPoor.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridcboLIFNR,
            this.gridcboMTYPE});
            this.gridPoor.Size = new System.Drawing.Size(743, 320);
            this.gridPoor.TabIndex = 9;
            this.gridPoor.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewPoor});
            // 
            // viewPoor
            // 
            this.viewPoor.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.EBELN,
            this.BSART,
            this.LIFNR,
            this.NAME1,
            this.BEDAT,
            this.LIFN2,
            this.NAME2,
            this.MTYPE,
            this.MESSAGE});
            this.viewPoor.DetailHeight = 404;
            this.viewPoor.GridControl = this.gridPoor;
            this.viewPoor.Name = "viewPoor";
            this.viewPoor.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.viewPoor.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.viewPoor.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.viewPoor.OptionsFind.ShowFindButton = false;
            this.viewPoor.OptionsView.ColumnAutoWidth = false;
            this.viewPoor.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.viewPoor.OptionsView.ShowGroupPanel = false;
            this.viewPoor.ViewCaption = "<color=lightgreen>■</color> 차량입고 정보입력";
            this.viewPoor.DoubleClick += new System.EventHandler(this.viewPoor_DoubleClick);
            // 
            // EBELN
            // 
            this.EBELN.Caption = "발주 번호";
            this.EBELN.FieldName = "EBELN";
            this.EBELN.Name = "EBELN";
            this.EBELN.Visible = true;
            this.EBELN.VisibleIndex = 0;
            this.EBELN.Width = 120;
            // 
            // BSART
            // 
            this.BSART.Caption = "PO 구분";
            this.BSART.FieldName = "BSART";
            this.BSART.Name = "BSART";
            this.BSART.Visible = true;
            this.BSART.VisibleIndex = 1;
            this.BSART.Width = 80;
            // 
            // LIFNR
            // 
            this.LIFNR.Caption = "내외자 구분";
            this.LIFNR.ColumnEdit = this.gridcboLIFNR;
            this.LIFNR.FieldName = "LIFNR";
            this.LIFNR.Name = "LIFNR";
            this.LIFNR.Visible = true;
            this.LIFNR.VisibleIndex = 2;
            // 
            // gridcboLIFNR
            // 
            this.gridcboLIFNR.AutoHeight = false;
            this.gridcboLIFNR.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboLIFNR.Name = "gridcboLIFNR";
            // 
            // NAME1
            // 
            this.NAME1.Caption = "부두 업체명";
            this.NAME1.FieldName = "NAME1";
            this.NAME1.Name = "NAME1";
            this.NAME1.Visible = true;
            this.NAME1.VisibleIndex = 3;
            this.NAME1.Width = 160;
            // 
            // BEDAT
            // 
            this.BEDAT.Caption = "발주 일자";
            this.BEDAT.FieldName = "BEDAT";
            this.BEDAT.Name = "BEDAT";
            this.BEDAT.Visible = true;
            this.BEDAT.VisibleIndex = 4;
            this.BEDAT.Width = 120;
            // 
            // LIFN2
            // 
            this.LIFN2.Caption = "운송 업체";
            this.LIFN2.FieldName = "LIFN2";
            this.LIFN2.Name = "LIFN2";
            this.LIFN2.Width = 120;
            // 
            // NAME2
            // 
            this.NAME2.Caption = "운송 업체명";
            this.NAME2.FieldName = "NAME2";
            this.NAME2.Name = "NAME2";
            this.NAME2.Visible = true;
            this.NAME2.VisibleIndex = 5;
            this.NAME2.Width = 160;
            // 
            // MTYPE
            // 
            this.MTYPE.Caption = "메시지 유형";
            this.MTYPE.ColumnEdit = this.gridcboMTYPE;
            this.MTYPE.FieldName = "MTYPE";
            this.MTYPE.Name = "MTYPE";
            this.MTYPE.Width = 100;
            // 
            // gridcboMTYPE
            // 
            this.gridcboMTYPE.AutoHeight = false;
            this.gridcboMTYPE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboMTYPE.Name = "gridcboMTYPE";
            // 
            // MESSAGE
            // 
            this.MESSAGE.Caption = "메시지";
            this.MESSAGE.FieldName = "MESSAGE";
            this.MESSAGE.Name = "MESSAGE";
            this.MESSAGE.Width = 200;
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 10;
            this.btn_cancel.Location = new System.Drawing.Point(390, 390);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_cancel.Size = new System.Drawing.Size(216, 28);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "취 소";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btnInput
            // 
            this.btnInput.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInput.ImageOptions.Image")));
            this.btnInput.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnInput.ImageOptions.ImageToTextIndent = 10;
            this.btnInput.Location = new System.Drawing.Point(170, 390);
            this.btnInput.Margin = new System.Windows.Forms.Padding(4);
            this.btnInput.Name = "btnInput";
            this.btnInput.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnInput.Size = new System.Drawing.Size(216, 28);
            this.btnInput.StyleController = this.layoutControl;
            this.btnInput.TabIndex = 6;
            this.btnInput.Text = "입 력";
            this.btnInput.ToolTip = "저장(Ctrl + S)";
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // dtDate
            // 
            this.dtDate.EditValue = null;
            this.dtDate.Location = new System.Drawing.Point(111, 30);
            this.dtDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtDate.Name = "dtDate";
            this.dtDate.Properties.AllowFocused = false;
            this.dtDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDate.Properties.CalendarTimeProperties.AllowFocused = false;
            this.dtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDate.Size = new System.Drawing.Size(92, 24);
            this.dtDate.StyleController = this.layoutControl;
            this.dtDate.TabIndex = 4;
            this.dtDate.EditValueChanged += new System.EventHandler(this.dtDate_EditValueChanged);
            this.dtDate.TextChanged += new System.EventHandler(this.dtDate_TextChanged);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.emptySpaceItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(759, 420);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem3,
            this.layoutControlItem_workdate});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(759, 388);
            this.layoutControlGroup.Text = "구매 발주 번호 선택하여 주세요";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridPoor;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 32);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(747, 324);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(200, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(547, 32);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem_workdate
            // 
            this.layoutControlItem_workdate.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workdate.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workdate.Control = this.dtDate;
            this.layoutControlItem_workdate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workdate.CustomizationFormText = "납품일";
            this.layoutControlItem_workdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem_workdate.ImageOptions.Image")));
            this.layoutControlItem_workdate.ImageOptions.ImageToTextDistance = 10;
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 4, 4);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(200, 32);
            this.layoutControlItem_workdate.Text = "납품 예정일";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(90, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 5;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 388);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(168, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnInput;
            this.layoutControlItem2.Location = new System.Drawing.Point(168, 388);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(220, 32);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_cancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(388, 388);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(220, 32);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(608, 388);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(151, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // m_SAP_INPUT_POORDERM_CON
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 420);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_SAP_INPUT_POORDERM_CON";
            this.Text = "구매 발주 번호 선택";
            this.Load += new System.EventHandler(this.m_EBELP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPoor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPoor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboLIFNR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboMTYPE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraEditors.SimpleButton btnInput;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraGrid.GridControl gridPoor;
        private DevExpress.XtraGrid.Views.Grid.GridView viewPoor;
        private DevExpress.XtraGrid.Columns.GridColumn EBELN;
        private DevExpress.XtraGrid.Columns.GridColumn BSART;
        private DevExpress.XtraGrid.Columns.GridColumn LIFNR;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboLIFNR;
        private DevExpress.XtraGrid.Columns.GridColumn NAME1;
        private DevExpress.XtraGrid.Columns.GridColumn BEDAT;
        private DevExpress.XtraGrid.Columns.GridColumn LIFN2;
        private DevExpress.XtraGrid.Columns.GridColumn NAME2;
        private DevExpress.XtraGrid.Columns.GridColumn MTYPE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboMTYPE;
        private DevExpress.XtraGrid.Columns.GridColumn MESSAGE;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.DateEdit dtDate;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
    }
}