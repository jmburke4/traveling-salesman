using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bruteforce;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Bruteforce <graph_file>");
            return;
        }

        string filePath = args[0];
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File '{filePath}' not found.");
            return;
        }

        try
        {
            // Read the distance matrix
            int[][] distanceMatrix = ReadDistanceMatrix(filePath);
            int n = distanceMatrix.Length;

            Console.WriteLine($"Solving TSP with {n} cities using brute force...");
            
            var startTime = DateTime.Now;
            var result = SolveTSP(distanceMatrix);
            var elapsed = DateTime.Now - startTime;

            Console.WriteLine($"Best distance: {result.Distance}");
            Console.WriteLine($"Best tour: {string.Join(" -> ", result.Tour)} -> {result.Tour[0]}");
            Console.WriteLine($"Time: {elapsed.TotalSeconds:F3} seconds");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads a lower triangular distance matrix from a file.
    /// Each row i contains distances from city i to cities 0...i
    /// </summary>
    static int[][] ReadDistanceMatrix(string filePath)
    {
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
            var values = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            
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
    /// Solves TSP using brute force (trying all permutations)
    /// </summary>
    static (long Distance, int[] Tour) SolveTSP(int[][] distanceMatrix)
    {
        int n = distanceMatrix.Length;
        int[] bestTour = Enumerable.Range(0, n).ToArray();
        long bestDistance = CalculateTourDistance(bestTour, distanceMatrix);

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

            long distance = CalculateTourDistance(tour, distanceMatrix);
            
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTour = tour;
            }
        }

        return (bestDistance, bestTour);
    }

    /// <summary>
    /// Calculates the total distance of a tour
    /// </summary>
    static long CalculateTourDistance(int[] tour, int[][] distanceMatrix)
    {
        long distance = 0;
        
        for (int i = 0; i < tour.Length; i++)
        {
            int from = tour[i];
            int to = tour[(i + 1) % tour.Length];
            distance += distanceMatrix[from][to];
        }

        return distance;
    }

    /// <summary>
    /// Generates all permutations of a list using Heap's algorithm
    /// </summary>
    static IEnumerable<List<int>> GetPermutations(List<int> list)
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
