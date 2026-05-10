namespace HARIM_FA_DOSING
{ 
    partial class m_SAP_INPUT_PROTRANSM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_SAP_INPUT_PROTRANSM));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridMain = new DevExpress.XtraGrid.GridControl();
            this.viewMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ERP_UP_YN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboTransFlag = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.TKNUM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TRAID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WERKS_GI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridscboPLANT = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.WERKS_GR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TRANS_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridMdateTime = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.ERP_TNUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnInput = new DevExpress.XtraEditors.SimpleButton();
            this.dtDate = new DevExpress.XtraEditors.DateEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboTransFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboPLANT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMdateTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMdateTime.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridMain);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.btnInput);
            this.layoutControl.Controls.Add(this.dtDate);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(629, 420);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridMain
            // 
            this.gridMain.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gridMain.Location = new System.Drawing.Point(2, 66);
            this.gridMain.MainView = this.viewMain;
            this.gridMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridMain.Name = "gridMain";
            this.gridMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridscboPLANT,
            this.gridMdateTime,
            this.gridcboTransFlag});
            this.gridMain.Size = new System.Drawing.Size(625, 320);
            this.gridMain.TabIndex = 9;
            this.gridMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewMain});
            // 
            // viewMain
            // 
            this.viewMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ERP_UP_YN,
            this.TKNUM,
            this.TRAID,
            this.WERKS_GI,
            this.WERKS_GR,
            this.TRANS_DATE,
            this.ERP_TNUMBER});
            this.viewMain.DetailHeight = 404;
            this.viewMain.GridControl = this.gridMain;
            this.viewMain.Name = "viewMain";
            this.viewMain.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.viewMain.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.viewMain.OptionsFind.ShowFindButton = false;
            this.viewMain.OptionsView.ColumnAutoWidth = false;
            this.viewMain.OptionsView.EnableAppearanceOddRow = true;
            this.viewMain.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.viewMain.OptionsView.ShowGroupPanel = false;
            this.viewMain.DoubleClick += new System.EventHandler(this.viewMain_DoubleClick);
            // 
            // ERP_UP_YN
            // 
            this.ERP_UP_YN.Caption = "ERP 전송상태";
            this.ERP_UP_YN.ColumnEdit = this.gridcboTransFlag;
            this.ERP_UP_YN.FieldName = "ERP_UP_YN";
            this.ERP_UP_YN.Name = "ERP_UP_YN";
            this.ERP_UP_YN.Width = 100;
            // 
            // gridcboTransFlag
            // 
            this.gridcboTransFlag.AutoHeight = false;
            this.gridcboTransFlag.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboTransFlag.Name = "gridcboTransFlag";
            this.gridcboTransFlag.NullText = "";
            // 
            // TKNUM
            // 
            this.TKNUM.Caption = "배차번호";
            this.TKNUM.FieldName = "TKNUM";
            this.TKNUM.Name = "TKNUM";
            this.TKNUM.Visible = true;
            this.TKNUM.VisibleIndex = 0;
            // 
            // TRAID
            // 
            this.TRAID.Caption = "운송차량번호";
            this.TRAID.FieldName = "TRAID";
            this.TRAID.Name = "TRAID";
            this.TRAID.Visible = true;
            this.TRAID.VisibleIndex = 1;
            this.TRAID.Width = 90;
            // 
            // WERKS_GI
            // 
            this.WERKS_GI.Caption = "출고 플랜트";
            this.WERKS_GI.ColumnEdit = this.gridscboPLANT;
            this.WERKS_GI.FieldName = "WERKS_GI";
            this.WERKS_GI.MinWidth = 160;
            this.WERKS_GI.Name = "WERKS_GI";
            this.WERKS_GI.Visible = true;
            this.WERKS_GI.VisibleIndex = 2;
            this.WERKS_GI.Width = 160;
            // 
            // gridscboPLANT
            // 
            this.gridscboPLANT.AutoHeight = false;
            this.gridscboPLANT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridscboPLANT.Name = "gridscboPLANT";
            this.gridscboPLANT.NullText = "";
            this.gridscboPLANT.PopupView = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // WERKS_GR
            // 
            this.WERKS_GR.Caption = "입고 플랜트";
            this.WERKS_GR.ColumnEdit = this.gridscboPLANT;
            this.WERKS_GR.FieldName = "WERKS_GR";
            this.WERKS_GR.MinWidth = 160;
            this.WERKS_GR.Name = "WERKS_GR";
            this.WERKS_GR.Visible = true;
            this.WERKS_GR.VisibleIndex = 3;
            this.WERKS_GR.Width = 160;
            // 
            // TRANS_DATE
            // 
            this.TRANS_DATE.Caption = "이고 출고일";
            this.TRANS_DATE.ColumnEdit = this.gridMdateTime;
            this.TRANS_DATE.FieldName = "TRANS_DATE";
            this.TRANS_DATE.MinWidth = 100;
            this.TRANS_DATE.Name = "TRANS_DATE";
            this.TRANS_DATE.Visible = true;
            this.TRANS_DATE.VisibleIndex = 4;
            this.TRANS_DATE.Width = 100;
            // 
            // gridMdateTime
            // 
            this.gridMdateTime.AutoHeight = false;
            this.gridMdateTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridMdateTime.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridMdateTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridMdateTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridMdateTime.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridMdateTime.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridMdateTime.MaskSettings.Set("mask", "yyyy-MM-dd HH:mm:ss");
            this.gridMdateTime.Name = "gridMdateTime";
            // 
            // ERP_TNUMBER
            // 
            this.ERP_TNUMBER.Caption = "ERP 전송일련번호";
            this.ERP_TNUMBER.FieldName = "ERP_TNUMBER";
            this.ERP_TNUMBER.Name = "ERP_TNUMBER";
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 10;
            this.btn_cancel.Location = new System.Drawing.Point(321, 390);
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
            this.btnInput.Location = new System.Drawing.Point(101, 390);
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
            this.dtDate.Location = new System.Drawing.Point(99, 30);
            this.dtDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtDate.Name = "dtDate";
            this.dtDate.Properties.AllowFocused = false;
            this.dtDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDate.Properties.CalendarTimeProperties.AllowFocused = false;
            this.dtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDate.Size = new System.Drawing.Size(212, 24);
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
            this.emptySpaceItem2,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(629, 420);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem_workdate,
            this.emptySpaceItem3});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(629, 64);
            this.layoutControlGroup.Text = "주문오더번호를 선택하여 주세요";
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
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(308, 32);
            this.layoutControlItem_workdate.Text = "출고 일자";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(78, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 5;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(308, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(309, 32);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 388);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(99, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnInput;
            this.layoutControlItem2.Location = new System.Drawing.Point(99, 388);
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
            this.layoutControlItem4.Location = new System.Drawing.Point(319, 388);
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
            this.emptySpaceItem2.Location = new System.Drawing.Point(539, 388);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(90, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridMain;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 64);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(629, 324);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // m_SAP_INPUT_PROTRANSM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 420);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_SAP_INPUT_PROTRANSM";
            this.Text = "주문 오더번호 선택";
            this.Load += new System.EventHandler(this.m_EBELP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboTransFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboPLANT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMdateTime.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMdateTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraEditors.DateEdit dtDate;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraGrid.GridControl gridMain;
        private DevExpress.XtraGrid.Views.Grid.GridView viewMain;
        private DevExpress.XtraGrid.Columns.GridColumn ERP_UP_YN;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboTransFlag;
        private DevExpress.XtraGrid.Columns.GridColumn TKNUM;
        private DevExpress.XtraGrid.Columns.GridColumn TRAID;
        private DevExpress.XtraGrid.Columns.GridColumn WERKS_GI;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridscboPLANT;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn WERKS_GR;
        private DevExpress.XtraGrid.Columns.GridColumn TRANS_DATE;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit gridMdateTime;
        private DevExpress.XtraGrid.Columns.GridColumn ERP_TNUMBER;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}