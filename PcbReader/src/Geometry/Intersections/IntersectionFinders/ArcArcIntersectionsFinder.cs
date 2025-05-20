using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections.IntersectionFinders;

public class ArcArcIntersectionsFinder: IIntersectionsFinder<ArcPathPart, ArcPathPart> {

    private static double CalculateX1(double xc1, double xc2, double yc1, double yc2, double r1, double r2) {
        return (yc1 - yc2) * (-2 * r2 * yc1 + 2 * r2 * yc2 + Math.Sqrt((-r1 * r1 + 2 * r1 * r2 - r2 * r2 + xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2) *
                                                                       (r1 * r1 + 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2))) /
               (2 * xc1 * xc1 - 4 * xc1 * xc2 + 2 * xc2 * xc2 + 2 * yc1 *
                   yc1 - 4 * yc1 * yc2 + 2 * yc2 * yc2) +
               (-r1 * r1 * xc1 + r1 * r1 * xc2 + r2 * r2 * xc1 - r2 * r2 * xc2 + 2 * r2 * yc1 * yc1 - 4 * r2 * yc1 * yc2 + 2 * r2 * yc2 * yc2 + xc1 * xc1 * xc1 - xc1 * xc1 * xc2 -
                xc1 * xc2 * xc2 + xc1 * yc1 * yc1 - 2 * xc1 * yc1 * yc2 + xc1 * yc2 * yc2 + xc2 * xc2 * xc2 + xc2 * yc1 * yc1 - 2 * xc2 * yc1 * yc2 +
                xc2 * yc2 * yc2) / (2 * xc1 * xc1 - 4 * xc1 * xc2 + 2 * xc2 * xc2 + 2 * yc1 * yc1 - 4 * yc1 * yc2 + 2 * yc2 * yc2);
    }
    private static double CalculateY1(double xc1, double xc2, double yc1, double yc2, double r1, double r2) {
        return -(yc1 - yc2) * (2 * r2 * yc1 - 2 * r2 * yc2 +
                               Math.Sqrt((-r1 * r1 + 2 * r1 * r2 - r2 * r2 + xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2) *
                                         (r1 * r1 + 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2))) /
            (2 * xc1 * xc1 - 4 * xc1 * xc2 + 2 * xc2 * xc2 + 2 * yc1 * yc1 - 4 * yc1 * yc2 + 2 * yc2 * yc2) + (-r1 * r1 * xc1 + r1 * r1 * xc2 +
                                                                                                               r2 * r2 * xc1 - r2 * r2 * xc2 + 2 * r2 * yc1 * yc1 - 4 * r2 * yc1 * yc2 +
                                                                                                               2 * r2 * yc2 * yc2 + xc1 * xc1 * xc1 - xc1 * xc1 * xc2 -
                                                                                                               xc1 * xc2 * xc2 + xc1 * yc1 * yc1 - 2 * xc1 * yc1 * yc2 + xc1 * yc2 * yc2 +
                                                                                                               xc2 * xc2 * xc2 + xc2 * yc1 * yc1 - 2 * xc2 * yc1 * yc2 +
                                                                                                               xc2 * yc2 * yc2) / (2 * xc1 * xc1 - 4 * xc1 * xc2 + 2 * xc2 * xc2 + 2 * yc1 * yc1 -
                4 * yc1 * yc2 + 2 * yc2 * yc2);
    }
    private static double CalculateX2(double xc1, double xc2, double yc1, double yc2, double r1, double r2) {
        return -(xc1 - xc2) * (-2 * r2 * yc1 + 2 * r2 * yc2 +
                               Math.Sqrt((-r1*r1 + 2 * r1 * r2 - r2*r2 + xc1*xc1 - 2 * xc1 * xc2 + xc2*xc2 + yc1*yc1 - 2 * yc1 * yc2 + yc2*yc2) *
                                         (r1*r1 + 2 * r1 * r2 + r2*r2 - xc1*xc1 + 2 * xc1 * xc2 - xc2*xc2 - yc1*yc1 + 2 * yc1 * yc2 - yc2*yc2))) /
            (2 * xc1*xc1 - 4 * xc1 * xc2 + 2 * xc2*xc2 + 2 * yc1*yc1 - 4 * yc1 * yc2 + 2 * yc2*yc2) + (-r1*r1 * yc1 + r1*r1 * yc2 +
                r2*r2 * yc1 - r2*r2 * yc2 - 2 * r2 * xc1 * yc1 + 2 * r2 * xc1 * yc2 + 2 * r2 * xc2 * yc1 - 2 * r2 * xc2 * yc2 + xc1*xc1 * yc1 + xc1*xc1 * yc2 -
                2 * xc1 * xc2 * yc1 - 2 * xc1 * xc2 * yc2 + xc2*xc2 * yc1 + xc2*xc2 * yc2 + yc1*yc1*yc1 - yc1*yc1 * yc2 - yc1 * yc2*yc2 + yc2*yc2*yc2) /
            (2 * xc1*xc1 - 4 * xc1 * xc2 + 2 * xc2*xc2 + 2 * yc1*yc1 - 4 * yc1 * yc2 + 2 * yc2*yc2);
    }
    private static double CalculateY2(double xc1, double xc2, double yc1, double yc2, double r1, double r2) {
        return (xc1 - xc2) * (2 * r2 * yc1 - 2 * r2 * yc2 +
                              Math.Sqrt((-r1*r1 + 2 * r1 * r2 - r2*r2 + xc1*xc1 - 2 * xc1 * xc2 + xc2*xc2 + yc1*yc1 - 2 * yc1 * yc2 + yc2*yc2) *
                                        (r1*r1 + 2 * r1 * r2 + r2*r2 - xc1*xc1 + 2 * xc1 * xc2 - xc2*xc2 - yc1*yc1 + 2 * yc1 * yc2 - yc2*yc2))) /
            (2 * xc1*xc1 - 4 * xc1 * xc2 + 2 * xc2*xc2 + 2 * yc1*yc1 - 4 * yc1 * yc2 + 2 * yc2*yc2) + (-r1*r1 * yc1 + r1*r1 * yc2 +
                r2*r2 * yc1 - r2*r2 * yc2 - 2 * r2 * xc1 * yc1 + 2 * r2 * xc1 * yc2 + 2 * r2 * xc2 * yc1 - 2 * r2 * xc2 * yc2 + xc1*xc1 * yc1 + xc1*xc1 * yc2 -
                2 * xc1 * xc2 * yc1 - 2 * xc1 * xc2 * yc2 + xc2*xc2 * yc1 + xc2*xc2 * yc2 + yc1*yc1*yc1 - yc1*yc1 * yc2 - yc1 * yc2*yc2 + yc2*yc2*yc2) /
            (2 * xc1*xc1 - 4 * xc1 * xc2 + 2 * xc2*xc2 + 2 * yc1*yc1 - 4 * yc1 * yc2 + 2 * yc2*yc2);
    }
    
