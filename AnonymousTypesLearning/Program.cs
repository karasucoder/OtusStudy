using System;
using System.Numerics;
using System.Xml.Linq;
namespace AnonymousTypesLearning;

internal class Program
{
    static void Main(string[] args)
    {
        var venus = new { Name = "Венера", SequenceNumber = 2, EquatorLength = 38025, PreviousPlanet = "Данные отсутствуют" };

        var earth = new { Name = "Земля", SequenceNumber = 3, EquatorLength = 40075, PreviousPlanet = venus };

        var mars = new { Name = "Марс", SequenceNumber = 4, EquatorLength = 21326, PreviousPlanet = earth };

        var venusDuplicate = new { Name = "Венера", SequenceNumber = 2, EquatorLength = 38025, PreviousPlanet = "Данные отсутствуют" };

        Console.WriteLine($"Название планеты - {venus.Name}, " +
            $"порядковый номер от Солнца - {venus.SequenceNumber}, " +
            $"длина экватора - {venus.EquatorLength}, " +
            $"предыдущая планета - {venus.PreviousPlanet}. " +
            $"Эквивалентность Венере - {venus.Equals(venus)}");

        Console.WriteLine($"Название планеты - {earth.Name}, " +
            $"порядковый номер от Солнца - {earth.SequenceNumber}, " +
            $"длина экватора - {earth.EquatorLength}, " +
            $"предыдущая планета - {earth.PreviousPlanet.Name}. " +
            $"Эквивалентность Венере - {earth.Equals(venus)}");

        Console.WriteLine($"Название планеты - {mars.Name}, " +
            $"порядковый номер от Солнца - {mars.SequenceNumber}, " +
            $"длина экватора - {mars.EquatorLength}, " +
            $"предыдущая планета - {mars.PreviousPlanet.Name}. " +
            $"Эквивалентность Венере - {mars.Equals(venus)}");


        Console.WriteLine($"Название планеты - {venusDuplicate.Name}, " +
            $"порядковый номер от Солнца - {venusDuplicate.SequenceNumber}, " +
            $"длина экватора - {venusDuplicate.EquatorLength}, " +
            $"предыдущая планета - {venusDuplicate.PreviousPlanet}. " +
            $"Эквивалентность Венере - {venusDuplicate.Equals(venus)}");
    }
}
