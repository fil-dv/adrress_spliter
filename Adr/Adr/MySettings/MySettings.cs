using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adr.MySettings
{
    public static class MySettings
    {
        static string _connectionString = "User ID=report;password=report;Data Source=CD_WORK"; 
        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }
    }
}
