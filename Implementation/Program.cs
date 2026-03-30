using DotNetEnv;
using System.Diagnostics;

namespace Implementation;

class Program
{
    static void Main(string[] args)
    {
        Env.TraversePath().Load();

        string graphPath;
        
        if (args.Length == 1)
        {
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

        ReadGraph(graphPath);
    }

    static void ReadGraph(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            
            var t = Stopwatch.StartNew();

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                Console.WriteLine();
            }

            t.Stop();
            Console.WriteLine($"Time taken: {t.Elapsed.Milliseconds} ms ({t.Elapsed.TotalSeconds} seconds)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading graph: {ex.Message}");
        }
    }
}
