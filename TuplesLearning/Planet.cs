namespace TuplesLearning;

public class Planet
{
    public string Name { get; set; }

    public int SequenceNumber { get; set; }

    public double EquatorLength { get; set; }

    public Planet? PreviousPlanet { get; set; }
}
