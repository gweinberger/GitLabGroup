using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace GitLabGroup
{
    internal class Worker
    {
        private string url = "";
        private string token = "";
        private Permission permission = Permission.guest;
        private string groupname = "";
        private string csvFile = "";

        internal enum Permission
        {
            guest = 10,
            reporter = 20,
            developer = 30,
            maintainer = 40,
            owner = 50
        }
            
        internal Worker (string url, string token, Permission permission, string groupname, string csvFile)
        {
            this.url = url.Trim();
            this.token = token.Trim();
            this.permission = permission;
            this.groupname = groupname.Trim();
            this.csvFile = csvFile.Trim();
            if (url.EndsWith("/")) url = url.Substring(0, url.Length - 1);
        }

        internal void Run()
        {
            int countAll = 0;
            int countExits = 0;
            int countNew = 0;
            int countNotFound = 0;

            if (!File.Exists(csvFile)) throw new FileNotFoundException($"{csvFile} not found.");

            using (WebClient client = new WebClient())
            {
                client.Headers.Add($"PRIVATE-TOKEN: {token}");
                foreach (string item in File.ReadAllText(csvFile).Replace("\n", "").Replace("\r", ",").Replace(";", ",").Split(','))
                {
                    if (item.Length > 0)
                    {
                        countAll++;
                        Console.Write(item);
                        string content = client.DownloadString($"{url}users?search={item}");
                        if (content.ToLower().Contains("\"id\":"))
                        {
                            string id = content.Split(",")[0].Split(":")[1];

                            NameValueCollection data = new NameValueCollection();
                            data["user_id"] = id;
                            data["access_level"] = Convert.ToInt32(permission).ToString();

                            try
                            {
                                string test = $"{url}/groups/{groupname}/members";
                                byte[] response = client.UploadValues($"{url}/groups/{groupname}/members", "POST", data);
                                string responseInString = Encoding.UTF8.GetString(response);
                                if (responseInString.ToLower().Contains("\"id\":"))
                                {
                                    log($"{item} added", ConsoleColor.Green);
                                    countNew++;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("409"))
                                {
                                    log(" exits", ConsoleColor.Yellow);
                                    countExits++;
                                }
                                else
                                {
                                    log($"{ex.Message}", ConsoleColor.Red);
                                }
                            }
                        }
                        else
                        {
                            log(" not found", ConsoleColor.Red);
                            countNotFound++;
                        }
                    }
                }

                log($"", ConsoleColor.White);
                log($"      All: {countAll}", ConsoleColor.White);
                log($"   Exists: {countExits}", ConsoleColor.Yellow);
                log($"      New: {countNew}", ConsoleColor.Green);
                log($"Not found: {countNotFound}", ConsoleColor.Red);
            }

        }
        private void log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

    }
}
