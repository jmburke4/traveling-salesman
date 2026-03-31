namespace Implementation;

/// <summary>
/// An interface representing an algorithm that solves the Travelling Salesman Problem (TSP).
/// Implementations of this interface should provide a method to compute the optimal route for a given graph of cities and distances.
/// </summary>
public abstract class Algorithm
{
    // TODO Add parameter to prevent immediate evaluation of graph
    //      Is this actually useful?

    /// <summary>
    /// Gets the distance matrix representing the graph of cities and distances.
    /// The matrix is expected to be symmetric, where DistanceMatrix[i][j] gives the distance from city i to city j.
    /// </summary>
    public int[][] DistanceMatrix { get; protected set; }

    /// <summary>
    /// Gets the time taken to compute the solution for the TSP instance.
    /// This can be used to compare the performance of different algorithms.
    /// </summary>
    /// <remarks> This time does not include the time taken to read the distance matrix from file.</remarks>
    public TimeSpan Elapsed { get; protected set; }

    /// <summary>
    /// Gets the result of the TSP solution, which includes the total distance of the best tour found and the sequence of cities in that tour.
    /// </summary>
    public (long Distance, int[] Tour) Result { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the Algorithm class by reading a distance matrix from a specified file path. 
    /// The file is expected to contain a lower triangular matrix of distances, where each row i contains the distances from city i to cities 0...i.
    /// </summary>
    /// <param name="filePath">The path to the file containing the distance matrix.</param>
    protected Algorithm(string filePath)
    {
        DistanceMatrix = ReadDistanceMatrix(filePath);
    }

    /// <summary>
    /// Initializes a new instance of the Algorithm class using a provided distance matrix.
    /// </summary>
    /// <param name="matrix">The distance matrix representing the graph of cities and distances.</param>
    protected Algorithm(int[][] matrix)
    {
        DistanceMatrix = matrix;
    }

    /// <summary>
    /// Calculates the total distance of a given tour based on the distance matrix.
    /// The tour is represented as an array of city indices, and the method sums up the distances between consecutive cities in the tour, including the return to the starting city.
    /// </summary>
    /// <param name="tour">An array of city indices representing the tour.</param>
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
    /// Prints the solution to the TSP instance in a human-readable format, including the best distance, the best tour, and the time taken to compute the solution.
    /// </summary>
    /// <param name="output">The text writer to which the solution should be printed.</param>
    public void PrintSolution(TextWriter output)
    {
        output.WriteLine($"Best distance: {Result.Distance}");
        output.WriteLine($"Best tour: {string.Join(" -> ", Result.Tour)} -> {Result.Tour[0]}");
        output.WriteLine($"Time: {Elapsed.TotalSeconds:F3} seconds");
    }

    /// <summary>
    /// Reads a distance matrix from a file. The file is expected to contain a lower triangular matrix of distances, where each row i contains the distances from city i to cities 0...i.
    /// </summary>
    /// <param name="filePath">A path to a .graph file</param>
    /// <returns>A matrix of distances between cities</returns>
    public static int[][] ReadDistanceMatrix(string filePath)
    {
        // TODO Include graceful check to make sure filePath is valid
        // TODO Include check to make sure graph is symmetric
        // TODO Include check to make sure graph has no negative distances
        // NOTE Is there a way to refactor this so that not the entire matrix needs to be stored in memory? 
        //      Maybe read the file line by line and compute distances on the fly? See yield keyword

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

    // TODO Devise a way to also count the number of steps
    /// <summary>
    /// Solves the TSP for the given distance matrix and returns the best distance and the corresponding tour of cities.
    /// </summary>
    /// <returns>A tuple containing the best distance and the corresponding tour.</returns>
    public abstract (long Distance, int[] Tour) Solve();
}
