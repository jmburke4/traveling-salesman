# Traveling Salesman - Gradient Potential Tour

This project implements two algorithms for solving the traveling salesman problem: simple brute force, and a novel algorithm called the gradient potential tour (GPT) algorithm. It is a polynomial time algorithm that incrementally builds a tour by inserting cities one at a time, prioritizing cities based on the potential value that balances local insertion cost against global isolation (how much the total cost of the tour will increase versus the average distance the city is removed from the rest of the cities in the tour). Cities that are farther removed from the rest of the graph are inserted earlier on to try and minimize disruptive placements. It's inspired by potential gradients in physics, which Wikipedia defines as the spatial rate of change of potential with respect to displacement.

The algorithm builds the tour incrementally, inserting one city per iteration until all cities are included. This is O(n) interations, where n is the number of cities in the graph. During each iteration, the algorithm evaluates every city not yet added to the tour, where it checks every edge (O(n) edges) for the best insertion cost, while calculating the distance of the edge between the cities on the tour and not on the tour. The evaluation of each city during an iteration is O(n). This gives an aggregate cost of O(n²). It does not guarantee a perfect solution, although it can find it on some graphs when compared to the brute force algorithm.

> I've used Microsoft Copilot to learn about potential gradients in physics, to learn how the concept could be applied to the traveling salesman problem, and to write unit tests for the algorithms.

The project is written in C# using .NET 9.0, and the solution contains an *Implementation* project that contains the two algorithms, and an *Implementation.Tests* project that contains unit tests for the two algorithms.

View this project on my [GitHub](https://github.com/jmburke4/traveling-salesman/).

## Repository Structure

```
traveling-salesman
    README.md
    testoutput.md
    traveling-salesman.sln
    Implementation/
        Algorithm.cs
        BruteForce.cs
        GradientPotential.cs
        Program.cs
        Implementation.csproj
    Implementation.Tests/
        AlgorithmTests.cs
        BruteForceTests.cs
        GradientPotentialTests.cs
        TestHelper.cs
        Implementation.Tests.csproj
```

## Prerequisites

- .NET 9 SDK (required to build and run tests)
- .NET 9 Runtime (enough if you only want to run a published app)

Download from Microsoft:
- https://dotnet.microsoft.com/download/dotnet/9.0

## Building and Running

Build from the repository root:

```bash
dotnet restore
dotnet build traveling-salesman.sln
```

Run from the repository root with one of the verbs:

```bash
dotnet run --project Implementation -- bf -f your-file.graph
dotnet run --project Implementation -- gp -f your-file.graph
```

### CLI Arguments

Command format:

```text
<verb> [options]
```

#### Required

- `verb`: `bf` (Brute Force) or `gp` (Gradient Potential)
- `-f, --file <value>`: graph file name or path; must be a `.graph` file

#### Optional

- `-d, --dir <path>`: directory to search for the graph file
- `-s, --status <true|false>`: show a progress percentage while the algorithm runs (default: `true`)
- `-t, --tour <true|false>`: print full tour route (default: `false` - large graphs clutter up the output)
- `-o, --outfile <path>`: write solution output to a file

#### Examples

```bash
dotnet run --project Implementation -- bf -f small.graph
dotnet run --project Implementation -- gp -f small.graph -d ./graphs -t true -o result.txt
dotnet run --project Implementation -- gp -f small.graph --debug true
```

> An error will be thrown if a graph with more than 12 nodes is supplied to the brute force algorithm, due to O(n!) runtime

### Input file resolution order

The `-f|--file` value must end with `.graph` and is resolved in this order:

1. As-is (exact path provided)
2. Combined with `-d|--dir`
3. Combined with `GRAPH_DIRECTORY` from a `.env` file

If the file is not found in any location, the app exits with an error.

To use `.env`, create a file in the repo (or parent directory) containing:

```env
GRAPH_DIRECTORY=path/to/graphs
```

## Testing

The unit tests are written using the .NET xUnit testing framework, and test the following:
- reading graphs from file
- calculating the total distance of a tour
- returning valid tours on graphs of sizes 6, 8, and 10
- returning the best tour on graphs of sizes 6, 8, and 10 (brute force)
- verifying that running the same graph twice returns the same result
- verifying that the returned tour is at least better than the sequential tour
- verifying that the returned tour from the GPT algorithm is not better than the guaranteed best tour from brute force

Run all unit tests from the root directory:

```bash
dotnet test
```

Print passing tests in addition to failing:

```bash
dotnet test --logger "console;verbosity=detailed"
```

The results of running ```dotnet test``` are copied to `testoutput.md`, in addition to the output from running the algorithm on the provided graphs of size 100, 250, 1000 nodes provided with the assignment. The graphs with 5000 and 15000 nodes have their own respective files for the tour printed.

## Third-Party Dependencies

I've used two external libraries in this project, [DotNetEnv](https://github.com/tonerdo/dotnet-env) and [CommandLine](https://github.com/commandlineparser/commandline). DotNetEnv provides easy support for working with ```.env``` files, which I use to store an optional default directory for graph files. CommandLine provides support for simplifying the parsing of arguments passed to the program via the command line interface.