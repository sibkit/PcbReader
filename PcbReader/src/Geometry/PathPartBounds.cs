using PcbReader.Geometry.PathParts;
// ReSharper disable InconsistentNaming

namespace PcbReader.Geometry;

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


public static class PathPartBounds {
    public static Bounds GetBounds(IPathPart pp) {
        return pp switch {
            LinePathPart lpp => GetLinePathPartBounds(lpp),
            ArcPathPart app => GetArcPathPartBounds(app),
            _ => throw new Exception("PathPartBounds : GetBounds not implemented"),
        };
    }

    private static Bounds GetLinePathPartBounds(LinePathPart pp) {
        
        double minX;
        double minY;
        double maxX;
        double maxY;

        if (pp.PointFrom.X > pp.PointTo.X) {
            minX = pp.PointFrom.X;
            maxX = pp.PointTo.X;
        } else {
            minX = pp.PointTo.X;
            maxX = pp.PointFrom.X;
        }

        if (pp.PointFrom.Y > pp.PointTo.Y) {
            minY = pp.PointFrom.Y;
            maxY = pp.PointTo.Y;
        } else {
            minY = pp.PointTo.Y;
            maxY = pp.PointFrom.Y;
        }
        
        return new Bounds {
            MinPoint = new Point(minX, minY),
            MaxPoint = new Point(maxX, maxY)
        };
    }

    private static Quadrant GetQuadrant(Point centerPoint, Point point) {
        if (centerPoint.Y < point.Y) {
            return point.X < centerPoint.X ? Quadrant.II : Quadrant.I;
        }
        return point.X < centerPoint.X ? Quadrant.III : Quadrant.IV;
    }

    private static Bounds GetArcPathPartBounds(ArcPathPart pp) {
        var cp = Geometry.ArcCenter(pp);

        var sp = pp.PointFrom;
        var ep = pp.PointTo;

        var spq = GetQuadrant(cp,sp);
        var epq = GetQuadrant(cp, ep);

        var quadrants = Quadrant.None;
        
        if (pp.RotationDirection == RotationDirection.CounterClockwise) {
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
            maxY = cp.Y + pp.Radius;
        } else {
            maxY = sp.Y>ep.Y ? sp.Y : ep.Y;
        }

        if ((quadrants & Quadrant.II_III) == Quadrant.II_III) {
            minX = cp.X - pp.Radius;
        } else {
            minX = sp.X < ep.X ? sp.X : ep.X;
        }

        if ((quadrants & Quadrant.III_IV) == Quadrant.III_IV) {
            minY = cp.Y - pp.Radius;
        } else {
            minY = sp.Y < ep.Y ? sp.Y : ep.Y;
        }
        
        if ((quadrants & Quadrant.IV_I) == Quadrant.IV_I) {
            maxX = cp.X + pp.Radius;
        } else {
            maxX = sp.X>ep.X ? sp.X : ep.X;
        }

        return new Bounds {
            MinPoint = new Point(minX, minY),
            MaxPoint = new Point(maxX, maxY)
        };
    }
}