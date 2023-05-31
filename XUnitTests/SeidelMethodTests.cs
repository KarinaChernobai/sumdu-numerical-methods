using System;
using System.Reflection;
using Xunit;

namespace NumMethods;

public class SeidelMethodTests
{
	private static readonly Action<double[,], double[], double[]> UMatrixProdVector = GetAction<double[,], double[], double[]>("UMatrixProdVector");
	private static Action<T1, T2, T3> GetAction<T1, T2, T3>(string name)
		=> (Action<T1, T2, T3>)Delegate.CreateDelegate(typeof(Action<T1, T2, T3>), typeof(SeidelMethod2).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic)!);

	private static readonly Action<double[,], int, double[]> VectorDotProduct = GetAction<double[,], int, double[]>("VectorDotProduct");

	[Fact]
	public void UMatrixProdVector1()
	{

		var matrix = new double[,] 
		{ 
			{ 11,   12,   13 }, 
			{ 210,  220,  230 }, 
			{ 3100, 3200, 3300 } ,
		};
		var vector = new double[] { 1, 2, 3 };
		var expected = new double[] { 12*2 + 13*3, 230*3, 0};
		var actual = new double[vector.Length];
		UMatrixProdVector(matrix, vector, actual);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void UMatrixProdVector2()
	{
		var matrix = new double[,]
		{
			{ 11,   12,   13,  14 },
			{ 110,  120,  130, 140 },
			{ 1100,  1200,  1300, 1400 },
			{ 11000,  12000,  13000, 14000 },
		};
		var vector = new double[] { 1, 2, 3, 4 };
		var expected = new double[] { 12 * 2 + 13 * 3 + 14 * 4, 130 * 3 + 140 * 4, 1400 * 4, 0 };
		var actual = new double[vector.Length];
		UMatrixProdVector(matrix, vector, actual);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void VectorDotProduct1()
	{
		var matrix = new double[,]
		{
			{ 11,   12,   13,  14 },
			{ 110,  120,  130, 140 },
			{ 1100,  1200,  1300, 1400 },
			{ 11000,  12000,  13000, 14000 },
		};
		var vector = new double[] { 1, 2, 3, 4 };
		var expected2 = new double[] { 110 * 1, 120 * 2, 130 * 3, 140 * 4 };
		var actual = new double[vector.Length];
		VectorDotProduct(matrix, 1, actual);
		Assert.Equal(expected2, vector);
	}
}