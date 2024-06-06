using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.QuestionNotes.Events;

public sealed record QuestionNoteCreatedEvent(QuestionId QuestionId) : IDomainEvents;
