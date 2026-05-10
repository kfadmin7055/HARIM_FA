namespace HARIM_FA_DOSING
{
    partial class frm_EqTrouble
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_EqTrouble));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_WORKDATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumn_EQ_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_EQ_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_EQ_TRB_MSG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.gridColumn_EQ_TRB_TASK_MSG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_REMARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_IN_GUBUN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_I_USER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.dateEdit_workDate = new DevExpress.XtraEditors.DateEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.dateEdit_workDate);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 14;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl.Location = new System.Drawing.Point(6, 72);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit,
            this.repositoryItemDateEdit});
            this.gridControl.Size = new System.Drawing.Size(1748, 542);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_WORKDATE,
            this.gridColumn_EQ_CODE,
            this.gridColumn_EQ_NAME,
            this.gridColumn_EQ_TRB_MSG,
            this.gridColumn_EQ_TRB_TASK_MSG,
            this.gridColumn_REMARK,
            this.gridColumn_IN_GUBUN,
            this.gridColumn_I_TIME,
            this.gridColumn_I_USER});
            this.gridView.DetailHeight = 377;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            this.gridView.OptionsSelection.MultiSelect = true;
            this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView_ShowingEditor);
            // 
            // gridColumn_WORKDATE
            // 
            this.gridColumn_WORKDATE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_WORKDATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_WORKDATE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_WORKDATE.Caption = "설비오류일자";
            this.gridColumn_WORKDATE.ColumnEdit = this.repositoryItemDateEdit;
            this.gridColumn_WORKDATE.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss  ";
            this.gridColumn_WORKDATE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_WORKDATE.FieldName = "WORKDATE";
            this.gridColumn_WORKDATE.Name = "gridColumn_WORKDATE";
            this.gridColumn_WORKDATE.Visible = true;
            this.gridColumn_WORKDATE.VisibleIndex = 1;
            this.gridColumn_WORKDATE.Width = 198;
            // 
            // repositoryItemDateEdit
            // 
            this.repositoryItemDateEdit.AutoHeight = false;
            this.repositoryItemDateEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemDateEdit.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit.MaskSettings.Set("mask", "yyyy-MM-dd HH:mm:ss");
            this.repositoryItemDateEdit.Name = "repositoryItemDateEdit";
            // 
            // gridColumn_EQ_CODE
            // 
            this.gridColumn_EQ_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EQ_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EQ_CODE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_EQ_CODE.Caption = "설비코드";
            this.gridColumn_EQ_CODE.FieldName = "EQ_CODE";
            this.gridColumn_EQ_CODE.Name = "gridColumn_EQ_CODE";
            this.gridColumn_EQ_CODE.Visible = true;
            this.gridColumn_EQ_CODE.VisibleIndex = 2;
            this.gridColumn_EQ_CODE.Width = 138;
            // 
            // gridColumn_EQ_NAME
            // 
            this.gridColumn_EQ_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EQ_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EQ_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_EQ_NAME.Caption = "설비명";
            this.gridColumn_EQ_NAME.FieldName = "EQ_NAME";
            this.gridColumn_EQ_NAME.Name = "gridColumn_EQ_NAME";
            this.gridColumn_EQ_NAME.Visible = true;
            this.gridColumn_EQ_NAME.VisibleIndex = 3;
            this.gridColumn_EQ_NAME.Width = 157;
            // 
            // gridColumn_EQ_TRB_MSG
            // 
            this.gridColumn_EQ_TRB_MSG.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EQ_TRB_MSG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EQ_TRB_MSG.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_EQ_TRB_MSG.Caption = "이슈내역";
            this.gridColumn_EQ_TRB_MSG.ColumnEdit = this.repositoryItemMemoEdit;
            this.gridColumn_EQ_TRB_MSG.FieldName = "EQ_TRB_MSG";
            this.gridColumn_EQ_TRB_MSG.Name = "gridColumn_EQ_TRB_MSG";
            this.gridColumn_EQ_TRB_MSG.Visible = true;
            this.gridColumn_EQ_TRB_MSG.VisibleIndex = 4;
            this.gridColumn_EQ_TRB_MSG.Width = 259;
            // 
            // repositoryItemMemoEdit
            // 
            this.repositoryItemMemoEdit.Name = "repositoryItemMemoEdit";
            // 
            // gridColumn_EQ_TRB_TASK_MSG
            // 
            this.gridColumn_EQ_TRB_TASK_MSG.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EQ_TRB_TASK_MSG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EQ_TRB_TASK_MSG.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_EQ_TRB_TASK_MSG.Caption = "처리내역";
            this.gridColumn_EQ_TRB_TASK_MSG.ColumnEdit = this.repositoryItemMemoEdit;
            this.gridColumn_EQ_TRB_TASK_MSG.FieldName = "EQ_TRB_TASK_MSG";
            this.gridColumn_EQ_TRB_TASK_MSG.Name = "gridColumn_EQ_TRB_TASK_MSG";
            this.gridColumn_EQ_TRB_TASK_MSG.Visible = true;
            this.gridColumn_EQ_TRB_TASK_MSG.VisibleIndex = 5;
            this.gridColumn_EQ_TRB_TASK_MSG.Width = 218;
            // 
            // gridColumn_REMARK
            // 
            this.gridColumn_REMARK.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_REMARK.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_REMARK.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_REMARK.Caption = "비고";
            this.gridColumn_REMARK.ColumnEdit = this.repositoryItemMemoEdit;
            this.gridColumn_REMARK.FieldName = "REMARK";
            this.gridColumn_REMARK.Name = "gridColumn_REMARK";
            this.gridColumn_REMARK.Visible = true;
            this.gridColumn_REMARK.VisibleIndex = 6;
            this.gridColumn_REMARK.Width = 186;
            // 
            // gridColumn_IN_GUBUN
            // 
            this.gridColumn_IN_GUBUN.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_IN_GUBUN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_IN_GUBUN.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_IN_GUBUN.Caption = "입력구분";
            this.gridColumn_IN_GUBUN.FieldName = "IN_GUBUN";
            this.gridColumn_IN_GUBUN.Name = "gridColumn_IN_GUBUN";
            this.gridColumn_IN_GUBUN.Visible = true;
            this.gridColumn_IN_GUBUN.VisibleIndex = 7;
            this.gridColumn_IN_GUBUN.Width = 86;
            // 
            // gridColumn_I_TIME
            // 
            this.gridColumn_I_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_I_TIME.Caption = "입력일자";
            this.gridColumn_I_TIME.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss  ";
            this.gridColumn_I_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_I_TIME.FieldName = "I_TIME";
            this.gridColumn_I_TIME.Name = "gridColumn_I_TIME";
            this.gridColumn_I_TIME.OptionsColumn.AllowFocus = false;
            this.gridColumn_I_TIME.OptionsColumn.ReadOnly = true;
            this.gridColumn_I_TIME.Visible = true;
            this.gridColumn_I_TIME.VisibleIndex = 8;
            this.gridColumn_I_TIME.Width = 186;
            // 
            // gridColumn_I_USER
            // 
            this.gridColumn_I_USER.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_I_USER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_I_USER.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_I_USER.Caption = "입력사원";
            this.gridColumn_I_USER.FieldName = "I_USER";
            this.gridColumn_I_USER.Name = "gridColumn_I_USER";
            this.gridColumn_I_USER.Visible = true;
            this.gridColumn_I_USER.VisibleIndex = 9;
            this.gridColumn_I_USER.Width = 82;
            // 
            // btn_delete
            // 
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1635, 26);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(119, 42);
            this.btn_delete.StyleController = this.layoutControl;
            this.btn_delete.TabIndex = 27;
            this.btn_delete.Text = "삭제";
            this.btn_delete.ToolTip = "삭제(Ctrl + D)";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowDel.Location = new System.Drawing.Point(1460, 26);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(36, 42);
            this.btn_rowDel.StyleController = this.layoutControl;
            this.btn_rowDel.TabIndex = 28;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1420, 26);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(36, 42);
            this.btn_rowAdd.StyleController = this.layoutControl;
            this.btn_rowAdd.TabIndex = 29;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1520, 26);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(111, 42);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 27;
            this.btn_save.Text = "저 장";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // dateEdit_workDate
            // 
            this.dateEdit_workDate.EditValue = null;
            this.dateEdit_workDate.Location = new System.Drawing.Point(117, 34);
            this.dateEdit_workDate.Name = "dateEdit_workDate";
            this.dateEdit_workDate.Properties.AllowFocused = false;
            this.dateEdit_workDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workDate.Size = new System.Drawing.Size(135, 24);
            this.dateEdit_workDate.StyleController = this.layoutControl;
            this.dateEdit_workDate.TabIndex = 4;
            this.dateEdit_workDate.EditValueChanged += new System.EventHandler(this.dateEdit_workDate_EditValueChanged);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(257, 26);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
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
            this.layoutControlItem_workdate,
            this.layoutControlItem7,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem10,
            this.layoutControlItem8});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup1.Text = "설비오류내역관리";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1752, 546);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(366, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(1048, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem_workdate
            // 
            this.layoutControlItem_workdate.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workdate.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workdate.Control = this.dateEdit_workDate;
            this.layoutControlItem_workdate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workdate.CustomizationFormText = "작업일자";
            this.layoutControlItem_workdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem_workdate.ImageOptions.Image")));
            this.layoutControlItem_workdate.ImageOptions.ImageToTextDistance = 10;
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem_workdate.MaxSize = new System.Drawing.Size(251, 37);
            this.layoutControlItem_workdate.MinSize = new System.Drawing.Size(251, 37);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(251, 46);
            this.layoutControlItem_workdate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workdate.Text = "설비이상일자";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(98, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 5;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_search;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem7.Location = new System.Drawing.Point(251, 0);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(115, 32);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "layoutControlItem3";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_rowDel;
            this.layoutControlItem5.Location = new System.Drawing.Point(1454, 0);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(60, 32);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 20, 0, 0);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btn_rowAdd;
            this.layoutControlItem6.Location = new System.Drawing.Point(1414, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btn_delete;
            this.layoutControlItem10.Location = new System.Drawing.Point(1629, 0);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(115, 26);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(123, 46);
            this.layoutControlItem10.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_save;
            this.layoutControlItem8.Location = new System.Drawing.Point(1514, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(115, 0);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(115, 32);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // frm_EqTrouble
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_EqTrouble.IconOptions.Image")));
            this.Name = "frm_EqTrouble";
            this.Text = "설비오류내역관리";
            this.Load += new System.EventHandler(this.frm_EqTrouble_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_WORKDATE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EQ_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EQ_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EQ_TRB_MSG;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EQ_TRB_TASK_MSG;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_REMARK;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_IN_GUBUN;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_I_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_I_USER;
        private DevExpress.XtraEditors.DateEdit dateEdit_workDate;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit;
    }
}