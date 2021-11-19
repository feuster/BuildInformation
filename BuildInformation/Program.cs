using System.IO;
using System.Diagnostics;

namespace BuildInformation
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string Figlet = @"
   ___       _ __   ______     ___                    __  _
  / _ )__ __(_) /__/ /  _/__  / _/__  ______ _  ___ _/ /_(_)__  ___
 / _  / // / / / _  // // _ \/ _/ _ \/ __/  ' \/ _ `/ __/ / _ \/ _ \
/____/\_,_/_/_/\_,_/___/_//_/_/ \___/_/ /_/_/_/\_,_/\__/_/\___/_//_/
";
            //Main start
            string Title = "BuildInformation Creator V1.0-" + Build.BuildIncrement.ToString() + " Rev. " + Build.RevisionControlSystem + "." + Build.Revision + " (" + BuildInformation.Build.BuildDate + ")";
            Console.Clear();
            Console.Title = Title;
            Console.WriteLine(Figlet);
            Console.WriteLine(Title);

            //Variables and commandline arguments
            String FilePath = "";
            string[] OldTemplate = new string[] { };
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToUpper().Contains("--FILE="))
                {
                    FilePath = args[i].Replace("--FILE=", "", StringComparison.InvariantCultureIgnoreCase);
                }
                if (args[i].ToUpper().Contains("--HELP") || args[i].ToUpper().Contains("-H"))
                {
                    Console.WriteLine(Environment.NewLine + "Usage: BuildInformation [--file=PATH_TO_EXISTING_CS]" + Environment.NewLine);
                    Environment.Exit(0);
                }

            }

            //default template
            List<string> DefaultTemplate = new List<string>();
            DefaultTemplate.Clear();
            DefaultTemplate.Add("namespace BuildInformation");
            DefaultTemplate.Add("{");
            DefaultTemplate.Add("	class Build");
            DefaultTemplate.Add("	{");
            DefaultTemplate.Add("		public static int BuildIncrement { get; } = 0;");
            DefaultTemplate.Add("		public static string BuildDate { get; } = \"" + Convert.ToString(DateTime.Now) + "\";");
            DefaultTemplate.Add("		public static string Revision { get; } = \"\";");
            DefaultTemplate.Add("		public static string RevisionControlSystem { get; } = \"\";");
            DefaultTemplate.Add("#if RELEASE");
            DefaultTemplate.Add("		public static string BuildType { get; } = \"Release\";");
            DefaultTemplate.Add("#elif TRACE");
            DefaultTemplate.Add("		public static string BuildType { get; } = \"Trace\";");
            DefaultTemplate.Add("#else");
            DefaultTemplate.Add("		public static string BuildType { get; } = \"Debug\";");
            DefaultTemplate.Add("#endif");
            DefaultTemplate.Add("	}");
            DefaultTemplate.Add("}");

            //use default template or already existing template
            if (FilePath == "")
            {
                FilePath = "BuildInformation.cs";
            }
            if (File.Exists(FilePath))
            {
                OldTemplate = File.ReadAllLines(FilePath);
            }
            else
            {
                OldTemplate = DefaultTemplate.ToArray();
            }
            if (OldTemplate.Count() == 0)
            {
                OldTemplate = DefaultTemplate.ToArray();
            }

            //create new template
            try
            {
                //BuildRevision
                int Increment = 0;
                int.TryParse(OldTemplate[4].Replace("		public static int BuildIncrement { get; } = ", "").Replace(";", "").Trim(), out Increment);
                Increment++;
                OldTemplate[4] = "		public static int BuildIncrement { get; } = " + Increment.ToString() + ";";

                //BuildDate
                OldTemplate[5] = DefaultTemplate[5];

                //Revision and RCS
                string Revision = "";
                string RevisionControlSystem = "";

                Process REVINFO = new Process();

                try
                {
                    Console.WriteLine(Environment.NewLine + "-------------------- SVN --------------------" + Environment.NewLine);
                    REVINFO.StartInfo.FileName = "svn.exe";
                    REVINFO.StartInfo.Arguments = "update";
                    REVINFO.StartInfo.UseShellExecute = false;
                    REVINFO.StartInfo.RedirectStandardOutput = false;
                    REVINFO.Start();
                    REVINFO.WaitForExit();
                    REVINFO.StartInfo.Arguments = "info --show-item=revision";
                    REVINFO.StartInfo.UseShellExecute = false;
                    REVINFO.StartInfo.RedirectStandardOutput = true;
                    REVINFO.Start();
                    Revision = REVINFO.StandardOutput.ReadToEnd().Trim();
                    REVINFO.WaitForExit();
                    if (Revision != "")
                    {
                        RevisionControlSystem = "svn";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("SVN Revision Exception: " + e.Message);
                    Revision = "";
                    RevisionControlSystem = "";
                }

                try
                {
                    if (Revision == "")
                    {
                        Console.WriteLine(Environment.NewLine + "-------------------- GIT --------------------" + Environment.NewLine);
                        REVINFO.StartInfo.FileName = "git.exe";
                        //REVINFO.StartInfo.Arguments = "rev-list --count --first-parent HEAD";
                        REVINFO.StartInfo.Arguments = "rev-parse --short HEAD";
                        REVINFO.StartInfo.UseShellExecute = false;
                        REVINFO.StartInfo.RedirectStandardOutput = true;
                        REVINFO.Start();
                        Revision = REVINFO.StandardOutput.ReadToEnd().Trim();
                        REVINFO.WaitForExit();
                        if (Revision != "")
                        {
                            RevisionControlSystem = "git";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Git Revision Exception: " + e.Message);
                    Revision = "";
                    RevisionControlSystem = "";
                }

                OldTemplate[6] = "		public static string Revision { get; } = \"" + Revision + "\";";
                OldTemplate[7] = "		public static string RevisionControlSystem { get; } = \"" + RevisionControlSystem + "\";";

                File.WriteAllLines(FilePath, OldTemplate);
                Console.WriteLine(Environment.NewLine + "-------------------- NEW --------------------" + Environment.NewLine);
                Console.WriteLine(String.Join(Environment.NewLine, OldTemplate));
            }
            catch (Exception e)
            {
                Console.WriteLine("Program Exception: " + e.Message);
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

    }
}