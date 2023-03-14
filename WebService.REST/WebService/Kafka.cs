using System.Collections.Generic;
using Confluent.Kafka;

public class Kafka
{
    private readonly string soket = GlobalMethods.ParametrObjects("KafkaSoket").AllKeys[0];
    private List<string> topics;
    private string threadId = "";


    public Kafka(List<string> topics, string threadId)
    { 
        this.topics = topics;
        this.threadId = threadId;
    }

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
                WebLogger.logger.Trace($"{threadId}: ��������� ��������� � kafka. Topic: {result.Topic}, ����� ��������� ���������: {result.Value}");
            }

            return true;
        }
        catch
        {
            WebLogger.logger.Error($"{threadId}: ��������� ������� �������� ��������� � kafka");
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