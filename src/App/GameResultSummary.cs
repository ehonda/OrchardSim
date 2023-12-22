namespace App;

public record GameResultSummary(int AbsoluteCount, double RelativeCount)
{
    public double Percentage => RelativeCount * 100;
}
