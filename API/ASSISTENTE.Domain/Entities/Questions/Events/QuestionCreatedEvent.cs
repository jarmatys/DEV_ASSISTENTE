using ASSISTENTE.Domain.Commons.Interfaces;

namespace ASSISTENTE.Domain.Entities.Questions.Events;

public sealed record QuestionCreatedEvent(Guid QuestionId, string? ConnectionId) : IDomainEvents;
