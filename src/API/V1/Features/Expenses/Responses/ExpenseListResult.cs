using Domain.Entities;
using Domain.Enums;

namespace API.V1.Features.Expenses.Responses
{
    public class ExpenseListResult
    {
        public Product Product { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int OrderId { get; set; }

        public int WorkerId { get; set; }
    }
}