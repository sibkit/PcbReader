using PcbReader.Core.Entities;

namespace PcbReader.Core;

public readonly struct Vector(double x, double y) {

    public Vector(Point p) : this(p.X, p.Y) { }
    
    public double X { get; } = x;
    public double Y { get; } = y;
}

public static class Vectors {
    public static double DotProduct(Vector a, Vector b) {
        return a.X * b.X + a.Y * b.Y;
    }

    public static double CrossProduct(Vector a, Vector b) {
        return a.X * b.Y - a.Y * b.X;
    }
}