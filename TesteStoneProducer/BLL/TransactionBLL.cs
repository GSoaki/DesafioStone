using Model;
using DAL;
using System;
using System.Collections.Generic;
using System.Transactions;
using Nest;

namespace BLL
{
    public class TransactionBLL
    {
        static public bool IndexTransactionData(IElasticClient elasticClient, TransactionModel transaction)
        {
            if (transaction.IsValid())
            {
                if (TransactionDAL.IndexTransactionData(elasticClient, transaction))
                {
                    return true;
                }
            }
            return false;
        }

        static public List<TransactionModel> GetTransactionsByClientIdAndDateRange(IElasticClient elasticClient, string clientId, DateTime startDate, DateTime endDate)
        {
            return TransactionDAL.GetTransactionsByClientIdAndDateRange(elasticClient, clientId, startDate, endDate);
        }

        static public Dictionary<string, decimal> SumAmountByType(IElasticClient elasticClient, string clientId, DateTime startDate, DateTime endDate)
        {
            return TransactionDAL.SumAmountByType(elasticClient, clientId, startDate, endDate);
        }
    }
}
