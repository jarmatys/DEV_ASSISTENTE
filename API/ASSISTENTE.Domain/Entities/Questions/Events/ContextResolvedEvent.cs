using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions.Events;

public sealed record ContextResolvedEvent(QuestionId QuestionId, QuestionContext Context) : IDomainEvents;
