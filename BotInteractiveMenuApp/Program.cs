namespace BotInteractiveMenuApp;

internal class Program
{
    static void Main(string[] args)
    {
        string userName = null;

        List<string> tasks = new List<string>();

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
                case "/addtask":
                    AddTask(tasks);
                    break;
                case "/showtasks":
                    ShowTasks(tasks);
                    break;
                case "/removetask":
                    RemoveTask(tasks);
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
            "/addtask" +
            Environment.NewLine +
            "/showtasks" +
            Environment.NewLine +
            "/removetask" +
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
            "- /addtask: Adds a new task to the list." +
            Environment.NewLine +
            "- /showtasks: Displays the list of tasks." +
            Environment.NewLine +
            "- /removetask: Removes the task from the list." +
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
            string echoText = userText.Substring(5);

            if (echoText.Length > 0)
            {
                if (echoText[0] == ' ')
                {
                    if (string.IsNullOrEmpty(echoText.Substring(1)))
                    {
                        Console.WriteLine("The text after /echo can't be empty." +
                            Environment.NewLine +
                            " ");
                    }
                    else
                    {
                        Console.WriteLine($"{userName}, your text is {echoText.Trim()}." +
                            Environment.NewLine +
                            " ");
                    }
                }
                else
                {
                    Console.WriteLine("The command is unavalaible." +
                        Environment.NewLine +
                        " ");
                }
            }
            else
            {
                Console.WriteLine("Make sure you entered some text after the command. For example: /echo <your text is here>." +
                    Environment.NewLine +
                    " ");
            }
        }
    }

    static void AddTask(List<string> tasks)
    {
        Console.WriteLine("Input the task:");

        string task = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(task))
        {
            Console.WriteLine("The task description can't be empty." +
                Environment.NewLine +
                " ");
        }
        else
        {
            tasks.Add(task);

            Console.WriteLine($"The task \"{task}\" has been successfully added to the list!" +
                Environment.NewLine +
                " ");
        }
    }

    static void ShowTasks(List<string> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("The task list is empty." +
                Environment.NewLine +
                " ");
        }
        else
        {
            Console.WriteLine("Here is the current list of your tasks:");
            foreach (string task in tasks)
            {
                Console.WriteLine($"{tasks.IndexOf(task) + 1} - {task}");
            }
            Console.WriteLine(Environment.NewLine + " ");
        }
    }

    static void RemoveTask(List<string> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("The command is unavailable because the task list is empty." +
                Environment.NewLine +
                " ");
            return;
        }

        Console.WriteLine("Here is the current list of your tasks:");

        foreach (string task in tasks)
        {
            Console.WriteLine($"{tasks.IndexOf(task) + 1} - {task}");
        }

        Console.WriteLine("Input the number of the task you want to remove:");

        bool isValid = int.TryParse(Console.ReadLine(), out int taskNumber);

        if (isValid)
        {
            var taskToRemove = tasks.ElementAtOrDefault(taskNumber - 1);
            
            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
                Console.WriteLine($"The task at number {taskNumber} has been successfully removed from the list." +
                    Environment.NewLine +
                    " ");
            }
            else
            {
                Console.WriteLine("Sorry, there is no task with this number." +
                        Environment.NewLine +
                        " ");
            }
        }
        else
        {
            Console.WriteLine("The value is invalid. Try once again." +
                Environment.NewLine +
                " ");
        }
    }
}