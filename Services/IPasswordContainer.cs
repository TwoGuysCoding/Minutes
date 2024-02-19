﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Minutes.Services
{
    internal interface IPasswordContainer: IDisposable 
    {
        public SecureString Password { get;}
    }
}
