using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fc.Auto.Common.Interface
{
    internal interface IClassLoader
    {
        T CreateOrLocate<T>(Type instance, params object[] ctorParams) where T : class;
        
    }
}