    public List<Point> FindIntersections(ArcPathPart part1, ArcPathPart part2, IntersectionsSorting sorting) {
        
        
        
        var c1 = Geometry.ArcCenter(part1);
        var c2 = Geometry.ArcCenter(part2);
        
        var r1 = part1.Radius;
        var r2 = part2.Radius;
        
        var xc1 = c1.X;
        var yc1 = c1.Y;
        
        var xc2 = c2.X;
        var yc2 = c2.Y;

        var result = new List<Point>();
        
        var startAxe1Point = new Point(c1.X + 1, c1.Y);
        
        var th11 = Geometry.ArcAngle(startAxe1Point, part1.PointFrom, c1);
        var th12 = Geometry.ArcAngle(startAxe1Point, part2.PointTo, c1);
        
        var t1 = (th11 + Math.Asin(yc1 / r1)) / (th11 - th12);
        
        if (t1 is >= 0 and <= 1) {
            var x1 = CalculateX1(xc1, xc2, yc1, yc2, r1, r2);
            var y1 = CalculateY1(xc1, xc2, yc1, yc2, r1, r2);
            if(!double.IsNaN(x1))
                result.Add(new Point(x1, y1));
        }

        var t2 = (th11 - Math.Asin(yc1 / r1) + Math.PI) / (th11 - th12);
        
        if (t2 is >= 0 and <= 1) {
            var x2 = CalculateX2(xc1, xc2, yc1, yc2, r1, r2);
            var y2 = CalculateY2(xc1, xc2, yc1, yc2, r1, r2);
            if(!double.IsNaN(x2))
                result.Add(new Point(x2, y2));
        }

        return result;
    }
}