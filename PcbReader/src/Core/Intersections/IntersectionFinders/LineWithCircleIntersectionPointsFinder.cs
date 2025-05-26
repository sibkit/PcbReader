using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections.IntersectionFinders;

public class LineWithCircleIntersectionPointsFinder : IIntersectionPointsFinder<LinePathPart, ArcPathPart> {



    public List<Point> FindIntersectionPoints(LinePathPart part1, ArcPathPart part2) {
        
        var p2C = Geometry.ArcCenter(part2);
        
        var p1X1 = part1.PointFrom.X - p2C.X;
        var p1Y1 = part1.PointFrom.Y - p2C.Y;
        
        var p1X2 = part1.PointTo.X - p2C.X;
        var p1Y2 = part1.PointTo.Y - p2C.Y;

        var p1Dx = p1X2 - p1X1;
        var p1Dy = p1Y2 - p1Y1;
        
        var a = p1Dy;
        var b = -p1Dx; 
        var c = p1Dx * p1Y1 - p1Dy * p1X1;

        var d = Math.Abs(c)/Math.Sqrt(a*a + b*b);
        if (d > part2.Radius)
            return [];
        
        var zpX = -(a * c) / (a * a + b * b);
        var zpY = -(b * c) / (a * a + b * b);

        if (Math.Abs(d - part2.Radius) < Geometry.Accuracy) {
            return [new Point(zpX+p2C.X, zpY+p2C.Y)];
        }
        
        var k = Math.Sqrt(part2.Radius*part2.Radius - d*d);

        var mult = Math.Sqrt(k * k / (a * a + b * b));
        
        var pI1 = new Point(zpX + b * mult, zpY - a * mult);
        var pI2 = new Point(zpX - b * mult, zpY + a * mult);

        return [
            new Point(pI1.X + p2C.X, pI1.Y + p2C.Y),
            new Point(pI2.X + p2C.X, pI2.Y + p2C.Y)
        ];
    }
}