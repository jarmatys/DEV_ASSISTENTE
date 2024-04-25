namespace ASSISTENTE.Common.Exceptions;

public sealed class MissingSettingsException(string message) : Exception(message);