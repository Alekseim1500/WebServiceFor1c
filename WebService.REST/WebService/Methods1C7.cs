using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

public class Methods1C7
{

    [HandleProcessCorruptedStateExceptions]
    public static bool Connect(string path)
    {
        //создаём экземпляр 1с7
        GlobalVar.type1C = Type.GetTypeFromProgID("V77.Application");
        GlobalVar.object1C = Activator.CreateInstance(GlobalVar.type1C);

        //соединяемся
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
            //получаем массивы из каталога
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

            //streamReader - открыли файл каталог
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