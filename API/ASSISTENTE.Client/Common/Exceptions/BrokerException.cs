namespace ASSISTENTE.Client.Common.Exceptions;

public sealed class BrokerException : Exception
{
    public BrokerException(string message) : base(message)
    {
    }

    public BrokerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}