using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.Audio;

namespace AudioLibraryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var mgr = new ResourceManager();
            mgr.LoadLibrary(Environment.CurrentDirectory + "\\..\\..\\test.assl");

            mgr.Start();

            var res = mgr.GetResource("chord");

            Console.ReadLine();
            mgr.Stop();
        }
    }
}
