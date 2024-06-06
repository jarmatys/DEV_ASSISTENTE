using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.QuestionCodes.Events;

public sealed record QuestionCodeCreatedEvent(QuestionId QuestionId) : IDomainEvents;
