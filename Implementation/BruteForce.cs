namespace Implementation;

public class BruteForce : Algorithm
{
    public BruteForce(string filePath) : this(ReadDistanceMatrix(filePath)){ }

    public BruteForce(int[][] matrix) : base(matrix)
    {
        var startTime = DateTime.Now;
        Result = Solve();
        Elapsed = DateTime.Now - startTime;
    }

    public override (long Distance, int[] Tour) Solve()
    {
        int n = DistanceMatrix.Length;
        int[] bestTour = Enumerable.Range(0, n).ToArray();
        long bestDistance = CalculateTourDistance(bestTour);

        // Generate all permutations and find the one with minimum distance
        var permutations = GetPermutations(Enumerable.Range(1, n - 1).ToList());

        foreach (var perm in permutations)
        {
            int[] tour = new int[n];
            tour[0] = 0; // Start from city 0
            
            for (int i = 0; i < n - 1; i++)
            {
                tour[i + 1] = perm[i];
            }

            long distance = CalculateTourDistance(tour);
            
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTour = tour;
            }
        }

        return (bestDistance, bestTour);
    }

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
}
