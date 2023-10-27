using BLL;
using Microsoft.AspNetCore.Mvc;
using Model;
using Nest;
using System;
using System.Collections.Generic;

// falta escrever texto e testar

namespace TesteElastic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IElasticClient _clientProvider;

        public TransactionController(IElasticClient clientProvider)
        {
            _clientProvider = clientProvider;
        }

        [HttpGet]
        public List<TransactionModel> Get(string client_id, DateTime dateFrom, DateTime dateTo)
        {
            return TransactionBLL.GetTransactionsByClientIdAndDateRange(_clientProvider, client_id,dateFrom,dateTo);
        }

        public Dictionary<string, decimal> GetTotalAmout(string client_id, DateTime dateFrom, DateTime dateTo)
        {
            return TransactionBLL.SumAmountByType(_clientProvider, client_id, dateFrom, dateTo);
        }
    }

}
