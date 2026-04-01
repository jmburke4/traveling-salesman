namespace Implementation;

/// <summary>
/// An interface representing an algorithm that solves the Traveling Salesman Problem (TSP).
/// Implementations of this interface should provide a method to compute the optimal route for a given
/// graph of nodes and distances.
/// </summary>
public abstract class Algorithm
{
    // TODO Add parameter to prevent immediate evaluation of graph
    //      Is this actually useful?

    /// <summary>
    /// A text writer used for outputting status updates and results. By default, it writes to the
    /// console, but it can be redirected to a file or other output stream if needed.
    /// </summary>
    private readonly TextWriter? writer;

    /// <summary>
    /// Synchronizes access to the progress reporter state.
    /// </summary>
    private readonly object progressLock = new();

    private DateTime startTime;

    /// <summary>
    /// Periodically writes the latest progress value.
    /// </summary>
    private Timer? progressTimer;

    /// <summary>
    /// Latest progress percentage reported by the algorithm.
    /// </summary>
    private int latestProgressPercent = -1;

    /// <summary>
    /// Last progress percentage written by the reporter.
    /// </summary>
    private int lastReportedProgressPercent = -1;

    /// <summary>
    /// Number of nodes in the graph, derived from the size of the distance matrix.
    /// </summary>
    protected int Nodes => DistanceMatrix.Length;

    /// <summary>
    /// Gets the distance matrix representing the graph of nodes and distances.
    /// The matrix is expected to be symmetric, where DistanceMatrix[i][j] gives the distance from node
    /// i to node j.
    /// </summary>
    public int[][] DistanceMatrix { get; protected set; }

    /// <summary>
    /// Gets the time taken to compute the solution for the TSP instance.
    /// This can be used to compare the performance of different algorithms.
    /// </summary>
    /// <remarks> This time does not include the time taken to read the distance matrix from file.</remarks>
    public TimeSpan Elapsed { get; protected set; }

