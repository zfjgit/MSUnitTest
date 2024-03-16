using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet1
{
    public class DeptNameRowHandler : AbstractRowDataHandler<Sheet1RowData>
    {
        public DeptNameRowHandler(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<Sheet1RowData> nextHandler) : base(colIndex, colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<Sheet1RowData> context)
        {
            string job = context.Row.GetCell(ColIndex).StringCellValue?.Trim().Replace("'", "") ?? "";
            if (string.IsNullOrWhiteSpace(job))
            {
                Count();
                SetRowBgColor(context.Row);
                Console.WriteLine("Dept name row, row: " + context.Row.RowNum + ".");
                return false;
            }

            context.Data.Job = job.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

            return base.Handle(context);
        }
    }
}