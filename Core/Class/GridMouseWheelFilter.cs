using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Class
{
    public class GridMouseWheelFilter : IMessageFilter
    {
        private GridControl grid;

        public GridMouseWheelFilter(GridControl grid)
        {
            this.grid = grid;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_MOUSEWHEEL = 0x020A;

            if (m.Msg == WM_MOUSEWHEEL)
            {
                // 🔥 현재 마우스 위치
                Point mousePos = Control.MousePosition;

                // 🔥 Grid 영역 (화면 좌표)
                Rectangle gridRect = grid.RectangleToScreen(grid.ClientRectangle);

                // 👉 Grid 안에 있을 때만 처리
                if (!gridRect.Contains(mousePos))
                    return false;

                var view = grid.MainView as GridView;
                if (view == null)
                    return false;

                int delta = (short)((m.WParam.ToInt32() >> 16) & 0xffff);

                int step = 3;

                int newIndex = view.TopRowIndex + (delta > 0 ? -step : step);

                if (newIndex < 0)
                    newIndex = 0;

                if (newIndex >= view.RowCount)
                    newIndex = view.RowCount - 1;

                view.TopRowIndex = newIndex;

                return true; // 🔥 Grid일 때만 이벤트 먹기
            }

            return false;
        }
    }
}
