namespace PcbReader.Core.Entities;

public readonly struct Point(double x, double y) : IEquatable<Point> {
    public double X { get; } = x;
    public double Y { get; } = y;
    
    public static Point operator +(Point a, Point b)
        => new Point(a.X + b.X, a.Y + b.Y);
    
    public static Point operator -(Point a, Point b)
        => new Point(a.X - b.X, a.Y - b.Y);
    
    public static bool operator ==(Point a, Point b) 
        => Math.Abs(a.X - b.X) < Geometry.Accuracy && Math.Abs(a.Y - b.Y) < Geometry.Accuracy;
    
    public static bool operator !=(Point a, Point b)
        => Math.Abs(a.X - b.X) > Geometry.Accuracy || Math.Abs(a.Y - b.Y) > Geometry.Accuracy;

    public override bool Equals(object? obj) {
        if (obj is Point p)
            return Math.Abs(X - p.X) < Geometry.Accuracy && Math.Abs(Y - p.Y) < Geometry.Accuracy;
        return false;
    }

    public bool Equals(Point other) {
        return Math.Abs(X-other.X)<Geometry.Accuracy && Math.Abs(Y-other.Y)<Geometry.Accuracy;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Math.Round(X,1), Math.Round(Y,1));
    }

    public Point WithNewX(double newX) {
        return new Point(newX, Y);
    }
    
    public Point WithNewY(double newY) {
        return new Point(X, newY);
    }
}