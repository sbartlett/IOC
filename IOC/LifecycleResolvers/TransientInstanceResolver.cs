using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.LifecycleResolvers
{
    public class TransientInstanceResolver<T> : IInstanceResolver where T : class 
    {
        public object ResolveInstance(object[] args)
        {
            var type = typeof(T);
            return Activator.CreateInstance(type, args);
        }
    }
}
