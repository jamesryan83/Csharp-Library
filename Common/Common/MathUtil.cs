
namespace Common
{
	public class MathUtil
	{
		// Calculate progress for a progress bar
		public static int CalculateProgress(long currentIteration, long totalCount)
		{
			ArgumentUtil.IsValueGreaterThan(totalCount + 1, currentIteration, "currentIteration", "CalculateProgress", 
				"More iterations than there were items");

			if (currentIteration < 0 || totalCount < 1)
				return 0;

			double val = ((double) currentIteration / (double) totalCount) * 100.0;
			return (int) val;
		}
	}
}
