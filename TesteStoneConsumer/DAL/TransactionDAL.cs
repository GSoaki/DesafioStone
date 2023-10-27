using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Nest;

namespace DAL
{
    public class TransactionDAL
    {
        static public bool MapIndex(IElasticClient elasticClient)
        {
            var createIndexResponse = elasticClient.Indices.Create("transactions", c => c
                .Settings(s => s
                    .NumberOfShards(4)
                    .NumberOfReplicas(1)
                )
                .Map<TransactionModel>(m => m
                    .AutoMap<TransactionModel>()
                    .AutoMap(typeof(TransactionModel))
                )
            );

            return createIndexResponse.IsValid;
        }

        static public bool IndexTransactionData(IElasticClient elasticClient, TransactionModel transaction)
        {
            var indexResponse = elasticClient.Index(transaction, i => i.Index("transactions"));

            if (indexResponse.ServerError != null && indexResponse.ServerError.Status == 404)
            {
                MapIndex(elasticClient);
                indexResponse = elasticClient.Index(transaction, i => i.Index("transactions"));
            }

            if (!indexResponse.IsValid)
            {
                return false;
            }

            return true;
        }

        static public List<TransactionModel> GetTransactionsByClientIdAndDateRange(IElasticClient elasticClient, string clientId, DateTime startDate, DateTime endDate)
        {
            var searchResponse = elasticClient.Search<TransactionModel>(s => s
                .Index("transactions")
                .Query(q => q
                    .Bool(b => b
                        .Filter(filter => filter.Term(t => t.Field(f => f.ClientId).Value(clientId)),
                            filter => filter.DateRange(r => r.Field(f => f.CreatedAt).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate))
                        )
                    )
                )
            );

            return searchResponse.Documents.ToList();
        }

        static public Dictionary<string, decimal> SumAmountByType(IElasticClient elasticClient, string clientId, DateTime startDate, DateTime endDate)
        {
            var searchResponse = elasticClient.Search<TransactionModel>(s => s
                .Index("transactions")
                .Query(q => q
                    .Bool(b => b
                        .Filter(filter => filter.Term(t => t.Field(f => f.ClientId).Value(clientId)),
                            filter => filter.DateRange(r => r.Field(f => f.CreatedAt).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate))
                        )
                    )
                )
                .Aggregations(a => a
                    .Terms("type_aggs", t => t
                        .Field(f => f.Type)
                        .Aggregations(agg => agg
                            .Sum("sum_amount", su => su
                                .Field(f => f.Amount)
                            )
                        )
                    )
                )
            );

            var sumByType = new Dictionary<string, decimal>();

            var typeAggs = searchResponse.Aggregations.Terms("type_aggs");
            foreach (var bucket in typeAggs.Buckets)
            {
                var type = bucket.Key;
                var sumAggs = bucket.Sum("sum_amount");
                var sum = (decimal)sumAggs.Value;

                sumByType[type] = sum;
            }

            return sumByType;
        }
    }

}
