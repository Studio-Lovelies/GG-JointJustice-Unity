using System;

public class ObjectLoadingException : Exception
{
    public ObjectLoadingException(string message, Exception exception) : base(message, exception)
    {
    }
}