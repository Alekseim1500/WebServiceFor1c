using System;
using System.Collections.Generic;
using Confluent.Kafka;
using KafkaNet.Protocol;

public class Kafka
{
    private readonly string soket = GlobalMethods.ParametrObjects("KafkaSoket").AllKeys[0];
    private List<string> topics;


    public Kafka(List<string> topics)
    { 
        this.topics = topics;
    }

    public bool Produser(dynamic mess)
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
            var message = new Message<Null, string> { Value = mess };
            
            foreach (var topic in topics)
            {
                var result = producer.ProduceAsync(topic, message).GetAwaiter().GetResult();
                WebLogger.logger.Trace($"Отправили в сообщение в kafka. Topic: {result.Topic}, Смещение: {result.Offset.Value}, Какое отправили сообщение: {result.Value}");
            }

            return true;
        }
        catch
        {
            WebLogger.logger.Error($"Неудачная попытка отправки сообщения в kafka");
            return false;
        }
    }

    public bool Consumer(out IConsumer<Null, string> consumer)
    {
        consumer = null;
        try
        {
            // создаем объект конфигурации ConsumerConfig, который будет использоваться для подключения к Kafka брокеру
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group", // идентификатор группы, к которой будет принадлежать Consumer
                BootstrapServers = soket,
                AutoOffsetReset = AutoOffsetReset.Earliest // начать чтение сообщений с самого начала топика 
            };

            var topics = new List<string> { "New_Topic", "New_Topic1" };

            consumer = new ConsumerBuilder<Null, string>(conf).Build();
            consumer.Subscribe(topics);

            return true;
        }
        catch
        {
            return false;
        }
    }
}