﻿using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCrewPlugin.Data
{
    public interface IConnectionFactory
    {
        void Setup();
        IDbConnection Open();
    }
}

