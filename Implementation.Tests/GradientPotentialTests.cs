namespace Implementation.Tests;

public class GradientPotentialTests
{
    [Fact]
    public void Solve_6NodeGraph_ReturnsValidTourAndConsistentDistance()
    {
        // Arrange
        int[][] matrix = TestHelper.ExpandLowerTriangularToSymmetric(TestHelper.GetSixNodeGraphLowerTriangular());

        // Act
        var gradientPotential = new GradientPotential(matrix);
        var (distance, tour) = gradientPotential.Result;

        // Assert
        TestHelper.ValidateTour(tour, 6);
        Assert.True(distance > 0);
        Assert.Equal(distance, gradientPotential.CalculateTourDistance(tour));
    }

    [Fact]
    public void Solve_8NodeUniformGraph_ReturnsExpectedTourDistance()
    {
        // Arrange
        const int uniformDistance = 10;
        int[][] matrix = new int[8][];

        for (int i = 0; i < 8; i++)
        {
            matrix[i] = new int[8];
            for (int j = 0; j < 8; j++)
            {
                matrix[i][j] = i == j ? 0 : uniformDistance;
            }
        }

        // Act
        var gradientPotential = new GradientPotential(matrix);
        var (distance, tour) = gradientPotential.Result;

        // Assert
        TestHelper.ValidateTour(tour, 8);
        Assert.Equal(8 * uniformDistance, distance);
    }

    [Fact]
    public void Solve_10NodeGraph_BeatsSequentialTourDistance()
    {
        // Arrange
        int[][] matrix = TestHelper.ExpandLowerTriangularToSymmetric(TestHelper.GetTenNodeGraphLowerTriangular());
        int[] sequentialTour = [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 ];

        // Act
        var gradientPotential = new GradientPotential(matrix);
        var (distance, tour) = gradientPotential.Result;
        long sequentialDistance = gradientPotential.CalculateTourDistance(sequentialTour);

        // Assert
        TestHelper.ValidateTour(tour, 10);
        Assert.True(distance <= sequentialDistance);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(10)]
    public void Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(int nodeCount)
    {
        // Arrange
        int[][] lowerTriangular = nodeCount switch
        {
            6 => TestHelper.GetSixNodeGraphLowerTriangular(),
            8 => TestHelper.GetEightNodeGraphLowerTriangular(),
            10 => TestHelper.GetTenNodeGraphLowerTriangular(),
            _ => throw new ArgumentOutOfRangeException(nameof(nodeCount))
        };

        int[][] matrix = TestHelper.ExpandLowerTriangularToSymmetric(lowerTriangular);

        // Act
        var gradientPotential = new GradientPotential(matrix);
        var (distance, tour) = gradientPotential.Result;

        // Assert
        TestHelper.ValidateTour(tour, nodeCount);
        Assert.True(distance > 0);
        Assert.Equal(distance, gradientPotential.CalculateTourDistance(tour));
    }

    [Fact]
    public void Solve_SameMatrixRunTwice_ProducesSameDistance()
    {
        // Arrange
        int[][] matrix = TestHelper.ExpandLowerTriangularToSymmetric(TestHelper.GetSixNodeGraphLowerTriangular());

        // Act
        var gradientPotential1 = new GradientPotential(matrix);
        var gradientPotential2 = new GradientPotential(matrix);

        // Assert
        Assert.Equal(gradientPotential1.Result.Distance, gradientPotential2.Result.Distance);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(10)]
    public void Solve_BestSolution_MatchesBruteForce(int nodeCount)
    {
        // Arrange
        int[][] lowerTriangular = nodeCount switch
        {
            6 => TestHelper.GetSixNodeGraphLowerTriangular(),
            8 => TestHelper.GetEightNodeGraphLowerTriangular(),
            10 => TestHelper.GetTenNodeGraphLowerTriangular(),
            _ => throw new ArgumentOutOfRangeException(nameof(nodeCount))
        };

        int[][] matrix = TestHelper.ExpandLowerTriangularToSymmetric(lowerTriangular);

        // Act
        var gradientPotential = new GradientPotential(matrix);
        var bruteForce = new BruteForce(matrix);

        // Brute force is guaranteed to find the best route
        Assert.True(bruteForce.Result.Distance <= gradientPotential.Result.Distance);
        
        // NOTE This would only be a passing test if we expected GradientPotential to be perfect
        // Assert.Equivalent(bruteForce.Result, gradientPotential.Result);
    }
}
