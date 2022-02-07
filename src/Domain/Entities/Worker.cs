using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Worker : BaseEntity
    {
        public string Name { get; set; }

        public string Company { get; set; }

        public string PhoneNumber { get; set; }
    }
}