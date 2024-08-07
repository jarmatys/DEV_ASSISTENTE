using SOFTURE.Contract.Common.Messaging;

namespace ASSISTENTE.Contract.Messages.Internal.Knowledge;

public sealed record CreateEmbeddingsMessage(Guid QuestionId) : IMessage;