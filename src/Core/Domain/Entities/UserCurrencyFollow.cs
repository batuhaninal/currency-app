using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class UserCurrencyFollow : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
    }    
}