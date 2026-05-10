using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DevExpress.XtraExport.Helpers;
using System.Data;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraPrinting;
using Microsoft.Win32;
using System.IO;
using DevExpress.XtraReports.Parameters;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using Core.Extension;
using System.Reflection;
using DevExpress.XtraGrid.Columns;

namespace Core.Class
{
    public class clsDevexpressGrid
    {
        /// <summary>
        /// 그리드 컨트롤 바인드 하기 델리게이트
        /// </summary>
        /// <param name="pGridControl">그리드 컨트롤</param>
        /// <param name="pGridView">그리드 뷰</param>
        /// <param name="pDataSource">데이타 소스</param>
        private delegate void BindGridControlDelegate(GridControl pGridControl, GridView pGridView, object pDataSource, bool pAutoColumnFit, bool pFocusIndex);

        public static void ReadGridViewInit(GridView pGridView, float fontSize)
        {
            try
            {
                Font baseFont = new Font("맑은 고딕", fontSize);
                Font focusFont = new Font(
                    baseFont.FontFamily,
                    baseFont.Size + 1f,
                    baseFont.Style
                );

                pGridView.Appearance.Row.Font = baseFont;
                pGridView.Appearance.HeaderPanel.Font = baseFont;
                pGridView.Appearance.FooterPanel.Font = baseFont;

                pGridView.Appearance.FocusedRow.Font = focusFont;
                pGridView.Appearance.FocusedCell.Font = focusFont;

                //pGridView.Appearance.Row.BackColor = Color.Yellow;
                pGridView.Appearance.FocusedRow.BackColor = Color.LightPink;
                pGridView.Appearance.SelectedRow.BackColor = Color.LightPink;
                pGridView.Appearance.FocusedCell.BackColor = Color.LightPink;
                pGridView.Appearance.SelectedRow.BackColor = Color.LightPink;
                pGridView.Appearance.HideSelectionRow.BackColor = Color.LightPink;

                pGridView.IndicatorWidth = 50;
                pGridView.OptionsBehavior.Editable = false;
                pGridView.OptionsCustomization.AllowColumnMoving = true;
                pGridView.OptionsCustomization.AllowSort = true;
                pGridView.OptionsCustomization.AllowFilter = true;
                pGridView.OptionsView.EnableAppearanceOddRow = true;

                pGridView.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(pGridView_PopupMenuShowing);

                pGridView.OptionsSelection.EnableAppearanceHotTrackedRow = DevExpress.Utils.DefaultBoolean.True;

                pGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "ReadGridViewInit", ex);
            }
        }

