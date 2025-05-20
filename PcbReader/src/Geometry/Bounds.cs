namespace PcbReader.Geometry;

public readonly struct Bounds {
    public Point MinPoint { get; init; }
    public Point MaxPoint { get; init; }

    public double GetWidth() {
        return MaxPoint.X - MinPoint.X;
    }

    public double GetHeight() {
        return MaxPoint.Y - MinPoint.Y;
    }

    public bool Contains(Point p) {
        return p.X > MinPoint.X && p.X < MaxPoint.X && p.Y > MinPoint.Y && p.Y < MaxPoint.Y;
    }
}