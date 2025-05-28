namespace PcbReader.Core;

public readonly struct Bounds {
    
    public double MinX { get; }
    public double MinY { get; }
    public double MaxX { get; }
    public double MaxY { get; }

    public Point MinPoint => new(MinX, MinY);
    public Point MaxPoint => new(MaxX, MaxY);
    
    public Bounds(Point minPoint, Point maxPoint) : this(minPoint.X, minPoint.Y, maxPoint.X, maxPoint.Y) { }

    public Bounds(double minX, double minY, double maxX, double maxY) {
        if(minX > maxX || minY > maxY)
            throw new Exception("Bounds: minPoint must be greater than or equal to maxPoint");
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }
    
    public double GetWidth() {
        return MaxX - MinX;
    }

    public double GetHeight() {
        return MaxY - MinY;
    }

    public bool Contains(Point p) {
        return p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;
    }
    
    public bool IsIntersected(Bounds b) {
        return !((MaxX<b.MinX || b.MaxX<MinX) && (MaxY<b.MaxY || b.MaxY<MinY));
    }
}