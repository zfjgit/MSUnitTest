using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.UserValidate
{
    public abstract class AbstractUserHandler<T>
        //(short colorIndex, Dictionary<short, int> counts, AbstractUserHandler<T> nextHandler)
        : AbstractUserRowHandler<T>
        //(colorIndex, counts, nextHandler) 
        where T : IList<User>
    {
        protected AbstractUserHandler(short colorIndex, Dictionary<short, int> counts, AbstractUserRowHandler<T> nextHandler) : base(colorIndex, counts, nextHandler)
        {
        }
    }
}