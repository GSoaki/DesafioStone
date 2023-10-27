using System;
using System.Collections.Generic;

namespace Model
{
    public class TransactionModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ClientId { get; set; }
        public string PayerId { get; set; }
        public decimal Amount { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(PayerId) && Amount >= 0;
        }
    }
}
