using System;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using static System.Net.WebRequestMethods;
using Confluent.Kafka;
using System.Xml.Linq;
using KafkaNet.Protocol;
using System.Net.Sockets;

public class Methods1C8
{
    //из 1с в kafka
    public static async Task<string> ResponseAsync(string url)
    {
        try
        {
            //получаем данные для фильтра
            var ListValidObjects = GlobalMethods.ParametrObjects("ValidObjects", url);
            var ListStructureObjects = GlobalMethods.ParametrObjects("StructureObjects", url);
            var ListTypeTransaction = GlobalMethods.ParametrObjects("TypeTransaction", url);
            if ((ListValidObjects.Count == 0) | (ListTypeTransaction.Count == 0) | (ListStructureObjects.Count == 0))
            {
                throw new Exception("Не заполнены фильтры!");
            }

            //открываем файл "My.config"
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "My.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            while (true)
            {
                //создаём запрос, что бы сказать 1с, какой объект нам нужен
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 300000;
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Credentials = new NetworkCredential(Parametr1C8.user, Parametr1C8.pass);

                //отправляем этот запрос
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
                //получаем ответ в doc
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
                                        //обрабатываем ответ
                                        dynamic Obj = (dynamic)typeof(Structure).GetNestedType(StructureObject);
                                        dynamic ElementArray = arrayItem.ToObject(Obj);
                                        dynamic Element = JsonConvert.SerializeObject(ElementArray, new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            ContractResolver = CustomDataContractResolver.Instance
                                        });

                                        //логируем и записывает последнюю транзакцию в "My.config"
                                        WebLogger.logger.Trace($"Пришёл элемент {Element}");
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
            var errorMessage = $"{ex.Message} {ex.InnerException?.Message}";
            Messenger.Post(errorMessage);
            WebLogger.logger.Error(errorMessage);
            return errorMessage;
        }
    }

    //из kafka в 1с
    public static async Task<string> PostObject(/*string url*/)
    {
        try
        {
            var soket = "192.168.205.106:9092"; //2181
            var topic = "New_Topic";

            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group", // идентификатор группы, к которой будет принадлежать Consumer
                BootstrapServers = soket,
                AutoOffsetReset = AutoOffsetReset.Earliest // начать чтение сообщений с самого начала топика 
            };

            using (var consumer = new ConsumerBuilder<Null, string>(conf).Build())
            {
                // подписываемся и получаем сообщение из топика
                consumer.Subscribe(topic);
                //var url = "http://192.168.205.112/1c8testBD2/hs/EDO/Post";
                while (true)
                {
                    //WebRequest request = WebRequest.Create(url);
                    //request.Timeout = 300000;
                    //request.ContentType = "application/json";
                    //request.Method = "POST";
                    //request.Credentials = new NetworkCredential(Parametr1C8.user, Parametr1C8.pass);

                    //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    //{
                    //Kafka.Consumer(out string mess);
                    //streamWriter.Write(mess);
                    var data = await Task.Run(() => consumer.Consume());
                    var mess = data.Message.Value;
                    WebLogger.logger.Trace($"Получили сообщение из kafka: {mess}");
                    //WebLogger.logger.Trace($"Отправили объект в 1с");
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"{ex.Message} {ex.InnerException?.Message}";
            Messenger.Post(errorMessage);
            WebLogger.logger.Error(errorMessage);
            return errorMessage;
        }
    }
}