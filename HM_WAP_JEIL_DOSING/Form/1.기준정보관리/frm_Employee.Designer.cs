namespace HARIM_FA_DOSING
{
    partial class frm_Employee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Employee));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_passChange = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_PLANT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboPlant_Code = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn_EMPLOYEE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_EMPLOYEE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_PASSWORD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_JUSO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_TEL_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_MANAGE_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repItemLkUpEdit_MANAGE_TYPE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn_I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repItemLkUEdit_EMPLOYEE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.cboPlant_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.btnPWReset = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem60 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPlant_Code)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_MANAGE_TYPE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUEdit_EMPLOYEE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem60)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_passChange);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.cboPlant_Code);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.btnPWReset);
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
            // btn_passChange
            // 
            this.btn_passChange.AllowFocus = false;
            this.btn_passChange.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_passChange.ImageOptions.Image")));
            this.btn_passChange.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_passChange.ImageOptions.ImageToTextIndent = 10;
            this.btn_passChange.Location = new System.Drawing.Point(383, 28);
            this.btn_passChange.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_passChange.Name = "btn_passChange";
            this.btn_passChange.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_passChange.Size = new System.Drawing.Size(125, 42);
            this.btn_passChange.StyleController = this.layoutControl;
            this.btn_passChange.TabIndex = 19;
            this.btn_passChange.Text = "비밀번호 변경";
            this.btn_passChange.Click += new System.EventHandler(this.btn_passChange_Click);
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
            this.repItemLkUpEdit_MANAGE_TYPE,
            this.repItemLkUEdit_EMPLOYEE_NO,
            this.gridcboPlant_Code});
            this.gridControl.Size = new System.Drawing.Size(1744, 538);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_PLANT_CODE,
            this.gridColumn_EMPLOYEE_NO,
            this.gridColumn_EMPLOYEE_NAME,
            this.gridColumn_PASSWORD,
            this.gridColumn_JUSO,
            this.gridColumn_TEL_NO,
            this.gridColumn_MANAGE_TYPE,
            this.gridColumn_I_TIME});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.AllowCellMerge = true;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView_ShowingEditor);
            this.gridView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView_InitNewRow);
            this.gridView.ColumnPositionChanged += new System.EventHandler(this.gridView_ColumnPositionChanged);
            this.gridView.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView_FocusedColumnChanged);
            // 
            // gridColumn_PLANT_CODE
            // 
            this.gridColumn_PLANT_CODE.Caption = "플랜트";
            this.gridColumn_PLANT_CODE.ColumnEdit = this.gridcboPlant_Code;
            this.gridColumn_PLANT_CODE.FieldName = "PLANT_CODE";
            this.gridColumn_PLANT_CODE.Name = "gridColumn_PLANT_CODE";
            this.gridColumn_PLANT_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_PLANT_CODE.Visible = true;
            this.gridColumn_PLANT_CODE.VisibleIndex = 0;
            this.gridColumn_PLANT_CODE.Width = 160;
            // 
            // gridcboPlant_Code
            // 
            this.gridcboPlant_Code.AutoHeight = false;
            this.gridcboPlant_Code.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboPlant_Code.Name = "gridcboPlant_Code";
            // 
            // gridColumn_EMPLOYEE_NO
            // 
            this.gridColumn_EMPLOYEE_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EMPLOYEE_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EMPLOYEE_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_EMPLOYEE_NO.Caption = "사원코드";
            this.gridColumn_EMPLOYEE_NO.FieldName = "EMPLOYEE_NO";
            this.gridColumn_EMPLOYEE_NO.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("gridColumn_EMPLOYEE_NO.ImageOptions.Image")));
            this.gridColumn_EMPLOYEE_NO.MinWidth = 16;
            this.gridColumn_EMPLOYEE_NO.Name = "gridColumn_EMPLOYEE_NO";
            this.gridColumn_EMPLOYEE_NO.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_EMPLOYEE_NO.Visible = true;
            this.gridColumn_EMPLOYEE_NO.VisibleIndex = 1;
            this.gridColumn_EMPLOYEE_NO.Width = 172;
            // 
            // gridColumn_EMPLOYEE_NAME
            // 
            this.gridColumn_EMPLOYEE_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EMPLOYEE_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EMPLOYEE_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_EMPLOYEE_NAME.Caption = "사원명";
            this.gridColumn_EMPLOYEE_NAME.FieldName = "EMPLOYEE_NAME";
            this.gridColumn_EMPLOYEE_NAME.Name = "gridColumn_EMPLOYEE_NAME";
            this.gridColumn_EMPLOYEE_NAME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_EMPLOYEE_NAME.Visible = true;
            this.gridColumn_EMPLOYEE_NAME.VisibleIndex = 2;
            this.gridColumn_EMPLOYEE_NAME.Width = 177;
            // 
            // gridColumn_PASSWORD
            // 
            this.gridColumn_PASSWORD.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_PASSWORD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_PASSWORD.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_PASSWORD.Caption = "비밀번호";
            this.gridColumn_PASSWORD.FieldName = "PASSWORD";
            this.gridColumn_PASSWORD.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("gridColumn_PASSWORD.ImageOptions.Image")));
            this.gridColumn_PASSWORD.Name = "gridColumn_PASSWORD";
            this.gridColumn_PASSWORD.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_PASSWORD.Visible = true;
            this.gridColumn_PASSWORD.VisibleIndex = 3;
            this.gridColumn_PASSWORD.Width = 151;
            // 
            // gridColumn_JUSO
            // 
            this.gridColumn_JUSO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_JUSO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_JUSO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_JUSO.Caption = "주소";
            this.gridColumn_JUSO.FieldName = "JUSO";
            this.gridColumn_JUSO.MinWidth = 16;
            this.gridColumn_JUSO.Name = "gridColumn_JUSO";
            this.gridColumn_JUSO.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_JUSO.Visible = true;
            this.gridColumn_JUSO.VisibleIndex = 4;
            this.gridColumn_JUSO.Width = 199;
            // 
            // gridColumn_TEL_NO
            // 
            this.gridColumn_TEL_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_TEL_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_TEL_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_TEL_NO.Caption = "연락처";
            this.gridColumn_TEL_NO.FieldName = "TEL_NO";
            this.gridColumn_TEL_NO.MinWidth = 16;
            this.gridColumn_TEL_NO.Name = "gridColumn_TEL_NO";
            this.gridColumn_TEL_NO.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_TEL_NO.Visible = true;
            this.gridColumn_TEL_NO.VisibleIndex = 5;
            this.gridColumn_TEL_NO.Width = 122;
            // 
            // gridColumn_MANAGE_TYPE
            // 
            this.gridColumn_MANAGE_TYPE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_MANAGE_TYPE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_MANAGE_TYPE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_MANAGE_TYPE.Caption = "관리타입";
            this.gridColumn_MANAGE_TYPE.ColumnEdit = this.repItemLkUpEdit_MANAGE_TYPE;
            this.gridColumn_MANAGE_TYPE.FieldName = "MANAGE_TYPE";
            this.gridColumn_MANAGE_TYPE.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("gridColumn_MANAGE_TYPE.ImageOptions.Image")));
            this.gridColumn_MANAGE_TYPE.MinWidth = 16;
            this.gridColumn_MANAGE_TYPE.Name = "gridColumn_MANAGE_TYPE";
            this.gridColumn_MANAGE_TYPE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_MANAGE_TYPE.Visible = true;
            this.gridColumn_MANAGE_TYPE.VisibleIndex = 6;
            this.gridColumn_MANAGE_TYPE.Width = 130;
            // 
            // repItemLkUpEdit_MANAGE_TYPE
            // 
            this.repItemLkUpEdit_MANAGE_TYPE.AutoHeight = false;
            this.repItemLkUpEdit_MANAGE_TYPE.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            this.repItemLkUpEdit_MANAGE_TYPE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUpEdit_MANAGE_TYPE.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "코드", 16, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "코드명", 16, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repItemLkUpEdit_MANAGE_TYPE.Name = "repItemLkUpEdit_MANAGE_TYPE";
            this.repItemLkUpEdit_MANAGE_TYPE.NullText = "";
            // 
            // gridColumn_I_TIME
            // 
            this.gridColumn_I_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_I_TIME.Caption = "등록일자";
            this.gridColumn_I_TIME.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn_I_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_I_TIME.FieldName = "I_TIME";
            this.gridColumn_I_TIME.MinWidth = 16;
            this.gridColumn_I_TIME.Name = "gridColumn_I_TIME";
            this.gridColumn_I_TIME.OptionsColumn.AllowEdit = false;
            this.gridColumn_I_TIME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_I_TIME.OptionsColumn.ReadOnly = true;
            this.gridColumn_I_TIME.OptionsColumn.TabStop = false;
            this.gridColumn_I_TIME.Visible = true;
            this.gridColumn_I_TIME.VisibleIndex = 7;
            this.gridColumn_I_TIME.Width = 172;
            // 
            // repItemLkUEdit_EMPLOYEE_NO
            // 
            this.repItemLkUEdit_EMPLOYEE_NO.AutoHeight = false;
            this.repItemLkUEdit_EMPLOYEE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUEdit_EMPLOYEE_NO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "사원코드", 16, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "사원명", 16, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repItemLkUEdit_EMPLOYEE_NO.Name = "repItemLkUEdit_EMPLOYEE_NO";
            this.repItemLkUEdit_EMPLOYEE_NO.NullText = "";
            this.repItemLkUEdit_EMPLOYEE_NO.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.repItemLkUpEdit_EMPLOYEE_NO_EditValueChanging);
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
            // cboPlant_Code
            // 
            this.cboPlant_Code.Location = new System.Drawing.Point(63, 36);
            this.cboPlant_Code.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboPlant_Code.Name = "cboPlant_Code";
            this.cboPlant_Code.Properties.AllowFocused = false;
            this.cboPlant_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlant_Code.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "코드", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "진행상태")});
            this.cboPlant_Code.Properties.NullText = "";
            this.cboPlant_Code.Properties.PopupSizeable = false;
            this.cboPlant_Code.Size = new System.Drawing.Size(200, 24);
            this.cboPlant_Code.StyleController = this.layoutControl;
            this.cboPlant_Code.TabIndex = 5;
            this.cboPlant_Code.EditValueChanged += new System.EventHandler(this.cboPlant_Code_EditValueChanged);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(269, 29);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(109, 40);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btnPWReset
            // 
            this.btnPWReset.AllowFocus = false;
            this.btnPWReset.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPWReset.ImageOptions.Image")));
            this.btnPWReset.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnPWReset.ImageOptions.ImageToTextIndent = 10;
            this.btnPWReset.Location = new System.Drawing.Point(1189, 28);
            this.btnPWReset.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnPWReset.Name = "btnPWReset";
            this.btnPWReset.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPWReset.Size = new System.Drawing.Size(129, 42);
            this.btnPWReset.StyleController = this.layoutControl;
            this.btnPWReset.TabIndex = 7;
            this.btnPWReset.Text = "비밀번호 초기화";
            this.btnPWReset.Click += new System.EventHandler(this.btnPWReset_Click);
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
            this.layoutControlItem60,
            this.layoutControlItem8,
            this.layoutControlItem6,
            this.emptySpaceItem1});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup.Text = "사용자관리";
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
            this.emptySpaceItem2.Location = new System.Drawing.Point(1314, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(104, 46);
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
            this.layoutControlItem7.Control = this.btn_passChange;
            this.layoutControlItem7.Location = new System.Drawing.Point(375, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(129, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(129, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(129, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem60
            // 
            this.layoutControlItem60.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem60.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem60.Control = this.cboPlant_Code;
            this.layoutControlItem60.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem60.CustomizationFormText = "플랜트";
            this.layoutControlItem60.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem60.MaxSize = new System.Drawing.Size(260, 39);
            this.layoutControlItem60.MinSize = new System.Drawing.Size(260, 39);
            this.layoutControlItem60.Name = "layoutControlItem60";
            this.layoutControlItem60.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 4, 4);
            this.layoutControlItem60.Size = new System.Drawing.Size(260, 46);
            this.layoutControlItem60.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem60.Text = "플랜트";
            this.layoutControlItem60.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem60.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem60.TextToControlDistance = 11;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_search;
            this.layoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem8.Location = new System.Drawing.Point(260, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem8.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "layoutControlItem3";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnPWReset;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem6.Location = new System.Drawing.Point(1181, 0);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(133, 32);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(133, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Text = "layoutControlItem3";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(504, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(677, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // frm_Employee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_Employee.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frm_Employee";
            this.Tag = "frm_Employee";
            this.Text = "사용자관리";
            this.Load += new System.EventHandler(this.frm_Employee_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPlant_Code)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_MANAGE_TYPE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUEdit_EMPLOYEE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem60)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EMPLOYEE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PASSWORD;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_JUSO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_TEL_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_MANAGE_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_I_TIME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUpEdit_MANAGE_TYPE;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUEdit_EMPLOYEE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EMPLOYEE_NO;
        private DevExpress.XtraEditors.SimpleButton btn_passChange;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraEditors.LookUpEdit cboPlant_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem60;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PLANT_CODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboPlant_Code;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton btnPWReset;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}