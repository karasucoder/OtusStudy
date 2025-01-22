namespace InterfacesLearning;

internal class Program
{
    static void Main(string[] args)
    {
        var quad = new Quadcopter();
        Console.WriteLine(quad.GetInfo());
    }
}
