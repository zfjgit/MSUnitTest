using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.UserValidate
{
    public class UserUpdateSuccessResultHandler
        //(int result, short colorIndex, Dictionary<short, int> counts, AbstractUserHandler<IList<RBAC_UserModel>> nextHandler) 
        : AbstractUserHandler<IList<User>>
    //(colorIndex, counts, nextHandler)
    {
        public UserUpdateSuccessResultHandler(short colorIndex, Dictionary<short, int> counts, AbstractUserRowHandler<IList<User>> nextHandler) : base(colorIndex, counts, nextHandler)
        {
        }


        public override bool Handle(HandleContext<IList<User>> context)
        {
            if (context.ResultCount > 0)
            {
                // success: Aqua 水绿色
                Count();
                SetRowBgColor(context.Row);
                Console.WriteLine("User updated: " + context.Name);
                return true;
            }

            return base.Handle(context);
        }
    }
}