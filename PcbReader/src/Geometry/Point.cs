namespace PcbReader.Geometry;

public struct Point(double x, double y) : IEquatable<Point> {
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    
    public static Point operator +(Point a, Point b)
        => new Point(a.X + b.X, a.Y + b.Y);
    
    public static Point operator -(Point a, Point b)
        => new Point(a.X - b.X, a.Y - b.Y);
    
    public static bool operator ==(Point a, Point b) 
        => Math.Abs(a.X - b.X) < 0.0000001 && Math.Abs(a.Y - b.Y) < 0.0000001;
    
    public static bool operator !=(Point a, Point b)
        => Math.Abs(a.X - b.X) > 0.0000001 || Math.Abs(a.Y - b.Y) > 0.0000001;

    public override bool Equals(object? obj) {
        if (obj is Point p)
            return Math.Abs(this.X - p.X) < 0.0000001 && Math.Abs(this.Y - p.Y) < 0.0000001;
        return false;
    }

    public bool Equals(Point other) {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override int GetHashCode() {
        return HashCode.Combine(X, Y);
    }
}