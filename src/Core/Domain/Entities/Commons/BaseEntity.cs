namespace Domain.Entities.Commons
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
