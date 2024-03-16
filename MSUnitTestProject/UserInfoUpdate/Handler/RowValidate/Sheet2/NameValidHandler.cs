using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;
using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet2
{
    public class NameValidHandler
        //(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<OffShoreRowData> nextHandler) :
        : AbstractRowDataHandler<Sheet2RowData>
    //(colIndex, colorIndex, counts, nextHandler)
    {
        public NameValidHandler(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<Sheet2RowData> nextHandler) : base(colIndex, colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<Sheet2RowData> context)
        {
            IRow row = context.Row;
            string name = row.GetCell(ColIndex)?.StringCellValue?.Trim().Replace("'", "") ?? "";
            if (string.IsNullOrWhiteSpace(name))
            {
                Count();
                SetRowBgColor(row);
                Console.WriteLine("Name is empty, row: " + row.RowNum + ".");
                return false;
            }

            Sheet2RowData rowData = context.Data;
            rowData.Name = name.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.PyName = row.GetCell(1)?.StringCellValue?.Trim().Replace("'", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.Job = row.GetCell(2)?.StringCellValue?.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.Phone = row.GetCell(4)?.StringCellValue?.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.Telephone = row.GetCell(5)?.StringCellValue?.Trim().Replace(" ", "").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
            rowData.Email = row.GetCell(6)?.StringCellValue?.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

            return base.Handle(context);
        }
    }
}