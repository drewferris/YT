﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd {
    class Program {
        static void Main(string[] args) {

            try {
                //for (int i = 0; i < 10000000; i++) {
                //    var d = new Dictionary<string, DateTime>();
                //    d.Add("A", DateTime.Now);
                //}

                var ll = new List<int>();
                YT.Comments.Get(args[0]);
            }
            catch (Exception ex) {

                Console.WriteLine("{0}:{1}", ex.Source, ex.Message);
            }
        }
    }
}
