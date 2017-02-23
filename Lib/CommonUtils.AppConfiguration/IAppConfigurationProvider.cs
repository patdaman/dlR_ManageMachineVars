using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.AppConfiguration
{
    public interface IAppConfigurationProvider
    {
        object GetValue(string key);
    }
}
