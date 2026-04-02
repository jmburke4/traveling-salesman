using DotNetEnv;
using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace Implementation;

class Program
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "Path to a graph file")]
        public string? File { get; set; }

        [Option('d', "dir", Required = false, HelpText = "Path to a directory with a graph file")]
        public string? Directory { get; set; }

        [Option('s', "status", Required = false, Default = true, HelpText = "Whether to print the percentage complete of the algorithm running")]
        public bool? PrintStatus 
        { 
            get => _printStatus;
            set
            {
                if (value != true) _printStatus = false;
                else _printStatus = true;
            }
        }
        private bool _printStatus;

        [Option('t', "tour", Required = false, Default = false, HelpText = "Whether to print the route of the optimal tour")]
        public bool? PrintTour { 
            get => _printTour; 
            set
            {
                if (value != true) _printTour = false;
                else _printTour = true;
            }
        }
        private bool _printTour;

        [Option('o', "outfile", Required = false, HelpText = "Print output to file")]
        public string? OutFile { get; set; }

        [Option("debug", Required = false, Default = false, HelpText = "Print debug messages")]
        public bool? Debug
        {
            get => _debug;
            set
            {
                if (value != true) _debug = false;
                else _debug = true;
            }
        }
        private bool _debug;

        public override string ToString()
        {
            return $"\tFile: {File ?? ""}\n\tDir: {Directory ?? ""}\n\tPrint Status: {PrintStatus}\n\tPrint Tour: {PrintTour}\n\tOut file: {OutFile}";
        }
    }

    [Verb("bf", HelpText = "Run the Brute Force algorithm")]
    class BFOptions : Options
    {
        public override string ToString() => "Brute Force Options\n" + base.ToString();
    }

    [Verb("gp", HelpText = "Run the Gradient Potential algorithm")]
    class GPOptions : Options
    {
        public override string ToString() => "Gradient Potential Options\n" + base.ToString();
    }

    static void Main(string[] args)
    {
        Env.TraversePath().Load();
        string graphPath = "";
        Parser.Default.ParseArguments<BFOptions, GPOptions>(args).WithParsed<Options>(
            o =>
            {       
                if (o.Debug ?? false) Console.WriteLine(o);

                // Double check that file exists
                o.File ??= "";        
                if (!o.File.EndsWith(".graph"))
                {
                    Console.WriteLine($"{o.File} must be a .graph file");
                    Environment.Exit(1);
                }
                string envDir = Environment.GetEnvironmentVariable("GRAPH_DIRECTORY") ?? "";
                if (File.Exists(o.File))
                    graphPath = o.File;
                else if (File.Exists(Path.Combine(o.Directory ?? "", o.File)))
                    graphPath = Path.Combine(o.Directory ?? "", o.File);
                else if (File.Exists(Path.Combine(envDir, o.File)))
                    graphPath = Path.Combine(envDir, o.File);
                else
                {
                    Console.WriteLine($"Could not find file \"{o.File}\" in:");
                    Console.WriteLine($"\t{Path.Combine(o.Directory ?? "", o.File)}");
                    Console.WriteLine($"\t{Path.Combine(envDir, o.File)}");
                    Environment.Exit(1);
                }
            }
            ).MapResult(
                (BFOptions o) =>
                {
                    BruteForce algorithm = new(graphPath, o.PrintStatus == null ? null : Console.Out);
                    if (!string.IsNullOrEmpty(o.OutFile))
                    {
                        using StreamWriter sw = new(o.OutFile);
                        algorithm.PrintSolution(output: sw, printTour: o.PrintTour ?? false);
                    }
                    algorithm.PrintSolution(printTour: o.PrintTour ?? false);
                    return 0;
                },
                (GPOptions o) =>
                {
                    GradientPotential algorithm = new(graphPath, o.PrintStatus == null ? null : Console.Out);
                    if (!string.IsNullOrEmpty(o.OutFile))
                    {
                        using StreamWriter sw = new(o.OutFile);
                        algorithm.PrintSolution(output: sw, printTour: o.PrintTour ?? false);
                    }
                    algorithm.PrintSolution(printTour: o.PrintTour ?? false);
                    return 0;
                },
                errs => 1
            );
    }
}
