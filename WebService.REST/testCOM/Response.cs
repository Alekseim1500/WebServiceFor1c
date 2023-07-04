using System;
using System.Collections.Generic;

namespace testCOM
{
    public class Response
    {
        public int VendorID { get; set; }
        public String TSHK { get; set; }
        public String DeviceName { get; set; }
        public String RequestType { get; set; }
        public String Комплектовщик { get; set; }
        public String ObjectName { get; set; }
        public String ObjectDate { get; set; }
        public String ObjectNumber { get; set; }
        public int isTest { get; set; }
        public List<ScanList> МассивШтрихкодов { get; set; }
        public Response()
        {
            МассивШтрихкодов = new List<ScanList>();
            isTest = 1;
        }
    }
}