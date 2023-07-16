namespace NumMethods;

public readonly struct Point2D
{
	public Point2D(double x, double y) => (X, Y) = (x, y);
	public readonly double X;
	public readonly double Y;
}
