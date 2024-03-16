namespace MSUnitTestProject.UserInfoUpdate.Handler
{
    public interface IUserRowHandler<T>
    {
        bool Handle(HandleContext<T> context);
    }
}
