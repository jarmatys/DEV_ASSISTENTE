namespace ASSISTENTE.Domain.Commons.Interfaces;

public interface IAuditableEntity
{
    string? CreatedBy { set; }
    DateTime Created { set; }
    string? ModifiedBy { set; }
    DateTime? Modified { set; }
}