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
		double[,] samples = { { 0, 1 }, { 0, 10 } };
		var lagrange = new LagrangeInterpolation2(samples);
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
		double[,] samples = { { 0, 1 }, { 0, 10 } };
		var lagrange = new LagrangeInterpolation2(samples);
		var x = 0.5d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(5d, res, Tolerance);
	}

	[Fact]
	public void InterpolateTest2()
	{
		double[,] samples = { { 3, 9 }, { 21, -3 } };
		var lagrange = new LagrangeInterpolation2(samples);
		var x = 6d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(9d, res, Tolerance);
	}

	[Fact]
	public void InterpolateTest3()
	{
		double[,] samples = { { 0, 1, 2, 3 }, { 0, 0.5, 2, 1.5 } };
		var lagrange = new LagrangeInterpolation2(samples);
		var x = 1.6d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(1.472, res, Tolerance);
	}

	[Fact]
	public void LBFTest4()
	{
		double[,] samples =
		{
			{ 0.4, 0.5, 0.7, 0.8 },
			{ -0.916291, -0.693147, -0.356675, -0.223144 }
		};
		var lagrange = new LagrangeInterpolation2(samples);
		var x = 0.6d;
		var numerator = lagrange.FullLBFNumerator(x);
		Assert.Equal(0.0004, numerator, Tolerance);
	}

	[Fact]
	public void InterpolateTest4()
	{
		double[,] samples = 
		{ 
			{ 0.4, 0.5, 0.7, 0.8 }, 
			{ -0.916291, -0.693147, -0.356675, -0.223144 } 
		};
		var lagrange = new LagrangeInterpolation2(samples);
		var x = 0.6d;
		var res = lagrange.Interpolate(x);
		Assert.Equal(-0.509976, res, Tolerance);
	}
}
