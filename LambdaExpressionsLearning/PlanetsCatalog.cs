namespace LambdaExpressionsLearning;

public class PlanetsCatalog
{
    private Planet[] _planets;

    public PlanetsCatalog()
    {
        var venus = new Planet { Name = "Венера", SequenceNumber = 2, EquatorLength = 38025, PreviousPlanet = null };
        var earth = new Planet { Name = "Земля", SequenceNumber = 3, EquatorLength = 40075, PreviousPlanet = venus };
        var mars = new Planet { Name = "Марс", SequenceNumber = 4, EquatorLength = 21326, PreviousPlanet = earth };

        _planets = [venus, earth, mars];
    }

    public (int sequenceNumber, double equatorLength, string? errorMessage) GetPlanet(string planetName, Func<string, string?> planetValidator)
    {
        var validationResult = planetValidator(planetName);

        var planet = _planets.FirstOrDefault(x => x.Name == planetName);

        if (planet == null)
        {
            if (validationResult != null)
            {
                return (0, 0, validationResult);
            }

            return (0, 0, "Не удалось найти планету");
        }
        else if (validationResult != null)
        {
            return (0, 0, validationResult);
        }

        return (planet.SequenceNumber, planet.EquatorLength, null);
    }
}