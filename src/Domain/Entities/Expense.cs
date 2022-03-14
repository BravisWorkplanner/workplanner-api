using System;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Expense : BaseEntity
    {
        public string Description { get; set; }

        public double Price { get; set; }

        public string InvoiceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int WorkerId { get; set; }

        public Worker Worker { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}