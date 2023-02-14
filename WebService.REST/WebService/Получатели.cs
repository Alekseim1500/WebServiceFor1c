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
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ConfigurationSection myParamsSection = config.GetSection("Consumer1C7");
            string myParamsSectionRawXml = myParamsSection.SectionInformation.GetRawXml();
            XmlDocument sectionXmlDoc = new XmlDocument();
            sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
            NameValueSectionHandler handler = new NameValueSectionHandler();
            NameValueCollection handlerCreatedCollection = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
            return handlerCreatedCollection;
        }
    }
    public static dynamic Consumer1С8
    {
        get
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ConfigurationSection myParamsSection = config.GetSection("Consumer1С8");
            string myParamsSectionRawXml = myParamsSection.SectionInformation.GetRawXml();
            XmlDocument sectionXmlDoc = new XmlDocument();
            sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
            NameValueSectionHandler handler = new NameValueSectionHandler();
            NameValueCollection handlerCreatedCollection = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
            return handlerCreatedCollection;
        }
    }
}