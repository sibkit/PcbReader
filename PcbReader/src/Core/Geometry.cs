using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReader.Core;

public readonly struct ArcWay(RotationDirection direction, bool isLarge) {
    public RotationDirection RotationDirection { get; init; } = direction;
    public bool IsLarge { get; init; } = isLarge;
}

public static class Geometry {

    public static double Accuracy { get; } = 0.00000000001;
    public static double LineLength(Point sp, Point ep){
        return Math.Sqrt(Math.Pow(ep.X-sp.X,2)+Math.Pow(ep.Y-sp.Y,2));
    }
    
    public static Point ArcCenter(Arc pp) {
        //находим центр окружности через точки пересечения окружностей с центрами в sp и ep.
        //rd 
        var sp = pp.PointFrom;
        var ep = pp.PointTo;
        
        
        var p0 = new Point((ep.X + sp.X) / 2, (ep.Y + sp.Y) / 2);
        var d = LineLength(sp, ep);
        var a = d / 2;
        var h = Math.Sqrt(pp.Radius*pp.Radius - a*a);
        
        // var x1 = p0.X+(h/d)*(ep.Y-sp.Y);
        // var y1 = p0.Y-(h/d)*(ep.X-sp.X);    
        //
        // var x2 = p0.X - (h/d)*(ep.Y-sp.Y);
        
        var p1 = new Point(
            p0.X + (ep.Y - sp.Y) * h / d, 
            p0.Y - (ep.X - sp.X) * h / d
            );
        var p2 = new Point(
            p0.X - (ep.Y - sp.Y) * h / d, 
            p0.Y + (ep.X - sp.X) * h / d
            );
        return pp.RotationDirection == RotationDirection.Clockwise ? (pp.IsLargeArc ? p2 : p1) : (pp.IsLargeArc ? p1 : p2);
    }

    public static Point ArcMiddlePoint(Arc arc) {
        var cp = ArcCenter(arc);
        var theta = Math.Atan2(arc.PointFrom.Y - cp.Y, arc.PointFrom.X - cp.X);
        
        var beta = CalculateAngle(arc.PointFrom, arc.PointTo, cp);
        beta = arc.RotationDirection switch {
            RotationDirection.Clockwise => NegativeNormalize(beta),
            RotationDirection.CounterClockwise => PositiveNormalize(beta),
            _ => beta
        };

        var mb = beta / 2;

        return new Point(
            cp.X + arc.Radius * Math.Cos(theta + mb),
            cp.Y + arc.Radius * Math.Sin(theta + mb)
        );


    }

    public static ArcWay ArcWay(Point sp, Point ep, Point cp) {
        var angle = CalculateAngle(sp, ep, cp);

        var vecA = new Vector(ep - sp);
        var vecB = new Vector(cp - sp);
        var crossProduct = Vectors.CrossProduct(vecA, vecB);
        
        return crossProduct > 0 ? 
            new ArcWay(RotationDirection.CounterClockwise, angle > Math.PI) : 
            new ArcWay(RotationDirection.Clockwise, angle > Math.PI);
    }

    public static double CalculateAngle(Point sp, Point ep, Point cp) {
        return  Math.Atan2(ep.Y - cp.Y, ep.X - cp.X) - Math.Atan2(sp.Y - cp.Y, sp.X - cp.X);
    }


    public static double PositiveNormalize(double angle) {
        return angle switch {
            <= 0 => PositiveNormalize(angle + 2*Math.PI),
            > Math.PI*2d => PositiveNormalize(angle - 2*Math.PI),
            _ => angle
        };
    }
    
    public static double NegativeNormalize(double angle) {
        return angle switch {
            >= 0 => NegativeNormalize(angle - 2*Math.PI),
            < -Math.PI*2d => NegativeNormalize(angle + 2*Math.PI),
            _ => angle
        };
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