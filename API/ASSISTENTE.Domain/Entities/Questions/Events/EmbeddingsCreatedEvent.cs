using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions.Events;

public sealed record EmbeddingsCreatedEvent(QuestionId QuestionId) : IDomainEvents;
