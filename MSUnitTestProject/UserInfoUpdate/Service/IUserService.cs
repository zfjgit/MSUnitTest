using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Service
{
    public interface IUserService
    {
        IList<User> FindUsers(IList<string> names);
        int Save(User user);
    }
}