    /// <summary>
    /// Gets the result of the TSP solution, which includes the total distance of the best tour found 
    /// and the sequence of nodes in that tour.
    /// </summary>
    public (long Distance, int[] Tour) Result { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the Algorithm class by reading a distance matrix from a specified 
    /// file path. The file is expected to contain a lower triangular matrix of distances, where each 
    /// row i contains the distances from node i to nodes 0...i.
    /// </summary>
    /// <param name="filePath">The path to the file containing the distance matrix.</param>
    protected Algorithm(string filePath, TextWriter? sink = null)
    {
        DistanceMatrix = ReadDistanceMatrix(filePath);
        if (sink != null)
            writer = TextWriter.Synchronized(sink);
    }

    /// <summary>
    /// Initializes a new instance of the Algorithm class using a provided distance matrix.
    /// </summary>
    /// <param name="matrix">The distance matrix representing the graph of nodes and distances.</param>
    protected Algorithm(int[][] matrix, TextWriter? sink = null)
    {
        DistanceMatrix = matrix;
        if (sink != null)
            writer = TextWriter.Synchronized(sink);
    }

    /// <summary>
    /// Calculates the total distance of a given tour based on the distance matrix.
    /// The tour is represented as an array of node indices, and the method sums up the distances
    /// between consecutive nodes in the tour, including the return to the starting node.
    /// </summary>
    /// <param name="tour">An array of node indices representing the tour.</param>
    /// <returns>The total distance of the tour.</returns>
    public long CalculateTourDistance(int[] tour)
    {
        long distance = 0;
        
        for (int i = 0; i < tour.Length; i++)
        {
            int from = tour[i];
            int to = tour[(i + 1) % tour.Length];
            distance += DistanceMatrix[from][to];
        }

        return distance;
    }

    /// <summary>
    /// Prints the solution to the TSP instance in a human-readable format, including the best distance, 
    /// the best tour, and the time taken to compute the solution.
    /// </summary>
    /// <param name="output">The text writer to which the solution should be printed.</param>
    /// <param name="printTour">A flag to print the tour or not</param>
    public void PrintSolution(TextWriter? output = null, bool printTour = true)
    {
        // TODO Maybe also include the number of steps taken to reach the solution?
        output ??= writer ?? Console.Out;
        output.WriteLine($"Best distance: {Result.Distance}");
        if (printTour) 
            output.WriteLine($"Best tour: {string.Join(" -> ", Result.Tour)} -> {Result.Tour[0]}");
        output.WriteLine($"Time: {Elapsed.TotalSeconds:F3} seconds");
    }

    /// <summary>
    /// Reads a distance matrix from a file. The file is expected to contain a lower triangular matrix
    /// of distances, where each row i contains the distances from node i to nodes 0...i.
    /// </summary>
    /// <param name="filePath">A path to a .graph file</param>
    /// <returns>A matrix of distances between nodes</returns>
    public static int[][] ReadDistanceMatrix(string filePath)
    {
        // TODO Include graceful check to make sure filePath is valid
        // TODO Include check to make sure graph is symmetric
        // TODO Include check to make sure graph has no negative distances
        // NOTE Is there a way to refactor this so that not the entire matrix needs to be stored in 
        //      memory? Maybe read the file line by line and compute distances on the fly? See yield 
        //      keyword

        var lines = File.ReadAllLines(filePath);
        int n = lines.Length;
        
        // Create symmetric distance matrix
        int[][] matrix = new int[n][];
        for (int i = 0; i < n; i++)
        {
            matrix[i] = new int[n];
        }

        // Parse the triangular matrix
        for (int i = 0; i < n; i++)
        {
            var values = lines[i].Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            
            for (int j = 0; j <= i; j++)
            {
                int distance = int.Parse(values[j]);
                matrix[i][j] = distance;
                matrix[j][i] = distance; // Make it symmetric
            }
        }

        return matrix;
    }

    /// <summary>
    /// Solves the TSP for the given distance matrix and returns the best distance and the corresponding
    /// tour of nodes.
    /// </summary>
    /// <returns>A tuple containing the best distance and the corresponding tour.</returns>
    public abstract (long Distance, int[] Tour) Solve();

    /// <summary>
    /// Starts a background timer that periodically prints the latest reported progress.
    /// </summary>
    /// <param name="intervalMilliseconds">How often progress should be written.</param>
    protected void StartProgressReporting(int intervalMilliseconds = 250)
    {
        startTime = DateTime.Now;

        lock (progressLock)
        {
            progressTimer ??= new Timer(
                _ => ReportProgress(),
                null,
                intervalMilliseconds,
                intervalMilliseconds);
        }

        UpdateStatus(0);
        ReportProgress(force: true);
    }

    /// <summary>
    /// Stops the background progress timer and writes a final newline.
    /// </summary>
    protected void StopProgressReporting()
    {
        Elapsed = DateTime.Now - startTime;
        
        Timer? timer;

        lock (progressLock)
        {
            timer = progressTimer;
            progressTimer = null;
        }

        timer?.Dispose();
        ReportProgress(force: true);
        writer?.WriteLine();
        writer?.Flush();
    }

    /// <summary>
    /// Updates the status of the algorithm's progress by writing a percentage to the console.
    /// </summary>
    /// <param name="percent"></param>
    protected void UpdateStatus(double percent) 
    {
        int roundedPercent = (int)Math.Round(percent);

        if (roundedPercent < 0)
            roundedPercent = 0;
        else if (roundedPercent > 100)
            roundedPercent = 100;

        Volatile.Write(ref latestProgressPercent, roundedPercent);
    }

    /// <summary>
    /// Writes the latest progress value if it changed.
    /// </summary>
    /// <param name="force">Whether to write even if the value has not changed.</param>
    private void ReportProgress(bool force = false)
    {
        int latestPercent = Volatile.Read(ref latestProgressPercent);

        if (!force && latestPercent == Volatile.Read(ref lastReportedProgressPercent))
            return;

        Volatile.Write(ref lastReportedProgressPercent, latestPercent);
        writer?.Write($"\r{latestPercent}%");
        writer?.Flush();
    }
}
