namespace HARIM_FA_DOSING
{
    partial class frm_SCR_MG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_SCR_MG));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.cboMenu = new DevExpress.XtraEditors.LookUpEdit();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PROGRAM_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboProgram = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.MENU_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboMenu = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.SCR_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SCR_NM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PROGRAM_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FORM_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PROCESS_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboProcess = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.USE_YN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboCHK = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.DISPLAY_SEQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.txtSCR_NM = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem22 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboMenu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboProgram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboCHK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSCR_NM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.cboMenu);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.txtSCR_NM);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(1, 1);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1758, 618);
            this.layoutControl.TabIndex = 2;
            this.layoutControl.Text = "layoutControl1";
            // 
            // cboMenu
            // 
            this.cboMenu.Location = new System.Drawing.Point(37, 37);
            this.cboMenu.Name = "cboMenu";
            this.cboMenu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboMenu.Size = new System.Drawing.Size(206, 24);
            this.cboMenu.StyleController = this.layoutControl;
            this.cboMenu.TabIndex = 23;
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gridControl.Location = new System.Drawing.Point(8, 74);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridcboProgram,
            this.gridcboMenu,
            this.gridcboProcess,
            this.gridcboCHK});
            this.gridControl.Size = new System.Drawing.Size(1742, 536);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PROGRAM_ID,
            this.MENU_ID,
            this.SCR_ID,
            this.SCR_NM,
            this.PROGRAM_DESC,
            this.FORM_NAME,
            this.PROCESS_KEY,
            this.USE_YN,
            this.DISPLAY_SEQ});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.AllowCellMerge = true;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.EnableAppearanceOddRow = true;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ColumnPositionChanged += new System.EventHandler(this.gridView_ColumnPositionChanged);
            // 
            // PROGRAM_ID
            // 
            this.PROGRAM_ID.Caption = "프로그램";
            this.PROGRAM_ID.ColumnEdit = this.gridcboProgram;
            this.PROGRAM_ID.FieldName = "PROGRAM_ID";
            this.PROGRAM_ID.MinWidth = 120;
            this.PROGRAM_ID.Name = "PROGRAM_ID";
            this.PROGRAM_ID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.PROGRAM_ID.Visible = true;
            this.PROGRAM_ID.VisibleIndex = 0;
            this.PROGRAM_ID.Width = 120;
            // 
            // gridcboProgram
            // 
            this.gridcboProgram.AutoHeight = false;
            this.gridcboProgram.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboProgram.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "프로그램ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "프로그램명")});
            this.gridcboProgram.Name = "gridcboProgram";
            this.gridcboProgram.NullText = "";
            // 
            // MENU_ID
            // 
            this.MENU_ID.Caption = "메뉴";
            this.MENU_ID.ColumnEdit = this.gridcboMenu;
            this.MENU_ID.FieldName = "MENU_ID";
            this.MENU_ID.MinWidth = 160;
            this.MENU_ID.Name = "MENU_ID";
            this.MENU_ID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.MENU_ID.Visible = true;
            this.MENU_ID.VisibleIndex = 1;
            this.MENU_ID.Width = 160;
            // 
            // gridcboMenu
            // 
            this.gridcboMenu.AutoHeight = false;
            this.gridcboMenu.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboMenu.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "메뉴ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "메뉴명"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("REF_1", "폼명")});
            this.gridcboMenu.Name = "gridcboMenu";
            this.gridcboMenu.NullText = "";
            // 
            // SCR_ID
            // 
            this.SCR_ID.Caption = "화면ID";
            this.SCR_ID.FieldName = "SCR_ID";
            this.SCR_ID.MinWidth = 80;
            this.SCR_ID.Name = "SCR_ID";
            this.SCR_ID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.SCR_ID.Visible = true;
            this.SCR_ID.VisibleIndex = 2;
            this.SCR_ID.Width = 80;
            // 
            // SCR_NM
            // 
            this.SCR_NM.Caption = "화면명";
            this.SCR_NM.FieldName = "SCR_NM";
            this.SCR_NM.MinWidth = 180;
            this.SCR_NM.Name = "SCR_NM";
            this.SCR_NM.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.SCR_NM.Visible = true;
            this.SCR_NM.VisibleIndex = 3;
            this.SCR_NM.Width = 180;
            // 
            // PROGRAM_DESC
            // 
            this.PROGRAM_DESC.Caption = "화면설명";
            this.PROGRAM_DESC.FieldName = "PROGRAM_DESC";
            this.PROGRAM_DESC.MinWidth = 200;
            this.PROGRAM_DESC.Name = "PROGRAM_DESC";
            this.PROGRAM_DESC.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.PROGRAM_DESC.Visible = true;
            this.PROGRAM_DESC.VisibleIndex = 4;
            this.PROGRAM_DESC.Width = 200;
            // 
            // FORM_NAME
            // 
            this.FORM_NAME.Caption = "폼명";
            this.FORM_NAME.FieldName = "FORM_NAME";
            this.FORM_NAME.MinWidth = 140;
            this.FORM_NAME.Name = "FORM_NAME";
            this.FORM_NAME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.FORM_NAME.Visible = true;
            this.FORM_NAME.VisibleIndex = 5;
            this.FORM_NAME.Width = 140;
            // 
            // PROCESS_KEY
            // 
            this.PROCESS_KEY.Caption = "공정";
            this.PROCESS_KEY.ColumnEdit = this.gridcboProcess;
            this.PROCESS_KEY.FieldName = "PROCESS_KEY";
            this.PROCESS_KEY.MinWidth = 100;
            this.PROCESS_KEY.Name = "PROCESS_KEY";
            this.PROCESS_KEY.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.PROCESS_KEY.Visible = true;
            this.PROCESS_KEY.VisibleIndex = 6;
            this.PROCESS_KEY.Width = 100;
            // 
            // gridcboProcess
            // 
            this.gridcboProcess.AutoHeight = false;
            this.gridcboProcess.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboProcess.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "공정코드"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "공정명")});
            this.gridcboProcess.Name = "gridcboProcess";
            this.gridcboProcess.NullText = "";
            // 
            // USE_YN
            // 
            this.USE_YN.Caption = "사용여부";
            this.USE_YN.ColumnEdit = this.gridcboCHK;
            this.USE_YN.FieldName = "USE_YN";
            this.USE_YN.MinWidth = 60;
            this.USE_YN.Name = "USE_YN";
            this.USE_YN.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.USE_YN.Visible = true;
            this.USE_YN.VisibleIndex = 7;
            // 
            // gridcboCHK
            // 
            this.gridcboCHK.AutoHeight = false;
            this.gridcboCHK.Name = "gridcboCHK";
            // 
            // DISPLAY_SEQ
            // 
            this.DISPLAY_SEQ.Caption = "화면표시순서";
            this.DISPLAY_SEQ.FieldName = "DISPLAY_SEQ";
            this.DISPLAY_SEQ.MinWidth = 60;
            this.DISPLAY_SEQ.Name = "DISPLAY_SEQ";
            this.DISPLAY_SEQ.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.DISPLAY_SEQ.Visible = true;
            this.DISPLAY_SEQ.VisibleIndex = 8;
            this.DISPLAY_SEQ.Width = 85;
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(485, 28);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1425, 29);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(34, 40);
            this.btn_rowAdd.StyleController = this.layoutControl;
            this.btn_rowAdd.TabIndex = 21;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowDel.Location = new System.Drawing.Point(1465, 29);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(34, 40);
            this.btn_rowDel.StyleController = this.layoutControl;
            this.btn_rowDel.TabIndex = 20;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1524, 28);
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
            this.btn_delete.Location = new System.Drawing.Point(1639, 28);
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
            // txtSCR_NM
            // 
            this.txtSCR_NM.Location = new System.Drawing.Point(308, 37);
            this.txtSCR_NM.Name = "txtSCR_NM";
            this.txtSCR_NM.Size = new System.Drawing.Size(173, 24);
            this.txtSCR_NM.StyleController = this.layoutControl;
            this.txtSCR_NM.TabIndex = 22;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1758, 618);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem8,
            this.layoutControlItem14,
            this.layoutControlItem7,
            this.layoutControlItem3,
            this.layoutControlItem22,
            this.layoutControlItem4,
            this.layoutControlItem2});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(1758, 618);
            this.layoutControlGroup.Text = "화면관리";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1746, 540);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(592, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(824, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_search;
            this.layoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem8.Location = new System.Drawing.Point(477, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "layoutControlItem3";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btn_rowAdd;
            this.layoutControlItem14.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem14.CustomizationFormText = "layoutControlItem11";
            this.layoutControlItem14.Location = new System.Drawing.Point(1416, 0);
            this.layoutControlItem14.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem14.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.Text = "layoutControlItem11";
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_rowDel;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem10";
            this.layoutControlItem7.Location = new System.Drawing.Point(1456, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 23, 3, 3);
            this.layoutControlItem7.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "layoutControlItem10";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(1516, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem22
            // 
            this.layoutControlItem22.Control = this.btn_delete;
            this.layoutControlItem22.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem22.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem22.Location = new System.Drawing.Point(1631, 0);
            this.layoutControlItem22.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem22.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem22.Name = "layoutControlItem22";
            this.layoutControlItem22.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem22.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem22.Text = "layoutControlItem4";
            this.layoutControlItem22.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem22.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.txtSCR_NM;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "운전자명";
            this.layoutControlItem4.Location = new System.Drawing.Point(239, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(238, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(238, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(238, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem4.Text = "화면명";
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem4.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.cboMenu;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(239, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(239, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(239, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "메뉴";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // frm_SCR_MG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_SCR_MG.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frm_SCR_MG";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "화면관리";
            this.Load += new System.EventHandler(this.frm_SCR_MG_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboMenu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboProgram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboCHK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSCR_NM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem22;
        private DevExpress.XtraEditors.TextEdit txtSCR_NM;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraGrid.Columns.GridColumn PROGRAM_ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboProgram;
        private DevExpress.XtraGrid.Columns.GridColumn MENU_ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboMenu;
        private DevExpress.XtraGrid.Columns.GridColumn SCR_ID;
        private DevExpress.XtraGrid.Columns.GridColumn SCR_NM;
        private DevExpress.XtraGrid.Columns.GridColumn PROGRAM_DESC;
        private DevExpress.XtraGrid.Columns.GridColumn FORM_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn USE_YN;
        private DevExpress.XtraGrid.Columns.GridColumn DISPLAY_SEQ;
        private DevExpress.XtraEditors.LookUpEdit cboMenu;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn PROCESS_KEY;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboProcess;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridcboCHK;
    }
}