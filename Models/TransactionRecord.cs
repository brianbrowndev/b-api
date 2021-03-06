﻿using System;
using System.Collections.Generic;

namespace B.API.Models
{
    public partial class TransactionRecord
    {
        public TransactionRecord()
        {
            TransactionRecordTag = new HashSet<TransactionRecordTag>();
        }

        public long Id { get; set; }
        public long BankId { get; set; }
        public long UserId { get; set; }
        public long CategoryId { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public string Note { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual TransactionCategory Category { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TransactionRecordTag> TransactionRecordTag { get; set; }
    }
}
