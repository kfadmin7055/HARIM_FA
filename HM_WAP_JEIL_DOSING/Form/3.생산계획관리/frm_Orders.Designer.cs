namespace HARIM_FA_DOSING
{
    partial class frm_Orders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Orders));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VEHICLETONCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VEHICLEGROUPCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DRIVERNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DRIVERMOBILE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ROTATIONNUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DISPATCHMEMO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TRANSACTIONFLAG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.REGISTERAT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.REGISTERBY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ORDERTYPECODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FROMLOCATIONCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TOLOCATIONCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SOLDTOCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DELIVERYSEQUENCE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ITEMCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLANQTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.XDATS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem22 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(1, 1);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1758, 618);
            this.layoutControl.TabIndex = 2;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gridControl.Location = new System.Drawing.Point(8, 74);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(1742, 536);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.VEHICLETONCODE,
            this.VEHICLEGROUPCODE,
            this.DRIVERNAME,
            this.DRIVERMOBILE,
            this.ROTATIONNUMBER,
            this.DISPATCHMEMO,
            this.TRANSACTIONFLAG,
            this.REGISTERAT,
            this.REGISTERBY,
            this.ORDERTYPECODE,
            this.FROMLOCATIONCODE,
            this.TOLOCATIONCODE,
            this.SOLDTOCODE,
            this.DELIVERYSEQUENCE,
            this.ITEMCODE,
            this.PLANQTY,
            this.XDATS});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.EnableAppearanceOddRow = true;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ColumnPositionChanged += new System.EventHandler(this.gridView_ColumnPositionChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "배차번호";
            this.gridColumn1.FieldName = "DISPATCHNO";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "주문번호";
            this.gridColumn2.FieldName = "ORDERNO";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "라인번호";
            this.gridColumn3.FieldName = "ORDERLINENO";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "TMS디비전 코드";
            this.gridColumn4.FieldName = "TMSDIVISIONCODE";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 100;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "TMS물류운영그룹코드";
            this.gridColumn5.FieldName = "TMSLOGISTICGROUP";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 140;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "납품유형";
            this.gridColumn6.FieldName = "LFART";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "배송일자";
            this.gridColumn7.FieldName = "DELIVERYDATE";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "운송사";
            this.gridColumn9.FieldName = "CARRIERCODE";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "차량코드";
            this.gridColumn10.FieldName = "VEHICLECODE";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 8;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "차량번호";
            this.gridColumn11.FieldName = "VEHICLENO";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 9;
            // 
            // VEHICLETONCODE
            // 
            this.VEHICLETONCODE.Caption = "차량톤급";
            this.VEHICLETONCODE.FieldName = "VEHICLETONCODE";
            this.VEHICLETONCODE.Name = "VEHICLETONCODE";
            this.VEHICLETONCODE.Visible = true;
            this.VEHICLETONCODE.VisibleIndex = 10;
            // 
            // VEHICLEGROUPCODE
            // 
            this.VEHICLEGROUPCODE.Caption = "차량그룹";
            this.VEHICLEGROUPCODE.FieldName = "VEHICLEGROUPCODE";
            this.VEHICLEGROUPCODE.Name = "VEHICLEGROUPCODE";
            this.VEHICLEGROUPCODE.Visible = true;
            this.VEHICLEGROUPCODE.VisibleIndex = 11;
            // 
            // DRIVERNAME
            // 
            this.DRIVERNAME.Caption = "기사명";
            this.DRIVERNAME.FieldName = "DRIVERNAME";
            this.DRIVERNAME.Name = "DRIVERNAME";
            this.DRIVERNAME.Visible = true;
            this.DRIVERNAME.VisibleIndex = 12;
            // 
            // DRIVERMOBILE
            // 
            this.DRIVERMOBILE.Caption = "기사휴대폰";
            this.DRIVERMOBILE.FieldName = "DRIVERMOBILE";
            this.DRIVERMOBILE.Name = "DRIVERMOBILE";
            this.DRIVERMOBILE.Visible = true;
            this.DRIVERMOBILE.VisibleIndex = 13;
            // 
            // ROTATIONNUMBER
            // 
            this.ROTATIONNUMBER.Caption = "회전수";
            this.ROTATIONNUMBER.FieldName = "ROTATIONNUMBER";
            this.ROTATIONNUMBER.Name = "ROTATIONNUMBER";
            this.ROTATIONNUMBER.Visible = true;
            this.ROTATIONNUMBER.VisibleIndex = 14;
            // 
            // DISPATCHMEMO
            // 
            this.DISPATCHMEMO.Caption = "배차메모";
            this.DISPATCHMEMO.FieldName = "DISPATCHMEMO";
            this.DISPATCHMEMO.Name = "DISPATCHMEMO";
            this.DISPATCHMEMO.Visible = true;
            this.DISPATCHMEMO.VisibleIndex = 15;
            // 
            // TRANSACTIONFLAG
            // 
            this.TRANSACTIONFLAG.Caption = "트랜잭션FLAG";
            this.TRANSACTIONFLAG.FieldName = "TRANSACTIONFLAG";
            this.TRANSACTIONFLAG.Name = "TRANSACTIONFLAG";
            this.TRANSACTIONFLAG.Visible = true;
            this.TRANSACTIONFLAG.VisibleIndex = 16;
            // 
            // REGISTERAT
            // 
            this.REGISTERAT.Caption = "생성일자";
            this.REGISTERAT.FieldName = "REGISTERAT";
            this.REGISTERAT.Name = "REGISTERAT";
            this.REGISTERAT.Visible = true;
            this.REGISTERAT.VisibleIndex = 17;
            // 
            // REGISTERBY
            // 
            this.REGISTERBY.Caption = "생성자";
            this.REGISTERBY.FieldName = "REGISTERBY";
            this.REGISTERBY.Name = "REGISTERBY";
            this.REGISTERBY.Visible = true;
            this.REGISTERBY.VisibleIndex = 18;
            // 
            // ORDERTYPECODE
            // 
            this.ORDERTYPECODE.Caption = "오더유형";
            this.ORDERTYPECODE.FieldName = "ORDERTYPECODE";
            this.ORDERTYPECODE.Name = "ORDERTYPECODE";
            this.ORDERTYPECODE.Visible = true;
            this.ORDERTYPECODE.VisibleIndex = 19;
            // 
            // FROMLOCATIONCODE
            // 
            this.FROMLOCATIONCODE.Caption = "출하센터";
            this.FROMLOCATIONCODE.FieldName = "FROMLOCATIONCODE";
            this.FROMLOCATIONCODE.Name = "FROMLOCATIONCODE";
            this.FROMLOCATIONCODE.Visible = true;
            this.FROMLOCATIONCODE.VisibleIndex = 20;
            // 
            // TOLOCATIONCODE
            // 
            this.TOLOCATIONCODE.Caption = "배송처";
            this.TOLOCATIONCODE.FieldName = "TOLOCATIONCODE";
            this.TOLOCATIONCODE.Name = "TOLOCATIONCODE";
            this.TOLOCATIONCODE.Visible = true;
            this.TOLOCATIONCODE.VisibleIndex = 21;
            // 
            // SOLDTOCODE
            // 
            this.SOLDTOCODE.Caption = "거래처";
            this.SOLDTOCODE.FieldName = "SOLDTOCODE";
            this.SOLDTOCODE.Name = "SOLDTOCODE";
            this.SOLDTOCODE.Visible = true;
            this.SOLDTOCODE.VisibleIndex = 22;
            // 
            // DELIVERYSEQUENCE
            // 
            this.DELIVERYSEQUENCE.Caption = "배송순서";
            this.DELIVERYSEQUENCE.FieldName = "DELIVERYSEQUENCE";
            this.DELIVERYSEQUENCE.Name = "DELIVERYSEQUENCE";
            this.DELIVERYSEQUENCE.Visible = true;
            this.DELIVERYSEQUENCE.VisibleIndex = 23;
            // 
            // ITEMCODE
            // 
            this.ITEMCODE.Caption = "상품코드";
            this.ITEMCODE.FieldName = "ITEMCODE";
            this.ITEMCODE.Name = "ITEMCODE";
            this.ITEMCODE.Visible = true;
            this.ITEMCODE.VisibleIndex = 24;
            // 
            // PLANQTY
            // 
            this.PLANQTY.Caption = "계획수량";
            this.PLANQTY.FieldName = "PLANQTY";
            this.PLANQTY.Name = "PLANQTY";
            this.PLANQTY.Visible = true;
            this.PLANQTY.VisibleIndex = 25;
            // 
            // XDATS
            // 
            this.XDATS.Caption = "인터페이스 일시";
            this.XDATS.FieldName = "XDATS";
            this.XDATS.Name = "XDATS";
            this.XDATS.Visible = true;
            this.XDATS.VisibleIndex = 26;
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1424, 28);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(36, 42);
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
            this.btn_rowDel.Location = new System.Drawing.Point(1464, 28);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(36, 42);
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
            this.layoutControlItem14,
            this.layoutControlItem7,
            this.layoutControlItem3,
            this.layoutControlItem22});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(1758, 618);
            this.layoutControlGroup.Text = "주문현황";
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
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(1416, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
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
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 22, 2, 2);
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
            // frm_Orders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_Orders.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frm_Orders";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "주문현황";
            this.Load += new System.EventHandler(this.frm_Oders_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_Orders_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).EndInit();
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
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem22;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn VEHICLETONCODE;
        private DevExpress.XtraGrid.Columns.GridColumn VEHICLEGROUPCODE;
        private DevExpress.XtraGrid.Columns.GridColumn DRIVERNAME;
        private DevExpress.XtraGrid.Columns.GridColumn DRIVERMOBILE;
        private DevExpress.XtraGrid.Columns.GridColumn ROTATIONNUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn DISPATCHMEMO;
        private DevExpress.XtraGrid.Columns.GridColumn TRANSACTIONFLAG;
        private DevExpress.XtraGrid.Columns.GridColumn REGISTERAT;
        private DevExpress.XtraGrid.Columns.GridColumn REGISTERBY;
        private DevExpress.XtraGrid.Columns.GridColumn ORDERTYPECODE;
        private DevExpress.XtraGrid.Columns.GridColumn FROMLOCATIONCODE;
        private DevExpress.XtraGrid.Columns.GridColumn TOLOCATIONCODE;
        private DevExpress.XtraGrid.Columns.GridColumn SOLDTOCODE;
        private DevExpress.XtraGrid.Columns.GridColumn DELIVERYSEQUENCE;
        private DevExpress.XtraGrid.Columns.GridColumn ITEMCODE;
        private DevExpress.XtraGrid.Columns.GridColumn PLANQTY;
        private DevExpress.XtraGrid.Columns.GridColumn XDATS;
    }
}