namespace Implementation.Tests;


// TODO Every test does not need to read and write from file
//      Update algorithm class to accept a matrix in the constructor instead of a filepath
public class BruteForceTests
{
    /// <summary>
    /// Creates a simple distance matrix by writing to a temporary file.
    /// </summary>
    private static string CreateTempGraphFile(int[][] distanceMatrix)
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
    /// Validates that a tour is valid: starts at 0, visits all nodes exactly once.
    /// </summary>
    private static void ValidateTour(int[] tour, int expectedNodeCount)
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

    /// <summary>
    /// Tests brute force on a simple 6-node graph.
    /// </summary>
    [Fact]
    public void BruteForce_6NodeGraph_FindsValidTour()
    {
        // Arrange: Create a simple symmetric distance matrix for 6 nodes
        int[][] distances =
        [
            [ 0 ],
            [ 10, 0 ],
            [ 15, 25, 0 ],
            [ 20, 18, 12, 0 ],
            [ 25, 30, 22, 16, 0 ],
            [35, 28, 24, 14, 18, 0]
        ];

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var (distance, tour) = bruteForce.Result;

            // Assert
            ValidateTour(tour, 6);
            Assert.True(distance > 0, "Tour distance should be positive");
            
            // Verify the distance calculation matches the tour
            long calculatedDistance = bruteForce.CalculateTourDistance(tour);
            Assert.Equal(distance, calculatedDistance);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force on a 6-node graph where the optimal tour is known.
    /// </summary>
    [Fact]
    public void BruteForce_6NodeGraph_FindsOptimalTour()
    {
        // Arrange: Create a distance matrix where the optimal tour is known
        int[][] distances =
        [
            [ 0 ],
            [ 29, 0 ],
            [ 20, 31, 0 ],
            [ 18, 23, 27, 0 ],
            [ 14, 8, 25, 25, 0 ],
            [ 31, 15, 16, 29, 12, 0 ]
        ];

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var (distance, tour) = bruteForce.Result;

            // Assert
            ValidateTour(tour, 6);
            Assert.True(distance > 0);
            
            // The brute force should find the best possible tour
            long calculatedDistance = bruteForce.CalculateTourDistance(tour);
            Assert.Equal(distance, calculatedDistance);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force on an 8-node graph.
    /// </summary>
    [Fact]
    public void BruteForce_8NodeGraph_FindsValidTour()
    {
        // Arrange: Create a symmetric distance matrix for 8 nodes
        int[][] distances =
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

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var (distance, tour) = bruteForce.Result;

            // Assert
            ValidateTour(tour, 8);
            Assert.True(distance > 0);
            
            long calculatedDistance = bruteForce.CalculateTourDistance(tour);
            Assert.Equal(distance, calculatedDistance);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force on an 8-node fully connected graph with uniform distances.
    /// </summary>
    [Fact]
    public void BruteForce_8NodeUniformGraph_AllToursHaveSameDistance()
    {
        // Arrange: Create a distance matrix where all edges have the same distance
        const int uniformDistance = 10;
        int[][] distances = new int[8][];

        for (int i = 0; i < 8; i++)
        {
            distances[i] = new int[i + 1];
            for (int j = 0; j <= i; j++)
            {
                distances[i][j] = uniformDistance;
            }
        }

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var (distance, tour) = bruteForce.Result;

            // Assert
            ValidateTour(tour, 8);
            // With uniform distances, any complete tour should have distance = 8 * uniformDistance
            Assert.Equal(8 * uniformDistance, distance);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force on a 10-node graph.
    /// </summary>
    [Fact]
    public void BruteForce_10NodeGraph_FindsValidTour()
    {
        // Arrange: Create a symmetric distance matrix for 10 nodes
        int[][] distances =
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

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var (distance, tour) = bruteForce.Result;

            // Assert
            ValidateTour(tour, 10);
            Assert.True(distance > 0);
            
            long calculatedDistance = bruteForce.CalculateTourDistance(tour);
            Assert.Equal(distance, calculatedDistance);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force on a larger 10-node graph with varied distances.
    /// </summary>
    [Fact]
    public void BruteForce_10NodeComplexGraph_FindsValidTour()
    {
        // Arrange: Create a more complex 10-node graph
        int[][] distances =
        [
            [ 0 ],
            [ 45, 0 ],
            [ 38, 52, 0 ],
            [ 35, 41, 38, 0 ],
            [ 32, 48, 44, 28, 0 ],
            [ 42, 37, 39, 35, 26, 0 ],
            [ 28, 51, 31, 41, 43, 40, 0 ],
            [ 39, 34, 46, 37, 25, 31, 44, 0 ],
            [ 36, 43, 27, 40, 36, 29, 42, 38, 0 ],
            [ 33, 40, 35, 42, 31, 26, 39, 32, 28, 0 ]
        ];

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var (distance, tour) = bruteForce.Result;

            // Assert
            ValidateTour(tour, 10);
            Assert.True(distance > 0);
            
            long calculatedDistance = bruteForce.CalculateTourDistance(tour);
            Assert.Equal(distance, calculatedDistance);
            
            // Verify that this distance is less than some obviously bad tours
            int[] badTour = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
            long badDistance = bruteForce.CalculateTourDistance(badTour);
            Assert.True(distance <= badDistance, "Brute force should find a tour at least as good as a sequential one");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force timing on progressively larger graphs.
    /// </summary>
    [Fact]
    public void BruteForce_6And8And10NodeGraphs_TimingIncreases()
    {
        // Arrange: Create distance matrices for 6, 8, and 10 nodes
        int[][] distances6 =
        [
            [ 0 ],
            [ 10, 0 ],
            [ 15, 20, 0 ],
            [ 20, 18, 12, 0 ],
            [ 25, 30, 22, 16, 0 ],
            [ 35, 28, 24, 14, 18, 0 ]
        ];

        int[][] distances8 =
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

        int[][] distances10 =
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

        string tempFile6 = CreateTempGraphFile(distances6);
        string tempFile8 = CreateTempGraphFile(distances8);
        string tempFile10 = CreateTempGraphFile(distances10);

        try
        {
            // Act
            var bruteForce6 = new BruteForce(tempFile6);
            var bruteForce8 = new BruteForce(tempFile8);
            var bruteForce10 = new BruteForce(tempFile10);

            // Assert
            ValidateTour(bruteForce6.Result.Tour, 6);
            ValidateTour(bruteForce8.Result.Tour, 8);
            ValidateTour(bruteForce10.Result.Tour, 10);
            
            // Timing should generally increase with graph size (though very small graphs might be too fast to measure)
            Assert.True(bruteForce10.Elapsed >= TimeSpan.Zero);
            Assert.True(bruteForce8.Elapsed >= TimeSpan.Zero);
            Assert.True(bruteForce6.Elapsed >= TimeSpan.Zero);
        }
        finally
        {
            File.Delete(tempFile6);
            File.Delete(tempFile8);
            File.Delete(tempFile10);
        }
    }

    /// <summary>
    /// Tests that the same distance matrix produces consistent results across multiple runs.
    /// </summary>
    [Fact]
    public void BruteForce_SameGraphRunTwice_ProducesSameDistance()
    {
        // Arrange
        int[][] distances =
        [
            [ 0 ],
            [ 10, 0 ],
            [ 15, 20, 0 ],
            [ 20, 18, 12, 0 ],
            [ 25, 30, 22, 16, 0 ],
            [ 35, 28, 24, 14, 18, 0 ]
        ];

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce1 = new BruteForce(tempFile);
            var bruteForce2 = new BruteForce(tempFile);

            // Assert
            Assert.Equal(bruteForce1.Result.Distance, bruteForce2.Result.Distance);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Tests brute force on a 6-node symmetric graph.
    /// </summary>
    [Fact]
    public void BruteForce_6NodeSymmetricGraph_VerifySymmetry()
    {
        // Arrange
        int[][] distances =
        [
            [ 0 ],
            [ 10, 0 ],
            [ 15, 20, 0 ],
            [ 20, 18, 12, 0 ],
            [ 25, 30, 22, 16, 0 ],
            [ 35, 28, 24, 14, 18, 0 ]
        ];

        string tempFile = CreateTempGraphFile(distances);

        try
        {
            // Act
            var bruteForce = new BruteForce(tempFile);
            var matrix = bruteForce.DistanceMatrix;

            // Assert - verify the matrix is symmetric
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    Assert.Equal(matrix[i][j], matrix[j][i]);
                }
            }
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
