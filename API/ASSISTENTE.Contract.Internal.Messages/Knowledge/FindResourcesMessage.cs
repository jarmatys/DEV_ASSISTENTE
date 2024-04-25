using ASSISTENTE.Common.Messaging;

namespace ASSISTENTE.Contract.Internal.Messages.Knowledge;

public sealed record FindResourcesMessage(Guid QuestionId, string? ConnectionId) : IMessage;