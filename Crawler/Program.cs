using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Crawler.Adapters;
using Crawler.Model;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                MysqlDatabase database = new MysqlDatabase(Properties.Settings.Default.ConnectionString);
                int taskID;

                if (Int32.TryParse(args[0], out taskID)) {
                    Task newtask = database.GetTaskInfo(taskID);
                    database.Close();
                    database = null;
                    newtask.StartCrawling();
                }
            }
        }
    }
}
