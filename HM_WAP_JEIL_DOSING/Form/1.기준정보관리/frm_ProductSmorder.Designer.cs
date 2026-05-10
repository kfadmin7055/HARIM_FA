namespace HARIM_FA_DOSING
{
    partial class frm_ProductSmorder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ProductSmorder));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PLANT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboPLANT_CODE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.PARTNER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridScboPartner = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.VEHICLENO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RESOURCE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridscboRESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EMPLOYEE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridScboVehicleNo = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.txtCustomer = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPLANT_CODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScboPartner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboRESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScboVehicleNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.txtCustomer);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2098, 173, 1314, 884);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 4;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1426, 28);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(36, 42);
            this.btn_rowAdd.StyleController = this.layoutControl;
            this.btn_rowAdd.TabIndex = 18;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowDel.Location = new System.Drawing.Point(1466, 28);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(34, 42);
            this.btn_rowDel.StyleController = this.layoutControl;
            this.btn_rowDel.TabIndex = 17;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.AllowFocus = false;
            this.btn_delete.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_delete.Appearance.Options.UseForeColor = true;
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1641, 28);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_delete.Size = new System.Drawing.Size(111, 42);
            this.btn_delete.StyleController = this.layoutControl;
            this.btn_delete.TabIndex = 8;
            this.btn_delete.Text = "삭 제";
            this.btn_delete.ToolTip = "삭제(Ctrl + D)";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gridControl.Location = new System.Drawing.Point(8, 74);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridscboRESOURCE_NO,
            this.gridcboPLANT_CODE,
            this.gridScboPartner,
            this.gridScboVehicleNo});
            this.gridControl.Size = new System.Drawing.Size(1744, 538);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PLANT_CODE,
            this.PARTNER,
            this.VEHICLENO,
            this.RESOURCE_NO,
            this.I_TIME,
            this.EMPLOYEE_NO});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView_ShowingEditor);
            this.gridView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView_InitNewRow);
            this.gridView.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView_FocusedColumnChanged);
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
            // PARTNER
            // 
            this.PARTNER.Caption = "거래처";
            this.PARTNER.ColumnEdit = this.gridScboPartner;
            this.PARTNER.FieldName = "PARTNER";
            this.PARTNER.Name = "PARTNER";
            this.PARTNER.Visible = true;
            this.PARTNER.VisibleIndex = 0;
            this.PARTNER.Width = 300;
            // 
            // gridScboPartner
            // 
            this.gridScboPartner.AutoHeight = false;
            this.gridScboPartner.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridScboPartner.Name = "gridScboPartner";
            this.gridScboPartner.PopupView = this.gridView1;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // VEHICLENO
            // 
            this.VEHICLENO.Caption = "차량번호";
            this.VEHICLENO.FieldName = "VEHICLENO";
            this.VEHICLENO.Name = "VEHICLENO";
            this.VEHICLENO.Visible = true;
            this.VEHICLENO.VisibleIndex = 1;
            this.VEHICLENO.Width = 120;
            // 
            // RESOURCE_NO
            // 
            this.RESOURCE_NO.Caption = "품목";
            this.RESOURCE_NO.ColumnEdit = this.gridscboRESOURCE_NO;
            this.RESOURCE_NO.FieldName = "RESOURCE_NO";
            this.RESOURCE_NO.Name = "RESOURCE_NO";
            this.RESOURCE_NO.Width = 280;
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
            // I_TIME
            // 
            this.I_TIME.Caption = "입력일시";
            this.I_TIME.FieldName = "I_TIME";
            this.I_TIME.Name = "I_TIME";
            // 
            // EMPLOYEE_NO
            // 
            this.EMPLOYEE_NO.Caption = "입력사원";
            this.EMPLOYEE_NO.FieldName = "EMPLOYEE_NO";
            this.EMPLOYEE_NO.Name = "EMPLOYEE_NO";
            // 
            // gridScboVehicleNo
            // 
            this.gridScboVehicleNo.AutoHeight = false;
            this.gridScboVehicleNo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridScboVehicleNo.Name = "gridScboVehicleNo";
            this.gridScboVehicleNo.PopupView = this.gridView2;
            // 
            // gridView2
            // 
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // btn_save
            // 
            this.btn_save.AllowFocus = false;
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1526, 28);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(111, 42);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 7;
            this.btn_save.Text = "저 장";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(214, 29);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(109, 40);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(56, 37);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(153, 24);
            this.txtCustomer.StyleController = this.layoutControl;
            this.txtCustomer.TabIndex = 25;
            this.txtCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtResource_KeyDown);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1760, 620);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.emptySpaceItem2,
            this.layoutControlItem4,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem6});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup.Text = "홍성 차량 관리";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1748, 542);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.Location = new System.Drawing.Point(1518, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(320, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(1098, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_delete;
            this.layoutControlItem4.Location = new System.Drawing.Point(1633, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btn_rowAdd;
            this.layoutControlItem2.Location = new System.Drawing.Point(1418, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_rowDel;
            this.layoutControlItem5.Location = new System.Drawing.Point(1458, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 24, 2, 2);
            this.layoutControlItem5.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_search;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem7.Location = new System.Drawing.Point(205, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem7.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "layoutControlItem3";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem6.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem6.Control = this.txtCustomer;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(205, 46);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(205, 46);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(205, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Text = "거래처";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(36, 15);
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // frm_ProductSmorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_ProductSmorder.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frm_ProductSmorder";
            this.Tag = "frm_ProductSmorder";
            this.Text = "홍성 차량 관리";
            this.Load += new System.EventHandler(this.frm_ProductSmorder_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPLANT_CODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScboPartner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboRESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScboVehicleNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridscboRESOURCE_NO;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn PLANT_CODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboPLANT_CODE;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.TextEdit txtCustomer;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraGrid.Columns.GridColumn PARTNER;
        private DevExpress.XtraGrid.Columns.GridColumn VEHICLENO;
        private DevExpress.XtraGrid.Columns.GridColumn RESOURCE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn I_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn EMPLOYEE_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridScboPartner;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridScboVehicleNo;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    }
}