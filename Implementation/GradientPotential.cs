namespace Implementation;

public class GradientPotential : Algorithm
{
    public GradientPotential(string filePath) : this(ReadDistanceMatrix(filePath)){ }

    public GradientPotential(int[][] matrix) : base(matrix)
    {
        var startTime = DateTime.Now;
        Result = Solve();
        Elapsed = DateTime.Now - startTime;
    }

    public override (long Distance, int[] Tour) Solve()
    {
        throw new NotImplementedException();
    }
}
