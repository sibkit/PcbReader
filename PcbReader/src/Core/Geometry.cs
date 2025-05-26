using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry;

public readonly struct ArcWay(RotationDirection direction, bool isLarge) {
    public RotationDirection RotationDirection { get; init; } = direction;
    public bool IsLarge { get; init; } = isLarge;
}

public static class Geometry {

    public static double Accuracy { get; } = 0.000000001;
    public static double LineLength(Point sp, Point ep){
        return Math.Sqrt(Math.Pow(ep.X-sp.X,2)+Math.Pow(ep.Y-sp.Y,2));
    }
    
    public static Point ArcCenter(ArcPathPart pp) {
        //находим центр окружности через точки пересечения окружностей с центрами в sp и ep.
        //rd 
        var sp = pp.PointFrom; 
        var ep = pp.PointTo;
        
        var p0 = new Point((ep.X + sp.X) / 2, (ep.Y + sp.Y) / 2);
        var d = LineLength(sp, ep);
        var h = Math.Sqrt(Math.Pow(pp.Radius, 2) - Math.Pow(d / 2, 2));
        var p1 = new Point(p0.X + (ep.Y - sp.Y) * h / d, p0.Y - (ep.X - sp.X) * h / d);
        var p2 = new Point(p0.X - (ep.X - sp.Y) * h / d, p0.Y - (ep.X - sp.X) * h / d);
        return pp.RotationDirection == RotationDirection.ClockWise ? (pp.IsLargeArc ? p2 : p1) : (pp.IsLargeArc ? p1 : p2);
    }

    public static ArcWay ArcWay(Point sp, Point ep, Point cp) {
        var angle = ArcAngle(sp, ep, cp);

        var vecA = new Vector(ep - sp);
        var vecB = new Vector(cp - sp);
        var crossProduct = Vectors.CrossProduct(vecA, vecB);
        
        return crossProduct > 0 ? 
            new ArcWay(RotationDirection.CounterClockwise, angle > Math.PI) : 
            new ArcWay(RotationDirection.ClockWise, angle > Math.PI);
    }

    public static double ArcAngle(Point sp, Point ep, Point cp) {
        //|a|*|b|*cos(θ) = xa * xb + ya * yb
        var x1 = sp.X - cp.X;
        var x2 = ep.X - cp.X;
        var y1 = sp.Y - cp.Y;
        var y2 = ep.Y - cp.Y;
        var d1 = Math.Sqrt((x1 * x1 + y1 * y1));
        var d2 = Math.Sqrt((x2 * x2 + y2 * y2));
        return Math.Acos((x1*x2 + y1*y2)/(d1*d2));
    }

    
    private static Quadrant GetQuadrant(double prX, double prY) {
        if (prX >= 0) {
            return prY >= 0 ? Quadrant.I : Quadrant.IV;
        } else {
            return prY >= 0 ? Quadrant.II : Quadrant.III;
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