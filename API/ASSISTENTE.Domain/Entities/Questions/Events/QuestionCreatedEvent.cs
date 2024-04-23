using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Commons.Interfaces;

namespace ASSISTENTE.Domain.Entities.Questions.Events;

public sealed record QuestionCreatedEvent(int QuestionId, string? ConnectionId) : IDomainEvents;
