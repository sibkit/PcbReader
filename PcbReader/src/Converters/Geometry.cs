using PcbReader.Layers.Common;
using Path = PcbReader.Layers.Svg.Entities.Path;

namespace PcbReader.Converters;

public readonly struct ArcWay(RotationDirection direction, bool isLarge) {
    public RotationDirection RotationDirection { get; init; } = direction;
    public bool IsLarge { get; init; } = isLarge;
}

public static class Geometry {
    public static double LineLength(Point sp, Point ep){
        return Math.Sqrt(Math.Pow(ep.X-sp.X,2)+Math.Pow(ep.Y-sp.Y,2));
    }
    

    public static Point ArcCenter(Point sp, Point ep, double radius, RotationDirection rd, bool isLarge) {
        //находим центр окружности через точки пересечения окружностей с центрами в sp и ep.
        //rd 
        var p0 = new Point((ep.X + sp.X) / 2, (ep.Y + sp.Y) / 2);
        var d = Geometry.LineLength(sp, ep);
        var h = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(d / 2, 2));
        var p1 = new Point(p0.X + (ep.Y - sp.Y) * h / d, p0.Y - (ep.X - sp.X) * h / d);
        var p2 = new Point(p0.X - (ep.X - sp.Y) * h / d, p0.Y - (ep.X - sp.X) * h / d);
        return rd == RotationDirection.ClockWise ? (isLarge ? p2 : p1) : (isLarge ? p1 : p2);
    }

    public static ArcWay ArcWay(Point sp, Point ep, Point cp) {
        var angle = ArcAngle(sp, ep, cp);
        var s = (ep.X-sp.X)*(cp.Y-sp.Y) - (ep.Y-sp.Y)*(cp.X-sp.X);
        return new ArcWay(s > 0 ? RotationDirection.CounterClockwise : RotationDirection.ClockWise, angle > Math.PI);
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
}