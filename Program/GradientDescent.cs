using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public static class GradientDescent
{
	const string DoubleFormat = "0.################";

	public static void Solve()
	{
		const double epsilon = 0.0001;

		var x0 = new double[] { 10, 10, 10 };

		var fnVector = new Func<double[], double>[3]
		{
			x=> x[0]*x[0]   + x[1]*x[1] - x[2]        - 1.7,
			x=> x[0]*2      + x[1]*x[1] + x[2]*x[2]*2 - 3.18,
			x=> x[0]*x[0]*3 - x[1]      + x[2]*x[2]   - 2.09,
		};

		var fnJacobiMatrix = new Func<double[], double>[3, 3]
		{
			{ x=> x[0]*2,      x=> x[1]*2, x=> -1 },
			{ x=> 2, x=> x[1]*2,      x=> x[2]*4 },
			{ x=> x[0]*6,      x=> -1,      x=> x[2]*2 },
		};

		var fnJacobiMatrixT = Transpose(fnJacobiMatrix);

		var numJacobiMatrix = new double[3, 3];
		var numJacobiMatrixT = new double[3, 3];
		var numGradStepVector = new double[3];
		var numVector = new double[3];
		var numGradVector = new double[3];
		var x = new double[3];

		var i = 0;
		while (true)
		{
			Apply(fnJacobiMatrix, x0, numJacobiMatrix);
			Apply(fnJacobiMatrixT, x0, numJacobiMatrixT);
			Apply(fnVector, x0, numVector);

			Mult(numJacobiMatrixT, numVector, numGradStepVector);
			Mult(numJacobiMatrix, numGradStepVector, numGradVector);
			var stepLength = DotProd(numVector, numGradVector) / DotProd(numGradVector, numGradVector);
			numGradStepVector.Scale(-stepLength);
			Add(x0, numGradStepVector, x);

			Console.Out.WriteVector("x", i, x0);
			Console.Out.WriteLine();
			Console.Out.WriteVector("x", i+1, x);
			Console.Out.WriteLine();

			Console.Out.WriteVector("Grad", null, numGradStepVector);
			Console.Out.WriteLine();
			var e = EuclidianNorm(numGradStepVector);
			Console.Out.WriteLine("E = {0}", e.ToString(DoubleFormat));
			
			Console.Out.WriteLine();

			if (e < epsilon) break;

			(x0, x) = (x, x0);
			i++;
		}


		var r = fnVector.Apply(x);
		Console.Out.WriteVector(r);
		Console.Out.WriteLine();
	}



	private static double[] Apply(this Func<double[], double>[] fnVector, double[] x) 
	{
		var res = new double[fnVector.Length];
		Apply(fnVector, x, res);
		return res;
	}

	private static void Apply(Func<double[], double>[] fnVector, double[] x, double[] res)
	{
		for (var i = 0; i < fnVector.Length; i++)
		{
			res[i] = fnVector[i](x);
		}
	}

	private static void Apply(Func<double[], double>[,] fnMatrix, double[] x, double[,] res)
	{
		var size = fnMatrix.GetLength(0);
		for (var r = 0; r < size; r++)
		{
			for (var c = 0; c < size; c++)
			{
				res[r, c] = fnMatrix[r, c](x);
			}
		}
	}

	public static T[,] Transpose<T>(T[,] m)
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

	private static double DotProd(double[] v1, double[] v2)
	{ 
		var size = v1.Length;
		var res = 0d;
		for (var i = 0; i < size; i++) res += v1[i] * v2[i];
		return res;
	}

	private static void Add(double[] v1, double[] v2, double[] res)
	{
		for (var i = 0; i < v1.Length; i++) res[i] = v1[i] + v2[i];
	}

	private static void Scale(this double[] v, double scalar)
	{ 
		for(var i=0; i<v.Length; i++) v[i] *= scalar;
	}

	private static double EuclidianNorm(double[] v)
	{
		var res = 0d;
		for (var i = 0; i < v.Length; i++) res += v[i] * v[i];
		return Math.Sqrt(res);
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

	private static void WriteVector(this TextWriter writer, string name, int? iterNum, double[] v)
	{
		writer.Write(name);
		if (iterNum != null) writer.Write(iterNum);
		writer.Write(" = [");
		writer.Write(v[0].ToString(DoubleFormat));
		for (var i = 1; i < v.Length; i++)
		{
			writer.Write("; ");
			writer.Write(v[i].ToString(DoubleFormat));
		}
		writer.Write("]");
	}
}
