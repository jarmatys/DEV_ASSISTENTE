using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language;

namespace ASSISTENTE.Domain.Commons
{
    public abstract class AuditableEntity<TIndentifier> : Entity<TIndentifier>, IAuditableEntity
        where TIndentifier : class, IIdentifier
    {
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
