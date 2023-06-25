using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

internal static class GradientDescent5
{
	const string DoubleFormat = "0.################";

	public static void Solve()
	{
		var x0 = new double[] { 1, 1, 1 };

		var fnVector = new Func<double[], double>[3]
		{
			x=> x[0] + x[1]*x[1]*3 - x[2],
			x=> x[0]*x[0]*3 + x[1] + x[2]*x[2],
			x=> x[0]*4 + x[1] - x[2]*x[2],
		};

		var jacobiMatrix = new Func<double[], double>[3, 3]
		{
			{ x=> 1, x=> x[1]*6, x=> -1},
			{ x=> x[0]*6, x=> 1, x=> x[2]*2},
			{ x=> 4, x=> 1, x=> -x[2]*2},
		};

		var jacobiMatrixR = new double[3, 3];
		var jacobiMatrixT = Transpose(jacobiMatrix);
		var jacobiMatrixTR = new double[3, 3];
		var jacobiMatrixMultR = new double[3, 3];
		
		var fnVectorR = new double[3];
		var vectorMultR = new double[3];

		jacobiMatrix.Apply(x0, jacobiMatrixR);
		jacobiMatrixT.Apply(x0, jacobiMatrixTR);
		fnVector.Apply(x0, fnVectorR);

		Mult(jacobiMatrixR, jacobiMatrixTR, jacobiMatrixMultR);
		Mult(jacobiMatrixMultR, fnVectorR, vectorMultR);

		var r = fnVector.Apply(new double[] { 1, 0.2, 1 });
		Console.Out.WriteVector(r);
	}



	private static double[] Apply(this Func<double[], double>[] fnVector, double[] x) 
	{
		var res = new double[fnVector.Length];
		fnVector.Apply(x, res);
		return res;
	}

	private static void Apply(this Func<double[], double>[] fnVector, double[] x, double[] res)
	{
		var size = fnVector.Length;
		for (var i = 0; i < size; i++)
		{
			res[i] = fnVector[i](x);
		}
	}

	private static void Apply(this Func<double[], double>[,] fnVector, double[] x, double[,] res)
	{
		var size = fnVector.Length;
		for (var r = 0; r < size; r++)
		{
			for (var c = 0; c < size; c++)
			{
				res[r, c] = fnVector[r, c](x);
			}
		}
	}

	private static T[,] Transpose<T>(T[,] m)
	{
		var rowCount = m.GetLength(0);
		var columnCount = m.GetLength(1);
		var res = new T[columnCount, rowCount];
		for (var r = 0; r < rowCount; r++)
		{
			for (var c = 0; c < columnCount; c++)
			{
				res[c, r] = m[r, c];
			}
		}
		return res;
	}

	private static void Mult(double[,] m1, double[,] m2, double[,] r)
	{
		var columnCount = m2.GetLength(1);
		for (var c = 0; c < columnCount; c++)
		{
			Mult(m1, m2, c, r);
		}
	}

	private static void Mult(double[,] m1, double[,] m2, int column, double[,] res)
	{
		var size = m1.GetLength(0);
		for (var r = 0; r < size; r++)
		{
			var s = 0d;
			for (var c = 0; c < size; c++)
			{
				s += m1[r, c] * m2[c, column];
			}
			res[r, column] = s;
		}
	}

	private static void Mult(double[,] m, double[] v, double[] res)
	{
		var size = m.GetLength(0);
		for (var r = 0; r < size; r++)
		{
			var s = 0d;
			for (var c = 0; c < size; c++)
			{
				s += m[r, c] * v[c];
			}
			res[r] = s;
		}
	}

	private static void WriteVector(this TextWriter writer, double[] v)
	{
		writer.Write("[");
		writer.Write(v[0].ToString(DoubleFormat));
		for (var i = 1; i < v.Length; i++)
		{
			writer.Write("; ");
			writer.Write(v[i].ToString(DoubleFormat));
		}
		writer.Write("]");
	}
}
