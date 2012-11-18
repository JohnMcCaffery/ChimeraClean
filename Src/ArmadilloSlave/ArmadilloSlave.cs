using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using UtilLib;
using ConsoleTest;
using System.IO;

namespace ArmadilloSlave {
    public class ArmadilloSlave {
        public static void Main(string[] args) {
            Init.InitCameraSlave(args);
            //else {
                //Console.WriteLine("Type 'Exit' to quit.");
                //while (!Console.ReadLine().ToUpper().Equals("EXIT"))
                    //Console.WriteLine("Type 'Exit' to quit.");
            //}
        }
    }
}
