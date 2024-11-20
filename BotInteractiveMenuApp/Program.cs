namespace BotInteractiveMenuApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string userName = null;

            while (true)
            {
                Greeting(userName);

                string userCommand = Console.ReadLine();

                if (userCommand.StartsWith("/echo"))
                {
                    Echo(userName, userCommand);
                    continue;
                }

                switch (userCommand)
                {
                    case "/start":
                        userName = Start();
                        break;
                    case "/help":
                        Help(userName);
                        break;
                    case "/info":
                        Info(userName);
                        break;
                    case "/exit":
                        return;
                    default:
                        Console.WriteLine("The command is anavailable." +
                            Environment.NewLine +
                            " ");
                        break;
                }
            }
        }

        static void Greeting(string userName)
        {
            string botGreeting = "Welcome to the bot's interactive menu";

            string botCommandsList = "Use the following commands to communicate with the bot:" +
                Environment.NewLine +
                "/start" +
                Environment.NewLine +
                "/help" +
                Environment.NewLine +
                "/info" +
                Environment.NewLine +
                "/exit";

            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine($"{botGreeting}." +
                    Environment.NewLine +
                    $"{botCommandsList}");
            }
            else
            {
                Console.WriteLine($"{botGreeting}, {userName}." +
                    Environment.NewLine +
                    $"{botCommandsList}");
            }
        }

        static string Start()
        {
            Console.WriteLine("Input your name, please.");

            string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name can't be null." +
                    Environment.NewLine +
                            " ");
            }
            else
            {
                Console.WriteLine($"Hello, {name}. The command /echo is available for you now." +
                        Environment.NewLine +
                        " ");
            }

            return name;
        }

        static void Help(string userName)
        {
            string helpTextHeading = "Here you can find more detailed information about available commands";

            string helpText = "- /start: Restarts the bot and initializes your session." +
                Environment.NewLine +
                "- /info: Displays an information about the program's version and it's creation date." +
                Environment.NewLine +
                "- /help: Provides information on how to use the bot." +
                Environment.NewLine +
                "- /echo: Echoes back the text you enter after this command. Note: The command is only available after you have used the command /start and provided your name." +
                Environment.NewLine +
                "- /exit: Exits the program." +
                Environment.NewLine +
                " ";

            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine($"{helpTextHeading}:" +
                    Environment.NewLine +
                    $"{helpText}" +
                    Environment.NewLine +
                    " ");
            }
            else
            {
                Console.WriteLine($"{helpTextHeading}, {userName}:" +
                    Environment.NewLine +
                    $"{helpText}" +
                    Environment.NewLine +
                    " ");
            }
        }

        static void Info(string userName)
        {
            string infoText = $"Version: 1.0.0" +
                Environment.NewLine +
                "Creation Date: 17/11/2024";

            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine($"{infoText}" +
                    Environment.NewLine +
                    " ");
            }
            else
            {
                Console.WriteLine($"{userName}," +
                    Environment.NewLine +
                    $"{infoText}" +
                    Environment.NewLine +
                    " ");
            }
        }

        static void Echo(string userName, string userText)
        {
            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Sorry, the command is not available now. Use the command /start instead." +
                    Environment.NewLine +
                    " ");
            }
            else
            {
                if (userText.Length > 5)
                {
                    if (userText[5] == ' ')
                    {
                        string echoText = userText.Substring(5);
                        Console.WriteLine($"Your text is {echoText.Substring(1)}." +
                            Environment.NewLine +
                            " ");
                    }
                    else
                    {
                        Console.WriteLine("The command is anavailable." +
                            Environment.NewLine +
                            " ");
                    }

                }
                else
                {
                    Console.WriteLine("Please, make sure to enter some text after the command. For example: /echo {your text here}." +
                        Environment.NewLine +
                            " ");
                }
            }
        }
    }
}