﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd {
    class Program {
        static void Main(string[] args) {

            try {
                YT.Comments.Get(args[0]);
            }
            catch (Exception ex) {

                throw;
            }
        }
    }
}
