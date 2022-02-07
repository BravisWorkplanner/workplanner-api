using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        // TODO: Temporary solution since sql lite does not support sequences
        public string ObjectNumber
        {
            get
            {
                if (Id < 10)
                {
                    return $"B-00{Id}";
                }

                if (Id > 9 && Id < 99)
                {
                    return $"B-0{Id}";
                }

                return $"B-{Id}";
            }
        }

        public string Description { get; set; }

        public string Address { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public ICollection<Expense> Expenses { get; set; }

        public ICollection<TimeRegistration> TimeRegistrations { get; set; }
    }
}