using System;

namespace HARIM_FA_DOSING
{
    partial class frm_PackUseResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_PackUseResult));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.dateEdit_workEndDate = new DevExpress.XtraEditors.DateEdit();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_RESOURCE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_DESCRIPTION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OR_QTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PRO_QTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_PRO_KG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repItemLkUpEdit_C_CONDITION = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemLkUpEdit_EMPLOYEE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemLkUpEdit_RESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemLkUpEdit_SBNO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemLkUpEdit_BAD_CODE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.dateEdit_workStartDate = new DevExpress.XtraEditors.DateEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.cboPlant_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboL_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboProcessKey = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem58 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_C_CONDITION)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_EMPLOYEE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_RESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_SBNO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_BAD_CODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem58)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.dateEdit_workEndDate);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.dateEdit_workStartDate);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.cboPlant_Code);
            this.layoutControl.Controls.Add(this.cboL_Code);
            this.layoutControl.Controls.Add(this.cboProcessKey);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 11;
            this.layoutControl.Text = "layoutControl1";
            // 
            // dateEdit_workEndDate
            // 
            this.dateEdit_workEndDate.EditValue = null;
            this.dateEdit_workEndDate.Location = new System.Drawing.Point(840, 35);
            this.dateEdit_workEndDate.Name = "dateEdit_workEndDate";
            this.dateEdit_workEndDate.Properties.AllowFocused = false;
            this.dateEdit_workEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workEndDate.Size = new System.Drawing.Size(116, 24);
            this.dateEdit_workEndDate.StyleController = this.layoutControl;
            this.dateEdit_workEndDate.TabIndex = 8;
            this.dateEdit_workEndDate.EditValueChanged += new System.EventHandler(this.dateEdit_workStartDate_EditValueChanged);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl.Location = new System.Drawing.Point(6, 72);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemLkUpEdit_C_CONDITION,
            this.repItemLkUpEdit_EMPLOYEE_NO,
            this.repItemLkUpEdit_RESOURCE_NO,
            this.repItemLkUpEdit_SBNO,
            this.repItemLkUpEdit_BAD_CODE});
            this.gridControl.Size = new System.Drawing.Size(1748, 542);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_RESOURCE_NO,
            this.gridColumn_DESCRIPTION,
            this.OR_QTY,
            this.PRO_QTY,
            this.gridColumn_PRO_KG});
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
            // gridColumn_RESOURCE_NO
            // 
            this.gridColumn_RESOURCE_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_RESOURCE_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_RESOURCE_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_RESOURCE_NO.Caption = "품목코드";
            this.gridColumn_RESOURCE_NO.FieldName = "RESOURCE_NO";
            this.gridColumn_RESOURCE_NO.Name = "gridColumn_RESOURCE_NO";
            this.gridColumn_RESOURCE_NO.OptionsColumn.ReadOnly = true;
            this.gridColumn_RESOURCE_NO.Visible = true;
            this.gridColumn_RESOURCE_NO.VisibleIndex = 0;
            this.gridColumn_RESOURCE_NO.Width = 250;
            // 
            // gridColumn_DESCRIPTION
            // 
            this.gridColumn_DESCRIPTION.Caption = "품목명";
            this.gridColumn_DESCRIPTION.FieldName = "DESCRIPTION";
            this.gridColumn_DESCRIPTION.Name = "gridColumn_DESCRIPTION";
            this.gridColumn_DESCRIPTION.Visible = true;
            this.gridColumn_DESCRIPTION.VisibleIndex = 1;
            // 
            // OR_QTY
            // 
            this.OR_QTY.AppearanceHeader.Options.UseTextOptions = true;
            this.OR_QTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.OR_QTY.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.OR_QTY.Caption = "계획수량 (QTY)";
            this.OR_QTY.DisplayFormat.FormatString = "{0:n0}";
            this.OR_QTY.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.OR_QTY.FieldName = "OR_QTY";
            this.OR_QTY.Name = "OR_QTY";
            this.OR_QTY.OptionsColumn.ReadOnly = true;
            this.OR_QTY.Visible = true;
            this.OR_QTY.VisibleIndex = 2;
            this.OR_QTY.Width = 128;
            // 
            // PRO_QTY
            // 
            this.PRO_QTY.AppearanceHeader.Options.UseTextOptions = true;
            this.PRO_QTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PRO_QTY.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PRO_QTY.Caption = "생산수량 (QTY)";
            this.PRO_QTY.DisplayFormat.FormatString = "{0:n0}";
            this.PRO_QTY.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.PRO_QTY.FieldName = "PRO_QTY";
            this.PRO_QTY.Name = "PRO_QTY";
            this.PRO_QTY.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PRO_QTY", "{0:n0} 개")});
            this.PRO_QTY.Visible = true;
            this.PRO_QTY.VisibleIndex = 3;
            this.PRO_QTY.Width = 128;
            // 
            // gridColumn_PRO_KG
            // 
            this.gridColumn_PRO_KG.Caption = "생산중량(KG)";
            this.gridColumn_PRO_KG.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_PRO_KG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_PRO_KG.FieldName = "PRO_KG";
            this.gridColumn_PRO_KG.Name = "gridColumn_PRO_KG";
            this.gridColumn_PRO_KG.Visible = true;
            this.gridColumn_PRO_KG.VisibleIndex = 4;
            // 
            // repItemLkUpEdit_C_CONDITION
            // 
            this.repItemLkUpEdit_C_CONDITION.AutoHeight = false;
            this.repItemLkUpEdit_C_CONDITION.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUpEdit_C_CONDITION.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "코드"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "작업상태")});
            this.repItemLkUpEdit_C_CONDITION.Name = "repItemLkUpEdit_C_CONDITION";
            this.repItemLkUpEdit_C_CONDITION.NullText = "";
            // 
            // repItemLkUpEdit_EMPLOYEE_NO
            // 
            this.repItemLkUpEdit_EMPLOYEE_NO.AutoHeight = false;
            this.repItemLkUpEdit_EMPLOYEE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUpEdit_EMPLOYEE_NO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Name7"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name8")});
            this.repItemLkUpEdit_EMPLOYEE_NO.Name = "repItemLkUpEdit_EMPLOYEE_NO";
            this.repItemLkUpEdit_EMPLOYEE_NO.NullText = "";
            // 
            // repItemLkUpEdit_RESOURCE_NO
            // 
            this.repItemLkUpEdit_RESOURCE_NO.AutoHeight = false;
            this.repItemLkUpEdit_RESOURCE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUpEdit_RESOURCE_NO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "제품코드"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "제품명")});
            this.repItemLkUpEdit_RESOURCE_NO.Name = "repItemLkUpEdit_RESOURCE_NO";
            this.repItemLkUpEdit_RESOURCE_NO.NullText = "";
            // 
            // repItemLkUpEdit_SBNO
            // 
            this.repItemLkUpEdit_SBNO.AutoHeight = false;
            this.repItemLkUpEdit_SBNO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUpEdit_SBNO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Name35"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name36")});
            this.repItemLkUpEdit_SBNO.Name = "repItemLkUpEdit_SBNO";
            // 
            // repItemLkUpEdit_BAD_CODE
            // 
            this.repItemLkUpEdit_BAD_CODE.AutoHeight = false;
            this.repItemLkUpEdit_BAD_CODE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemLkUpEdit_BAD_CODE.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "Name11"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Name12")});
            this.repItemLkUpEdit_BAD_CODE.Name = "repItemLkUpEdit_BAD_CODE";
            // 
            // dateEdit_workStartDate
            // 
            this.dateEdit_workStartDate.EditValue = null;
            this.dateEdit_workStartDate.Location = new System.Drawing.Point(697, 35);
            this.dateEdit_workStartDate.Name = "dateEdit_workStartDate";
            this.dateEdit_workStartDate.Properties.AllowFocused = false;
            this.dateEdit_workStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workStartDate.Size = new System.Drawing.Size(121, 24);
            this.dateEdit_workStartDate.StyleController = this.layoutControl;
            this.dateEdit_workStartDate.TabIndex = 4;
            this.dateEdit_workStartDate.EditValueChanged += new System.EventHandler(this.dateEdit_workStartDate_EditValueChanged);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(960, 26);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cboPlant_Code
            // 
            this.cboPlant_Code.Location = new System.Drawing.Point(47, 35);
            this.cboPlant_Code.Name = "cboPlant_Code";
            this.cboPlant_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlant_Code.Size = new System.Drawing.Size(195, 24);
            this.cboPlant_Code.StyleController = this.layoutControl;
            this.cboPlant_Code.TabIndex = 23;
            this.cboPlant_Code.EditValueChanged += new System.EventHandler(this.cboPlant_Code_EditValueChanged);
            // 
            // cboL_Code
            // 
            this.cboL_Code.Location = new System.Drawing.Point(469, 35);
            this.cboL_Code.Name = "cboL_Code";
            this.cboL_Code.Properties.AllowFocused = false;
            this.cboL_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboL_Code.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "코드", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("공정", "공정")});
            this.cboL_Code.Properties.NullText = "";
            this.cboL_Code.Properties.PopupSizeable = false;
            this.cboL_Code.Size = new System.Drawing.Size(136, 24);
            this.cboL_Code.StyleController = this.layoutControl;
            this.cboL_Code.TabIndex = 5;
            this.cboL_Code.EditValueChanged += new System.EventHandler(this.cboL_Code_EditValueChanged);
            // 
            // cboProcessKey
            // 
            this.cboProcessKey.Location = new System.Drawing.Point(295, 35);
            this.cboProcessKey.Name = "cboProcessKey";
            this.cboProcessKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProcessKey.Size = new System.Drawing.Size(107, 24);
            this.cboProcessKey.StyleController = this.layoutControl;
            this.cboProcessKey.TabIndex = 23;
            this.cboProcessKey.EditValueChanged += new System.EventHandler(this.cboProcessKey_EditValueChanged);
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
            this.layoutControlGroup1.CustomizationFormText = "포장 작업지시";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem2,
            this.layoutControlItem_workdate,
            this.layoutControlItem7,
            this.layoutControlItem2,
            this.simpleLabelItem1,
            this.layoutControlItem13,
            this.layoutControlItem4,
            this.layoutControlItem58});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup1.Text = "기간별 포장생산내역";
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
            this.emptySpaceItem2.Location = new System.Drawing.Point(1069, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(683, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem_workdate
            // 
            this.layoutControlItem_workdate.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workdate.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workdate.Control = this.dateEdit_workStartDate;
            this.layoutControlItem_workdate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workdate.CustomizationFormText = "작업일자";
            this.layoutControlItem_workdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem_workdate.ImageOptions.Image")));
            this.layoutControlItem_workdate.ImageOptions.ImageToTextDistance = 10;
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(604, 0);
            this.layoutControlItem_workdate.MaxSize = new System.Drawing.Size(213, 46);
            this.layoutControlItem_workdate.MinSize = new System.Drawing.Size(213, 46);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(213, 46);
            this.layoutControlItem_workdate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workdate.Text = "작업일자";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(74, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 5;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_search;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem7.Location = new System.Drawing.Point(954, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "layoutControlItem3";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.dateEdit_workEndDate;
            this.layoutControlItem2.Location = new System.Drawing.Point(834, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(120, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(120, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleLabelItem1.Location = new System.Drawing.Point(817, 0);
            this.simpleLabelItem1.MaxSize = new System.Drawing.Size(17, 46);
            this.simpleLabelItem1.MinSize = new System.Drawing.Size(17, 46);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(17, 46);
            this.simpleLabelItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.simpleLabelItem1.Text = "~";
            this.simpleLabelItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(8, 15);
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem13.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem13.Control = this.cboPlant_Code;
            this.layoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem13.CustomizationFormText = "플랜트";
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.MaxSize = new System.Drawing.Size(240, 46);
            this.layoutControlItem13.MinSize = new System.Drawing.Size(240, 46);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(240, 46);
            this.layoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem13.Text = "플랜트";
            this.layoutControlItem13.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem13.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem13.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.cboL_Code;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "진행상태";
            this.layoutControlItem4.Location = new System.Drawing.Point(400, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(204, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(204, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem4.Size = new System.Drawing.Size(204, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem4.Text = "라인";
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem4.TextToControlDistance = 11;
            // 
            // layoutControlItem58
            // 
            this.layoutControlItem58.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem58.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem58.Control = this.cboProcessKey;
            this.layoutControlItem58.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem58.CustomizationFormText = "공정";
            this.layoutControlItem58.Location = new System.Drawing.Point(240, 0);
            this.layoutControlItem58.MaxSize = new System.Drawing.Size(160, 46);
            this.layoutControlItem58.MinSize = new System.Drawing.Size(160, 46);
            this.layoutControlItem58.Name = "layoutControlItem58";
            this.layoutControlItem58.Size = new System.Drawing.Size(160, 46);
            this.layoutControlItem58.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem58.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem58.Text = "공정";
            this.layoutControlItem58.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem58.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem58.TextToControlDistance = 5;
            // 
            // frm_PackUseResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_PackUseResult.IconOptions.Image")));
            this.Name = "frm_PackUseResult";
            this.Text = "기간별 포장생산내역";
            this.Load += new System.EventHandler(this.frm_PackUseResult_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_C_CONDITION)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_EMPLOYEE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_RESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_SBNO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemLkUpEdit_BAD_CODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem58)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.DateEdit dateEdit_workEndDate;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_RESOURCE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn EXP_QTY;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUpEdit_C_CONDITION;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUpEdit_EMPLOYEE_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUpEdit_RESOURCE_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUpEdit_SBNO;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemLkUpEdit_BAD_CODE;
        private DevExpress.XtraEditors.DateEdit dateEdit_workStartDate;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraGrid.Columns.GridColumn PRO_QTY;
        private DevExpress.XtraEditors.LookUpEdit cboPlant_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraEditors.LookUpEdit cboL_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.LookUpEdit cboProcessKey;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem58;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_DESCRIPTION;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PRO_KG;
        private DevExpress.XtraGrid.Columns.GridColumn OR_QTY;
    }
}