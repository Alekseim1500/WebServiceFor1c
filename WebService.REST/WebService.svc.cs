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

namespace WebService.REST
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class WebService
    {


        // Чтобы использовать протокол HTTP GET, добавьте атрибут [WebGet]. (По умолчанию ResponseFormat имеет значение WebMessageFormat.Json.)
        // Чтобы создать операцию, возвращающую XML,
        //     добавьте [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     и включите следующую строку в текст операции:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";



        //Подключение к 1С базе
        [WebGet(UriTemplate = "/Connect1C")]
        public string Connect1C()
        {

            try
            {

                // Methods1C7.Connect("D:\\ТШК_МАРКО\\");
                //Methods1C7.Connect("D:\\1C_7.7_Марко\\");
                Methods1C7.Connect("D:\\Bases\\RED_OKTOBER\\");
                //object valueTable = GlobalVar.type1C.InvokeMember("CreateObject", BindingFlags.InvokeMethod, null, GlobalVar.object1C, new object[] { "ТаблицаЗначений" });
                // if (!Methods1C7.Connect("D:\\Bases\\RED_OKTOBER\\"))
                // {
                //     throw new Exception("Ошибка подключения к 1С 7!");
                // }
                //Methods1C7.Connect("D:\\1C_7.7_Марко\\");
                Methods1C7.Disconnect();
                return "Открыли 1С !";

            }
            catch (Exception ex)
            {
                return  ex.Message;
            }
        }


        [WebGet(UriTemplate = "/GetDocument")]
        public string GetDocument()
        {
            Methods1C7 _1C7 = new Methods1C7();
            bool result = _1C7.GetDocument(out dynamic Element, out string Error);
            if (result == true)
            {
                //string Str = "";
                //foreach (var enabledEndpoint in ((IEnumerable<dynamic>)Element.endpoints).Where(t => t.enabled))
                //{
                //    Str = enabledEndpoint.UID;
                // }
                //JArray jsonResponse = JArray.Parse(Element.ToString);
                //foreach (var item in jsonResponse)
                //{
                //JObject jRaces = (JObject)item["races"];
                //}


                /*Element = Convert.ToInt32(Element);
                foreach (int i in Element)
                {
                    Str = Str + " " + Element[i].UID;
                }*/
                return "получили документ 1С , стуктура " + Element;
            }
            else
            {
                return "Ошибка получения документа 1С " + Error;
            }
        }

        [WebGet(UriTemplate = "/GetReference")]
        public string GetReference()
        {
            Methods1C7 _1C7 = new Methods1C7();
            bool result = _1C7.GetReference(out dynamic Element, out string Error);
            if (result == true)
            {
                return "получили справочник 1С , стуктура " + Element;
            }
            else
            {
                return "Ошибка получения справочника 1С !";
            }
        }

        [WebGet(UriTemplate = "/UpWork")] //Удерживание пула от закрытия, либо старт при закрытии
        public string UpWork()
        {
            return "UpWork";
        }


        [WebGet(UriTemplate = "/ReadLog")]
        public string ReadLogAsync()
        {
            //Task<string> task = Methods1C7.ReadAllTextAsync(Parametr1C7.path + "SYSLOG\\1cv7.mlg");
            return "ок";
        }
        //_1C7.Methods.ReadAllTextAsync("example.txt");


        [WebGet(UriTemplate = "/PostKafka")]
        public string PostKafka()
        {
            bool result = Kafka.Produser("");
            if (result == true)
            {
                return "Отправили сообщение кафке !";
            }
            else
            {
                return "Ошибка отправки сообщения кафке ";
            }
        }

        [WebGet(UriTemplate = "/GetKafka")]
        public string GetKafka()
        {
            bool result = Kafka.Consumer(out string Mess, out string Error);
            if (result == true)
            {
                return "Получили сообщение от кафки : " + Mess;
            }
            else
            {
                return "Ошибка получения сообщения от кафки !" + Error;
            }
        }


        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/CreateDocument1C7",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public string CreateDocument1C7()
        {

            var requestMessage = OperationContext.Current.RequestContext.RequestMessage;
            var messageDataProperty = requestMessage.GetType().GetProperty("MessageData", (BindingFlags)0x1FFFFFF);
            var messageData = messageDataProperty.GetValue(requestMessage, null);
            var bufferProperty = messageData.GetType().GetProperty("Buffer");
            var buffer = bufferProperty.GetValue(messageData, null) as ArraySegment<byte>?;
            string json = Encoding.UTF8.GetString(buffer.Value.Array);

            try
            {
                // Methods1C7.Connect();
                var jarray = JsonConvert.DeserializeObject<JArray>(json);

                foreach (var arrayItem in jarray)
                {

                    dynamic ElementArrays = arrayItem.ToObject<Structure.BUH>();
                    dynamic Element = JsonConvert.SerializeObject(ElementArrays, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = CustomDataContractResolver.Instance
                    });

                    //bool result = Methods1C7.PostObject(Element);

                }
                Methods1C7.Disconnect();


                return "Создали документ";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          UriTemplate = "/TSD",
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Wrapped)]
        public string TSD(Stream body)
        {
            //var requestMessage = OperationContext.Current.RequestContext.RequestMessage;
            //var messageDataProperty = requestMessage.GetType().GetProperty("MessageData", (BindingFlags)0x1FFFFFF);
            //var messageData = messageDataProperty.GetValue(requestMessage, null);
            //var bufferProperty = messageData.GetType().GetProperty("Buffer");
            //var buffer = bufferProperty.GetValue(messageData, null) as ArraySegment<byte>?;
            //string json = Encoding.UTF8.GetString(buffer.Value.Array);
            String json = new StreamReader(body).ReadToEnd().ToString();
            return testCOM.Program.Main(json);
        }

    }

}



