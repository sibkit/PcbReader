using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReader.Core.Intersections.IntersectionFinders;

public static class TCalculator {
    
    private static double PositiveNormalize(double angle) {
        return angle switch {
            <= 0 => PositiveNormalize(angle + Math.PI),
            > Math.PI*2d => PositiveNormalize(angle - Math.PI),
            _ => angle
        };
    }

    private static double NegativeNormalize(double angle) {
        return angle switch {
            >= 0 => NegativeNormalize(angle - Math.PI),
            < -Math.PI*2d => NegativeNormalize(angle + Math.PI),
            _ => angle
        };
    }

    public static double CalculateT(IPathPart pp, Point ip) {
        return pp switch {
            LinePathPart linePart => CalculateT(linePart, ip),
            ArcPathPart arcPart => CalculateT(arcPart, ip),
            _ => throw new ArgumentOutOfRangeException(nameof(pp), pp, null)
        };
    }
    
    public static double CalculateT(LinePathPart part, Point ip) {
        var dx = Math.Abs(part.PointTo.X - part.PointFrom.X);
        var dy = Math.Abs(part.PointTo.Y - part.PointFrom.Y);
        if (dx > dy) {
            return (ip.X - part.PointFrom.X) / (part.PointTo.X - part.PointFrom.X);
        } else {
            return (ip.Y - part.PointFrom.Y) / (part.PointTo.Y - part.PointFrom.Y);
        }
    }
    
    public static double CalculateT(ArcPathPart part, Point ip) {
        //sp - startPoint
        //ep - endPoint
        //ip - intersectionPoint
        
        //PrX - X axe projection
        //PrY - Y axe projection

        var cp = Geometry.ArcCenter(part);
        
        var spPrX = part.PointFrom.X-cp.X;
        var spPrY = part.PointFrom.Y-cp.Y;
        
        var epPrX = part.PointTo.X-cp.X;
        var epPrY = part.PointTo.Y-cp.Y;
        
        var ipPrX = ip.X-cp.X;
        var ipPrY = ip.Y-cp.Y;
        
        
        var startAngle = PositiveNormalize(Math.Atan2(spPrY, spPrX));
        var endAngle = PositiveNormalize(Math.Atan2(epPrY, epPrX));
        var intersectionAngle = PositiveNormalize(Math.Atan2(ipPrY, ipPrX));
        
        double fullArcAngle;
        switch (part.RotationDirection) {
            case RotationDirection.ClockWise:
                //знак с минусом
                fullArcAngle = NegativeNormalize(endAngle - startAngle);
                return NegativeNormalize(intersectionAngle-startAngle) / fullArcAngle;
            case RotationDirection.CounterClockwise:
                //знак с плюсом
                fullArcAngle = PositiveNormalize(endAngle - startAngle);
                return PositiveNormalize(intersectionAngle-startAngle) / fullArcAngle;
            default:
                throw new Exception("ArcArcIntersectionsFinder : CalculateT");
        }
    }
}