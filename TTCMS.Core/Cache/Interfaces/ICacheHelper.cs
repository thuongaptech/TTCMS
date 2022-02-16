using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCMS.Core.Interfaces
{
    public interface ICacheHelper
    {
        object Get(string key);
        void Set(string key, object data, int cacheTime=1);
        bool IsSet(string key);
        void Invalidate(string key);
    }
}
