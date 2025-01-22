namespace TuplesLearning;

internal class Program
{
    static void Main(string[] args)
    {
        var planetCatalog = new PlanetsCatalog();

        var earthResult = planetCatalog.GetPlanet("Земля");

        if (earthResult.sequenceNumber == 0 && earthResult.equatorLength == 0)
        {
            Console.WriteLine(earthResult.errorMessage);
        }
        else
        {
            Console.WriteLine($"Земля {earthResult.Item1}, {earthResult.Item2}");
        }

        var limoniaResult = planetCatalog.GetPlanet("Лимония");

        if (limoniaResult.sequenceNumber == 0 && limoniaResult.equatorLength == 0)
        {
            Console.WriteLine(limoniaResult.errorMessage);
        }
        else
        {
            Console.WriteLine("Лимония", limoniaResult.sequenceNumber, limoniaResult.equatorLength);
        }

        var marsResult = planetCatalog.GetPlanet("Марс");
        marsResult = planetCatalog.GetPlanet("Марс");

        if (marsResult.sequenceNumber == 0 && marsResult.equatorLength == 0)
        {
            Console.WriteLine(marsResult.errorMessage);
        }
        else
        {
            Console.WriteLine("Марс", marsResult.sequenceNumber, marsResult.equatorLength);
        }
    }
}
