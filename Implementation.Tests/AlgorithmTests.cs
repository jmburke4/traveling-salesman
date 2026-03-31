namespace Implementation.Tests;

public class AlgorithmTests
{
	[Fact]
	public void Algorithm_ReadsDistanceMatrixFromFile_IntoSymmetricMatrix()
	{
		// Arrange
		int[][] lowerTriangular =
		[
			[ 0 ],
			[ 7, 0 ],
			[ 9, 4, 0 ],
			[ 3, 8, 2, 0 ]
		];

		int[][] expected = TestHelper.ExpandLowerTriangularToSymmetric(lowerTriangular);

		// Act
		int[][] actual = TestHelper.WithTempGraphFile(lowerTriangular, filePath =>
		{
			Algorithm algorithm = new BruteForce(filePath);
			return algorithm.DistanceMatrix;
		});

		// Assert
		Assert.Equal(expected.Length, actual.Length);

		for (int i = 0; i < expected.Length; i++)
		{
			Assert.Equal(expected[i], actual[i]);
		}
	}

	[Fact]
	public void Algorithm_CalculateTourDistance_MatchesExpectedDistance()
	{
		// Arrange
		int[][] lowerTriangular =
		[
			[ 0 ],
			[ 7, 0 ],
			[ 9, 4, 0 ],
			[ 3, 8, 2, 0 ]
		];

		int[][] matrix = TestHelper.ExpandLowerTriangularToSymmetric(lowerTriangular);
		int[] tour = [0, 2, 3, 1];
		long expectedDistance = TestHelper.CalculateTourDistance(matrix, tour);

		// Act
		Algorithm algorithm = new BruteForce(matrix);
		long actualDistance = algorithm.CalculateTourDistance(tour);

		// Assert
		Assert.Equal(expectedDistance, actualDistance);
	}
}
