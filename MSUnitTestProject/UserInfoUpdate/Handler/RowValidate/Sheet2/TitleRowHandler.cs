using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;
using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet2
{
    public class TitleRowHandler
        //(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<OffShoreRowData> nextHandler) 
        : AbstractRowDataHandler<Sheet2RowData>
    //(colIndex, colorIndex, counts, nextHandler)
    {
        public TitleRowHandler(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<Sheet2RowData> nextHandler) : base(colIndex, colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<Sheet2RowData> context)
        {
            IRow row = context.Row;
            string name = row.GetCell(ColIndex)?.StringCellValue;
            if (!string.IsNullOrWhiteSpace(name) && name == "姓名")
            {
                Count();
                SetRowBgColor(row);
                Console.WriteLine("Title row: " + row.RowNum + ".");
                return false;
            }
            return base.Handle(context);
        }
    }
}