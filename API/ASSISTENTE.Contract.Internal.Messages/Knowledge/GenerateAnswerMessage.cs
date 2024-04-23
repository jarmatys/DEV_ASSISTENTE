using ASSISTENTE.Common.Messaging;

namespace ASSISTENTE.Contract.Internal.Messages.Knowledge;

public sealed record GenerateAnswerMessage(int QuestionId, string? ConnectionId) : IMessage;