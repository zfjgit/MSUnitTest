using System.Collections.Generic;

namespace MSUnitTestProject.UserInfoUpdate.Handler
{
    public class HandlerChain<T>
    {
        private readonly List<IUserRowHandler<T>> _handlers = new List<IUserRowHandler<T>>();

        public void AddHandler(IUserRowHandler<T> handler)
        {
            _handlers.Add(handler);
        }

        public bool Handle(HandleContext<T> context)
        {
            foreach (var handler in _handlers)
            {
                if (!handler.Handle(context))
                    return false;
            }
            return true;
        }
    }
}
