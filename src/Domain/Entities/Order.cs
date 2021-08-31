using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string ObjectNumber { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public ICollection<Expense> Expenses { get; set; }

        public ICollection<TimeRegistration> TimeRegistrations { get; set; }
    }
}
