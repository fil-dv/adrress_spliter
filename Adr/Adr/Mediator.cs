using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adr
{
    public static class Mediator
    {
        public static bool IsIncomParam { get; set; }  // программа запускается со входящим параметром или без

        public  static int ApYes { get; set; }
        public  static int ApNo { get; set; }
        public  static int AfYes { get; set; }
        public  static int AfNo { get; set; }
        public  static int AwYes { get; set; }
        public  static int AwNo { get; set; }
        public  static int AvrYes { get; set; }
        public  static int AvrNo { get; set; }
    }
}
