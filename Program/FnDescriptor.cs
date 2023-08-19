using System;
namespace NumMethods2;

public interface IFnDescriptor
{
	static abstract FnDescriptor CreateInstance(Point2D[] samples);
	static abstract double CalcRegressQuality(FnDescriptor.RegressQualityInfo q);
}

public struct Point2D
{
	public Point2D(double x, double y) => (X, Y) = (x, y);
	public double X;
	public double Y;
}

public abstract class FnDescriptor
{
	public struct RegressQualityInfo
	{
		public double arithMeanX;
		public double arithMeanY;
		public double geomMeanX;
		public double geomMeanY;
		public double harmMeanX;
		public double harmMeanY;
		public int arthInx;
		public int geomInx;
		public int harmInx;
		public Point2D[] samples;
	}

	public static FnDescriptor Create(Point2D[] samples)
		=> Create<
			LinearFnDescriptor,
			LogFnDescriptor,
			Hyper1FnDescriptor,
			ExpFnDescriptor,
			PolinomFnDescriptor,
			EExpFnDescriptor,
			Hyper2FnDescriptor,
			HyperLogFnDescriptor,
			XOverCMulXFnDescriptor
		>(samples);

	private static FnDescriptor Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Point2D[] samples)
		where T1 : IFnDescriptor
		where T2 : IFnDescriptor
		where T3 : IFnDescriptor
		where T4 : IFnDescriptor
		where T5 : IFnDescriptor
		where T6 : IFnDescriptor
		where T7 : IFnDescriptor
		where T8 : IFnDescriptor
		where T9 : IFnDescriptor
	{
		var n = samples.Length;

		var q = default(RegressQualityInfo);
		q.samples = samples;
		q.geomMeanX = 1d;
		q.geomMeanY = 1d;

		for (int i = 0; i < n; i++)
		{
			q.arithMeanX += samples[i].X;
			q.arithMeanY += samples[i].Y;
			q.geomMeanX *= samples[i].X;
			q.geomMeanY *= samples[i].Y;
			q.harmMeanX += 1 / samples[i].X;
			q.harmMeanY += 1 / samples[i].Y;
		}
		q.arithMeanX /= n;
		q.arithMeanY /= n;
		q.geomMeanX = Math.Pow(q.geomMeanX, 1.0 / n);
		q.geomMeanY = Math.Pow(q.geomMeanY, 1.0 / n);
		q.harmMeanX = n / q.harmMeanX;
		q.harmMeanY = n / q.harmMeanY;

		var regressQualityFns = new (Func<RegressQualityInfo, double> Calc, Func<Point2D[], FnDescriptor> Create)[]
		{
			(T1.CalcRegressQuality, T1.CreateInstance),
			(T2.CalcRegressQuality, T2.CreateInstance),
			(T3.CalcRegressQuality, T3.CreateInstance),
			(T4.CalcRegressQuality, T4.CreateInstance),
			(T5.CalcRegressQuality, T5.CreateInstance),
			(T6.CalcRegressQuality, T6.CreateInstance),
			(T7.CalcRegressQuality, T7.CreateInstance),
			(T8.CalcRegressQuality, T8.CreateInstance),
			(T9.CalcRegressQuality, T9.CreateInstance),
		};

		var j = 1;
		for (; j < n; j++)
		{
			var x = samples[j].X;
			if (x > q.harmMeanX)
			{
				q.harmInx = j - 1;
				break;
			}
		}
		for (; j < n; j++)
		{
			var x = samples[j].X;
			if (x > q.geomMeanX)
			{
				q.geomInx = j - 1;
				break;
			}
		}
		for (; j < n; j++)
		{
			var x = samples[j].X;
			if (x > q.arithMeanX)
			{
				q.arthInx = j - 1;
				break;
			}
		}

		var minRegrY = regressQualityFns[0].Calc(q);
		var minInx = 0;
		for (int i = 1; i < regressQualityFns.Length; i++)
		{
			var regrY = regressQualityFns[i].Calc(q);
			if (regrY < minRegrY)
			{
				minRegrY = regrY;
				minInx = i;
			}
		}

		return regressQualityFns[minInx].Create(samples);
	}

	protected FnDescriptor(Point2D[] samples) { _samples = samples; }
	protected readonly Point2D[] _samples;

	public abstract string Name { get; }

	// коефіцієнт детермінації
	public double DetermCoeff { get; private set; }
	// критерій Фішера
	public double FishersCriteria { get; private set; }
	public double A0 { get; protected set; }
	public double A1 { get; protected set; }
	protected abstract void Linearize();
	protected virtual void ArrangeCoefs() {}

	public void LinearRegrCoeffs()
	{
		Linearize();

		var n = _samples.Length;

		var sumX = 0d;
		var sumY = 0d;
		var sumSqrtX = 0d;
		var sumSqrtY = 0d;
		var sumProdXY = 0d;

		for (int i = 0; i < n; i++)
		{
			sumX += _samples[i].X;
			sumY += _samples[i].Y;
			sumSqrtX += _samples[i].X * _samples[i].X;
			sumSqrtY += _samples[i].Y * _samples[i].Y;
			sumProdXY += _samples[i].X * _samples[i].Y;
		}

		SolveEq(n, sumX, sumX, sumSqrtX, sumY, sumProdXY);
		ArrangeCoefs();

		DetermCoeff = A1 * (sumProdXY - (sumX * sumY) / n) / (sumSqrtY - (sumY * sumY) / n);
		FishersCriteria = (DetermCoeff / (1 - DetermCoeff)) * ((n - 1 - 1) / 1);
	}

	protected virtual (double, double) AdjustCoeffs(double a0, double a1) => (a0, a1); 

	private void SolveEq(double a, double b, double c, double d, double res1, double res2)
	{
		var det = a * d - b * c;
		var det1 = res1 * d - b * res2;
		var det2 = a * res2 - c * res1;
		A0 = det1 / det;
		A1 = det2 / det;
	}
}