        public static void EditGridViewInit(GridView pGridView, float fontSize)
        {
            try
            {
                Font baseFont = new Font("맑은 고딕", fontSize);
                Font focusFont = new Font(
                    baseFont.FontFamily,
                    baseFont.Size + 1f,
                    baseFont.Style
                );

                pGridView.Appearance.Row.Font = baseFont;
                pGridView.Appearance.HeaderPanel.Font = baseFont;
                pGridView.Appearance.FooterPanel.Font = baseFont;

                pGridView.Appearance.FocusedRow.Font = focusFont;
                pGridView.Appearance.FocusedCell.Font = focusFont;

                //pGridView.RowHeight = (int)(fontSize * 2.6f);

                //pGridView.Appearance.Row.BackColor = Color.Yellow;
                pGridView.Appearance.FocusedRow.BackColor = Color.LightPink;
                pGridView.Appearance.SelectedRow.BackColor = Color.LightPink;
                pGridView.Appearance.FocusedCell.BackColor = Color.LightPink;
                pGridView.Appearance.SelectedRow.BackColor = Color.LightPink;
                pGridView.Appearance.HideSelectionRow.BackColor = Color.LightPink;

                pGridView.IndicatorWidth = 50;
                pGridView.OptionsCustomization.AllowColumnMoving = true;
                pGridView.OptionsCustomization.AllowSort = true;
                pGridView.OptionsCustomization.AllowFilter = true;
                pGridView.OptionsNavigation.EnterMoveNextColumn = true;
                pGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
                pGridView.OptionsView.EnableAppearanceOddRow = true;


                pGridView.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
                pGridView.NewItemRowText = "데이터를 추가하려면 이곳에 입력하여 주세요";
                pGridView.Appearance.TopNewRow.BackColor = Color.LightYellow;

                pGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(pGridView_RowCellStyle);
                pGridView.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(pGridView_PopupMenuShowing);

                pGridView.OptionsNavigation.AutoFocusNewRow = false;


                pGridView.OptionsSelection.EnableAppearanceHotTrackedRow = DevExpress.Utils.DefaultBoolean.True;
                pGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "EditGridViewInit", ex);
            }

        }

        public static void pGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView gridView = sender as GridView;
                if (gridView == null)
                {
                    return;
                }
                if (e.RowHandle >= 0)
                {
                    DataRow row = gridView.GetDataRow(e.RowHandle);
                    //new row style

                    if (row != null)
                    {
                        if (row.RowState == DataRowState.Added)
                        {
                            e.Appearance.BackColor = Color.LightYellow;
                            //e.Appearance.BackColor2 = Color.LightGoldenrodYellow;
                            //e.Appearance.FontStyleDelta = FontStyle.Bold;
                        }
                        //modify row style
                        else if (row.RowState == DataRowState.Modified)
                        {
                            e.Appearance.FontStyleDelta = FontStyle.Bold;

                        }
                    }

                    //if (e.Column.OptionsColumn.ReadOnly)
                    //e.Appearance.BackColor = Color.FromArgb(223,223,223);
                }



                if (gridView.Tag != null)
                {
                    if (gridView.Tag.ToString().Equals("work_style"))
                    {
                        string condition = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["C_CONDITION"]);
                        if (condition == "완료" || condition == "강제완료") //완료
                        {
                            e.Appearance.BackColor = Color.LightGray;
                            e.Appearance.ForeColor = Color.Black;

                            e.Appearance.FontStyleDelta = FontStyle.Bold;
                        }
                        else if (condition == "진행")
                        {
                            e.Appearance.BackColor = Color.LawnGreen;
                            e.Appearance.ForeColor = Color.Black;
                            e.Appearance.FontStyleDelta = FontStyle.Bold;
                        }
                        else if (condition == "계획")
                        {
                            e.Appearance.BackColor = Color.Yellow;
                            e.Appearance.ForeColor = Color.Black;
                        }


                        if (e.RowHandle == gridView.FocusedRowHandle)
                        {
                            e.Appearance.BackColor = Color.LightPink;
                            e.Appearance.ForeColor = Color.Black;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "pGridView_RowCellStyle", ex);
            }

        }



        public static void pGridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            GridView view = sender as GridView;

            if (view == null)
                return;

            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                e.Menu.Items.Clear();
                DXMenuItem[] item = new DXMenuItem[5];

                item[0] = new DXMenuItem();
                item[0].ImageOptions.Image = Properties.Resources.exporttoxls_16x16;
                item[0].Tag = "xls";
                item[0].Caption = "엑셀파일로 내보내기 (xls)";

                e.Menu.Items.Add(item[0]);


                item[1] = new DXMenuItem();
                item[1].ImageOptions.Image = Properties.Resources.exporttoxls_16x16;
                item[1].Tag = "xlsx";
                item[1].Caption = "엑셀파일로 내보내기 (xlsx)";
                e.Menu.Items.Add(item[1]);


                item[2] = new DXMenuItem();
                item[2].ImageOptions.Image = Properties.Resources.exporttotxt_16x16;
                item[2].Tag = "txt";
                item[2].Caption = "TEXT파일로 내보내기 (txt)";
                e.Menu.Items.Add(item[2]);

                item[3] = new DXMenuItem();
                item[3].ImageOptions.Image = Properties.Resources.saveall_16x16;
                item[3].Caption = "그리드 레이아웃 저장";
                e.Menu.Items.Add(item[3]);

                item[4] = new DXMenuItem();
                item[4].ImageOptions.Image = Properties.Resources.saveandclose_16x16;
                item[4].Caption = "그리드 레이아웃 초기화";
                e.Menu.Items.Add(item[4]);

                void Item_XlsExport_Click(object ss, EventArgs ee)
                {
                    DXMenuItem menu = ss as DXMenuItem;

                    SaveFileDialog sfd = new SaveFileDialog();

                    string saveDlgfilter = string.Empty;
                    string ext = menu.Tag.ToString();

                    switch (ext)
                    {
                        case "xls": saveDlgfilter = "XLS File(*.xls)|*.xls"; break;
                        case "xlsx": saveDlgfilter = "XLSX File(*.xlsx)|*.xlsx"; break;
                        case "txt": saveDlgfilter = "Text File(*.txt)|*.txt"; break;
                    }

                    // 🔴 여기부터 핵심
                    Form frm = view.GridControl.FindForm();
                    string formName = frm?.Text ?? frm?.Name ?? "Export";

                    // 파일명에 사용할 수 없는 문자 제거
                    foreach (char c in Path.GetInvalidFileNameChars())
                        formName = formName.Replace(c.ToString(), "");

                    sfd.FileName = $"{formName}_{DateTime.Now:yyyyMMdd_HHmmss}.{ext}";

                    sfd.Filter = saveDlgfilter;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        DevExpress.Export.ExportSettings.DefaultExportType =
                            DevExpress.Export.ExportType.Default;

                        switch (ext)
                        {
                            case "xls": view.ExportToXls(sfd.FileName); break;
                            case "xlsx": view.ExportToXlsx(sfd.FileName); break;
                            case "txt": view.ExportToText(sfd.FileName); break;
                        }

                        if (DialogResult.Yes == ShowMessageBox.Confirm("파일을 바로 여시겠습니까?"))
                            System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }

                void ClearGridColumn_Click(object ss, EventArgs ee)
                {
                    // 🔴 여기부터 핵심
                    Form frm = view.GridControl.FindForm();
                    string formName = frm?.Name;

                    // 파일명에 사용할 수 없는 문자 제거
                    foreach (char c in Path.GetInvalidFileNameChars())
                        formName = formName.Replace(c.ToString(), "");

                    ClearGridColumnSeq(formName, view.GridControl, view);
                }

                void SaveGridColumn_Click(object ss, EventArgs ee)
                {
                    // 🔴 여기부터 핵심
                    Form frm = view.GridControl.FindForm();
                    string formName = frm?.Name;

                    // 파일명에 사용할 수 없는 문자 제거
                    foreach (char c in Path.GetInvalidFileNameChars())
                        formName = formName.Replace(c.ToString(), "");

                    SaveGridColumnSeq(formName, view.GridControl, view);
                }


                item[0].Click += Item_XlsExport_Click;
                item[1].Click += Item_XlsExport_Click;
                item[2].Click += Item_XlsExport_Click;
                item[3].Click += SaveGridColumn_Click;
                item[4].Click += ClearGridColumn_Click;
            }
        }

        public static void GridEndEdit(GridView pGridView)
        {
            if (pGridView == null)
                return;

            if (pGridView.IsEditing)
            {
                pGridView.GetFocusedDataRow().EndEdit();
            }
            pGridView.ClearColumnErrors();
            pGridView.CloseEditor();
            pGridView.UpdateCurrentRow();
        }

        public static void GridCancelUpdate(GridView pGridView)
        {
            if (pGridView == null)
                return;

            if (pGridView.IsEditing)
            {
                pGridView.GetFocusedDataRow().CancelEdit();
            }

            pGridView.ClearColumnErrors();
            pGridView.CloseEditor();
            pGridView.CancelUpdateCurrentRow();
        }

        #region 초기화 시작하기 - BeginInitialize(pGridView)

        /// <summary>
        /// 초기화 시작하기
        /// </summary>
        /// <param name="pGridView">그리드 뷰</param>
        public static void BeginInitialize(GridView pGridView)
        {
            try
            {
                ((ISupportInitialize)pGridView).BeginInit();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "EndInitialize", ex);
                ShowMessageBox.XtraShowError("에러가 발생했습니다");
            }
        }

        #endregion

        #region 초기화 종료하기 - EndInitialize(pGridView)

        /// <summary>
        /// 초기화 종료하기
        /// </summary>
        /// <param name="pGridView">그리드 뷰</param>
        public static void EndInitialize(GridView pGridView)
        {
            try
            {
                ((ISupportInitialize)pGridView).EndInit();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "EndInitialize", ex);
                ShowMessageBox.XtraShowError("에러가 발생했습니다");
            }
        }

        #endregion

        #region 그리드 컨트롤 바인드 하기 - BindGridControl(pGridControl, pGridView, pDataSource)

        /// <summary>
        /// 그리드 컨트롤 바인드 하기
        /// </summary>
        /// <param name="pGridControl">그리드 컨트롤</param>
        /// <param name="pGridView">그리드 뷰</param>
        /// <param name="pDataSource">데이타 소스</param>
        /// <param name="pAutoColumnFit">자동컬럼크기조정 Flag</param>
        /// <param name="pFocusIndex">그리드포커스위치 고정 Flag</param>
        public static void BindGridControl(GridControl pGridControl, GridView pGridView, object pDataSource, bool pAutoColumnFit = false, bool pFocusIndex = false)
        {
            try
            {
                if (pGridControl.InvokeRequired)
                {
                    BindGridControlDelegate pBindGridControlDelegate = new BindGridControlDelegate(BindGridControl);

                    pGridControl.Invoke(pBindGridControlDelegate, pGridControl, pGridView, pDataSource);
                }
                else
                {

                    try
                    {
                        var topRowIndex = pGridView.TopRowIndex;
                        var focusedRowHandle = pGridView.FocusedRowHandle;

                        pGridView.ShowLoadingPanel();
                        pGridControl.DataSource = null;
                        pGridControl.DataSource = pDataSource;
                        pGridView.HideLoadingPanel();

                        if (pAutoColumnFit)
                        {
                            pGridView.BestFitColumns();
                        }

                        if (pFocusIndex)
                        {
                            pGridView.FocusedRowHandle = focusedRowHandle;
                            pGridView.TopRowIndex = topRowIndex;
                        }

                    }
                    catch (Exception e)
                    {
                        clsLog.logSave("clsDevexpressGrid", "BindGridControl", e);
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "BindGridControl", ex);
                ShowMessageBox.XtraShowError("에러가 발생했습니다");
            }
        }

        #endregion

        public static bool GridDeleteCheck(GridView pGridView)
        {

            GridEndEdit(pGridView);

            if (pGridView.SelectedRowsCount == 0)
            {
                return false;
            }

            DataRow row = pGridView.GetDataRow(pGridView.FocusedRowHandle);

            if (row.RowState == DataRowState.Added)
            {
                return false;
            }

            return true;
        }
        public static void GridViewAddRow(GridView pGridView)
        {
            try
            {
                pGridView.CloseEditor();
                pGridView.UpdateCurrentRow();
                pGridView.AddNewRow();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewAddRow", ex);
                ShowMessageBox.XtraShowError("행을 추가하는 도중 에러가 발생했습니다");
            }
        }

        public static void GridViewLastAddRowDelete(GridView pGridView)
        {
            try
            {
                if (pGridView.RowCount == 0)
                {
                    return;
                }

                GridViewInfo viewInfo = pGridView.GetViewInfo() as GridViewInfo;

                DataRow row = pGridView.GetDataRow(viewInfo.RowsInfo.Last().RowHandle);

                if (row.RowState == DataRowState.Added)
                {

                    pGridView.DeleteRow(viewInfo.RowsInfo.Last().RowHandle);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewLastAddRowDelete", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        public static void GridViewAddRowDelete(GridView pGridView)
        {
            try
            {
                if (pGridView.RowCount == 0)
                {
                    return;
                }


                DataRow row = pGridView.GetDataRow(pGridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    pGridView.DeleteRow(pGridView.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewLastAddRowDelete", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        public static void BindGridLoopUpEdit(GridLookUpEdit pLookUpControl, DataTable pDataSource)
        {
            try
            {
                if (pLookUpControl.InvokeRequired)
                {
                    BindGridControlDelegate pBindGridControlDelegate = new BindGridControlDelegate(BindGridControl);

                    pLookUpControl.Invoke(pBindGridControlDelegate, pLookUpControl, pDataSource);
                }
                else
                {
                    pLookUpControl.Properties.PopupView.OptionsBehavior.AutoPopulateColumns = false;
                    pLookUpControl.Properties.DataSource = pDataSource;
                    pLookUpControl.Properties.DisplayMember = "NAME";
                    pLookUpControl.Properties.ValueMember = "CODE";
                    pLookUpControl.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "BindGridLoopUpEdit", ex);
                ShowMessageBox.XtraShowError("에러가 발생했습니다");
            }
        }

        public static void ItemLookUpEditSetup(DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lookupEdit, DataTable dt, string nullText = "", bool codeView = true, bool headerView = true, string[] headerDisplay = null, Dictionary<string, string> addCol = null, string sValueMember = "CODE", string sDisplayMember = "NAME")
        {
            try
            {
                //lookupEdit.Columns.Add(new LookUpColumnInfo("CODE", "CODE"));  // 추가 컬럼
                //lookupEdit.Columns.Add(new LookUpColumnInfo("NAME", "NAME"));  // 추가 컬럼

                lookupEdit.Columns.Clear();

                lookupEdit.DataSource = dt;
                lookupEdit.DisplayMember = sDisplayMember;
                lookupEdit.ValueMember = sValueMember;
                lookupEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;

                lookupEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                lookupEdit.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;

                lookupEdit.NullValuePrompt = "제품정보를 찾을수 없습니다";
                lookupEdit.NullText = nullText;

                lookupEdit.Columns.Add(new LookUpColumnInfo("CODE", headerDisplay == null ? "코드" : headerDisplay[0]));
                // 표시할 컬럼 추가
                lookupEdit.Columns.Add(new LookUpColumnInfo("NAME", headerDisplay == null ? "명칭" : headerDisplay[1]));

                if (!nullText.IsNullEmpty() && dt != null && dt.Rows.Count == 0)
                {
                    lookupEdit.NullText = nullText;
                }

                if (!codeView)
                {
                    // ID 컬럼 추가하고 숨김 처리
                    lookupEdit.Columns["CODE"].Visible = false;
                }

                if (!headerView)
                {
                    lookupEdit.ShowHeader = false;
                }

                if (addCol != null)
                {
                    foreach (var item in addCol)
                    {
                        lookupEdit.Columns.Add(new LookUpColumnInfo(item.Key, item.Value));  // 추가 컬럼
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "ItemLookUpEditSetup", ex);
                ShowMessageBox.XtraShowError("리스트박스를 불러오는 도중 에러가 발생했습니다");
            }
        }


        public static void ItemLookUpEditSetup(DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lookupEdit, string wkcd, string commcd)
        {
            try
            {
                string SQL = "SELECT COMM_DTCODE, COMM_DTNM FROM COMM_DTCODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}'  " +
                "ORDER BY DISPLAY_SEQ";
                SQL = string.Format(SQL, wkcd, commcd);

                DataSet dt = Dbconn.conn.ExecutDataset(SQL);

                lookupEdit.DataSource = dt.Tables[0];
                lookupEdit.DisplayMember = "COMM_DTNM";
                lookupEdit.ValueMember = "COMM_DTCODE";
                lookupEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                lookupEdit.NullText = "";
                lookupEdit.NullValuePrompt = "";

                dt.Dispose();


            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "ItemLookUpEditSetup", ex);
                ShowMessageBox.XtraShowError("리스트박스를 불러오는 도중 에러가 발생했습니다");
            }
        }

        public static void ItemSearchLookUpEditSetup(DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit lookupEdit, DataTable dt, string nullText = "", bool bCodeVisible = true, Dictionary<string, string> addCol = null, string sValueMember = "CODE", string sDisplayMember = "NAME", string sValueName = "코드", string sDisplayName = "이름")
        {
            try
            {
                lookupEdit.DataSource = dt;
                lookupEdit.DisplayMember = sDisplayMember;
                lookupEdit.ValueMember = sValueMember;
                lookupEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.None;
                lookupEdit.View.OptionsView.ColumnAutoWidth = true;

                lookupEdit.View = new GridView();
                lookupEdit.View.Columns.AddVisible(sValueMember, sValueName);
                lookupEdit.View.Columns.AddVisible(sDisplayMember, sDisplayName);

                if (addCol != null)
                {
                    foreach (var item in addCol)
                    {
                        lookupEdit.View.Columns.AddVisible(item.Key, item.Value);
                    }
                }

                if (!bCodeVisible)
                {
                    // 컬럼 숨기기
                    var view = lookupEdit.View;
                    view.Columns["CODE"].Visible = false;
                }


                lookupEdit.NullValuePrompt = "제품정보를 찾을수 없습니다";
                lookupEdit.NullText = nullText;
                lookupEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

                //lookupEdit.View.BestFitColumns();

                //lookupEdit.View.Columns["CODE"].Width = 400;
                //lookupEdit.View.Columns["NAME"].Width = 180;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "ItemLookUpEditSetup", ex);
                ShowMessageBox.XtraShowError("리스트박스를 불러오는 도중 에러가 발생했습니다");
            }
        }

        public static string GetFocusedRowCellValue(GridView pGridView, string filedNm)
        {
            string tmp = string.Empty;
            try
            {
                if (pGridView.GetFocusedRowCellValue(filedNm) == null)
                {
                    return "";
                }

                tmp = pGridView.GetFocusedRowCellValue(filedNm).ToString().Trim();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GetFocusedRowCellValue", ex);
                ShowMessageBox.XtraShowError("선택된 데이터를 읽는데 실패했습니다");
            }

            return tmp;
        }

        public static string GetFocusedRowDisplayText(GridView pGridView, string filedNm)
        {
            string tmp = string.Empty;
            try
            {
                if (pGridView.GetRowCellDisplayText(pGridView.FocusedRowHandle, pGridView.Columns[filedNm]) == null)
                {
                    return "";
                }

                tmp = pGridView.GetRowCellDisplayText(pGridView.FocusedRowHandle, pGridView.Columns[filedNm]).Trim();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GetFocusedRowCellValue", ex);
                ShowMessageBox.XtraShowError("선택된 데이터를 읽는데 실패했습니다");
            }

            return tmp;
        }

        public static int GetSelectRowCount(GridView pGridView)
        {
            int cnt = 0;
            try
            {
                cnt = pGridView.SelectedRowsCount;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GetSelectRowCount", ex);
                ShowMessageBox.XtraShowError("선택된 데이터를 읽는데 실패했습니다");
            }

            return cnt;
        }

        public static void ResetGridColumnSeq(string formName, GridControl pGridControl, GridView pGridView)
        {
            try
            {
                RegistryKey regKey1 = Registry.CurrentUser.CreateSubKey("DevExpress").CreateSubKey("GridLayouts");
                regKey1.DeleteSubKeyTree(formName + "_" + pGridControl.Name);
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "ResetGridColumnSeq", ex);
            }
        }

        /// <summary>
        /// 그리드디테일 행추가
        /// </summary>
        /// <param name="pGridView">그리드뷰</param>
        /// <param name="relationIndex">릴레이션 인덱스</param>
        public static void GridViewDetailAddRow(GridView pGridView, int relationIndex = 0)
        {
            try
            {
                GridView detailView = pGridView.GetDetailView(pGridView.FocusedRowHandle, relationIndex) as GridView;

                if (detailView == null) return;

                detailView.AddNewRow();
                detailView.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewDetailAddRow", ex);
                ShowMessageBox.XtraShowError("행을 추가하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 그리드디테일 행추가상태 마지막행 제거
        /// </summary>
        /// <param name="pGridView">그리드뷰</param>
        /// <param name="relationIndex">릴레이션 인덱스</param>
        public static void GridViewDetailLastAddRowDelete(GridView pGridView, int relationIndex = 0)
        {
            try
            {

                GridView detailView = pGridView.GetDetailView(pGridView.FocusedRowHandle, relationIndex) as GridView;

                if (detailView.RowCount == 0)
                {
                    return;
                }

                GridViewInfo viewInfo = pGridView.GetViewInfo() as GridViewInfo;

                DataRow row = detailView.GetDataRow(viewInfo.RowsInfo.Last().RowHandle);

                if (row.RowState == DataRowState.Added)
                {

                    detailView.DeleteRow(viewInfo.RowsInfo.Last().RowHandle);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewDetailLastAddRowDelete", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 그리드디테일 행추가상태 선택행 제거
        /// </summary>
        /// <param name="pGridView">그리드뷰</param>
        /// <param name="relationIndex">릴레이션 인덱스</param>
        public static void GridViewDetailAddRowDelete(GridView pGridView, int relationIndex = 0)
        {
            try
            {
                GridView detailView = pGridView.GetDetailView(pGridView.FocusedRowHandle, relationIndex) as GridView;

                if (detailView.RowCount == 0)
                {
                    return;
                }


                DataRow row = detailView.GetDataRow(detailView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    detailView.DeleteRow(detailView.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewDetailAddRowDelete", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 그리드디테일 선택된 필드 값읽기
        /// </summary>
        /// <param name="pGridView">그리드뷰</param>
        /// <param name="field">필드명</param>
        /// <param name="relationIndex">릴레이션 인덱스</param>
        /// <returns></returns>
        public static string GetDataGridViewDetail(GridView pGridView, string field, int relationIndex = 0)
        {
            try
            {
                GridView detailView = pGridView.GetDetailView(pGridView.FocusedRowHandle, relationIndex) as GridView;

                return detailView.GetRowCellValue(detailView.FocusedRowHandle, field).ToString();

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GetDataGridViewDetail", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
                return "";
            }
        }


        /// <summary>
        /// 그리드디테일 선택된 필드 값쓰기
        /// </summary>
        /// <param name="pGridView">그리드뷰</param>
        /// <param name="field">필드명</param>
        /// <param name="value">쓰기값</param>
        /// <param name="relationIndex">릴레이션 인덱스</param>
        public static void SetDataGridViewDetail(GridView pGridView, string field, string value, int relationIndex = 0)
        {
            try
            {
                GridView detailView = pGridView.GetDetailView(pGridView.FocusedRowHandle, relationIndex) as GridView;

                detailView.SetRowCellValue(detailView.FocusedRowHandle, field, value);

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "SetDataGridViewDetail", ex);
                ShowMessageBox.XtraShowError("행을 추가하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 그리드디테일 에디트모드 해제
        /// </summary>
        /// <param name="pGridView">그리드뷰</param>
        /// <param name="relationIndex">릴레이션 인덱스</param>

        public static void GridDetailEndEdit(GridView pGridView, int relationIndex = 0)
        {
            GridView detailView = pGridView.GetDetailView(pGridView.FocusedRowHandle, relationIndex) as GridView;

            if (detailView == null)
                return;

            if (detailView.IsEditing)
            {
                detailView.GetFocusedDataRow().EndEdit();
            }
            detailView.ClearColumnErrors();
            detailView.CloseEditor();
            detailView.UpdateCurrentRow();

        }

        /// <summary>
        /// 그리드 레이아웃 상태 불러오기
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="pGridControl"></param>
        /// <param name="pGridView"></param>
        public static void LoadGridColumnSeq(string formName, GridControl pGridControl, GridView pGridView)
        {
            try
            {
                //레지스트리 저장방식 (윈도우 호환성 문제 사용X)
                /*                
                    string regKey = "DevExpress\\GridLayouts\\" + formName + "_" + pGridControl.Name;
                    pGridView.BeginUpdate();
                    pGridView.RestoreLayoutFromRegistry(regKey);
                    pGridView.EndUpdate();
                */


                //파일저장방식
                DevExpress.Utils.OptionsLayoutGrid options = new DevExpress.Utils.OptionsLayoutGrid();
                options.StoreAppearance = true;

                string fileName = @"c:\kfMesLayout\" + formName + "_" + pGridControl.Name + "_" + pGridView.Name + "_" + "gridlayout.xml";

                if (File.Exists(fileName))
                {
                    pGridView.RestoreLayoutFromXml(fileName, options);
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// 그리드 레이아웃 상태 저장
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="pGridControl"></param>
        /// <param name="pGridView"></param>
        public static void SaveGridColumnSeq(string formName, GridControl pGridControl, GridView pGridView)
        {
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(@"C:\kfMesLayout");

                if (dinfo.Exists == false)
                {
                    dinfo.Create();
                }

                DevExpress.Utils.OptionsLayoutGrid options = new DevExpress.Utils.OptionsLayoutGrid();
                options.StoreAppearance = true;

                string fileName = @"c:\kfMesLayout\" + formName + "_" + pGridControl.Name + "_" + pGridView.Name + "_" + "gridlayout.xml";

                pGridView.SaveLayoutToXml(fileName, options);
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// 그리드 레이아웃 상태 삭제(초기화)
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="pGridControl"></param>
        /// <param name="pGridView"></param>
        public static void ClearGridColumnSeq(string formName, GridControl pGridControl, GridView pGridView)
        {
            try
            {
                string dirPath = @"C:\kfMesLayout";
                string fileName = Path.Combine(
                    dirPath,
                    $"{formName}_{pGridControl.Name}_{pGridView.Name}_gridlayout.xml"
                );

                if (File.Exists(fileName))
                    File.Delete(fileName);

                pGridView.BeginUpdate();

                // 정렬 / 필터 / 그룹 제거
                pGridView.ClearSorting();
                pGridView.ClearColumnsFilter();
                pGridView.ClearGrouping();

                // 컬럼 상태 초기화
                foreach (GridColumn col in pGridView.Columns)
                {
                    col.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                    col.VisibleIndex = col.AbsoluteIndex;
                }

                pGridView.EndUpdate();
            }
            catch (Exception ex)
            {
                clsLog.logSave(
                    MethodBase.GetCurrentMethod().ReflectedType.FullName,
                    MethodBase.GetCurrentMethod().Name,
                    ex
                );
            }
        }
    }
}
