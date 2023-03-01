using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

public class Отправители
{

    public static dynamic Produser1C7
    {
        get
        {
            return GlobalMethods.ParametrObjects("Produser1C7");
        }
    }
    public static dynamic Produser1C8
    {
        get
        {
            return GlobalMethods.ParametrObjects("Produser1C8");
        }
    }
}