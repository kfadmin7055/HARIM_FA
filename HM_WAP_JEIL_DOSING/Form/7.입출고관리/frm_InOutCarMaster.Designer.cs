namespace HARIM_FA_DOSING
{
    partial class frm_InOutCarMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_InOutCarMaster));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridInOutCar = new DevExpress.XtraGrid.GridControl();
            this.viewInOutCar = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dVEHICLENO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dCAR_TYPE_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dETC_DETAIL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dINCAR_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dOUTCAR_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dIN_WEIGHT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dOUT_WEIGHT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dREAL_WEIGHT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dDESCRIPTION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dINVOICE_WEIGHT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dTOLOCATION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dDELIVERYDATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.reptemCheckEdit_SEQ1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridPascboPTMCD1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView61 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dtFromDeliveryDate = new DevExpress.XtraEditors.DateEdit();
            this.dtToDeliveryDate = new DevExpress.XtraEditors.DateEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.cboCar_Type = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem_workDateEd = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.dCHARG_TEXT = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInOutCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewInOutCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit_SEQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPascboPTMCD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView61)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDeliveryDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDeliveryDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDeliveryDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDeliveryDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCar_Type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workDateEd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridInOutCar);
            this.layoutControl.Controls.Add(this.dtFromDeliveryDate);
            this.layoutControl.Controls.Add(this.dtToDeliveryDate);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.cboCar_Type);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 11;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridInOutCar
            // 
            this.gridInOutCar.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gridInOutCar.Location = new System.Drawing.Point(2, 76);
            this.gridInOutCar.MainView = this.viewInOutCar;
            this.gridInOutCar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridInOutCar.Name = "gridInOutCar";
            this.gridInOutCar.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.reptemCheckEdit_SEQ1,
            this.gridPascboPTMCD1});
            this.gridInOutCar.Size = new System.Drawing.Size(1756, 542);
            this.gridInOutCar.TabIndex = 6;
            this.gridInOutCar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewInOutCar});
            this.gridInOutCar.Click += new System.EventHandler(this.gridInOutCar_Click);
            // 
            // viewInOutCar
            // 
            this.viewInOutCar.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dVEHICLENO,
            this.dCAR_TYPE_DESC,
            this.dETC_DETAIL,
            this.dINCAR_DATE,
            this.dOUTCAR_DATE,
            this.dIN_WEIGHT,
            this.dOUT_WEIGHT,
            this.dREAL_WEIGHT,
            this.dDESCRIPTION,
            this.dINVOICE_WEIGHT,
            this.dTOLOCATION,
            this.dDELIVERYDATE,
            this.dCHARG_TEXT});
            this.viewInOutCar.DetailHeight = 404;
            this.viewInOutCar.GridControl = this.gridInOutCar;
            this.viewInOutCar.Name = "viewInOutCar";
            this.viewInOutCar.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.viewInOutCar.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.viewInOutCar.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.viewInOutCar.OptionsFind.ShowFindButton = false;
            this.viewInOutCar.OptionsView.ColumnAutoWidth = false;
            this.viewInOutCar.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.viewInOutCar.OptionsView.ShowFooter = true;
            this.viewInOutCar.OptionsView.ShowGroupPanel = false;
            this.viewInOutCar.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.viewInOutCar_CustomDrawRowIndicator);
            this.viewInOutCar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridBrand_KeyDown);
            // 
            // dVEHICLENO
            // 
            this.dVEHICLENO.Caption = "차량번호";
            this.dVEHICLENO.FieldName = "VEHICLENO";
            this.dVEHICLENO.MaxWidth = 140;
            this.dVEHICLENO.MinWidth = 120;
            this.dVEHICLENO.Name = "dVEHICLENO";
            this.dVEHICLENO.Visible = true;
            this.dVEHICLENO.VisibleIndex = 0;
            this.dVEHICLENO.Width = 130;
            // 
            // dCAR_TYPE_DESC
            // 
            this.dCAR_TYPE_DESC.Caption = "차량구분";
            this.dCAR_TYPE_DESC.FieldName = "CAR_TYPE_DESC";
            this.dCAR_TYPE_DESC.MaxWidth = 140;
            this.dCAR_TYPE_DESC.MinWidth = 100;
            this.dCAR_TYPE_DESC.Name = "dCAR_TYPE_DESC";
            this.dCAR_TYPE_DESC.Visible = true;
            this.dCAR_TYPE_DESC.VisibleIndex = 1;
            this.dCAR_TYPE_DESC.Width = 120;
            // 
            // dETC_DETAIL
            // 
            this.dETC_DETAIL.Caption = "기타입고";
            this.dETC_DETAIL.FieldName = "ETC_DETAIL";
            this.dETC_DETAIL.MaxWidth = 140;
            this.dETC_DETAIL.MinWidth = 100;
            this.dETC_DETAIL.Name = "dETC_DETAIL";
            this.dETC_DETAIL.Visible = true;
            this.dETC_DETAIL.VisibleIndex = 2;
            this.dETC_DETAIL.Width = 120;
            // 
            // dINCAR_DATE
            // 
            this.dINCAR_DATE.Caption = "입차일시";
            this.dINCAR_DATE.FieldName = "INCAR_DATE";
            this.dINCAR_DATE.MaxWidth = 190;
            this.dINCAR_DATE.MinWidth = 160;
            this.dINCAR_DATE.Name = "dINCAR_DATE";
            this.dINCAR_DATE.Visible = true;
            this.dINCAR_DATE.VisibleIndex = 3;
            this.dINCAR_DATE.Width = 170;
            // 
            // dOUTCAR_DATE
            // 
            this.dOUTCAR_DATE.Caption = "출차일시";
            this.dOUTCAR_DATE.FieldName = "OUTCAR_DATE";
            this.dOUTCAR_DATE.MaxWidth = 190;
            this.dOUTCAR_DATE.MinWidth = 160;
            this.dOUTCAR_DATE.Name = "dOUTCAR_DATE";
            this.dOUTCAR_DATE.Visible = true;
            this.dOUTCAR_DATE.VisibleIndex = 4;
            this.dOUTCAR_DATE.Width = 170;
            // 
            // dIN_WEIGHT
            // 
            this.dIN_WEIGHT.Caption = "입차계근량";
            this.dIN_WEIGHT.DisplayFormat.FormatString = "{0:n0}";
            this.dIN_WEIGHT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.dIN_WEIGHT.FieldName = "IN_WEIGHT";
            this.dIN_WEIGHT.MaxWidth = 120;
            this.dIN_WEIGHT.MinWidth = 100;
            this.dIN_WEIGHT.Name = "dIN_WEIGHT";
            this.dIN_WEIGHT.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "IN_WEIGHT", "{0:n0}")});
            this.dIN_WEIGHT.Visible = true;
            this.dIN_WEIGHT.VisibleIndex = 5;
            this.dIN_WEIGHT.Width = 110;
            // 
            // dOUT_WEIGHT
            // 
            this.dOUT_WEIGHT.Caption = "출차계근량";
            this.dOUT_WEIGHT.DisplayFormat.FormatString = "{0:n0}";
            this.dOUT_WEIGHT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.dOUT_WEIGHT.FieldName = "OUT_WEIGHT";
            this.dOUT_WEIGHT.MaxWidth = 120;
            this.dOUT_WEIGHT.MinWidth = 100;
            this.dOUT_WEIGHT.Name = "dOUT_WEIGHT";
            this.dOUT_WEIGHT.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "OUT_WEIGHT", "{0:n0}")});
            this.dOUT_WEIGHT.Visible = true;
            this.dOUT_WEIGHT.VisibleIndex = 6;
            this.dOUT_WEIGHT.Width = 110;
            // 
            // dREAL_WEIGHT
            // 
            this.dREAL_WEIGHT.Caption = "실계근량";
            this.dREAL_WEIGHT.DisplayFormat.FormatString = "{0:n0}";
            this.dREAL_WEIGHT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.dREAL_WEIGHT.FieldName = "REAL_WEIGHT";
            this.dREAL_WEIGHT.MaxWidth = 120;
            this.dREAL_WEIGHT.MinWidth = 100;
            this.dREAL_WEIGHT.Name = "dREAL_WEIGHT";
            this.dREAL_WEIGHT.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "REAL_WEIGHT", "{0:n0}")});
            this.dREAL_WEIGHT.Visible = true;
            this.dREAL_WEIGHT.VisibleIndex = 7;
            this.dREAL_WEIGHT.Width = 110;
            // 
            // dDESCRIPTION
            // 
            this.dDESCRIPTION.Caption = "품목명(원료)";
            this.dDESCRIPTION.FieldName = "DESCRIPTION";
            this.dDESCRIPTION.MaxWidth = 240;
            this.dDESCRIPTION.MinWidth = 180;
            this.dDESCRIPTION.Name = "dDESCRIPTION";
            this.dDESCRIPTION.Visible = true;
            this.dDESCRIPTION.VisibleIndex = 8;
            this.dDESCRIPTION.Width = 200;
            // 
            // dINVOICE_WEIGHT
            // 
            this.dINVOICE_WEIGHT.Caption = "송장량";
            this.dINVOICE_WEIGHT.DisplayFormat.FormatString = "{0:n0}";
            this.dINVOICE_WEIGHT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.dINVOICE_WEIGHT.FieldName = "INVOICE_WEIGHT";
            this.dINVOICE_WEIGHT.MaxWidth = 120;
            this.dINVOICE_WEIGHT.MinWidth = 100;
            this.dINVOICE_WEIGHT.Name = "dINVOICE_WEIGHT";
            this.dINVOICE_WEIGHT.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "INVOICE_WEIGHT", "{0:n0}")});
            this.dINVOICE_WEIGHT.Visible = true;
            this.dINVOICE_WEIGHT.VisibleIndex = 9;
            this.dINVOICE_WEIGHT.Width = 110;
            // 
            // dTOLOCATION
            // 
            this.dTOLOCATION.Caption = "농장명(거래처명)";
            this.dTOLOCATION.FieldName = "TOLOCATION";
            this.dTOLOCATION.MaxWidth = 240;
            this.dTOLOCATION.MinWidth = 100;
            this.dTOLOCATION.Name = "dTOLOCATION";
            this.dTOLOCATION.Visible = true;
            this.dTOLOCATION.VisibleIndex = 10;
            this.dTOLOCATION.Width = 180;
            // 
            // dDELIVERYDATE
            // 
            this.dDELIVERYDATE.Caption = "납품요청일";
            this.dDELIVERYDATE.FieldName = "DELIVERYDATE";
            this.dDELIVERYDATE.MaxWidth = 140;
            this.dDELIVERYDATE.MinWidth = 100;
            this.dDELIVERYDATE.Name = "dDELIVERYDATE";
            this.dDELIVERYDATE.Visible = true;
            this.dDELIVERYDATE.VisibleIndex = 11;
            this.dDELIVERYDATE.Width = 120;
            // 
            // reptemCheckEdit_SEQ1
            // 
            this.reptemCheckEdit_SEQ1.AutoHeight = false;
            this.reptemCheckEdit_SEQ1.Name = "reptemCheckEdit_SEQ1";
            this.reptemCheckEdit_SEQ1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.reptemCheckEdit_SEQ1.ValueChecked = 1;
            this.reptemCheckEdit_SEQ1.ValueUnchecked = 2;
            // 
            // gridPascboPTMCD1
            // 
            this.gridPascboPTMCD1.AutoHeight = false;
            this.gridPascboPTMCD1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridPascboPTMCD1.Name = "gridPascboPTMCD1";
            this.gridPascboPTMCD1.PopupView = this.gridView61;
            // 
            // gridView61
            // 
            this.gridView61.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView61.Name = "gridView61";
            this.gridView61.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView61.OptionsView.ShowGroupPanel = false;
            // 
            // dtFromDeliveryDate
            // 
            this.dtFromDeliveryDate.EditValue = null;
            this.dtFromDeliveryDate.Location = new System.Drawing.Point(93, 35);
            this.dtFromDeliveryDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtFromDeliveryDate.Name = "dtFromDeliveryDate";
            this.dtFromDeliveryDate.Properties.AllowFocused = false;
            this.dtFromDeliveryDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtFromDeliveryDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFromDeliveryDate.Properties.CalendarTimeProperties.AllowFocused = false;
            this.dtFromDeliveryDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFromDeliveryDate.Size = new System.Drawing.Size(170, 24);
            this.dtFromDeliveryDate.StyleController = this.layoutControl;
            this.dtFromDeliveryDate.TabIndex = 4;
            // 
            // dtToDeliveryDate
            // 
            this.dtToDeliveryDate.EditValue = null;
            this.dtToDeliveryDate.Location = new System.Drawing.Point(281, 35);
            this.dtToDeliveryDate.Name = "dtToDeliveryDate";
            this.dtToDeliveryDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtToDeliveryDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtToDeliveryDate.Size = new System.Drawing.Size(147, 24);
            this.dtToDeliveryDate.StyleController = this.layoutControl;
            this.dtToDeliveryDate.TabIndex = 23;
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(650, 26);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 27;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cboCar_Type
            // 
            this.cboCar_Type.Location = new System.Drawing.Point(561, 35);
            this.cboCar_Type.Name = "cboCar_Type";
            this.cboCar_Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCar_Type.Size = new System.Drawing.Size(85, 24);
            this.cboCar_Type.StyleController = this.layoutControl;
            this.cboCar_Type.TabIndex = 23;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1760, 620);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup1.CaptionImageOptions.Image")));
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItem_workdate,
            this.layoutControlItem_workDateEd,
            this.layoutControlItem15,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 74);
            this.layoutControlGroup1.Text = "차량 입출문 현황";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(759, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(993, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem_workdate
            // 
            this.layoutControlItem_workdate.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workdate.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workdate.Control = this.dtFromDeliveryDate;
            this.layoutControlItem_workdate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workdate.CustomizationFormText = "입차시간";
            this.layoutControlItem_workdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem_workdate.ImageOptions.Image")));
            this.layoutControlItem_workdate.ImageOptions.ImageToTextDistance = 10;
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem_workdate.MaxSize = new System.Drawing.Size(262, 46);
            this.layoutControlItem_workdate.MinSize = new System.Drawing.Size(262, 46);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 4, 4);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(262, 46);
            this.layoutControlItem_workdate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workdate.Text = "입차시간";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(74, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 5;
            // 
            // layoutControlItem_workDateEd
            // 
            this.layoutControlItem_workDateEd.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workDateEd.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workDateEd.Control = this.dtToDeliveryDate;
            this.layoutControlItem_workDateEd.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workDateEd.CustomizationFormText = "~";
            this.layoutControlItem_workDateEd.Location = new System.Drawing.Point(262, 0);
            this.layoutControlItem_workDateEd.MaxSize = new System.Drawing.Size(164, 46);
            this.layoutControlItem_workDateEd.MinSize = new System.Drawing.Size(164, 46);
            this.layoutControlItem_workDateEd.Name = "layoutControlItem_workDateEd";
            this.layoutControlItem_workDateEd.Size = new System.Drawing.Size(164, 46);
            this.layoutControlItem_workDateEd.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workDateEd.Text = "~";
            this.layoutControlItem_workDateEd.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workDateEd.TextSize = new System.Drawing.Size(8, 15);
            this.layoutControlItem_workDateEd.TextToControlDistance = 5;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btn_search;
            this.layoutControlItem15.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem15.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem15.Location = new System.Drawing.Point(644, 0);
            this.layoutControlItem15.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem15.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem15.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem15.Text = "layoutControlItem5";
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.cboCar_Type;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "실적기준";
            this.layoutControlItem2.Location = new System.Drawing.Point(426, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(218, 46);
            this.layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem2.Text = "차량구분(입차타입)";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(104, 15);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridInOutCar;
            this.layoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem25";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 74);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(104, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1760, 546);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "layoutControlItem25";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // dCHARG_TEXT
            // 
            this.dCHARG_TEXT.Caption = "모선명";
            this.dCHARG_TEXT.FieldName = "CHARG_TEXT";
            this.dCHARG_TEXT.Name = "dCHARG_TEXT";
            this.dCHARG_TEXT.Visible = true;
            this.dCHARG_TEXT.VisibleIndex = 12;
            // 
            // frm_InOutCarMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_InOutCarMaster.IconOptions.Image")));
            this.Name = "frm_InOutCarMaster";
            this.Text = "차량 입출문 현황";
            this.Load += new System.EventHandler(this.frm_InCarMaster_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridInOutCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewInOutCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit_SEQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPascboPTMCD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView61)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDeliveryDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDeliveryDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDeliveryDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDeliveryDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCar_Type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workDateEd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gridInOutCar;
        private DevExpress.XtraGrid.Views.Grid.GridView viewInOutCar;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit reptemCheckEdit_SEQ1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridPascboPTMCD1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView61;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.DateEdit dtFromDeliveryDate;
        private DevExpress.XtraEditors.DateEdit dtToDeliveryDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workDateEd;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraGrid.Columns.GridColumn dVEHICLENO;
        private DevExpress.XtraGrid.Columns.GridColumn dCAR_TYPE_DESC;
        private DevExpress.XtraGrid.Columns.GridColumn dETC_DETAIL;
        private DevExpress.XtraGrid.Columns.GridColumn dINCAR_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn dOUTCAR_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn dIN_WEIGHT;
        private DevExpress.XtraGrid.Columns.GridColumn dOUT_WEIGHT;
        private DevExpress.XtraGrid.Columns.GridColumn dREAL_WEIGHT;
        private DevExpress.XtraGrid.Columns.GridColumn dDESCRIPTION;
        private DevExpress.XtraGrid.Columns.GridColumn dPLANQTY;
        private DevExpress.XtraGrid.Columns.GridColumn dTOLOCATION;
        private DevExpress.XtraGrid.Columns.GridColumn dDELIVERYDATE;
        private DevExpress.XtraEditors.LookUpEdit cboCar_Type;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn dINVOICE_WEIGHT;
        private DevExpress.XtraGrid.Columns.GridColumn dCHARG_TEXT;
    }
}