public class LinearFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.arthInx].Y + ((q.samples[q.arthInx + 1].Y - q.samples[q.arthInx].Y) / (q.samples[q.arthInx + 1].X - q.samples[q.arthInx].X)) * (q.arithMeanX - q.samples[q.arthInx].X);
		return Math.Abs((q.arithMeanY - regrY) / regrY);
	}

	public static FnDescriptor CreateInstance(Point2D[] samples ) => new LinearFnDescriptor(samples);
	public override string Name => "y = a0 + a1 * x";
	private LinearFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize() {}
}

public class LogFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.geomInx].Y + ((q.samples[q.geomInx + 1].Y - q.samples[q.geomInx].Y) / (q.samples[q.geomInx + 1].X - q.samples[q.geomInx].X)) * (q.geomMeanX - q.samples[q.geomInx].X);
		return Math.Abs((q.arithMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new LogFnDescriptor(samples);
	public override string Name => "y = a0 + a1 * ln(x)";
	private LogFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].X = Math.Log(_samples[i].X);
		}
	}
}

public class Hyper1FnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.harmInx].Y + ((q.samples[q.harmInx + 1].Y - q.samples[q.harmInx].Y) / (q.samples[q.harmInx + 1].X - q.samples[q.harmInx].X)) * (q.harmMeanX - q.samples[q.harmInx].X);
		return Math.Abs((q.arithMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new Hyper1FnDescriptor(samples);
	public override string Name => "y = a0 + a1 / x";
	private Hyper1FnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].X = 1 / _samples[i].X;
		}
	}
}

