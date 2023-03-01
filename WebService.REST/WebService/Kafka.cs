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
            // ��������� ���������� ���������
            var config = new ProducerConfig
            {
                BootstrapServers = soket,
                ClientId = "test-client"
            };
            var producer = new ProducerBuilder<Null, string>(config).Build();

            // �������� ��������� � �����
            var message = new Message<Null, string> { Value = Element };
            var result = producer.ProduceAsync(topic, message).GetAwaiter().GetResult();

            WebLogger.logger.Trace($"��������� � ��������� � kafka. Topic: {result.Topic}, ��������: {result.Offset.Value}, ����� ��������� ���������: {result.Value}");
            return true;
        }
        catch
        {
            WebLogger.logger.Error($"��������� ������� �������� ��������� � kafka");
            return false;
        }
    }

    public static bool Consumer(out string Mess)
    {
        Mess = "";
        try
        {
            // ������� ������ ������������ ConsumerConfig, ������� ����� �������������� ��� ����������� � Kafka �������
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group", // ������������� ������, � ������� ����� ������������ Consumer
                BootstrapServers = soket,
                AutoOffsetReset = AutoOffsetReset.Earliest // ������ ������ ��������� � ������ ������ ������ 
            };

            using (var consumer = new ConsumerBuilder<Null, string>(conf).Build())
            {
                // ������������� � �������� ��������� �� ������
                consumer.Subscribe(topic);
                var message = consumer.Consume();
                Mess = message.Message.Value;
            }

            WebLogger.logger.Trace($"�������� ��������� �� kafka: {Mess}");
            return true;
        }
        catch (Exception ex)
        {
            WebLogger.logger.Error($"��������� ������� ��������� ��������� �� kafka. ��������� ������: {ex.Message}");
            return false;
        }
    }
}