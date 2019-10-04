using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCrewPlugin.Data
{
    /// <summary>
    /// Data providers are built in the IoC as singleton systems that
    /// are initialized and used to hold/access data through
    /// program/session lifetime.
    /// </summary>
    public interface IDataProvider
    {
        void Start();
    }
}
