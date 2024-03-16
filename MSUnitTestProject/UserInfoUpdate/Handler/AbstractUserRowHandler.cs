using System.Collections.Generic;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler
{
    public abstract class AbstractUserRowHandler<T> : IUserRowHandler<T>, IHandleResultAction
    {
        protected short ColorIndex { get; set; }
        protected Dictionary<short, int> Counts { get; set; }
        protected AbstractUserRowHandler<T> NextHandler { get; set; }

        protected AbstractUserRowHandler(short colorIndex, Dictionary<short, int> counts, AbstractUserRowHandler<T> nextHandler)
        {
            Counts = counts;
            ColorIndex = colorIndex;
            NextHandler = nextHandler;
        }

        public virtual bool Handle(HandleContext<T> context)
        {
            return NextHandler == null || NextHandler.Handle(context);
        }

        public void Count()
        {
            if (Counts.ContainsKey(ColorIndex))
            {
                Counts[ColorIndex]++;
            }
            else
            {
                Counts.Add(ColorIndex, 1);
            }
        }

        public void SetRowBgColor(IRow row)
        {
            row.Height = 500;
            row.RowStyle = row.Sheet.Workbook.CreateCellStyle();

            IFont font = row.RowStyle.GetFont(row.Sheet.Workbook);
            font.Color = HSSFColor.White.Index;
            font.IsBold = true;

            for (int i = 0; i < row.LastCellNum; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell != null)
                {
                    cell.CellStyle = row.RowStyle;
                    cell.CellStyle.SetFont(font);
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;

                    cell.CellStyle.FillForegroundColor = ColorIndex;
                    cell.CellStyle.FillPattern = FillPattern.SolidForeground;
                }
            }
        }
    }
}