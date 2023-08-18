using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NumMethods.CubicSplineInterpolation;
using Xunit;

namespace NumMethods;

public class LeastSquaresTest
{
	[Fact]
	public void SolvEqTest()
	{
		var samples = new LeastSquares.Point2D[]
		{
			new(2, 14),
			new(5, 21),
		};
		var leastSquares = new LeastSquares(samples);
		var (x1, x2) = leastSquares.SolveEq(2, 3, 1, 4, 7, 6);
		Assert.Equal(2, x1);
		Assert.Equal(1, x2);
	}
}
