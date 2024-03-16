using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.UserValidate
{
    public class UserDuplicationHandler
        //(short colorIndex, Dictionary<short, int> counts, Dictionary<string, IList<RBAC_UserModel>> duplicateUsers, AbstractUserHandler<IList<RBAC_UserModel>> nextHandler) 
        : AbstractUserHandler<IList<User>>
    //(colorIndex, counts, nextHandler)
    {
        protected Dictionary<string, IList<User>> DuplicateUsers { get; set; }
        public UserDuplicationHandler(short colorIndex, Dictionary<short, int> counts, Dictionary<string, IList<User>> duplicateUsers, AbstractUserRowHandler<IList<User>> nextHandler) : base(colorIndex, counts, nextHandler)
        {
            DuplicateUsers = duplicateUsers;
        }

        private void GetDuplicateUsers(IList<User> users, string name)
        {
            foreach (var user in users)
            {
                if (user.CompanyID != 1)
                {
                    continue;
                }

                if (!DuplicateUsers.ContainsKey(name))
                {
                    DuplicateUsers.Add(name, new List<User>());
                }
                DuplicateUsers[name].Add(user);
            }
        }

        public override bool Handle(HandleContext<IList<User>> context)
        {
            if (context.Data != null && context.Data.Count > 1)
            {
                GetDuplicateUsers(context.Data, context.Name);

                if (DuplicateUsers[context.Name].Count > 1)
                {
                    Count();
                }
            }

            return base.Handle(context);
        }
    }
}