namespace HARIM_FA_DOSING
{
    partial class frm_BagStock
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_BagStock));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btnStockAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.dtClose = new DevExpress.XtraEditors.DateEdit();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PLANT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboPLANT_CODE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.RESOURCE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridscboRESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.END_YM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridDt = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.END_Q = new DevExpress.XtraGrid.Columns.GridColumn();
            this.I_USER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridScboProcessKey = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView11 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.U_USER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.U_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridCboYn = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.txtResource = new DevExpress.XtraEditors.TextEdit();
            this.btnCopy = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, true);
            this.timerRemark = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtClose.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtClose.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPLANT_CODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboRESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDt.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScboProcessKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCboYn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btnStockAdd);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.dtClose);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.txtResource);
            this.layoutControl.Controls.Add(this.btnCopy);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btnStockAdd
            // 
            this.btnStockAdd.AllowFocus = false;
            this.btnStockAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStockAdd.ImageOptions.Image")));
            this.btnStockAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnStockAdd.ImageOptions.ImageToTextIndent = 10;
            this.btnStockAdd.Location = new System.Drawing.Point(1247, 26);
            this.btnStockAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStockAdd.Name = "btnStockAdd";
            this.btnStockAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStockAdd.Size = new System.Drawing.Size(118, 42);
            this.btnStockAdd.StyleController = this.layoutControl;
            this.btnStockAdd.TabIndex = 7;
            this.btnStockAdd.Text = "재고 추가";
            this.btnStockAdd.ToolTip = "저장(Ctrl + S)";
            this.btnStockAdd.Click += new System.EventHandler(this.btnStockAdd_Click);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(437, 27);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(109, 40);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1528, 26);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(111, 42);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 7;
            this.btn_save.Text = "저 장";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_delete.Appearance.Options.UseForeColor = true;
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1643, 26);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(4);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_delete.Size = new System.Drawing.Size(111, 42);
            this.btn_delete.StyleController = this.layoutControl;
            this.btn_delete.TabIndex = 8;
            this.btn_delete.Text = "삭 제";
            this.btn_delete.ToolTip = "삭제(Ctrl + D)";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // dtClose
            // 
            this.dtClose.EditValue = null;
            this.dtClose.Location = new System.Drawing.Point(87, 35);
            this.dtClose.Name = "dtClose";
            this.dtClose.Properties.AllowFocused = false;
            this.dtClose.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtClose.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtClose.Properties.DisplayFormat.FormatString = "yyyy-MM";
            this.dtClose.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtClose.Properties.EditFormat.FormatString = "yyyy-MM";
            this.dtClose.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtClose.Properties.MaskSettings.Set("mask", "yyyy-MM");
            this.dtClose.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            this.dtClose.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            this.dtClose.Size = new System.Drawing.Size(88, 24);
            this.dtClose.StyleController = this.layoutControl;
            this.dtClose.TabIndex = 4;
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gridControl.Location = new System.Drawing.Point(2, 76);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridscboRESOURCE_NO,
            this.gridcboPLANT_CODE,
            this.gridScboProcessKey,
            this.gridSpin,
            this.gridCboYn,
            this.gridDt});
            this.gridControl.Size = new System.Drawing.Size(1756, 542);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PLANT_CODE,
            this.RESOURCE_NO,
            this.END_YM,
            this.END_Q,
            this.I_USER,
            this.I_TIME,
            this.U_USER,
            this.U_TIME});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowFooter = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // PLANT_CODE
            // 
            this.PLANT_CODE.Caption = "플랜트";
            this.PLANT_CODE.ColumnEdit = this.gridcboPLANT_CODE;
            this.PLANT_CODE.FieldName = "PLANT_CODE";
            this.PLANT_CODE.Name = "PLANT_CODE";
            this.PLANT_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.PLANT_CODE.Width = 160;
            // 
            // gridcboPLANT_CODE
            // 
            this.gridcboPLANT_CODE.AutoHeight = false;
            this.gridcboPLANT_CODE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboPLANT_CODE.Name = "gridcboPLANT_CODE";
            // 
            // RESOURCE_NO
            // 
            this.RESOURCE_NO.Caption = "품목";
            this.RESOURCE_NO.ColumnEdit = this.gridscboRESOURCE_NO;
            this.RESOURCE_NO.FieldName = "RESOURCE_NO";
            this.RESOURCE_NO.Name = "RESOURCE_NO";
            this.RESOURCE_NO.Visible = true;
            this.RESOURCE_NO.VisibleIndex = 0;
            this.RESOURCE_NO.Width = 320;
            // 
            // gridscboRESOURCE_NO
            // 
            this.gridscboRESOURCE_NO.AutoHeight = false;
            this.gridscboRESOURCE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridscboRESOURCE_NO.Name = "gridscboRESOURCE_NO";
            this.gridscboRESOURCE_NO.PopupView = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // END_YM
            // 
            this.END_YM.Caption = "마감연월";
            this.END_YM.ColumnEdit = this.gridDt;
            this.END_YM.FieldName = "END_YM";
            this.END_YM.Name = "END_YM";
            this.END_YM.Visible = true;
            this.END_YM.VisibleIndex = 1;
            this.END_YM.Width = 120;
            // 
            // gridDt
            // 
            this.gridDt.AutoHeight = false;
            this.gridDt.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridDt.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.False;
            this.gridDt.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridDt.DisplayFormat.FormatString = "yyyy-MM";
            this.gridDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridDt.EditFormat.FormatString = "yyyy-MM";
            this.gridDt.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridDt.MaskSettings.Set("mask", "yyyy-MM");
            this.gridDt.Name = "gridDt";
            this.gridDt.ShowToday = false;
            this.gridDt.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            this.gridDt.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            // 
            // END_Q
            // 
            this.END_Q.Caption = "마감수량";
            this.END_Q.DisplayFormat.FormatString = "{0:n3}";
            this.END_Q.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.END_Q.FieldName = "END_Q";
            this.END_Q.Name = "END_Q";
            this.END_Q.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CLOSE_QTY", "{0:n3}")});
            this.END_Q.Visible = true;
            this.END_Q.VisibleIndex = 2;
            this.END_Q.Width = 150;
            // 
            // I_USER
            // 
            this.I_USER.Caption = "입력사원";
            this.I_USER.ColumnEdit = this.gridScboProcessKey;
            this.I_USER.FieldName = "I_USER";
            this.I_USER.Name = "I_USER";
            this.I_USER.Width = 200;
            // 
            // gridScboProcessKey
            // 
            this.gridScboProcessKey.AutoHeight = false;
            this.gridScboProcessKey.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridScboProcessKey.Name = "gridScboProcessKey";
            this.gridScboProcessKey.PopupView = this.gridView11;
            // 
            // gridView11
            // 
            this.gridView11.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView11.Name = "gridView11";
            this.gridView11.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView11.OptionsView.ShowGroupPanel = false;
            // 
            // I_TIME
            // 
            this.I_TIME.Caption = "입력일시";
            this.I_TIME.FieldName = "I_TIME";
            this.I_TIME.Name = "I_TIME";
            // 
            // U_USER
            // 
            this.U_USER.Caption = "수정사원";
            this.U_USER.FieldName = "U_USER";
            this.U_USER.Name = "U_USER";
            // 
            // U_TIME
            // 
            this.U_TIME.Caption = "수정일시";
            this.U_TIME.FieldName = "U_TIME";
            this.U_TIME.Name = "U_TIME";
            // 
            // gridSpin
            // 
            this.gridSpin.AutoHeight = false;
            this.gridSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridSpin.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gridSpin.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.gridSpin.MinValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gridSpin.Name = "gridSpin";
            // 
            // gridCboYn
            // 
            this.gridCboYn.AutoHeight = false;
            this.gridCboYn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridCboYn.Name = "gridCboYn";
            // 
            // txtResource
            // 
            this.txtResource.Location = new System.Drawing.Point(260, 35);
            this.txtResource.Name = "txtResource";
            this.txtResource.Size = new System.Drawing.Size(172, 24);
            this.txtResource.StyleController = this.layoutControl;
            this.txtResource.TabIndex = 25;
            // 
            // btnCopy
            // 
            this.btnCopy.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCopy.ImageOptions.ImageToTextIndent = 10;
            this.btnCopy.Location = new System.Drawing.Point(1369, 26);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCopy.Size = new System.Drawing.Size(145, 42);
            this.btnCopy.StyleController = this.layoutControl;
            this.btnCopy.TabIndex = 26;
            this.btnCopy.Text = "기말재고 생성";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1760, 620);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.emptySpaceItem1,
            this.layoutControlItem4,
            this.layoutControlItem6,
            this.layoutControlItem3,
            this.emptySpaceItem2,
            this.layoutControlItem10,
            this.layoutControlItem2,
            this.layoutControlItem7});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup.Size = new System.Drawing.Size(1760, 74);
            this.layoutControlGroup.Text = "타이콘 재고 관리";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_search;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem5.Location = new System.Drawing.Point(430, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem5.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "layoutControlItem3";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(545, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(696, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_save;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem4.Location = new System.Drawing.Point(1522, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Text = "layoutControlItem3";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btn_delete;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem6.Location = new System.Drawing.Point(1637, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Text = "layoutControlItem4";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnStockAdd;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(1241, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(122, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(122, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(122, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(1512, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(10, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem10.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem10.Control = this.dtClose;
            this.layoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem10.CustomizationFormText = "기준연월";
            this.layoutControlItem10.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem10.ImageOptions.Image")));
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem10.MaxSize = new System.Drawing.Size(173, 46);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(173, 46);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(173, 46);
            this.layoutControlItem10.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem10.Text = "기준연월";
            this.layoutControlItem10.TextSize = new System.Drawing.Size(69, 16);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.txtResource;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "품   목";
            this.layoutControlItem2.Location = new System.Drawing.Point(173, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(257, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(257, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(257, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "품   목";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(69, 15);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnCopy;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(1363, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(149, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(149, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(149, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 74);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1760, 546);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // timerRemark
            // 
            this.timerRemark.Interval = 500;
            // 
            // frm_BagStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_BagStock.IconOptions.Image")));
            this.Name = "frm_BagStock";
            this.Text = "타이콘 재고 관리";
            this.Load += new System.EventHandler(this.frm_BagStock_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtClose.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtClose.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPLANT_CODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboRESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDt.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScboProcessKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCboYn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private System.Windows.Forms.Timer timerRemark;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btnStockAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.DateEdit dtClose;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn PLANT_CODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboPLANT_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn RESOURCE_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridscboRESOURCE_NO;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn END_YM;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit gridDt;
        private DevExpress.XtraGrid.Columns.GridColumn END_Q;
        private DevExpress.XtraGrid.Columns.GridColumn I_USER;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridScboProcessKey;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView11;
        private DevExpress.XtraGrid.Columns.GridColumn I_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn U_USER;
        private DevExpress.XtraGrid.Columns.GridColumn U_TIME;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit gridSpin;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridCboYn;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.TextEdit txtResource;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton btnCopy;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}