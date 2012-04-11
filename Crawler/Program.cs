using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args.Count());
            Console.WriteLine(Properties.Settings.Default.ConnectionString);
            Console.Read();
        }
    }
}
