using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adr
{
    public static class Loger
    {
        /// <summary>
        /// Method for adding text message to log file.
        /// </summary>
        /// <param name="message">This text will add to log file</param>
        public static void AddRecordToLog(string message)
        {
            try
            {
                string path = "log.txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                //string text = DateTime.Now + " " + Environment.UserName + Environment.NewLine + message + Environment.NewLine;                       
                string text = DateTime.Now + " " + message + Environment.NewLine;
                File.AppendAllText(path, text);
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }            
        }
    }
}
