using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Expense : BaseEntity
    {
        public Product Product { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public Order Order { get; set; }

        public Worker Worker { get; set; }
    }
}