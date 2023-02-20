using System;
using Confluent.Kafka;

public class Kafka
{
    private static readonly string soket = "192.168.205.106:9092"; //2181
    private static readonly string topic = "New_Topic";

    public static string Produser(dynamic Element)
    {
        try
        {
            // Настройка параметров продюсера
            var config = new ProducerConfig
            {
                BootstrapServers = soket,
                ClientId = "test-client"
            };
            var producer = new ProducerBuilder<Null, string>(config).Build();

            // Отправка сообщения в топик
            var message = new Message<Null, string> { Value = Element };
            var result = producer.ProduceAsync(topic, message).GetAwaiter().GetResult(); //на этой строчке долго сидит, не может отправить (не получается дойти до kafka?)
            return $"Message: {result.Message}, Offset: {result.Offset}, TopicPartition: {result.TopicPartition}";
        }
        catch
        {
            return "Ошибка отправки сообщения ): ";
        }
    }

    public static bool Consumer(out string Mess, out string Error)
    {
        Mess = "";
        Error = "";
        try
        {
            // создаем объект конфигурации ConsumerConfig, который будет использоваться для подключения к Kafka брокеру
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
                var message = consumer.Consume(); //и тут застревает
                Mess = message.Message.ToString();
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            return false;
        }
        return true;
    }
}