using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections.IntersectionFinders;

public class CircleWithCircleIntersectionPointsFinder: IIntersectionPointsFinder<ArcPathPart, ArcPathPart> {
    
    public List<Point> FindIntersectionPoints(ArcPathPart part1, ArcPathPart part2) {
        if (!part1.Bounds.IsIntersected(part2.Bounds))
            return [];
        
        var p1C = Geometry.ArcCenter(part1);
        var p2C = Geometry.ArcCenter(part2);
        
        var p1Xc = p1C.X;
        var p1Yc = p1C.Y;

        var p2Xc = p2C.X;
        var p2Yc = p2C.Y;
        
        var p1R = part1.Radius;
        var p2R = part2.Radius;

        var d = Math.Sqrt((p2Xc-p1Xc)*(p2Xc-p1Xc)+(p2Yc-p1Yc)*(p2Yc-p1Yc));
        if(d>p1R+p2R || d<Math.Abs(p1R-p2R) || d==0)
            return [];
        
        var a = (p1R*p1R - p2R*p2R + d*d)/(2d*d);
        var h = Math.Sqrt(p1R*p1R - a*a);

        var xm = p1Xc + (a / d) * (p2Xc - p1Xc);
        var ym = p1Yc + (a / d) * (p2Yc - p1Yc);
        
        var x1 = xm+(h/d)*(p2Yc - p1Yc);
        var y1 = ym-(h/d)*(p2Xc - p1Xc);

        var x2 = xm-(h/d)*(p2Yc - p1Yc);
        var y2 = ym+(h/d)*(p2Xc - p1Xc);
        
        if (double.IsNaN(x1) || double.IsNaN(y1)) {
            return [];
        }

        return [new Point(x1, y1), new Point(x2, y2)];
    }
}