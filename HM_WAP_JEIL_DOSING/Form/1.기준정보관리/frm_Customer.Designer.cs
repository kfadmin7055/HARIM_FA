namespace HARIM_FA_DOSING
{
    partial class frm_Customer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Customer));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.scboCustomer = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PARTNER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NAME_ORG1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RLTYP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TEL_NUMBER_1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MOD_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.J_1KFREPRE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ZZREPBPNM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CRDAT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CRTIM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CHDAT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CHTIM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.XDELE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scboCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.scboCustomer);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3229, 548, 650, 400);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // scboCustomer
            // 
            this.scboCustomer.Location = new System.Drawing.Point(47, 35);
            this.scboCustomer.Name = "scboCustomer";
            this.scboCustomer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.scboCustomer.Properties.PopupView = this.searchLookUpEdit1View;
            this.scboCustomer.Size = new System.Drawing.Size(227, 24);
            this.scboCustomer.StyleController = this.layoutControl;
            this.scboCustomer.TabIndex = 19;
            this.scboCustomer.EditValueChanged += new System.EventHandler(this.scboCustomer_EditValueChanged);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gridControl.Location = new System.Drawing.Point(5, 71);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(1750, 544);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PARTNER,
            this.NAME_ORG1,
            this.RLTYP,
            this.TEL_NUMBER_1,
            this.MOD_NUMBER,
            this.J_1KFREPRE,
            this.ZZREPBPNM,
            this.CRDAT,
            this.CRTIM,
            this.CHDAT,
            this.CHTIM,
            this.XDELE,
            this.I_TIME});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.ViewCaption = "거래처정보관리 (ERP)";
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ColumnPositionChanged += new System.EventHandler(this.gridView_ColumnPositionChanged);
            // 
            // PARTNER
            // 
            this.PARTNER.Caption = "거래처코드";
            this.PARTNER.FieldName = "PARTNER";
            this.PARTNER.MinWidth = 100;
            this.PARTNER.Name = "PARTNER";
            this.PARTNER.Visible = true;
            this.PARTNER.VisibleIndex = 0;
            this.PARTNER.Width = 100;
            // 
            // NAME_ORG1
            // 
            this.NAME_ORG1.Caption = "거래처명";
            this.NAME_ORG1.FieldName = "NAME_ORG1";
            this.NAME_ORG1.MinWidth = 140;
            this.NAME_ORG1.Name = "NAME_ORG1";
            this.NAME_ORG1.Visible = true;
            this.NAME_ORG1.VisibleIndex = 1;
            this.NAME_ORG1.Width = 140;
            // 
            // RLTYP
            // 
            this.RLTYP.Caption = "주     소";
            this.RLTYP.FieldName = "RLTYP";
            this.RLTYP.MinWidth = 80;
            this.RLTYP.Name = "RLTYP";
            this.RLTYP.Visible = true;
            this.RLTYP.VisibleIndex = 2;
            this.RLTYP.Width = 80;
            // 
            // TEL_NUMBER_1
            // 
            this.TEL_NUMBER_1.Caption = "통신전화";
            this.TEL_NUMBER_1.FieldName = "TEL_NUMBER_1";
            this.TEL_NUMBER_1.MinWidth = 80;
            this.TEL_NUMBER_1.Name = "TEL_NUMBER_1";
            this.TEL_NUMBER_1.Visible = true;
            this.TEL_NUMBER_1.VisibleIndex = 3;
            this.TEL_NUMBER_1.Width = 80;
            // 
            // MOD_NUMBER
            // 
            this.MOD_NUMBER.Caption = "휴대폰";
            this.MOD_NUMBER.FieldName = "MOD_NUMBER";
            this.MOD_NUMBER.MinWidth = 80;
            this.MOD_NUMBER.Name = "MOD_NUMBER";
            this.MOD_NUMBER.Visible = true;
            this.MOD_NUMBER.VisibleIndex = 4;
            this.MOD_NUMBER.Width = 80;
            // 
            // J_1KFREPRE
            // 
            this.J_1KFREPRE.Caption = "대표이름 담당자";
            this.J_1KFREPRE.FieldName = "J_1KFREPRE";
            this.J_1KFREPRE.MinWidth = 80;
            this.J_1KFREPRE.Name = "J_1KFREPRE";
            this.J_1KFREPRE.Visible = true;
            this.J_1KFREPRE.VisibleIndex = 5;
            this.J_1KFREPRE.Width = 100;
            // 
            // ZZREPBPNM
            // 
            this.ZZREPBPNM.Caption = "대표거래처명";
            this.ZZREPBPNM.FieldName = "ZZREPBPNM";
            this.ZZREPBPNM.MinWidth = 80;
            this.ZZREPBPNM.Name = "ZZREPBPNM";
            this.ZZREPBPNM.Visible = true;
            this.ZZREPBPNM.VisibleIndex = 6;
            this.ZZREPBPNM.Width = 80;
            // 
            // CRDAT
            // 
            this.CRDAT.Caption = "생성일";
            this.CRDAT.FieldName = "CRDAT";
            this.CRDAT.MinWidth = 80;
            this.CRDAT.Name = "CRDAT";
            this.CRDAT.Visible = true;
            this.CRDAT.VisibleIndex = 7;
            this.CRDAT.Width = 80;
            // 
            // CRTIM
            // 
            this.CRTIM.Caption = "생성시간";
            this.CRTIM.FieldName = "CRTIM";
            this.CRTIM.MinWidth = 80;
            this.CRTIM.Name = "CRTIM";
            this.CRTIM.Visible = true;
            this.CRTIM.VisibleIndex = 8;
            this.CRTIM.Width = 80;
            // 
            // CHDAT
            // 
            this.CHDAT.Caption = "변경일";
            this.CHDAT.FieldName = "CHDAT";
            this.CHDAT.MinWidth = 80;
            this.CHDAT.Name = "CHDAT";
            this.CHDAT.Visible = true;
            this.CHDAT.VisibleIndex = 9;
            this.CHDAT.Width = 80;
            // 
            // CHTIM
            // 
            this.CHTIM.Caption = "변경시간";
            this.CHTIM.FieldName = "CHTIM";
            this.CHTIM.MinWidth = 80;
            this.CHTIM.Name = "CHTIM";
            this.CHTIM.Visible = true;
            this.CHTIM.VisibleIndex = 10;
            this.CHTIM.Width = 80;
            // 
            // XDELE
            // 
            this.XDELE.Caption = "거래상태";
            this.XDELE.FieldName = "XDELE";
            this.XDELE.MinWidth = 80;
            this.XDELE.Name = "XDELE";
            this.XDELE.Visible = true;
            this.XDELE.VisibleIndex = 11;
            this.XDELE.Width = 80;
            // 
            // I_TIME
            // 
            this.I_TIME.Caption = "등록일자";
            this.I_TIME.FieldName = "I_TIME";
            this.I_TIME.MinWidth = 80;
            this.I_TIME.Name = "I_TIME";
            this.I_TIME.Visible = true;
            this.I_TIME.VisibleIndex = 12;
            this.I_TIME.Width = 80;
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1428, 26);
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
            this.btn_rowDel.Location = new System.Drawing.Point(1468, 26);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(34, 42);
            this.btn_rowDel.StyleController = this.layoutControl;
            this.btn_rowDel.TabIndex = 17;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_save
            // 
            this.btn_save.AllowFocus = false;
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1528, 26);
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
            // btn_delete
            // 
            this.btn_delete.AllowFocus = false;
            this.btn_delete.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_delete.Appearance.Options.UseForeColor = true;
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1643, 26);
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
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(278, 26);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(117, 42);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 27;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1760, 620);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup1.CaptionImageOptions.Image")));
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem2,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem7,
            this.layoutControlItem15});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup1.Text = "거래처정보관리 (ERP)";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlItem1.Size = new System.Drawing.Size(1752, 546);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(393, 0);
            this.emptySpaceItem2.MinSize = new System.Drawing.Size(80, 21);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(1029, 46);
            this.emptySpaceItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btn_rowAdd;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(1422, 0);
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
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(1462, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 24, 2, 2);
            this.layoutControlItem5.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(1522, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_delete;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(1637, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem7.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem7.Control = this.scboCustomer;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(272, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(272, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(272, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "거래처";
            this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem7.TextToControlDistance = 5;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btn_search;
            this.layoutControlItem15.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem15.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem15.Location = new System.Drawing.Point(272, 0);
            this.layoutControlItem15.MaxSize = new System.Drawing.Size(121, 46);
            this.layoutControlItem15.MinSize = new System.Drawing.Size(121, 46);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(121, 46);
            this.layoutControlItem15.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem15.Text = "layoutControlItem5";
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // frm_Customer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_Customer.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frm_Customer";
            this.Tag = "frm_Customer";
            this.Text = "거래처정보관리";
            this.Load += new System.EventHandler(this.frm_Customer_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scboCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraGrid.Columns.GridColumn PARTNER;
        private DevExpress.XtraGrid.Columns.GridColumn NAME_ORG1;
        private DevExpress.XtraGrid.Columns.GridColumn RLTYP;
        private DevExpress.XtraGrid.Columns.GridColumn TEL_NUMBER_1;
        private DevExpress.XtraGrid.Columns.GridColumn MOD_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn J_1KFREPRE;
        private DevExpress.XtraGrid.Columns.GridColumn ZZREPBPNM;
        private DevExpress.XtraGrid.Columns.GridColumn CRDAT;
        private DevExpress.XtraGrid.Columns.GridColumn CRTIM;
        private DevExpress.XtraGrid.Columns.GridColumn CHDAT;
        private DevExpress.XtraGrid.Columns.GridColumn CHTIM;
        private DevExpress.XtraGrid.Columns.GridColumn XDELE;
        private DevExpress.XtraGrid.Columns.GridColumn I_TIME;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraEditors.SearchLookUpEdit scboCustomer;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
    }
}