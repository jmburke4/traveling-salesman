using DotNetEnv;

namespace Implementation;

class Program
{
    static void Main(string[] args)
    {
        Env.TraversePath().Load();

        string graphPath;
        
        if (args.Length == 1)
        {
            // TODO Update this to determine whether this is a path or city count
            graphPath = args[0];
        }
        else if (Environment.GetEnvironmentVariable("GRAPH_DIRECTORY") is string graphDirectory)
        {
            Console.WriteLine($"Enter graph filename: ");
            graphPath = graphDirectory + Console.ReadLine() + ".graph";
        }
        else
        {
            Console.WriteLine("usage: dotnet run <graph_file_path>");
            return;
        }

        if (!File.Exists(graphPath))
        {
            Console.WriteLine($"Graph file not found: {graphPath}");
            return;
        }

        var bruteforce = new BruteForce(graphPath);
        bruteforce.PrintSolution(Console.Out);
    }
}
