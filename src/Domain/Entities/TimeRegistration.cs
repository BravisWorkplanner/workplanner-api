using Domain.Entities.Base;
using System;

namespace Domain.Entities
{
    public class TimeRegistration : BaseEntity
    {
        public DateTime DateTime { get; set; }

        public string Week { get; set; }

        public double Hours { get; set; }

        public Order Order { get; set; }

        public Worker Worker { get; set; }
    }
}