namespace LambdaExpressionsLearning;

internal class Program
{
    static void Main(string[] args)
    {
        var planetCatalog = new PlanetsCatalog();

        Action<(int sequenceNumber, double equatorLength, string? errorMessage), string> displayPlanetInfo = (planetTuple, planetName) =>
        {
            if (planetTuple.sequenceNumber == 0 && planetTuple.equatorLength == 0)
                Console.WriteLine(planetTuple.errorMessage);
            else
                Console.WriteLine($"{planetName} {planetTuple.sequenceNumber}, {planetTuple.equatorLength}");
        };

        var methodCallingCounter = 0;

        Func<string, string?> planetValidator = planetName =>
        {
            methodCallingCounter++;

            if (methodCallingCounter == 3)
            {
                methodCallingCounter = 0;

                return $"При попытке получить название планеты {planetName}" +
                $" произошла ошибка: Вы спрашиваете слишком часто";
            }

            return null;
        };

        displayPlanetInfo(planetCatalog.GetPlanet("Земля", planetValidator), "Земля");
        displayPlanetInfo(planetCatalog.GetPlanet("Лимония", planetValidator), "Лимония");
        displayPlanetInfo(planetCatalog.GetPlanet("Марс", planetValidator), "Марс");
        displayPlanetInfo(planetCatalog.GetPlanet("Марс", planetValidator), "Марс");

        Func<string, string?> forbiddenPlanetValidator = planetName => planetName == "Лимония" ? "Это запретная планета" : null;
        displayPlanetInfo(planetCatalog.GetPlanet("Земля", forbiddenPlanetValidator), "Земля");
        displayPlanetInfo(planetCatalog.GetPlanet("Лимония", forbiddenPlanetValidator), "Лимония");
        displayPlanetInfo(planetCatalog.GetPlanet("Марс", forbiddenPlanetValidator), "Марс");
    }
}
