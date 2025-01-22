namespace QuadraticEquationErrorHandling;

public class NoRootsException : Exception
{
    public NoRootsException() 
    { 
    }

    public NoRootsException(string message)
        : base(message)
    { 
    }

    public NoRootsException(string message, Exception inner) 
        : base(message, inner)
    { 
    }
}