class GlobalVar
{
    public static object object1C;
    public static Type type1C;
    public static object object1CTSD;
    public static Type type1CTSD;
    //public static string NamePC = Environment.MachineName;

}


public class Parametr1C7
{
    //public static string user = "Северинов_Андрей";
    //public static string pass = "20111";
    public static string user = "WebService";
    // public static string pass = "15975";
    //public static string user = "Северинов_Андрей";
    public static string pass = "15975";
}
public class Parametr1C8
{
    public static string user = "Admin";
    public static string pass = "15975";
}
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



public class Structure
{
    public class BUH
    {
        public string UID { get; set; }

        private string _ВидОбъекта;
        public string ВидОбъекта
        {
            set
            {
                if (value == "Документ_ВозвратТоваровОтПокупателя") { _ВидОбъекта = "Документ.ВозвратТовараПокупателем"; }
                else { _ВидОбъекта = value; }
            }
            get { return _ВидОбъекта; }
        }
        public string Транзакция { get; set; }
        public string Договор { get; set; }
        public string Договор_UID { get; set; }
        public string Контрагент { get; set; }
        public string Контрагент_УНП { get; set; }
      //  public string Контрагент_Код { get; set; }
        public string Склад { get; set; }
        public string Склад_UID { get; set; }
        private bool Склад_ОтветХранение { get; set; }
        public string ТорговаяТочка_КонтактнаяИнформация_Представление { get; set; }
        public int ФлЭлНакл { get; } = 1;
        public int АвтоЗаполнение { get; } = 0;
        public string ВидТМЦ { get; } = "Продукция";
        public string EDiN_эТТН_ID { get; set; }
        public string Дата { get; set; }
        public string ДатаДокВходящий { get { return Дата; } }

        private int _ВидВ;
        public int ВидВ
        {
            set
            {
                if (Склад_ОтветХранение == true) { _ВидВ = 2; }
                else { _ВидВ = 1; }
            }
            get { return _ВидВ; }
        }
        public List<ТабличнаяЧастьBUH> Номенклатура { get; set; }
    }


        public class ТабличнаяЧастьBUH
    {
        public string Номенклатура { get; set; }
        public int Количество { get; set; }
        public string Номенклатура_Код { get; set; }
        public int Уценка { get; set; }
        public float Цена { get; set; }
        public string СтавкаНДС { get; set; }
        public float Сумма { get; set; }
        public float СуммаНДС { get; set; }
        public float Всего { get; set; }
    }
}


