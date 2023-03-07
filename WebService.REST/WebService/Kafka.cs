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
            // ��������� ���������� ���������
            var config = new ProducerConfig
            {
                BootstrapServers = soket,
                ClientId = "test-client"
            };
            var producer = new ProducerBuilder<Null, string>(config).Build();

            // �������� ��������� � �����
            var message = new Message<Null, string> { Value = mess };
            
            foreach (var topic in topics)
            {
                var result = producer.ProduceAsync(topic, message).GetAwaiter().GetResult();
                WebLogger.logger.Trace($"��������� � ��������� � kafka. Topic: {result.Topic}, ��������: {result.Offset.Value}, ����� ��������� ���������: {result.Value}");
            }

            return true;
        }
        catch
        {
            WebLogger.logger.Error($"��������� ������� �������� ��������� � kafka");
            return false;
        }
    }

    public bool Consumer(out IConsumer<Null, string> consumer)
    {
        consumer = null;
        try
        {
            // ������� ������ ������������ ConsumerConfig, ������� ����� �������������� ��� ����������� � Kafka �������
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group", // ������������� ������, � ������� ����� ������������ Consumer
                BootstrapServers = soket,
                AutoOffsetReset = AutoOffsetReset.Earliest // ������ ������ ��������� � ������ ������ ������ 
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