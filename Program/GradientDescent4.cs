using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class GradientDescent4
{
	// 40x^3+4xy^2+12xz^2-12xy-4xz-23.88x+4y^2+8z^2-12.72
	public static double F1(double x, double y, double z)
	{
		return 40 * Math.Pow(x, 3) + 4 * x * Math.Pow(y, 2) + 12 * x * Math.Pow(z, 2) - 12 * x * y - 4 * x * z - 23.88 * x + 4 * Math.Pow(y, 2) + 8 * Math.Pow(z, 2) - 12.72;
	}
	// 8y^3+4x^2y+8xy+8yz^2-4yz-17.52y+4.18-6x^2-2z^2
	public static double F2(double x, double y, double z)
	{
		return 8 * Math.Pow(y, 3) + 4 * Math.Pow(x, 2) * y + 8 * x * y + 8 * y * Math.Pow(z, 2) - 4 * y * z - 17.52 * y + 4.18 - 6 * Math.Pow(x, 2) - 2 * Math.Pow(z, 2);
	}
	// 20z^3+12x^2z+16xz+8y^2z-4yz-31.8z+3.4-2x^2-2y^2
	public static double F3(double x, double y, double z)
	{
		return 20 * Math.Pow(z, 3) + 12 * Math.Pow(x, 2) * z + 16 * x * z + 8 * Math.Pow(y, 2) * z - 4 * y * z - 31.8 * z + 3.4 - 2 * Math.Pow(x, 2) - 2 * Math.Pow(y, 2);
	}

	public static void Run() 
	{
		var x = default(double);
		var y = default(double);
		var z = default(double);
		var lambda = default(double);
		var trW = default(double);
		while (true) 
		{
			x = x - 2 * lambda * trW * F1(x, y, z);
			y = y - 2 * lambda * trW * F2(x, y, z);
			z = z - 2 * lambda * trW * F3(x, y, z);
		}
	}
}
