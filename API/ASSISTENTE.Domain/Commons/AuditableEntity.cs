namespace ASSISTENTE.Domain.Commons
{
    public class AuditableEntity
    {
        public int Id { get; set; }
        
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
