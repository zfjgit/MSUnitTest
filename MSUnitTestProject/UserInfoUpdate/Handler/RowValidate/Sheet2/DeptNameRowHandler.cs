using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;
using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet2
{
    public class DeptNameRowHandler
        //(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<OffShoreRowData> nextHandler) 
        : AbstractRowDataHandler<Sheet2RowData>
    //(colIndex, colorIndex, counts, nextHandler)
    {
        public DeptNameRowHandler(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<Sheet2RowData> nextHandler) : base(colIndex, colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<Sheet2RowData> context)
        {
            IRow row = context.Row;
            string enName = row.GetCell(ColIndex).StringCellValue?.Trim().Replace("'", "") ?? "";
            if (string.IsNullOrWhiteSpace(enName))
            {
                Count();
                SetRowBgColor(row);
                Console.WriteLine("Dept name row, row: " + row.RowNum + ".");
                return false;
            }

            return base.Handle(context);
        }
    }
}