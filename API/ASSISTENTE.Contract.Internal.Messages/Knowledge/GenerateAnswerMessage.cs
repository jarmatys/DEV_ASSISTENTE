using ASSISTENTE.Common.Messaging;

namespace ASSISTENTE.Contract.Internal.Messages.Knowledge;

public sealed record GenerateAnswerMessage(Guid QuestionId, string? ConnectionId) : IMessage;