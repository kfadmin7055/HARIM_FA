using Core.Extension;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Core.Class
{
    public class clsDevexpressUtil
    {
        public static void ButtonSkinChange(SimpleButton btn, string skinName)
        {
            btn.LookAndFeel.SkinName = skinName;
            btn.LookAndFeel.UseDefaultLookAndFeel = false;
        }

        public static void ItemLookUpStandardEditSetup(DevExpress.XtraEditors.LookUpEdit lookupEdit, DataTable dt)
        {
            ItemLookUpEditSetup(lookupEdit, dt, "", false, -1, false, false, true);
        }

        public static void ItemLookUpEditSetup(DevExpress.XtraEditors.LookUpEdit lookupEdit, DataSet ds)
        {
            ItemLookUpEditSetup(lookupEdit, ds.Tables[0]);
        }

        public static void ItemLookUpEditSetup(DevExpress.XtraEditors.LookUpEdit lookupEdit, DataTable dt, string nullText = "", bool codeVisible = false, int defaultidx = -1, bool allSelect = false, bool headerView = false, bool standard = false, string[] viewDisplay = null, string addColCode = null, string sValueMember = "CODE", string sDisplayMember = "NAME")
        {
            try
            {
                lookupEdit.Properties.DataSource = null;
                if (lookupEdit.Properties.DataSource != null)
                {
                    lookupEdit.EditValue = DBNull.Value;
                }

                DataTable clonDt = dt.Clone();

                if (allSelect.Equals(true))
                {
                    clonDt.Rows.Add("", "전체선택");
                }

                clonDt.Merge(dt);

                lookupEdit.Properties.DataSource = clonDt;
                lookupEdit.Properties.DisplayMember = sDisplayMember;
                lookupEdit.Properties.ValueMember = sValueMember;


                if (viewDisplay != null && viewDisplay.Length > 0)
                {
                    lookupEdit.Properties.Columns.Clear();
                    lookupEdit.Properties.Columns.Add(new LookUpColumnInfo("CODE", viewDisplay[0] == null ? "코드" : viewDisplay[0]));
                    // 표시할 컬럼 추가
                    lookupEdit.Properties.Columns.Add(new LookUpColumnInfo("NAME", viewDisplay[1] == null ? "명칭" : viewDisplay[1]));

                    if (viewDisplay.Length == 3)
                        lookupEdit.Properties.Columns.Add(new LookUpColumnInfo("PER", viewDisplay[2] == null ? "비율" : viewDisplay[2]));
                }
                else
                {
                    lookupEdit.Properties.PopulateColumns();
                }

                if (!nullText.IsNullEmpty() && clonDt != null && clonDt.Rows.Count == 0)
                {
                    lookupEdit.Properties.NullText = nullText;

                    defaultidx = -1;
                }

                if (defaultidx >= 0 && clonDt.Rows.Count > 0)
                    lookupEdit.ItemIndex = defaultidx;
                else
                    lookupEdit.ItemIndex = -1;

                if (lookupEdit.Properties.Columns["REF_1"] != null)
                {
                    lookupEdit.Properties.Columns["REF_1"].Visible = codeVisible;
                    lookupEdit.Properties.Columns["REF_2"].Visible = codeVisible;
                }

                if (lookupEdit.Properties.Columns[lookupEdit.Properties.ValueMember] != null)
                    lookupEdit.Properties.Columns[lookupEdit.Properties.ValueMember].Visible = codeVisible;

                if (!headerView)
                {
                    lookupEdit.Properties.ShowHeader = false;
                }

                if (standard)
                {
                    lookupEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                }

                lookupEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                lookupEdit.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
                lookupEdit.Properties.AutoSearchColumnIndex = 2; // DisplayMember 컬럼 인덱스

            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog("리스트박스를 불러오는 도중 에러가 발생했습니다", null, "ItemLookUpEditSqlSetup", ex);
            }
        }

        public static void ItemLookUpEditSqlSetup(DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lookupEdit, string SQL)
        {
            try
            {
                DataSet ListDs = Dbconn.conn.ExecutDataset(SQL);
                lookupEdit.DataSource = ListDs.Tables[0];
                lookupEdit.DisplayMember = "NAME";
                lookupEdit.ValueMember = "CODE";
                ListDs.Dispose();
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog("리스트박스를 불러오는 도중 에러가 발생했습니다", null, "ItemLookUpEditSqlSetup", ex);
            }
        }

        public static void ItemSearchLookUpEditSetup(DevExpress.XtraEditors.SearchLookUpEdit lookupEdit, DataTable dt, string nullText = "", bool bCodeVisible = true, Dictionary<string, string> addCol = null, string sValueMember = "CODE", string sDisplayMember = "NAME", string sValueName = "코드", string sDisplayName = "이름")
        {
            try
            {
                lookupEdit.Properties.DataSource = null;
                lookupEdit.EditValue = null;
                lookupEdit.Properties.View.OptionsCustomization.AllowFilter = true;
                lookupEdit.Properties.View.ActiveFilter.Clear();

                lookupEdit.Properties.DataSource = dt;
                lookupEdit.Properties.DisplayMember = sDisplayMember;
                lookupEdit.Properties.ValueMember = sValueMember;
                lookupEdit.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.None;
                lookupEdit.Properties.View.OptionsView.ColumnAutoWidth = true;

                lookupEdit.Properties.View = new GridView();
                lookupEdit.Properties.View.Columns.Clear();
                lookupEdit.Properties.View.Columns.AddVisible(sValueMember, sValueName);
                lookupEdit.Properties.View.Columns.AddVisible(sDisplayMember, sDisplayName);

                if (addCol != null)
                {
                    foreach (var item in addCol)
                    {
                        lookupEdit.Properties.View.Columns.AddVisible(item.Key, item.Value);
                    }
                }

                if (!bCodeVisible)
                {
                    // 컬럼 숨기기
                    var view = lookupEdit.Properties.View;
                    view.Columns["CODE"].Visible = false;
                }

                lookupEdit.Properties.View.OptionsView.ShowAutoFilterRow = true;
                lookupEdit.Properties.View.OptionsCustomization.AllowFilter = true;
                
                lookupEdit.Properties.NullValuePrompt = "제품정보를 찾을수 없습니다";
                lookupEdit.Properties.NullText = nullText;
                lookupEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog("리스트박스를 불러오는 도중 에러가 발생했습니다", null, "ItemLookUpEditSqlSetup", ex);
            }
        }

        public static void SetCheckedComboBoxEdit(DevExpress.XtraEditors.CheckedComboBoxEdit chkCboEdit, DataTable dt, string nullText = "", bool codeVisible = false, int defaultidx = -1, bool allSelect = false, bool headerView = false)
        {
            try
            {
                DataTable clonDt = dt.Clone();

                if (allSelect.Equals(true))
                {
                    clonDt.Rows.Add("", "전체선택");
                }

                clonDt.Merge(dt);

                chkCboEdit.Properties.DataSource = clonDt;
                chkCboEdit.Properties.DisplayMember = "NAME";
                chkCboEdit.Properties.ValueMember = "CODE";

                if (!nullText.IsNullEmpty() && clonDt != null && clonDt.Rows.Count == 0)
                {
                    defaultidx = -1;
                }

                chkCboEdit.CheckAll();
            }
            catch (Exception ex)
            {
                ShowMessageBox.XtraShowErrorLog("리스트박스를 불러오는 도중 에러가 발생했습니다", null, "ItemLookUpEditSqlSetup", ex);
            }
        }
    }
}
