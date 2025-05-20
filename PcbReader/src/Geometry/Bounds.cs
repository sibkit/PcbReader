namespace PcbReader.Geometry;

public readonly struct Bounds {
    
    public double MinX { get; }
    public double MinY { get; }
    public double MaxX { get; }
    public double MaxY { get; }

    public Point MinPoint => new(MinX, MinY);

    public Point MaxPoint => new(MaxX, MaxY);


    public Bounds(Point minPoint, Point maxPoint) :this(minPoint.X, minPoint.Y, maxPoint.X, maxPoint.Y) { }
    
    public Bounds(double minX, double minY, double maxX, double maxY) {
        if(minX > maxX || minY > maxY)
            throw new Exception("Bounds: minPoint must be greater than or equal to maxPoint");
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }
    
    public double GetWidth() {
        return MaxPoint.X - MinPoint.X;
    }

    public double GetHeight() {
        return MaxPoint.Y - MinPoint.Y;
    }

    public bool Contains(Point p) {
        return p.X > MinPoint.X && p.X < MaxPoint.X && p.Y > MinPoint.Y && p.Y < MaxPoint.Y;
    }

    private static bool IsIntersectedByAxe(double aMin, double aMax, double bMin, double bMax) {
        return !(aMax < bMin) && !(bMax < aMin);
    }
    public bool IsIntersected(Bounds b) {
        return IsIntersectedByAxe(MinPoint.X, MaxPoint.X, b.MinPoint.X, b.MaxPoint.X) && 
               IsIntersectedByAxe(MinPoint.Y, MaxPoint.Y, b.MinPoint.Y, b.MaxPoint.Y);
    }
}