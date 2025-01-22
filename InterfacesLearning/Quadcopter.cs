namespace InterfacesLearning;

public class Quadcopter : IFlyingRobot, IChargeable
{
    private readonly List<string> _components = ["rotor1", "rotor2", "rotor3", "rotor4"];

    public List<string> GetComponents()
    {
        return _components;
    }

    public void Charge()
    {
        Console.WriteLine("Charging...");

        Thread.Sleep(3000);

        Console.WriteLine("Charged!");
    }

    public string GetInfo()
    {
        return $"Here is the components of the quadcopter: {string.Join(", ", _components.Select(x => x))}";
    }
}
