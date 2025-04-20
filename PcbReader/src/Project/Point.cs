namespace PcbReader.Project;

public struct Point(decimal x, decimal y) {
    public decimal X { get; set; } = x;
    public decimal Y { get; set; } = y;
    
    public static Point operator +(Point a, Point b)
        => new Point(a.X + b.X, a.Y * b.Y);
}