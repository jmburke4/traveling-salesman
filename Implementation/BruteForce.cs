namespace Implementation;

/// <summary>
/// A brute-force algorithm that generates all possible permutations of the nodes (except the starting node)
/// to find the optimal TSP tour.
/// </summary>
public class BruteForce : Algorithm
{
    public BruteForce(string filePath, TextWriter? sink = null) : this(ReadDistanceMatrix(filePath), sink){ }

    public BruteForce(int[][] matrix, TextWriter? sink = null) : base(matrix, sink)
    {
        if (Nodes > 12) throw new BruteForceException("Brute-force algorithm is not feasible for more than 12 nodes due to factorial growth of permutations.");
        try
        {
            StartProgressReporting();
            Result = Solve();
        }
        finally
        {
            StopProgressReporting();
        }
    }

    public override (long Distance, int[] Tour) Solve()
    {
        int[] bestTour = Enumerable.Range(0, Nodes).ToArray();
        long bestDistance = CalculateTourDistance(bestTour);
        long totalPermutations = 1;

        for (int i = 2; i <= Nodes - 1; i++)
        {
            totalPermutations *= i;
        }

        // Generate all permutations and find the one with minimum distance
        var permutations = GetPermutations(Enumerable.Range(1, Nodes - 1).ToList());

        long step = 0;
        foreach (var perm in permutations)
        {
            int[] tour = new int[Nodes];
            tour[0] = 0; // Start from node 0
            
            for (int i = 0; i < Nodes - 1; i++)
            {
                tour[i + 1] = perm[i];
            }

            long distance = CalculateTourDistance(tour);
            
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTour = tour;
            }

            step++;
            UpdateStatus(step * 100.0 / totalPermutations);
        }

        UpdateStatus(100);

        return (bestDistance, bestTour);
    }

    /// <summary>
    /// Generates all permutations of a list of integers. This is a recursive method that builds 
    /// permutations by fixing each element and permuting the rest of the list. The base case is when the 
    /// list has only one element, in which case it returns that single permutation. For larger lists, it 
    /// generates permutations of the sublist (excluding the last element) and then inserts the last 
    /// element into every possible position in those permutations. This method is used to generate all 
    /// possible tours for the TSP problem, starting from a fixed node (node 0 in this case). Note that 
    /// this approach is computationally expensive and is only feasible for small numbers of nodes due to 
    /// the factorial growth of permutations.
    /// </summary>
    /// <param name="list">A list of integers representing node indices to be permuted.</param>
    /// <returns>An enumerable of lists, where each list is a unique permutation of the input list.</returns>
    private static IEnumerable<List<int>> GetPermutations(List<int> list)
    {
        if (list.Count == 1)
        {
            yield return list;
            yield break;
        }

        foreach (var perm in GetPermutations(list.Take(list.Count - 1).ToList()))
        {
            for (int i = 0; i < list.Count; i++)
            {
                var newPerm = new List<int>(perm);
                newPerm.Insert(i, list[list.Count - 1]);
                yield return newPerm;
            }
        }
    }

    public class BruteForceException(string message) : Exception(message)
    {
    }
}
