using System;

namespace testCOM
{
    class DataBase1c7Info
    {
        public String Path { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public DataBase1c7Info(int isTest)
        {
            if (isTest == 1)
            {
                Path = "T:\\DB_Hashkovskij\\TSD_testing";
                Login = "AutoWriterTSD";
                Pass = "v6HE62";
            }
            else if (isTest == 0)
            {
                Path = "D:\\TSHK_RO";
                Login = "AutoWriterTSD";
                Pass = "v6HE62";
            }
            else if (isTest == 2)
            {
                Path = "E:\\Bases1c7\\TSHK_M";
                Login = "User";
                Pass = "1850";
            }

        }
    }
}