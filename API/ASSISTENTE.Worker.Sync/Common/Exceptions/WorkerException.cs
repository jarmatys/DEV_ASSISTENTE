namespace ASSISTENTE.Worker.Sync.Common.Exceptions;

public sealed class WorkerException(string message) : Exception($"Consumed failed: {message}");