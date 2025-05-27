using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;

public class Program
{
    public static async Task Main(string[] args)
    {
        var directories = new List<DirectoryInfo>
        {
            new DirectoryInfo(@$"C:\Otus\TestDir1"),
            new DirectoryInfo(@$"C:\Otus\TestDir2")
        };

        foreach (var dir in directories)
        {
            if (!dir.Exists)
            {
                dir.Create();
            }

            SetAccess(dir);

            for (int c = 1; c <= 10; c++)
            {
                await using FileStream fs = File.Create(@$"{dir.ToString()}\File{c}");
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(@$"File{c}" +
                    Environment.NewLine +
                    $"{DateTime.Now}");

                    await fs.WriteAsync(info);
                }
            }
        }

        foreach (var d in directories)
        {
            for (var i = 1; i <= 10; i++)
            {
                using (StreamReader sr = File.OpenText(@$"{d.FullName}\File{i}"))
                {
                    Console.WriteLine($"Имя файла: File{i}. Содержимое файла: {await sr.ReadLineAsync()} + {await sr.ReadLineAsync()}");
                }
            }
        }
    }

    private static void SetAccess(DirectoryInfo dirInfo)
    {
        var dirSecurity = dirInfo.GetAccessControl();

        dirSecurity.AddAccessRule(new FileSystemAccessRule(
        WindowsIdentity.GetCurrent().Name,
       FileSystemRights.Read | FileSystemRights.Write,
            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None,
            AccessControlType.Allow));

        dirInfo.SetAccessControl(dirSecurity);
    }
}