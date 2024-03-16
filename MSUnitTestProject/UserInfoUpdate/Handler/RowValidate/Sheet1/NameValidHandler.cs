using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;
using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet1
{
    public class NameValidHandler
        //(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<MDS_WHSRowData> nextHandler) 
        : AbstractRowDataHandler<Sheet1RowData>
    //(colIndex, colorIndex, counts, nextHandler)
    {
        public NameValidHandler(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<Sheet1RowData> nextHandler) : base(colIndex, colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<Sheet1RowData> context)
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

            Sheet1RowData rowData = context.Data;
            rowData.Name = name.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.Job = row.GetCell(1)?.StringCellValue?.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.Phone = row.GetCell(2)?.StringCellValue?.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            rowData.Telephone = row.GetCell(3)?.StringCellValue?.Trim().Replace(" ", "").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
            rowData.Email = row.GetCell(4)?.StringCellValue?.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

            return base.Handle(context);
        }
    }
}