﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockchain_prototype.Transaction
{
    public interface ITransaction
    {
        byte[] HexToBytes(string hex);
    }
}