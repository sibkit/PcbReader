namespace PcbReader.Geometry.PathParts;



public class ArcPathPart : IPathPart {

    private Bounds? _bounds;
    
    public required Point PointTo { get; init; }
    public required Point PointFrom { get; init; }
    public required IPathPartsOwner Owner { get; init; }
    
    public required double Radius { get; init; }
    public required bool IsLargeArc { get; init; } = false;
    public required RotationDirection RotationDirection { get; set; } = 0;
    
    
    private static Quadrant GetQuadrant(Point centerPoint, Point point) {
        if (centerPoint.Y < point.Y) {
            return point.X < centerPoint.X ? Quadrant.II : Quadrant.I;
        }
        return point.X < centerPoint.X ? Quadrant.III : Quadrant.IV;
    }
    
    public Bounds Bounds {
        get {

            if (_bounds == null) {
                var cp = Geometry.ArcCenter(this);

                var sp = this.PointFrom;
                var ep = this.PointTo;

                var spq = GetQuadrant(cp,sp);
                var epq = GetQuadrant(cp, ep);

                var quadrants = Quadrant.None;
        
                if (this.RotationDirection == RotationDirection.CounterClockwise) {
                    while (epq != spq) {
                        quadrants |= spq;
                        spq = spq.Next();
                    }
                } else {
                    while (epq != spq) {
                        quadrants |= spq;
                        spq = spq.Prev();
                    }
                }
        
                double minX;
                double minY;
                double maxX;
                double maxY;


                if ((quadrants & Quadrant.I_II) == Quadrant.I_II) {
                    maxY = cp.Y + Radius;
                } else {
                    maxY = sp.Y>ep.Y ? sp.Y : ep.Y;
                }

                if ((quadrants & Quadrant.II_III) == Quadrant.II_III) {
                    minX = cp.X - Radius;
                } else {
                    minX = sp.X < ep.X ? sp.X : ep.X;
                }

                if ((quadrants & Quadrant.III_IV) == Quadrant.III_IV) {
                    minY = cp.Y - Radius;
                } else {
                    minY = sp.Y < ep.Y ? sp.Y : ep.Y;
                }
        
                if ((quadrants & Quadrant.IV_I) == Quadrant.IV_I) {
                    maxX = cp.X + Radius;
                } else {
                    maxX = sp.X>ep.X ? sp.X : ep.X;
                }

                _bounds = new Bounds(minX, minY,maxX, maxY);   
            }
            return _bounds.Value;
        }
    } 
}

[Flags]
public enum Quadrant {
    None = 0,
    I = 1,
    II = 2,
    III = 4,
    IV = 8,
    I_II = I | II,
    II_III = II | III,
    III_IV = III | IV,
    IV_I = IV |I
}

public static class QuadrantExtensions
{
    public static Quadrant Next(this Quadrant quadrant) {
        return quadrant switch {
            Quadrant.I => Quadrant.II,
            Quadrant.II => Quadrant.III,
            Quadrant.III => Quadrant.IV,
            Quadrant.IV => Quadrant.I,
            _ => throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, null)
        };
    }
    
    public static Quadrant Prev(this Quadrant quadrant) {
        return quadrant switch {
            Quadrant.I => Quadrant.IV,
            Quadrant.IV => Quadrant.III,
            Quadrant.III => Quadrant.II,
            Quadrant.II => Quadrant.I,
            _ => throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, null)
        };
    }
}