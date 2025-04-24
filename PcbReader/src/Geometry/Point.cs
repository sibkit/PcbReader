namespace PcbReader.Layers.Common;

public struct Point(double x, double y) {
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    
    public static Point operator +(Point a, Point b)
        => new Point(a.X + b.X, a.Y * b.Y);
    
    public static bool operator ==(Point a, Point b) 
        => Math.Abs(a.X - b.X) < 0.000001 && Math.Abs(a.Y - b.Y) < 0.000001;
    
    public static bool operator !=(Point a, Point b)
        => Math.Abs(a.X - b.X) > 0.000001 || Math.Abs(a.Y - b.Y) > 0.000001;
}