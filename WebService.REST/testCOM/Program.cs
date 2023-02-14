using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace testCOM
{  
    internal class Program
    {
        public static string Main(string response_text)
        {
            Response response_data = JsonConvert.DeserializeObject<Response>(response_text);

            return ConnectToCOM(response_data);


            string ConnectToCOM(Response data)
            {
                DataBase1c7Info dbInfo = new DataBase1c7Info(data.isTest);

                string BasePath = dbInfo.Path;
                string Login = dbInfo.Login;
                string Pass = dbInfo.Pass;

                try
                {
                    if (GlobalVar.type1CTSD == null || GlobalVar.object1CTSD == null)
                    {
                        GlobalVar.type1CTSD = Type.GetTypeFromProgID("V77.Application");
                        GlobalVar.object1CTSD = Activator.CreateInstance(GlobalVar.type1CTSD);
                        object rmtrade = GlobalVar.type1CTSD.InvokeMember("RMTrade", BindingFlags.GetProperty, null, GlobalVar.object1CTSD, null);
                        var arguments = new object[] { rmtrade, "/D\"" + BasePath + "\" /N\"" + Login + "\" /P\"" + Pass + "\"", "NO_SPLASH_SHOW" };
                        bool res = (bool)GlobalVar.type1CTSD.InvokeMember("Initialize", BindingFlags.InvokeMethod, null, GlobalVar.object1CTSD, arguments);
                        if (!res)
                        {
                            Methods1C7.DisconnectTSD();
                            return "Ошибка при инициализации подключения к 1с7";
                        }
                    }

                    ////тест методов глобального контекста
                    //object mydate = GlobalVar.type1CTSD.InvokeMember("EvalExpr", BindingFlags.InvokeMethod, null, GlobalVar.object1CTSD, new object[] { "Дата(\"01.05.2021\")" });
                    //Console.WriteLine(mydate.ToString());
                    //return;
                    ////

                    //Установка параметров формы
                    object valueTable = GlobalVar.type1CTSD.InvokeMember("CreateObject", BindingFlags.InvokeMethod, null, GlobalVar.object1CTSD, new object[] { "ТаблицаЗначений" });

                    String[] ColumnName = { "", "ТШК", "Короб", "Ячейка", "Комплектовщик" };
                    int tzColumnCount = 4;
                    for (int i = 1; i <= tzColumnCount; i++)
                    {
                        GlobalVar.object1CTSD.GetType().InvokeMember("NewColumn", BindingFlags.InvokeMethod, null, valueTable, new object[] { ColumnName[i] });
                    }
                    int _TZrowIndex = 1;
                    foreach (ScanList scan in data.МассивШтрихкодов)
                    {
                        object newRow = GlobalVar.object1CTSD.GetType().InvokeMember("НоваяСтрока", BindingFlags.InvokeMethod, null, valueTable, null);// new object[] {"a","b","c"});

                        GlobalVar.object1CTSD.GetType().InvokeMember("УстановитьЗначение", BindingFlags.InvokeMethod, null, valueTable, new object[] { _TZrowIndex, 1, scan.ТШК });
                        GlobalVar.object1CTSD.GetType().InvokeMember("УстановитьЗначение", BindingFlags.InvokeMethod, null, valueTable, new object[] { _TZrowIndex, 2, scan.Короб });
                        GlobalVar.object1CTSD.GetType().InvokeMember("УстановитьЗначение", BindingFlags.InvokeMethod, null, valueTable, new object[] { _TZrowIndex, 3, scan.Ячейка });
                        GlobalVar.object1CTSD.GetType().InvokeMember("УстановитьЗначение", BindingFlags.InvokeMethod, null, valueTable, new object[] { _TZrowIndex, 4, data.Комплектовщик });
                        _TZrowIndex++;
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        Console.WriteLine(GlobalVar.object1CTSD.GetType().InvokeMember("ПолучитьЗначение", BindingFlags.InvokeMethod, null, valueTable, new object[] { 1, i }));
                    }
                    //------------------------------------------------------------------
                    //valueTable - параметр формы 
                    object FormContext = valueTable;

                    object RequestDoc = GlobalVar.type1CTSD.InvokeMember("CreateObject", BindingFlags.InvokeMethod, null, GlobalVar.object1CTSD, new object[] { "Документ." + data.ObjectName });

                    GlobalVar.object1CTSD.GetType().InvokeMember("НайтиПоНомеру", BindingFlags.InvokeMethod, null, RequestDoc, new object[] { int.Parse(data.ObjectNumber), data.ObjectDate });

                    object _obj_RequestDoc = GlobalVar.object1CTSD.GetType().InvokeMember("ТекущийДокумент", BindingFlags.InvokeMethod, null, RequestDoc, null);

                    object extDocLink = GlobalVar.object1CTSD.GetType().InvokeMember(data.ObjectName == "Задание" ? "ДокументНаПеремещение" : "СвязанныйДокумент", BindingFlags.GetProperty, null, RequestDoc, null);

                    object extDocNumber = GlobalVar.object1CTSD.GetType().InvokeMember("НомерДок", BindingFlags.GetProperty, null, extDocLink, null);
                    object extDocDate = GlobalVar.object1CTSD.GetType().InvokeMember("ДатаДок", BindingFlags.GetProperty, null, extDocLink, null);
                    String extDocDateToString = extDocDate.ToString().Substring(0, 10);
                    WebLogger.logger.Error(int.Parse(extDocNumber.ToString()));
                    WebLogger.logger.Error(extDocDate + "_______" + extDocDateToString);

                    WebLogger.logger.Error("Документ." + data.ObjectName == "Задание" ? "ВнПеремОтпуск" : "ОтпускНаСторону");
                    object _finalDoc = GlobalVar.type1CTSD.GetType().InvokeMember("CreateObject", BindingFlags.InvokeMethod, null, GlobalVar.object1CTSD, new object[] { "Документ.ОтпускНаСторону" });
                    GlobalVar.object1CTSD.GetType().InvokeMember("НайтиПоНомеру", BindingFlags.InvokeMethod, null, _finalDoc, new object[] { int.Parse(extDocNumber.ToString()), extDocDateToString });
                    object _finalDocObj = GlobalVar.object1CTSD.GetType().InvokeMember("ТекущийДокумент", BindingFlags.InvokeMethod, null, _finalDoc, null);
                    WebLogger.logger.Error(_finalDocObj);
                    ///
                    //возможно реверсивная логика, потом проверить
                    //--needed--object _finalDocObj = GlobalVar.object1CTSD.GetType().InvokeMember("ТекущийДокумент", BindingFlags.InvokeMethod, null, extDocLink, null);
                    //--needed--object _isfinalDocSelected = GlobalVar.object1CTSD.GetType().InvokeMember("Выбран", BindingFlags.InvokeMethod, null, _finalDocObj, null);

                    ///----------------
                    //блок не работает, но вроде и не нужен
                    //Console.WriteLine(extDocNumber.ToString().Replace(" ","")); 
                    //Console.WriteLine(extDocDate.ToString().Substring(0,10));


                    //string finaleDocName = data.ObjectName == "Задание" ? "ВнПеремОтпуск" : "ОтпускНаСторону";
                    ////Console.WriteLine(finaleDocName);

                    //object finaleDoc = GlobalVar.type1CTSD.GetType().InvokeMember("CreateObject", BindingFlags.GetProperty, null, GlobalVar.object1CTSD, new object[] { "Документ." + finaleDocName });
                    //int finaleDocNumber = int.Parse(extDocNumber.ToString().Replace(" ", ""));
                    ////Console.WriteLine(finaleDocNumber);

                    //string finaleDocDate = extDocDate.ToString().Substring(0, 10);
                    ////Console.WriteLine(finaleDocDate);

                    //GlobalVar.object1CTSD.GetType().InvokeMember("НайтиПоНомеру", BindingFlags.InvokeMethod, null, finaleDoc, new object[] {finaleDocNumber, finaleDocDate});
                    ////Console.WriteLine("doc finded");

                    //object isExtDocSelected = GlobalVar.object1CTSD.GetType().InvokeMember("Выбран", BindingFlags.InvokeMethod, null, finaleDoc, null);
                    ////Console.WriteLine(isExtDocSelected);
                    //-------------------

                    //--needed--if (int.Parse(_isfinalDocSelected.ToString()) == 1)
                    //--needed--{
                    //object extDocObject = GlobalVar.object1CTSD.GetType().InvokeMember("ТекущийДокумент", BindingFlags.InvokeMethod, null, finaleDoc, null);

                    //object docno = GlobalVar.object1CTSD.GetType().InvokeMember("НомерДок", BindingFlags.GetProperty, null, _finalDocObj, null);

                    object openFormResult = GlobalVar.object1CTSD.GetType().InvokeMember("OpenForm", BindingFlags.InvokeMethod, null, GlobalVar.object1CTSD, new object[] { _finalDocObj, FormContext, 0 });
                    WebLogger.logger.Error(openFormResult);
                    return openFormResult.ToString();

                    //if (int.Parse(openFormResult.ToString()) == 1)
                    //{
                    //object FormContext_Form = GlobalVar.object1C.GetType().InvokeMember("Форма", BindingFlags.GetProperty, null, FormContext, null);
                    //object FormContext_Params = GlobalVar.object1C.GetType().InvokeMember("Параметр", BindingFlags.GetProperty, null, FormContext_Form, null);
                    //}

                    //neeededdddd-
                    //}
                    //else
                    //{
                    //    return "Документ для записи не выбран" + int.Parse(_isfinalDocSelected.ToString());
                    //}

                }
                catch (Exception ex)
                {
                    string errorLine = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(":") + 1);
                    //if (ex.Message.Contains("Сервер RPC недоступен"))
                    //{
                    Methods1C7.DisconnectTSD();
                    //}
                    return ex.Message + "[" + errorLine + "]";
                }
            }

            void ExitSystem_COM()
            {
                object result = GlobalVar.type1CTSD.InvokeMember(
                    "ExitSystem",
                    BindingFlags.InvokeMethod,
                    null,
                    GlobalVar.object1CTSD,
                    new object[] { 0 });
                GarbageCollection_COM();
            }

            void GarbageCollection_COM()
            {
                Marshal.FinalReleaseComObject(GlobalVar.object1CTSD);
                GlobalVar.object1CTSD = null;
                GlobalVar.type1CTSD = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
