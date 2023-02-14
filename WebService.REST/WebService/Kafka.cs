using System;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

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