using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler
{
    public interface IHandleResultAction
    {
        void Count();
        void SetRowBgColor(IRow row);
    }
}
