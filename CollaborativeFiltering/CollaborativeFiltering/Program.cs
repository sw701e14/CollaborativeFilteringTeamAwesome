﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeFiltering
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = Parser.parseshit("_ignoretraining_set/");
            var b = Parser.parseshit("_ignoreProbe/");
        }
    }
}
