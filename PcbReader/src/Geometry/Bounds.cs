namespace PcbReader.Geometry;

public struct Bounds {
    public Point MinPoint { get; set; }
    public Point MaxPoint { get; set; }

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