using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

public class GlobalMethods
{
    //отправляет элемент на url
    public static void PostObject(dynamic Element, string URL)
    {

        WebRequest request = WebRequest.Create(URL);
        request.ContentType = "application/json";
        request.Method = "POST";

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

    //для извлечения значений параметров из конфигурационного файла My.config (<section> <add key="key" value="return1;return2"/> </section>)
    public static dynamic ParametrObjects(string section, string key)
    {
        NameValueCollection handlerCreatedCollection = ParametrObjects(section);

        foreach (string keyConfig in handlerCreatedCollection.AllKeys)
        {
            if (key == keyConfig)
            {
                return new List<string>(handlerCreatedCollection[key].Split(new char[] { ';' }));
            }
        }

        return new List<string>();
    }

    public static dynamic ParametrObjects(string section)
    {
        ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };

        var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        ConfigurationSection myParamsSection = config.GetSection(section);

        string myParamsSectionRawXml = myParamsSection.SectionInformation.GetRawXml();
        XmlDocument sectionXmlDoc = new XmlDocument();
        sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));

        NameValueSectionHandler handler = new NameValueSectionHandler();
        NameValueCollection handlerCreatedCollection = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;

        return handlerCreatedCollection;
    }
}