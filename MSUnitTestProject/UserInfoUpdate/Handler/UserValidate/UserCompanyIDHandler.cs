using System;
using System.Collections.Generic;
using MSUnitTestProject.UserInfoUpdate.UserData;

namespace MSUnitTestProject.UserInfoUpdate.Handler.UserValidate
{
    public class UserCompanyIDHandler
        //(short colorIndex, Dictionary<short, int> counts, AbstractUserHandler<IList<RBAC_UserModel>> nextHandler) 
        : AbstractUserHandler<IList<User>>
    //(colorIndex, counts, nextHandler)
    {
        public UserCompanyIDHandler(short colorIndex, Dictionary<short, int> counts, AbstractUserRowHandler<IList<User>> nextHandler) : base(colorIndex, counts, nextHandler)
        {
        }

        public override bool Handle(HandleContext<IList<User>> context)
        {
            if (context.Data.Count > 1)
            {
                int rc = 0;
                foreach (var user in context.Data)
                {
                    if (user.CompanyID == 1)
                    {
                        rc++;
                    }
                }

                if (rc == 0)
                {
                    Count();
                    SetRowBgColor(context.Row);
                    Console.WriteLine("company id not 1: " + context.Name);
                    return false;
                }
            }

            return base.Handle(context);
        }
    }
}