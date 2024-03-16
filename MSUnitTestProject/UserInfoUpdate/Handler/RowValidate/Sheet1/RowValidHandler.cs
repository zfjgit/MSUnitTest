using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate.Sheet1
{
    public class RowValidHandler
        //(int colCount, int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<MDS_WHSRowData> nextHandler) 
        : AbstractRowDataHandler<Sheet1RowData>
    //(colIndex, colorIndex, counts, nextHandler)
    {
        protected int ColCount { get; set; }
        public RowValidHandler(int colCount, int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<Sheet1RowData> nextHandler) : base(colIndex, colorIndex, counts, nextHandler)
        {
            ColCount = colCount;
        }

        public override bool Handle(HandleContext<Sheet1RowData> context)
        {
            if (context.Row == null || context.Row.LastCellNum < ColCount) //null is when the row only contains empty cells
            {
                Count();
                SetRowBgColor(context.Row);
                Console.WriteLine("Row not fullfilled row: " + context.Row.RowNum + ".");
                return false;
            }
            return base.Handle(context);
        }
    }
}