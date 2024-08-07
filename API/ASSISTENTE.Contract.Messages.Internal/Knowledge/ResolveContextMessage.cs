using SOFTURE.Contract.Common.Messaging;

namespace ASSISTENTE.Contract.Messages.Internal.Knowledge;

public sealed record ResolveContextMessage(Guid QuestionId) : IMessage;