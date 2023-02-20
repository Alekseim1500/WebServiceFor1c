using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WebService.REST
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WebService
    {

        [WebGet(UriTemplate = "/AAA")]
        public string AAA()
        {
            var a = GlobalMethods.ParametrObjects("appSettings", "УНП");
            return string.Join(", ", a);
        }

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
        public string GetDocument() // тут ошибка вылетает
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
            return Kafka.Produser("Привет kafka!");
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
                return "Ошибка получения сообщения от кафки! " + Error;
            }
        }


        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/CreateDocument1C7",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public string CreateDocument1C7() // метод не разрешён
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
        public string TSD(Stream body) // метод не разрешён 
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
