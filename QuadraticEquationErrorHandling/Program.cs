
namespace QuadraticEquationErrorHandling;

internal class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            int a = 0;
            int b = 0;
            int c = 0;

            Console.WriteLine("Имеется квадратное уравнение следующего вида: a * x^2 + b * x + c = 0, где a != 0");
            Console.WriteLine("Введите значение a, не равное 0:");

            var aInput = Console.ReadLine();
            Console.WriteLine("Введите значение b:");

            var bInput = Console.ReadLine();
            Console.WriteLine("Введите значение c:");

            var cInput = Console.ReadLine();

            var dictionary = new Dictionary<string, string>
            {
                { "a", aInput },
                { "b", bInput },
                { "c", cInput }
            };

            var exceptionMessage = "Неверный формат параметров: ";

            try
            {
                var aParsingResult = int.TryParse(aInput, out a);
                var bParsingResult = int.TryParse(bInput, out b);
                var cParsingResult = int.TryParse(cInput, out c);

                if (!aParsingResult || a == 0) exceptionMessage += $" a";
                if (!bParsingResult) exceptionMessage += $" b";
                if (!cParsingResult) exceptionMessage += $" c";

                if (!aParsingResult || !bParsingResult || !cParsingResult) throw new ArgumentException(exceptionMessage);
            }
            catch (Exception)
            {
                FormatData(exceptionMessage, Severity.Error, dictionary);
                continue;
            }

            var discriminant = DiscriminantCalculation(a, b, c);

            try
            {
                switch (discriminant)
                {
                    case < 0:
                        throw new Exception("Вещественных значений не найдено");
                    case > 0:
                        var x1 = (-b + (int)Math.Sqrt(discriminant)) / 2 * a;
                        var x2 = (-b - (int)Math.Sqrt(discriminant)) / 2 * a;
                        Console.WriteLine($"x1 = {x1}, x2 = {x2}");
                        break;
                    case 0:
                        var x = (-b + (int)Math.Sqrt(discriminant)) / 2 * a;
                        Console.WriteLine($"x = {x}");
                        break;
                }
            }
            catch (Exception noRootsEx)
            {
                FormatData(noRootsEx.Message, Severity.Warning, dictionary);
                return;
            }
        }
    }

    static void FormatData(string message, Severity severity, IDictionary<string, string> data)
    {
        if (severity == Severity.Error)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("----------------------------------" +
                Environment.NewLine +
                $"{message}" +
                Environment.NewLine +
                "----------------------------------");

            foreach (var pair in data)
            {
                Console.WriteLine($"{pair.Key} = {pair.Value}");
            }

            Console.ResetColor();
        }

        else if (severity == Severity.Warning)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("----------------------------------" +
                Environment.NewLine + $"{message}" +
                Environment.NewLine +
                "----------------------------------");

            foreach (var pair in data)
            {
                Console.WriteLine($"{pair.Key} = {pair.Value}");
            }

            Console.ResetColor();
        }
    }

    static int DiscriminantCalculation(int a, int b, int c)
    {
        double discriminant = Math.Pow(b, 2) - 4 * a * c;

        return (int)discriminant;
    }

    enum Severity
    {
        Warning,
        Error
    }
}

