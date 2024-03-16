using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.RowData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.RowValidate
{
    public abstract class AbstractRowDataHandler<T>
        //(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<T> nextHandler)  // >= v12.0
        : AbstractUserRowHandler<T>
        //(colorIndex, counts, nextHandler) 
        where T : AbstractRowData
    {
        protected int ColIndex { get; set; } // = colIndex;

        protected AbstractRowDataHandler(int colIndex, short colorIndex, Dictionary<short, int> counts, AbstractRowDataHandler<T> nextHandler) : base(colorIndex, counts, nextHandler)
        {
            ColIndex = colIndex;
        }
    }
}