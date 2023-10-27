using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Model;
using System.Collections.Generic;
using System;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var queue = "transactionQueue";

            for (int i = 0; i < 100; i++)
            {

                Random random = new Random();
                List<string> types = new List<string> { "card", "pix", "boleto" };
                int typeRandom = random.Next(2);

                var data = new TransactionModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = types[typeRandom],
                    CreatedAt = DateTime.Today,
                    ClientId = Guid.NewGuid().ToString(),
                    PayerId = Guid.NewGuid().ToString(),
                    Amount = random.Next(101) + (random.Next(101) / 100),
                };

                var message = new Message<Null, string>
                {
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(data)
                };

                var deliveryReport = await producer.ProduceAsync(queue, message);

                Thread.Sleep(500);
            }
        }
    }
}
