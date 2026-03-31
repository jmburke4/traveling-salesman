namespace Implementation;

/// <summary>
/// A heuristic algorithm that constructs a TSP tour by iteratively inserting unvisited nodes into the
/// current tour based on a potential function that considers the cost of insertion relative to the
/// average distance to the current tour. The algorithm starts with a small initial tour and repeatedly
/// selects the unvisited node with the highest potential for insertion, inserting it into the tour at 
/// the position that minimizes the increase in tour distance. After each insertion, the potential 
/// values are recomputed for all remaining unvisited nodes to reflect the updated tour structure. 
/// This approach aims to build a high-quality tour by prioritizing nodes that are relatively close to 
/// the current tour and have a favorable cost-to-distance ratio. The algorithm runs in O(n^3) time 
/// due to the need to recompute potentials for all unvisited nodes after each insertion, making it 
/// suitable for moderate-sized TSP instances.
/// </summary>
public class GradientPotential : Algorithm
{
    /// <summary>
    /// The current tour being constructed. Initialized with the first three nodes in order.
    /// </summary>
    private readonly List<int> tour;

    /// <summary>
    /// Set of nodes that have not yet been inserted into the tour. Initially contains all nodes
    /// except the first three.
    /// </summary>
    private readonly HashSet<int> unvisited;

    /// <summary>
    /// Number of nodes in the graph, derived from the size of the distance matrix.
    /// </summary>
    private int Nodes => DistanceMatrix.Length;

    /// <summary>
    /// Initializes the algorithm by reading the distance matrix from a file and setting up the
    /// initial tour and unvisited nodes.
    /// </summary>
    /// <param name="filePath"></param>
    public GradientPotential(string filePath) : this(ReadDistanceMatrix(filePath)){ }

    /// <summary>
    /// Initializes the algorithm with a given distance matrix, setting up the initial tour and
    /// unvisited nodes.
    /// </summary>
    /// <param name="matrix"></param>
    public GradientPotential(int[][] matrix) : base(matrix)
    {
        var startTime = DateTime.Now;
        
        tour = [0, 1, 2];
        unvisited = [.. Enumerable.Range(3, Nodes - 3)];
        Result = Solve();

        Elapsed = DateTime.Now - startTime;
    }

    public override (long Distance, int[] Tour) Solve()
    {
        Dictionary<int, double> bestCost = [];
        Dictionary<int, int> bestEdge = [];
        Dictionary<int, double> potential = [];

        RecomputeAll(bestCost, bestEdge, potential);

        // Main loop
        while (unvisited.Count > 0)
        {
            // Select node with maximum potential (e.g. highest cost-to-average-distance ratio)
            int chosenNode = unvisited.OrderByDescending(c => potential[c]).First();

            // Insert the chosen node into the tour at the best edge found
            int insertAfter = bestEdge[chosenNode];
            tour.Insert(insertAfter + 1, chosenNode);

            unvisited.Remove(chosenNode);

            RecomputeAll(bestCost, bestEdge, potential);
        }

        return (CalculateTourDistance([.. tour]), tour.ToArray());
    }

    /// <summary>
    /// Recomputes the best insertion cost, best edge for insertion, and potential for all unvisited
    /// nodes. This is called after each insertion to update the state of the algorithm.
    /// </summary>
    /// <param name="bestCost">A dictionary to store the best insertion cost for each unvisited node.</param>
    /// <param name="bestEdge">A dictionary to store the best edge for insertion for each unvisited node.</param>
    /// <param name="potential">A dictionary to store the potential for each unvisited node.</param>
    private void RecomputeAll(Dictionary<int, double> bestCost, Dictionary<int, int> bestEdge,
        Dictionary<int, double> potential)
    {
        foreach (int node in unvisited)
        {
            // Compute the best insertion cost and edge for this node
            ComputeBestInsertion(node, out double cost, out int edge);
            bestCost[node] = cost;
            bestEdge[node] = edge;

            double avgDist = AverageDistance(node);
            potential[node] = cost / (avgDist + double.Epsilon);
        }
    }

    /// <summary>
    /// Computes the best insertion cost and corresponding edge for a given unvisited node. The best
    /// insertion cost is calculated as the increase in tour distance if the node were inserted
    /// between two consecutive nodes in the current tour. The method iterates through all edges in
    /// the current tour to find the edge that results in the lowest insertion cost for the given node.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="bestCost"></param>
    /// <param name="bestEdgeIndex"></param>
    private void ComputeBestInsertion(int node, out double bestCost,
        out int bestEdgeIndex)
    {
        bestCost = double.PositiveInfinity;
        bestEdgeIndex = -1;

        int m = tour.Count;
        for (int i = 0; i < m; i++)
        {
            int u = tour[i];
            int v = tour[(i + 1) % m];

            double delta = 
                DistanceMatrix[u][node] + 
                DistanceMatrix[node][v] - 
                DistanceMatrix[u][v];
            
            if (delta < bestCost)
            {
                bestCost = delta;
                bestEdgeIndex = i;
            }
        }
    }

    /// <summary>
    /// Calculates the average distance from a given node to all nodes currently in the tour.
    /// This is used in the potential calculation to normalize the insertion cost by the average
    /// distance, which helps to prioritize nodes that are relatively closer to the current tour.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private double AverageDistance(int node)
    {
        int sum = 0;
        foreach (int t in tour)
            sum += DistanceMatrix[node][t];

        return (double)sum / tour.Count;
    }
}
