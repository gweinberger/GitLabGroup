using Microsoft.Extensions.Configuration;

namespace GitLabGroup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"GitLabGroup (c)2024{(DateTime.Now.Year != 2024 ? "-" + DateTime.Now.Year.ToString() : "")} Gerald Weinberger");
            Console.WriteLine("");
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

                if (configuration == null ||
                    configuration["url"] == null ||
                    configuration["token"] == null ||
                    configuration["group"] == null ||
                    configuration["permission"] == null ||
                    configuration["file"] == null
                )
                {
                    Console.WriteLine("  Usage: GitLabGroup --url=[Gitlab-URL] --token=[PRIVATE-TOKEN] --group=[groupname] --permission=[Guest|Reporter|Developer|Maintainer|Owner] --file=[CSV-File]");
                    Console.WriteLine("  Example: GitLabGroup --url=https://mygitlab/api/v4/ --token=XYZ --group=grpDev --permission=Developer --file=input.csv");
                    Console.WriteLine("  Info: Group must exist");
                    Environment.Exit(1);
                }
                Enum.TryParse((configuration["permission"] ?? "guest").ToLower(), out Worker.Permission permission);

                Worker worker = new(configuration["url"] ?? "", configuration["token"] ?? "", permission, configuration["group"] ?? "", configuration["file"] ?? "");
                worker.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}
