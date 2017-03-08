using Fc.Auto.Common.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fc.Auto.Common.Foundation
{
    public class ClassLoader : IClassLoader
    {
        private Dictionary<Type, object> _instanceLoaded;

        private ClassLoader()
        {
            _instanceLoaded = new Dictionary<Type, object>();
        }

        private static ClassLoader _instance;
        public static ClassLoader Instance => (_instance ?? (_instance = new ClassLoader()));


        

        public T CreateOrLocate<T>(Type instance, params object[] ctorParams) where T : class
        {

            var type = typeof(T);
            if (_instanceLoaded.ContainsKey(type))
            {
                return _instanceLoaded[type] as T;
            }

            T retVal = ctorParams == null || ctorParams.Count() == 0
                ? Activator.CreateInstance(instance, true) as T
                : Activator.CreateInstance(instance, ctorParams) as T;
            _instanceLoaded.Add(type, retVal);
            return retVal;
        }

    }
}
