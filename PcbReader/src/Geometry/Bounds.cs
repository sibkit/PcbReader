namespace PcbReader.Geometry;

public struct Bounds {
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }

    public double GetWidth() {
        return EndPoint.X - StartPoint.X;
    }

    public double GetHeight() {
        return EndPoint.Y - StartPoint.Y;
    }
}