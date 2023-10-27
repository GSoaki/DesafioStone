using BLL;
using Confluent.Kafka;
using Elasticsearch.Net;
using Model;
using Nest;
using Newtonsoft.Json;
using System;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConsumerConfig
        {
            GroupId = "group-transaction",
            BootstrapServers = "kafka:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe("transactionQueue");

            var elasticsearchUri = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(elasticsearchUri)
                .DefaultIndex("transactions");

            IElasticClient client = new ElasticClient(settings);

            while (true)
            {
                var consumeItem = consumer.Consume();
                if (consumeItem != null)
                {
                    Console.WriteLine($"Consumed message: {consumeItem.Message.Value}");
                    TransactionModel transaction = JsonConvert.DeserializeObject<TransactionModel>(consumeItem.Message.Value);

                    TransactionBLL.IndexTransactionData(client, transaction);
                }
            }
        }
    }
}
