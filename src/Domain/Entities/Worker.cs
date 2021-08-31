using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Worker : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}