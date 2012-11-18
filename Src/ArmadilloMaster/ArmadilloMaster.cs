using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using ConsoleTest;
using Nini.Config;
using System.IO;

namespace ArmadilloMaster {
    public class ArmadilloMaster {
        public static void Main(string[] args) {
            ArgvConfigSource config = new ArgvConfigSource(args);

            Init.InitCameraMaster(args);
            //else {
                //Console.WriteLine("Type 'Exit' to quit.");
                //while (!Console.ReadLine().ToUpper().Equals("EXIT"))
                    //Console.WriteLine("Type 'Exit' to quit.");
            //}
        }
    }
}
