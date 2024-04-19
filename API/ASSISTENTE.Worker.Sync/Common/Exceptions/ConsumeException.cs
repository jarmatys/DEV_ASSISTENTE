namespace ASSISTENTE.Worker.Sync.Common.Exceptions;

internal sealed class ConsumeException(string message) : Exception($"Consumed failed: {message}");