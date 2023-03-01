using System;
using Confluent.Kafka;

public class Kafka
{
    private static readonly string soket = "192.168.205.106:9092"; //2181
    private static readonly string topic = "New_Topic";

    public static bool Produser(dynamic Element)
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
            var result = producer.ProduceAsync(topic, message).GetAwaiter().GetResult();

            WebLogger.logger.Trace($"Отправили в сообщение в kafka. Topic: {result.Topic}, Смещение: {result.Offset.Value}, Какое отправили сообщение: {result.Value}");
            return true;
        }
        catch
        {
            WebLogger.logger.Error($"Неудачная попытка отправки сообщения в kafka");
            return false;
        }
    }

    public static bool Consumer(out string Mess)
    {
        Mess = "";
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
                var message = consumer.Consume();
                Mess = message.Message.Value;
            }

            WebLogger.logger.Trace($"Получили сообщение из kafka: {Mess}");
            return true;
        }
        catch (Exception ex)
        {
            WebLogger.logger.Error($"Неудачная попытка получения сообщения из kafka. Сообщение ошибки: {ex.Message}");
            return false;
        }
    }
}