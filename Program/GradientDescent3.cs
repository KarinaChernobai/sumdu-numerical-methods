using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class GradientDescent3
{
	// 40x^3+4xy^2+12xz^2-12xy-4xz-23.88x+4y^2+8z^2-12.72
	public static double U1(double x, double y, double z) 
	{
		return 40 * Math.Pow(x, 3) + 4 * x * Math.Pow(y, 2) + 12 * x * Math.Pow(z, 2) - 12 * x * y - 4 * x * z - 23.88 * x + 4 * Math.Pow(y, 2) + 8 * Math.Pow(z, 2) - 12.72;
	}
	// 8y^3+4x^2y+8xy+8yz^2-4yz-17.52y+4.18-6x^2-2z^2
	public static double U2(double x, double y, double z)
	{
		return 8 * Math.Pow(y, 3) + 4 * Math.Pow(x, 2) * y + 8 * x * y + 8 * y * Math.Pow(z, 2) - 4 * y * z - 17.52 * y + 4.18 - 6 * Math.Pow(x, 2) - 2 * Math.Pow(z, 2);
	}
	// 20z^3+12x^2z+16xz+8y^2z-4yz-31.8z+3.4-2x^2-2y^2
	public static double U3(double x, double y, double z)
	{
		return 20 * Math.Pow(z, 3) + 12 * Math.Pow(x, 2) * z + 16 * x * z + 8 * Math.Pow(y, 2) * z - 4 * y * z - 31.8 * z + 3.4 - 2 * Math.Pow(x, 2) - 2 * Math.Pow(y, 2);
	}

	public static double[] F(double x, double y, double z)
	{
		double[] res = new double[3];
		res[0] = Math.Pow(x, 2) + Math.Pow(y, 2) - z - 1.7;
		res[1] = 2 * x + Math.Pow(y, 2) - 2 * Math.Pow(z, 2) - 3.18;
		res[2] = 3 * Math.Pow(x, 2) - y + Math.Pow(z, 2) - 2.09;
		return res;
	}

	public static double[] U(double x, double y, double z) 
	{
		double[] res = new double[3];
		res[0] = U1(x, y, z);
		res[1] = U2(x, y, z);
		res[2] = U3(x, y, z);
		return res;
	}

	public static double[,] W(double x, double y, double z)
	{
		double[,] res = new double[3, 3];
		res[0, 0] = 2 * x;
		res[0, 1] = 2 * y;
		res[0, 2] = -1;
		res[1, 0] = 2;
		res[1, 1] = 2 * y;
		res[1, 2] = 4 * z;
		res[2, 0] = 6 * x;
		res[2, 1] = -1;
		res[2, 2] = 2 * z;
		return res;
	}
	public static void Run()
	{
		/*		double x = 1.0074378666663095;
				double y = 1.296952618981638;
				double z = 0.5805343734363784;*/
		double x = 0;
		double y = 0;
		double z = 0;
		double lambda = default(double);
		double epsilon = 10e-4;

		var wMatrix = new double[3, 3];
		var uVector = new double[3];
		var resV = new double[3];
		var fVector = new double[3];
		var sum1 = default(double);
		var sum2 = default(double);

		var counter = 0;

		while(true) 
		{
			// calculate lambda start

			wMatrix = W(x, y, z);
			uVector = U(x, y, z);
			fVector = F(x, y, z);
			resV = new double[3];

			for (int i = 0; i < wMatrix.GetLength(0); i++)
			{
				for (int j = 0; j < wMatrix.GetLength(1); j++)
				{
					resV[i] += wMatrix[i, j] * uVector[i];
				}
			}
			
			for (int i = 0; i < resV.Length; i++)
			{
				sum1 += fVector[i] + resV[i];
				sum2 += resV[i] + resV[i];
			}
			lambda = sum1 / sum2;

			Console.WriteLine("sum1: " + sum1);
			Console.WriteLine("sum2: " + sum2);
			Console.WriteLine("lambda: " + lambda);
			Console.WriteLine("counter: " + counter);

			// end

			x = x - lambda * uVector[0];
			y = y - lambda * uVector[1];
			z = z - lambda * uVector[2];
			var max = Math.Abs(x);
			if (Math.Abs(y) > max) max = Math.Abs(y);
			if (Math.Abs(z) > max) max= Math.Abs(z);
			Console.WriteLine($"x = {x}, y = {y}, z = {z}");
			if (max < epsilon)
			{
				Console.WriteLine("The answer was found");
				Console.WriteLine($"x = {x}, y = {y}, z = {z}");
				break;
			}	
			counter++;
			if(counter > 5) 
			{
				Console.WriteLine("No answer was found");
				break;
			}

			sum1 = 0;
			sum2 = 0;
		}
	}
}
