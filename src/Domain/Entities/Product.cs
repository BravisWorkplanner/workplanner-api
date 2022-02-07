using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Type { get; set; }

        public string Description { get; set; }
    }
}