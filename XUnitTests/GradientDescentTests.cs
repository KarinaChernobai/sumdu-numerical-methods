using Xunit;

namespace NumMethods;

public class GradientDescentTests
{
	[Fact]
	public void TransposeTest()
	{
		var a1 = new double[,]
		{
			{ 1, 2, 3 },
			{ 4, 5, 6 },
		};
		var a2 = new double[,]
		{
			{ 1, 4 },
			{ 2, 5 },
			{ 3, 6 },
		};
		var actual = GradientDescent.Transpose(a1);
		AssertMatrixEqual(actual, a2);
		actual = GradientDescent.Transpose(a2);
		AssertMatrixEqual(actual, a1);
	}

	[Fact]
	public void Transpose2Test()
	{
		var a1 = new double[,]
		{
			{ 1, 2, 3 },
			{ 4, 5, 6 },
			{ 7, 8, 9 },
		};
		var a2 = new double[,]
		{
			{ 1, 4, 7 },
			{ 2, 5, 8 },
			{ 3, 6, 9 },
		};
		var actual = GradientDescent.Transpose(a1);
		AssertMatrixEqual(actual, a2);
		actual = GradientDescent.Transpose(a2);
		AssertMatrixEqual(actual, a1);
	}

	private static void AssertMatrixEqual(double[,] expected, double[,] actual)
	{
		var rowCount = expected.GetLength(0);
		var columnCount = expected.GetLength(1);
		Assert.Equal(rowCount, actual.GetLength(0));
		Assert.Equal(columnCount, actual.GetLength(1));
		for (var r = 0; r < rowCount; r++)
		{
			for (var c = 0; c < columnCount; c++)
			{
				Assert.Equal(expected[r, c], actual[r, c]);
			}
		}
	}
}
