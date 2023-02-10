using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Threading;
//using System.Configuration;
//using System.Linq;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using Newtonsoft.Json;
//using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
//k

public class GlobalMethods
{
    public static void PostObject(dynamic Element, string URL)
    {

        WebRequest request = WebRequest.Create(URL);
        request.ContentType = "application/json";
        request.Method = "POST"; // для отправки используется метод Post
        var credentials = new NetworkCredential("Admin", "15975");
        request.Credentials = credentials;
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(Element);
        }
        var httpResponse = request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }
    }

    public static dynamic ParametrObjects(string section, string key)
    {
        ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
        var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        ConfigurationSection myParamsSection = config.GetSection(section);
        string myParamsSectionRawXml = myParamsSection.SectionInformation.GetRawXml();
        XmlDocument sectionXmlDoc = new XmlDocument();
        sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
        NameValueSectionHandler handler = new NameValueSectionHandler();
        NameValueCollection handlerCreatedCollection = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
        foreach (string keyConfig in handlerCreatedCollection.AllKeys)
        {
            if (key == keyConfig)
            {
                return new List<string>(handlerCreatedCollection[key].Split(new char[] { ';' }));
            }
        }

        return new List<string>();

    }

}