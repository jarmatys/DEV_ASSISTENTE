namespace ASSISTENTE.Worker.Sync.Exceptions;

public sealed class WorkerException(string message) : Exception($"Consumed failed: {message}");