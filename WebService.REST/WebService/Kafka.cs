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
            // ��������� ���������� ���������
            var config = new ProducerConfig
            {
                BootstrapServers = soket,
                ClientId = "test-client"
            };
            var producer = new ProducerBuilder<Null, string>(config).Build();

            // �������� ��������� � �����
            var message = new Message<Null, string> { Value = Element };
            var result = producer.ProduceAsync(topic, message).GetAwaiter().GetResult(); //�� ���� ������� ����� �����, �� ����� ��������� (�� ���������� ����� �� kafka?)
            return $"Message: {result.Message}, Offset: {result.Offset}, TopicPartition: {result.TopicPartition}";
        }
        catch
        {
            return "������ �������� ��������� ): ";
        }
    }

    public static bool Consumer(out string Mess, out string Error)
    {
        Mess = "";
        Error = "";
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
                var message = consumer.Consume(); //� ��� ����������
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