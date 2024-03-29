﻿using RPGCrewPlugin.Data.Schema;

namespace RPGCrewPlugin.Data.DataObjects
{
    public class TransactionDataObject : IDataObject
    {
        [PrimaryKey]
        public long Id { get; set; }
        [Required]
        public long ToAccountId { get; set; }
        [Required]
        public long FromAccountId { get; set; }
        [Required]
        public decimal TransactionAmount { get; set; }
        [Required]
        public int TransactedOn { get; set; }
        [StringLength(256)]
        public string Reason { get; set; }
    }
}
