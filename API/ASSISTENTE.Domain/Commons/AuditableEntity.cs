using ASSISTENTE.Domain.Commons.Interfaces;

namespace ASSISTENTE.Domain.Commons
{
    public abstract class AuditableEntity<TIndentifier> : Entity<TIndentifier>, IAuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
