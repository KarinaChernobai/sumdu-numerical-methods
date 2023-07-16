using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static NumMethods.CubicSplineInterpolation;

namespace NumMethods;

public class CubicSplineInterpolationTest
{
	private const double Tolerance = 0.000001;

	[Fact]
	public void TridiagonalMatrixTest()
	{
		var samples = new Point2D[] { new(0, 0), new(0, 0), new(0, 0) }; /// ??? 
		var spline = new CubicSplineInterpolation(samples);
		TridiagonalData[] data = { new TridiagonalData(0, 1, 0, 10), new TridiagonalData(0, 1, 0, 20), new TridiagonalData(0, 1, 0, 30) };
		spline.TridiagonalMatrix(data);
		Assert.Equal(10, spline.Coeffs[0].B, Tolerance);
		Assert.Equal(20, spline.Coeffs[1].B, Tolerance);
		Assert.Equal(30, spline.Coeffs[2].B, Tolerance);
	}

	[Fact]
	public void TridiagonalMatrixTest2()
	{
		var samples = new Point2D[] { new(0, 0), new(0, 0), new(0, 0) };
		var spline = new CubicSplineInterpolation(samples);
		TridiagonalData[] data = { new TridiagonalData(0, 2, 1, 40), new TridiagonalData(1, 2, 1, 55), new TridiagonalData(1, 1, 0, 25) };
		spline.TridiagonalMatrix(data);
		Assert.Equal(10, spline.Coeffs[0].B, Tolerance);
		Assert.Equal(20, spline.Coeffs[1].B, Tolerance);
		Assert.Equal(5, spline.Coeffs[2].B, Tolerance);
	}

	[Fact]
	public void TridiagonalMatrixTest3()
	{
		var samples = new Point2D[] { new(0, 0), new(0, 0), new(0, 0) };
		var spline = new CubicSplineInterpolation(samples);
		TridiagonalData[] data = { new TridiagonalData(0, 1, 1, 30), new TridiagonalData(1, 2, 1, 55), new TridiagonalData(1, 1, 0, 25) };
		Assert.Throws<MatrixException>(() => spline.TridiagonalMatrix(data));
	}
}
