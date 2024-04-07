namespace ASSISTENTE.Client.Common.Exceptions;

public sealed class ClientException : Exception
{
    public ClientException(string message) : base(message)
    {
    }

    public ClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
}