public class CustomDataContractResolver : DefaultContractResolver
{
    public static readonly CustomDataContractResolver Instance = new CustomDataContractResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (property.DeclaringType == typeof(Structure.BUH))
        {
            if (property.PropertyName.Equals("Номенклатура", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ТабличнаяЧасть"; }
            else if (property.PropertyName.Equals("ТорговаяТочка_КонтактнаяИнформация_Представление", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ПунктРазгрузки"; }
            else if (property.PropertyName.Equals("Организация", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Контрагент"; }
            else if (property.PropertyName.Equals("Организация_УНП", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Контрагент_УНН"; }
            else if (property.PropertyName.Equals("Контрагент_Код", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Контрагент_GLN"; }
            else if (property.PropertyName.Equals("Дата", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ДатаДок"; }
            else if (property.PropertyName.Equals("Склад", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "МестоХранения"; }
            else if (property.PropertyName.Equals("Склад_UID", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "МестоХранения_UID"; }
            else if (property.PropertyName.Equals("EDiN_эТТН_ID", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "НомерЭлНакл"; }
        }
        else if (property.DeclaringType == typeof(Structure.ТабличнаяЧастьBUH))
        {
            if (property.PropertyName.Equals("Номенклатура", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Товар"; }
            else if (property.PropertyName.Equals("СуммаНДС", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "НДС"; }
            else if (property.PropertyName.Equals("Уценка", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ПроцентУценки"; }
            else if (property.PropertyName.Equals("Номенклатура_Код", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Товар_GTIN"; }
        }
        return property;
    }
}


public class Messenger
{
    public static void Post(string errorMessage)
    {

    }
}


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

public class WebLogger
{
    public static Logger logger = LogManager.GetCurrentClassLogger();
}

public class Methods1C8
{
    public static async Task<string> ResponseAsync(string url)
    {
        try
        {
            var ListValidObjects = GlobalMethods.ParametrObjects("ValidObjects", url);
            var ListStructureObjects = GlobalMethods.ParametrObjects("StructureObjects", url);
            var ListTypeTransaction = GlobalMethods.ParametrObjects("TypeTransaction", url);
            if ((ListValidObjects.Count == 0) | (ListTypeTransaction.Count == 0) | (ListStructureObjects.Count == 0))
            {
                throw new Exception("Не заполнены фильтры !");
            }
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            while (true)
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 300000;
                request.ContentType = "application/json";
                request.Method = "POST"; // для отправки используется метод Post
                var credentials = new NetworkCredential(Parametr1C8.user, Parametr1C8.pass);
                request.Credentials = credentials;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var Filter = new { УНП = config.AppSettings.Settings["УНП"].Value, ValidObjects = ListValidObjects, TypeTransaction = ListTypeTransaction, Transaction = config.AppSettings.Settings[url].Value };
                    streamWriter.Write(JsonConvert.SerializeObject(Filter));
                }
                using (WebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var jarray = JsonConvert.DeserializeObject<JArray>(reader.ReadToEnd());
                            if (jarray.Count > 0)
                            {
                                foreach (var Получатель in Получатели.Consumer1С7)
                                {
                                    if (!Methods1C7.Connect(Получатель))
                                    {
                                        throw new Exception("Ошибка подключения к 1С 7!");
                                    }
                                    foreach (var arrayItem in jarray)
                                    {
                                        foreach (var StructureObject in ListStructureObjects)
                                        {
                                            dynamic Obj = (dynamic)typeof(Structure).GetNestedType(StructureObject);
                                            dynamic ElementArray = arrayItem.ToObject(Obj);
                                            dynamic Element = JsonConvert.SerializeObject(ElementArray, new JsonSerializerSettings
                                            {
                                                NullValueHandling = NullValueHandling.Ignore,
                                                ContractResolver = CustomDataContractResolver.Instance
                                            });

                                            Methods1C7.PostObject(Element, Получатель);
                                            WebLogger.logger.Trace(Element);
                                            config.AppSettings.Settings[url].Value = ElementArray.GetType().GetProperty("Транзакция").GetValue(ElementArray, null);
                                            config.Save();
                                        }
                                    }
                                    Methods1C7.Disconnect();
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(200);
            }
        }
        catch (Exception ex)
        {
            Methods1C7.Disconnect();
            var errorMessage = $"{ex.Message} {ex.InnerException?.Message}";
            Messenger.Post(errorMessage);
            WebLogger.logger.Error(errorMessage);
            return errorMessage;
        }
    }
}


public class Methods1C7
{

    [HandleProcessCorruptedStateExceptions]
    public static bool Connect(string path)
    {
        
        //GlobalVar.type1C = Type.GetTypeFromProgID("V77.Application");
        GlobalVar.type1C = Type.GetTypeFromProgID("V77.Application");
        GlobalVar.object1C = Activator.CreateInstance(GlobalVar.type1C);
        object rmtrade = GlobalVar.type1C.InvokeMember("RMTrade", BindingFlags.GetProperty, null, GlobalVar.object1C, null);
        var arguments = new object[] { rmtrade, " /D" + path + " /N" + Parametr1C7.user + " /P" + Parametr1C7.pass, "NO_SPLASH_SHOW" };
        bool res = (bool)GlobalVar.type1C.InvokeMember("Initialize", BindingFlags.InvokeMethod, null, GlobalVar.object1C, arguments);
        return res;
    }

    public static void Disconnect()
    {
        try
        {
            Marshal.FinalReleaseComObject(GlobalVar.object1C);
            GlobalVar.object1C = null;
            GlobalVar.type1C = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        catch
        { }

    }
    public static void DisconnectTSD()
    {
        try
        {
            Marshal.FinalReleaseComObject(GlobalVar.object1CTSD);
            GlobalVar.object1CTSD = null;
            GlobalVar.type1CTSD = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        catch
        { }
    }

    public static async Task<string> ReadAllTextAsync(string Catalog)
    {
        try
        {
            var ListValidObjects = GlobalMethods.ParametrObjects("ValidObjects", Catalog);
            var ListStructureObjects = GlobalMethods.ParametrObjects("StructureObjects", Catalog);
            var ListTypeTransaction = GlobalMethods.ParametrObjects("TypeTransaction", Catalog);
            if ((ListValidObjects.Count == 0) | (ListTypeTransaction.Count == 0) | (ListStructureObjects.Count == 0))
            {
                var errorMessage = $"Не заполнены фильтры !";
                Messenger.Post(errorMessage);
                WebLogger.logger.Error(errorMessage);
                return errorMessage;
            }

            var LogTrace1C7 = "";
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var stringBuilder = new StringBuilder();
            FileInfo log = new FileInfo(Catalog + "SYSLOG\\1cv7.mlg");
            using (var fileStream = log.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream, Encoding.GetEncoding(1251)))
            {
                HashSet<string> _Log1C = new HashSet<string>();
                fileStream.Seek(-500 * 1024, SeekOrigin.End); // размер файла 4 кб
                string line = await streamReader.ReadLineAsync();

                while (true)
                {

                    while (!streamReader.EndOfStream)

                    {

                        line = await streamReader.ReadLineAsync();
                        string[] subs = line.Split(new char[] { ';' }, StringSplitOptions.None);

                        foreach (var ValidObjects in ListValidObjects)
                        {
                            foreach (var TypeTransaction in ListTypeTransaction)
                            {
                                if ((subs[8].StartsWith(ValidObjects)) && (subs[2] != "WebService") && (subs[5].StartsWith(TypeTransaction)))
                                {
                                    _Log1C.Add(subs[8]);
                                    LogTrace1C7 = line;
                                }

                            }
                        }

                        if (config.AppSettings.Settings[Catalog].Value == line)
                        {
                            _Log1C.Clear();
                        }
                    }

                    if (_Log1C.Count > 0)
                    {
                        dynamic Json = JsonConvert.SerializeObject(_Log1C);
                        _Log1C.Clear();
                        //Methods1C7.Connect();
                        var jarray = Methods1C7.GetObjectByLog(Json);
                        foreach (var arrayItem in jarray)
                        {
                            var ElementArray = "";
                            //var ElementArray = arrayItem.ToObject(Parametr1C7.StructureObject());
                            dynamic Element = JsonConvert.SerializeObject(ElementArray, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                            foreach (string Получатель in Получатели.Consumer1С8)
                            {
                                GlobalMethods.PostObject(Element, Получатель);
                            }
                        }
                        WebLogger.logger.Trace("trace message");
                        config.AppSettings.Settings[Catalog].Value = LogTrace1C7;
                        config.Save();
                        Methods1C7.Disconnect();
                    }
                    Thread.Sleep(200);
                }
            }

        }
        catch (Exception ex)
        {
            Methods1C7.Disconnect();
            var errorMessage = $"{ex.Message} {ex.InnerException?.Message}";
            Messenger.Post(errorMessage);
            WebLogger.logger.Error(errorMessage);
            return errorMessage;
        }
    }

    public static void WriteToLog()
    {
        Object[] args1C = new Object[3];
        args1C = new object[5];
        args1C[0] = "Успешная отправка пакета данных";
        args1C[1] = "WebОтправкаДанных";
        args1C[2] = "WebОтправкаДанных";
        args1C[4] = 3;
        GlobalVar.type1C.InvokeMember(@"LogMessageWrite", BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);
    }
    public static object GetObjectByLog(dynamic Json)
    {

        Object[] args1C = new Object[3];
        args1C = new object[1];
        args1C[0] = "СписокЗначений";
        object listValues = GlobalVar.type1C.InvokeMember(@"CreateObject", BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);

        args1C = new object[2];
        args1C[0] = Json;
        args1C[1] = "GetObjectByLog";
        listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
        args1C = new object[3];
        args1C[0] = @"Обработка";
        args1C[1] = listValues;
        // args1C[2] = Parametr1C7.ert;

        object form1C = GlobalVar.type1C.InvokeMember(@"OpenForm",
        BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);

        Object[] arg = new Object[3];
        arg = new object[1];
        arg[0] = "GetObjectByLog";
        dynamic Result = args1C[1].GetType().InvokeMember(@"Получить", BindingFlags.Public | BindingFlags.InvokeMethod, null, args1C[1], arg);
        var jarray = JsonConvert.DeserializeObject<JArray>(Result);
        return jarray;
    }

    public bool GetDocument(out dynamic Element, out string Error)
    {
        Element = null;
        Error = null;
        try
        {

            //Methods1C7.Connect();

            Object[] args1C = new Object[3];
            args1C = new object[1];
            args1C[0] = "СписокЗначений";
            object listValues = GlobalVar.type1C.InvokeMember(@"CreateObject", BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);
            args1C = new object[2];
            args1C[0] = 1;
            args1C[1] = "GetObject";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C[0] = 1;
            args1C[1] = "GetMXL";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C[0] = "Документ.РасходнаяНакладная";
            args1C[1] = "ВидОбъекта";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C[0] = "1472";
            args1C[1] = "НомерДок";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C[0] = "28.02.22";
            args1C[1] = "ДатаНач";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C[0] = "28.02.22";
            args1C[1] = "ДатаКон";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C = new object[3];
            args1C[0] = @"Обработка";
            args1C[1] = listValues;
            //args1C[2] = Parametr1C7.ert;

            object form1C = GlobalVar.type1C.InvokeMember(@"OpenForm",
            BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);

            Object[] arg = new Object[3];
            arg = new object[1];
            arg[0] = "GetObject";
            //string Elements = Convert.ToString(args1C[1].GetType().InvokeMember(@"Получить", BindingFlags.Public | BindingFlags.InvokeMethod, null, args1C[1], arg));
            Element = args1C[1].GetType().InvokeMember(@"Получить", BindingFlags.Public | BindingFlags.InvokeMethod, null, args1C[1], arg);
            //Object[] arg2 = new Object[3];
            //arg2 = new object[1];

            //arg2[0] = "GetMXL";
            //byte Path = (byte)args1C[1].GetType().InvokeMember(@"Получить", BindingFlags.Public | BindingFlags.InvokeMethod, null, args1C[1], arg2);
            //BinaryReader MXL = new BinaryReader(File.Open(Path, FileMode.Open, FileAccess.Read));
            var settings = new JsonSerializerSettings();
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;



            var jarray = JsonConvert.DeserializeObject<JArray>(Element);

            //object[] arrayObject = {};
            foreach (var arrayItem in jarray)
            {
                //var ElementArray = arrayItem.ToObject<List<Structure.Buh_KO>>();

                dynamic ElementArrays = arrayItem.ToObject<List<Structure.BUH>>();
                Element = JsonConvert.SerializeObject(ElementArrays);
                //Element = JsonConvert.SerializeObject(ElementArrays, Formatting.Indented);
                //Element = JsonConvert.SerializeObject(ElementArrays, settings);
                // Array.Resize(ref arrayObject, arrayObject.Length + 1);
                // arrayObject[arrayObject.Length - 1] = Element;
                //Array.Resize(ref mass, mass.Length + 1);
                //mass[mass.Length - 1] = Element;
                foreach (string Получатель in Получатели.Consumer1С8)
                {
                    GlobalMethods.PostObject(Element, Получатель);
                }

                //foreach (var item in innerArray)
                //{
                //return innerArray.ToString();
                //}

            }

            // Element = arrayObject;



            //WorkKafka.Produser _WorkKafka = new WorkKafka.Produser();
            //bool result = _WorkKafka.PostMessage(Element);
            Methods1C7.Disconnect();

        }
        catch
        {
            return false;
        }
        return true;
    }

    public bool GetReference(out dynamic Element, out string Error)
    {
        Element = null;
        Error = null;
        try
        {
            Methods1C7.Connect("D:\\Bases\\RED_OKTOBER\\");
            Object[] args1C = new Object[3];
            args1C = new object[1];
            args1C[0] = "СписокЗначений";
            object listValues = GlobalVar.type1C.InvokeMember(@"CreateObject", BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);
            args1C = new object[2];
            args1C[0] = 1;
            args1C[1] = "GetAuto";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C[0] = "Справочник.Контрагенты";
            args1C[1] = "ВидОбъекта";
            listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
            args1C = new object[3];
            args1C[0] = @"Обработка";
            args1C[1] = listValues;
            args1C[2] = "D:\\Bases\\RED_OKTOBER\\EXTFORMS\\EDO\\Test\\UN_JSON.ert";

            object form1C = GlobalVar.type1C.InvokeMember(@"OpenForm",
            BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);

            Object[] arg = new Object[3];
            arg = new object[1];
            arg[0] = "Ответ";
            string el = Convert.ToString(args1C[1].GetType().InvokeMember(@"Получить", BindingFlags.Public | BindingFlags.InvokeMethod, null, args1C[1], arg));
            Element = JsonConvert.SerializeObject(el);
            //Element = JsonConvert.DeserializeObject<List<Structure.Buh_1C7>>(el);
            //Element = Element.ToObject<List<Structure.Buh_1C7>>();
            //WorkKafka.Produser _WorkKafka = new WorkKafka.Produser();
            //bool result = _WorkKafka.PostMessage(Element,);
            Methods1C7.Disconnect();
            //WorkKafka.Produser _WorkKafka = new WorkKafka.Produser();
            //bool result = _WorkKafka.PostMessage(Element);

        }
        catch
        {
            return false;
        }
        return true;
    }


    public static void PostObject(dynamic Element, string Получатель)
    {

        Object[] args1C = new Object[3];
        args1C = new object[1];
        args1C[0] = "СписокЗначений";
        object listValues = GlobalVar.type1C.InvokeMember(@"CreateObject", BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);
        args1C = new object[2];
        args1C[0] = Element;
        args1C[1] = "PostObject";
        listValues.GetType().InvokeMember(@"AddValue", BindingFlags.Public | BindingFlags.InvokeMethod, null, listValues, args1C);
        args1C = new object[3];
        args1C[0] = @"Обработка";
        args1C[1] = listValues;
        args1C[2] = Получатель + "EXTFORMS\\EDO\\Test\\UN_JSON.ert";

        object form1C = GlobalVar.type1C.InvokeMember(@"OpenForm",
        BindingFlags.Public | BindingFlags.InvokeMethod, null, GlobalVar.object1C, args1C);

        Object[] arg = new Object[3];
        arg = new object[1];

        Element = args1C[1].GetType().InvokeMember(@"Получить", BindingFlags.Public | BindingFlags.InvokeMethod, null, args1C[1], arg);


    }

}

public class Kafka
{
    public static bool Produser(dynamic Element)
    {

        try
        {
            // var options = new KafkaOptions(new Uri("http://192.168.205.95:9092"));
            var options = new KafkaOptions(new Uri("http://192.168.205.215:9092"));
            var router = new BrokerRouter(options);
            var client = new Producer(router);

            client.SendMessageAsync("quickstart-events", new[] { new Message("LFLFLFLF") }).Wait();

            using (client) { }
        }
        catch
        {
            //Error = ex.Message;
            return false;
        }

        return true;
        //var options = new KafkaOptions(new Uri("http://192.168.205.95:9092"), new Uri("http://SERVER2:9092"));

    }

    public static bool Consumer(out string Mess, out string Error)
    {
        Mess = "";
        Error = "";
        long offset = 0;
        try
        {

            //var options = new KafkaOptions(new Uri("http://192.168.205.95:9092"));
            var options = new KafkaOptions(new Uri("http://192.168.205.215:9092"));
            var router = new BrokerRouter(options);
            var consumer = new KafkaNet.Consumer(new ConsumerOptions("quickstart-events", router));
            consumer.SetOffsetPosition(new OffsetPosition { PartitionId = 0, Offset = offset });
            //var results = consumer.Consume().Take(3).ToList();
            // Consume returns a blocking IEnumerable(ie: never ending stream)
            //ensure the produced messages arrived

            foreach (var message in consumer.Consume())
            {
                Mess = System.Text.Encoding.Default.GetString(message.Value);

            }

            //Console.WriteLine("Response: P{0},O{1} : {2}",
            //    message.Meta.PartitionId, message.Meta.Offset, message.Value);      
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            return false;
        }
        return true;
    }
}
