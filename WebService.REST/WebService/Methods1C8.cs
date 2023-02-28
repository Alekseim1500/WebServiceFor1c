using System;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;

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
                    //тут нужно будет поменять
                    var a = config.AppSettings.Settings["УНП"].Value;
                    var b = ListValidObjects;
                    var c = ListTypeTransaction;
                    var d = config.AppSettings.Settings[url].Value;
                    var Filter = new { 
                        УНП = a, 
                        ValidObjects = b, 
                        TypeTransaction = c, 
                        Transaction = d 
                    };
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