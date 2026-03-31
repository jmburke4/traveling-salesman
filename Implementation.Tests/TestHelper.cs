namespace Implementation.Tests;

internal static class TestHelper
{
    /// <summary>
    /// Creates a simple distance matrix by writing to a temporary file.
    /// </summary>
    public static string CreateTempGraphFile(int[][] distanceMatrix)
    {
        string tempFile = Path.Combine(Path.GetTempPath(), $"graph_{Guid.NewGuid()}.graph");
        var lines = new List<string>();

        // Write lower triangular matrix format
        for (int i = 0; i < distanceMatrix.Length; i++)
        {
            var row = new List<int>();
            for (int j = 0; j <= i; j++)
            {
                row.Add(distanceMatrix[i][j]);
            }
            lines.Add(string.Join(" ", row));
        }

        File.WriteAllLines(tempFile, lines);
        return tempFile;
    }

    /// <summary>
    /// Runs a callback with a temporary .graph file and always deletes it afterwards.
    /// </summary>
    public static T WithTempGraphFile<T>(int[][] lowerTriangularMatrix, Func<string, T> action)
    {
        string tempFile = CreateTempGraphFile(lowerTriangularMatrix);

        try
        {
            return action(tempFile);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Converts a lower-triangular matrix representation into a full symmetric matrix.
    /// </summary>
    public static int[][] ExpandLowerTriangularToSymmetric(int[][] lowerTriangularMatrix)
    {
        int n = lowerTriangularMatrix.Length;
        int[][] matrix = new int[n][];

        for (int i = 0; i < n; i++)
        {
            matrix[i] = new int[n];
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                int distance = lowerTriangularMatrix[i][j];
                matrix[i][j] = distance;
                matrix[j][i] = distance;
            }
        }

        return matrix;
    }

    /// <summary>
    /// Calculates a tour distance directly from a full matrix, used as a test oracle.
    /// </summary>
    public static long CalculateTourDistance(int[][] matrix, int[] tour)
    {
        long distance = 0;

        for (int i = 0; i < tour.Length; i++)
        {
            int from = tour[i];
            int to = tour[(i + 1) % tour.Length];
            distance += matrix[from][to];
        }

        return distance;
    }

    public static int[][] GetSixNodeGraphLowerTriangular() =>
    [
        [ 0 ],
        [ 10, 0 ],
        [ 15, 20, 0 ],
        [ 20, 18, 12, 0 ],
        [ 25, 30, 22, 16, 0 ],
        [ 35, 28, 24, 14, 18, 0 ]
    ];

    public static int[][] GetEightNodeGraphLowerTriangular() =>
    [
        [ 0 ],
        [ 10, 0 ],
        [ 15, 20, 0 ],
        [ 20, 18, 12, 0 ],
        [ 25, 30, 22, 16, 0 ],
        [ 35, 28, 24, 14, 18, 0 ],
        [ 12, 22, 18, 26, 14, 32, 0 ],
        [ 18, 16, 24, 20, 28, 22, 10, 0 ]
    ];

    public static int[][] GetTenNodeGraphLowerTriangular() =>
    [
        [ 0 ],
        [ 29, 0 ],
        [ 20, 31, 0 ],
        [ 18, 23, 27, 0 ],
        [ 14, 8, 25, 25, 0 ],
        [ 31, 15, 16, 29, 12, 0 ],
        [ 11, 22, 19, 26, 13, 28, 0 ],
        [ 17, 12, 24, 21, 9, 18, 14, 0 ],
        [ 26, 25, 15, 22, 20, 10, 27, 16, 0 ],
        [ 19, 18, 23, 11, 17, 24, 8, 13, 21, 0 ]
    ];

    /// <summary>
    /// Validates that a tour is valid: starts at 0, visits all nodes exactly once.
    /// </summary>
    public static void ValidateTour(int[] tour, int expectedNodeCount)
    {
        Assert.NotNull(tour);
        Assert.Equal(expectedNodeCount, tour.Length);
        Assert.Equal(0, tour[0]); // Must start at node 0
        
        var visitedNodes = new HashSet<int>(tour);
        Assert.Equal(expectedNodeCount, visitedNodes.Count); // All nodes visited exactly once
        
        for (int i = 0; i < expectedNodeCount; i++)
        {
            Assert.Contains(i, visitedNodes);
        }
    }
}
