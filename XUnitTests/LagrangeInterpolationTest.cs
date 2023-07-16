using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NumMethods;

public class LagrangeInterpolationTest
{
	private const double Tolerance = 0.000001;

	[Fact]
	public void LBFTest()
	{
		var samples = new Point2D[] { new(0, 0), new(1, 10) };
		var lagrange = new LagrangeInterpolation(samples);
		var x = 0.5;
		var numerator = lagrange.FullLBFNumerator(x);
		Assert.Equal(-0.25, numerator, Tolerance);
		var res = lagrange.LBF(0, x, numerator);
		Assert.Equal(0.5, res, Tolerance);
		res = lagrange.LBF(1, x, numerator);
		Assert.Equal(0.5, res, Tolerance);
	}

	[Fact]
	public void InterpolateTest()
	{
		var samples = new Point2D[] { new(0, 0), new(1, 10) };
		var lagrange = new LagrangeInterpolation(samples);
		var x = 0.5d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(5d, res, Tolerance);
	}

	[Fact]
	public void InterpolateTest2()
	{
		var samples = new Point2D[] 
		{
			new(3, 21),
			new(9, -3),
		};
		var lagrange = new LagrangeInterpolation(samples);
		var x = 6d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(9d, res, Tolerance);
	}

	[Fact]
	public void InterpolateTest3()
	{
		var samples = new Point2D[] 
		{
			new(0, 0),
			new(1, 0.5),
			new(2, 2),
			new(3, 1.5),
		};
		var lagrange = new LagrangeInterpolation(samples);
		var x = 1.6d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(1.472, res, Tolerance);
	}

	[Fact]
	public void LBFTest4()
	{
		var samples = new Point2D[]
		{
			new(0.4, -0.916291),
			new(0.5, -0.693147),
			new(0.7, -0.356675),
			new(0.8, -0.223144),
		};
		var lagrange = new LagrangeInterpolation(samples);
		var x = 0.6d;
		var numerator = lagrange.FullLBFNumerator(x);
		Assert.Equal(0.0004, numerator, Tolerance);
	}

	[Fact]
	public void InterpolateTest4()
	{
		var samples = new Point2D[]
		{
			new(0.4, -0.916291),
			new(0.5, -0.693147),
			new(0.7, -0.356675),
			new(0.8, -0.223144),
		};
		var lagrange = new LagrangeInterpolation(samples);
		var x = 0.6d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(-0.509976, res, Tolerance);
	}
}
