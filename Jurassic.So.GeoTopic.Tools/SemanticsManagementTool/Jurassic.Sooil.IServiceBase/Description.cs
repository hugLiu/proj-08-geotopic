﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Sooil.IServiceBase
{
    public class Description:TypeValue
    {
        public Description()
        { 
        }
        public Description(string type, string value)
            : base(type, value)
        {
        }
    }
}