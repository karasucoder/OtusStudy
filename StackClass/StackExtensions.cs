namespace StackClass;

public static class StackExtensions
{
    public static void Merge(this Stack s1, Stack s2)
    {
        int size = s2.Size;

        for (int i = 0; i < size; i++)
        {
            s1.Add(s2.Pop());
        }
    }
}