public class ExpFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.arthInx].Y + ((q.samples[q.arthInx + 1].Y - q.samples[q.arthInx].Y) / (q.samples[q.arthInx + 1].X - q.samples[q.arthInx].X)) * (q.arithMeanX - q.samples[q.arthInx].X);
		return Math.Abs((q.geomMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new ExpFnDescriptor(samples);
	public override string Name => "y = a0 * a1 ^ x";
	private ExpFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].Y = Math.Log(_samples[i].Y);
		}
	}
	protected override void ArrangeCoefs()
	{
		A0 = Math.Pow(Math.E, A0);
		A1 = Math.Pow(Math.E, A1);
	}
}

public class PolinomFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.geomInx].Y + ((q.samples[q.geomInx + 1].Y - q.samples[q.geomInx].Y) / (q.samples[q.geomInx + 1].X - q.samples[q.geomInx].X)) * (q.geomMeanX - q.samples[q.geomInx].X);
		return Math.Abs((q.geomMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new PolinomFnDescriptor(samples);
	public override string Name => "y = a0 * x ^ a1";
	private PolinomFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].X = Math.Log(_samples[i].X);
			_samples[i].Y = Math.Log(_samples[i].Y);
		}
	}
	protected override void ArrangeCoefs()
	{
		A0 = Math.Pow(Math.E, A0);
	}
}

public class EExpFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.harmInx].Y + ((q.samples[q.harmInx + 1].Y - q.samples[q.harmInx].Y) / (q.samples[q.harmInx + 1].X - q.samples[q.harmInx].X)) * (q.harmMeanX - q.samples[q.harmInx].X);
		return Math.Abs((q.geomMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new EExpFnDescriptor(samples);
	public override string Name => "y = e ^ a0 + a1 / x";
	private EExpFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].X = 1 / _samples[i].X;
			_samples[i].Y = Math.Log(_samples[i].Y);
		}
	}
}

public class Hyper2FnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.arthInx].Y + ((q.samples[q.arthInx + 1].Y - q.samples[q.arthInx].Y) / (q.samples[q.arthInx + 1].X - q.samples[q.arthInx].X)) * (q.arithMeanX - q.samples[q.arthInx].X);
		return Math.Abs((q.harmMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new Hyper2FnDescriptor(samples);
	public override string Name => "y = 1 / ( a0 + a1 * x)";
	private Hyper2FnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].Y = 1 / _samples[i].Y;
		}
	}
}

public class HyperLogFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.geomInx].Y + ((q.samples[q.geomInx + 1].Y - q.samples[q.geomInx].Y) / (q.samples[q.geomInx + 1].X - q.samples[q.geomInx].X)) * (q.geomMeanX - q.samples[q.geomInx].X);
		return Math.Abs((q.harmMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new HyperLogFnDescriptor(samples);
	public override string Name => "y = 1 / ( a0 + a1 * ln(x) )";
	private HyperLogFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].X = Math.Log(_samples[i].X);
			_samples[i].Y = 1 / _samples[i].Y;
		}
	}
}

public class XOverCMulXFnDescriptor : FnDescriptor, IFnDescriptor
{
	public static double CalcRegressQuality(RegressQualityInfo q)
	{
		var regrY = q.samples[q.harmInx].Y + ((q.samples[q.harmInx + 1].Y - q.samples[q.harmInx].Y) / (q.samples[q.harmInx + 1].X - q.samples[q.harmInx].X)) * (q.harmMeanX - q.samples[q.harmInx].X);
		return Math.Abs((q.harmMeanY - regrY) / regrY);
	}
	public static FnDescriptor CreateInstance(Point2D[] samples) => new XOverCMulXFnDescriptor(samples);
	public override string Name => "y = x / ( a0 + a1 * x)";
	private XOverCMulXFnDescriptor(Point2D[] samples) : base(samples) { }
	protected override void Linearize()
	{
		for (int i = 0; i < _samples.Length; i++)
		{
			_samples[i].X = 1 / _samples[i].X;
			_samples[i].Y = 1 / _samples[i].Y;
		}
	}
	protected override void ArrangeCoefs()
	{
		(A0, A1) = (A1, A0);
	}
}
