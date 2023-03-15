using Confluent.Kafka;
using KafkaNet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

public class SqlConnector
{
    public static async Task<string> PostObject()
    {
        //получаем id потока
        var id = Thread.CurrentThread.ManagedThreadId;
        var threadId = "S" + id.ToString();
        WebLogger.logger.Trace($"({threadId}): из kafka в sql");

        try
        {
            //экземпляр kafka создаётся с помощью данных из "My.config"
            var kafka = new Kafka(GlobalMethods.ParametrObjects("KafkaConsumerTopics", "Тестовое событие"), threadId);
            IConsumer<Null, string> consumer;
            if (!kafka.GetConsumer(out consumer))
            {
                throw new Exception("Не получилось подключиться к kafka!");
            }
            else
            {
                using (consumer)
                {
                    while (true)
                    {
                        var data = await Task.Run(() => consumer.Consume());
                        var mess = data.Message.Value;
                        WebLogger.logger.Trace($"{threadId}: Получили сообщение из kafka: {mess}");

                        var sql = new Sql(GlobalMethods.ParametrObjects("Sql", data.Topic));
                        sql.insert(mess);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            var errorMessage = $"{ex.Message} {ex.InnerException?.Message}";
            Messenger.Post(errorMessage);
            WebLogger.logger.Error($"{threadId}: {errorMessage}");
            return errorMessage;
        }
    }
}