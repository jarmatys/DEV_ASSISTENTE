using SOFTURE.Contract.Common.Messaging;

namespace ASSISTENTE.Contract.Messages.Internal.Knowledge;

public sealed record GenerateAnswerMessage(Guid QuestionId) : IMessage;