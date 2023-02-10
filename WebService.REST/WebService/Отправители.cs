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

public class Отправители
{

    public static dynamic Produser1C7
    {
        get
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ConfigurationSection myParamsSection = config.GetSection("Produser1C7");
            string myParamsSectionRawXml = myParamsSection.SectionInformation.GetRawXml();
            XmlDocument sectionXmlDoc = new XmlDocument();
            sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
            NameValueSectionHandler handler = new NameValueSectionHandler();
            NameValueCollection handlerCreatedCollection = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
            return handlerCreatedCollection;
        }
    }
    public static dynamic Produser1C8
    {
        get
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ConfigurationSection myParamsSection = config.GetSection("Produser1C8");
            string myParamsSectionRawXml = myParamsSection.SectionInformation.GetRawXml();
            XmlDocument sectionXmlDoc = new XmlDocument();
            sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
            NameValueSectionHandler handler = new NameValueSectionHandler();
            NameValueCollection handlerCreatedCollection = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
            return handlerCreatedCollection;
        }
    }
}