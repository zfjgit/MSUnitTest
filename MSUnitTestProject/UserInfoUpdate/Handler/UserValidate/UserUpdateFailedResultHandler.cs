using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.UserValidate
{
    public class UserUpdateFailedResultHandler
        //(int result, short colorIndex, Dictionary<short, int> counts, AbstractUserHandler<IList<RBAC_UserModel>> nextHandler) 
        : AbstractUserHandler<IList<User>>
    //(colorIndex, counts, nextHandler)
    {
        public UserUpdateFailedResultHandler(short colorIndex, Dictionary<short, int> counts, AbstractUserRowHandler<IList<User>> nextHandler) : base(colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<IList<User>> context)
        {
            if (context.ResultCount <= 0)
            {
                Count();
                SetRowBgColor(context.Row);
                Console.WriteLine("Failed to update user: " + context.Name);
                return false;
            }

            return base.Handle(context);
        }
    }
}