using System;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;

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
                throw new Exception("Не заполнены фильтры!");
            }

            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            while (true)
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 300000;
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Credentials = new NetworkCredential(Parametr1C8.user, Parametr1C8.pass);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var Filter = new { 
                        УНП = config.AppSettings.Settings["УНП"].Value, 
                        ValidObjects = ListValidObjects, 
                        TypeTransaction = ListTypeTransaction, 
                        Transaction = config.AppSettings.Settings[url].Value
                    };
                    streamWriter.Write(JsonConvert.SerializeObject(Filter));
                }
                using (WebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var doc = reader.ReadToEnd();
                            var jarray = JsonConvert.DeserializeObject<JArray>(doc);
                            if (jarray.Count > 0)
                            {
                                foreach (var arrayItem in jarray.Reverse())
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

                                        WebLogger.logger.Trace(Element);
                                        //в файл "My.config" <appSettings> поменяется value, у key = "url"
                                        config.AppSettings.Settings[url].Value = ElementArray.GetType().GetProperty("Транзакция").GetValue(ElementArray, null);
                                        config.Save();
                                        Kafka.Produser(arrayItem.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(2000); //200
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