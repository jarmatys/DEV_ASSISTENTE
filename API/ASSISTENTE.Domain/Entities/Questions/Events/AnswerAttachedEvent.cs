using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions.Events;

public sealed record AnswerAttachedEvent(QuestionId QuestionId) : IDomainEvents;
