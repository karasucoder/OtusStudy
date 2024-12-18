namespace StackClass;

public class Stack
{
    private List<string> _stack = new List<string>();

    public int Size
    {
        get { return _stack.Count; }
    }

    public string? Top
    {
        get
        {
            if (_stack.Count == 0)
            {
                return null;
            }

            return _stack.Last();
        }
    }


    public Stack(params string[] items)
    {
        foreach (var s in items)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new Exception("The list of arguments to be passed can't be empty");
            }

            _stack.Add(s);
        }
    }

    public void Add(string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            throw new Exception("The list of arguments to be passed can't be empty");
        }

        _stack.Add(item);
    }

    public string Pop()
    {
        if (_stack.Count == 0)
        {
            throw new Exception("Stack is empty");
        }

        var lastItem = _stack.Last();

        _stack.Remove(lastItem);

        return lastItem;
    }

    public static Stack Concat(params Stack[] stacks)
    {
        var result = new Stack();

        foreach (var s in stacks)
        {
            result.Merge(s);
        }

        return result;
    }
}
