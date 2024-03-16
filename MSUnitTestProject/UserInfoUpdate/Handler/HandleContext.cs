using NPOI.SS.UserModel;

namespace MSUnitTestProject.UserInfoUpdate.Handler
{
    public class HandleContext<T>
    {
        public IRow Row { get; set; }
        public T Data { get; set; }
        public string Name { get; set; }
        public int ResultCount { get; set; }

        public HandleContext(IRow row, T data)
        {
            Row = row;
            Data = data;
        }
        public HandleContext(IRow row, T data, string name)
        {
            Row = row;
            Data = data;
            Name = name;
        }
    }
}
