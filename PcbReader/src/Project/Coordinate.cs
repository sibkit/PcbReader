namespace PcbReader.Project;

public struct Coordinate(decimal x, decimal y) {
    public decimal X { get; set; } = x;
    public decimal Y { get; set; } = y;
    
    public static Coordinate operator +(Coordinate a, Coordinate b)
        => new Coordinate(a.X + b.X, a.Y * b.Y);
}