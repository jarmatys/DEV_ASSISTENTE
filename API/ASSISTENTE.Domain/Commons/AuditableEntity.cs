using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language;

namespace ASSISTENTE.Domain.Commons
{
    public abstract class AuditableEntity<TIdentifier> : Entity<TIdentifier>, IAuditableEntity
        where TIdentifier : class, IIdentifier
    {
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
