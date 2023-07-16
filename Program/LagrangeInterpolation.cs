using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods;

public class LagrangeInterpolation
{
	private readonly Point2D[] _samples;

	public LagrangeInterpolation(Point2D[] samples) 
	{
		_samples = samples;
	}
	public double Interpolate(double x) 
	{
		var sum = 0d;
		var numerator = FullLBFNumerator(x);
		for(int i = 0; i < _samples.Length; i++) 
		{
			sum += _samples[i].Y * LBF(i, x, numerator);
		}
		return sum;
	}
	public double FullLBFNumerator(double x) 
	{
		var numerator = 1d;
		for (int i = 0; i < _samples.Length; i++)
		{
			numerator *= x - _samples[i].X;
		}
		return numerator;
	}
	//  Lagrange basis function
	public double LBF(int index, double x, double numerator) 
	{
		var denominator = 1d;
		for (int i = 0; i < index; i++)
		{
			denominator *= _samples[index].X - _samples[i].X;
		}
		for (int i = index+1; i < _samples.Length; i++)
		{
			denominator *= _samples[index].X - _samples[i].X;
		}
		return numerator / denominator / (x - _samples[index].X);
	}
}
