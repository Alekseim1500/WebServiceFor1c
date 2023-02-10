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