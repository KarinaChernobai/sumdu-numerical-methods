using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class LagrangeInterpolation2
{
	double[,] _samples;
	int N;
	public LagrangeInterpolation2(double[,] samples) 
	{
		_samples = samples;
		N = samples.GetLength(1);
	}
	public double Interpolate(double x) 
	{
		var sum = 0d;
		var numerator = FullLBFNumerator(x);
		for(int i = 0; i < N; i++) 
		{
			sum += _samples[1, i] * LBF(i, x, numerator);
		}
		Console.WriteLine(sum);
		return sum;
	}
	public double FullLBFNumerator(double x) 
	{
		var numerator = 1d;
		for (int i = 0; i < N; i++)
		{
			numerator *= x - _samples[0, i];
		}
		return numerator;
	}
	//  Lagrange basis functions
	public double LBF(int index, double x, double numerator) 
	{
		var denominator = 1d;
		for (int i = 0; i < index; i++)
		{
			denominator *= _samples[0, index] - _samples[0, i];
		}
		for (int i = index+1; i < N; i++)
		{
			denominator *= _samples[0, index] - _samples[0, i];
		}
		return numerator / denominator / (x - _samples[0, index]);
	}
}
