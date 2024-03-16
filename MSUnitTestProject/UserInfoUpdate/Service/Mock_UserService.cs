using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Service
{
    public class Mock_UserService : IUserService
    {
        public IList<User> FindUsers(IList<string> names)
        {
            var users = new List<User>
            {
                new User { Name = "test1", CompanyID = 1, CnName = "test1", FullName = "test1" },
                new User { Name = "test2", CompanyID = 2, CnName = "test2", FullName = "test2" },
                new User { Name = "test3", CompanyID = 1, CnName = "test3", FullName = "test3" },
                new User { Name = "test4", CompanyID = 1, CnName = "test4", FullName = "test4" },
                new User { Name = "test5", CompanyID = 5, CnName = "test5", FullName = "test5" },
                new User { Name = "test6", CompanyID = 6, CnName = "test6", FullName = "test6" },
                new User { Name = "test7", CompanyID = 1, CnName = "test7", FullName = "test7" },
                new User { Name = "test8", CompanyID = 8, CnName = "test8", FullName = "test8" },
                new User { Name = "test9", CompanyID = 9, CnName = "test9", FullName = "test9" },
                new User { Name = "test10", CompanyID = 1, CnName = "test10", FullName = "test10" },
                new User { Name = "test11", CompanyID = 1, CnName = "test11", FullName = "test11" },
                new User { Name = "test12", CompanyID = 12, CnName = "test12", FullName = "test12" },
                new User { Name = "test13", CompanyID = 1, CnName = "test13", FullName = "test13" },
                new User { Name = "test14", CompanyID = 1, CnName = "test14", FullName = "test14" },
                new User { Name = "test15", CompanyID = 15, CnName = "test15", FullName = "test15" }
            };

            return users;
        }

        public int Save(User user)
        {
            return 1;
        }

    }
}
