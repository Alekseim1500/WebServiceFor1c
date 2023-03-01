using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

public class Получатели
{
    public static dynamic Consumer1С7
    {
        get
        {
            return GlobalMethods.ParametrObjects("Consumer1C7");
        }
    }
    public static dynamic Consumer1С8
    {
        get
        {
            return GlobalMethods.ParametrObjects("Consumer1С8");
        }
    }
}