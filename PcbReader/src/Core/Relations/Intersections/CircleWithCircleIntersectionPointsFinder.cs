using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements.Curves;

namespace PcbReader.Core.Location.Intersections.IntersectionFinders;

public class CircleWithCircleIntersectionPointsFinder: IIntersectionPointsFinder<Arc, Arc> {
    
    public List<Point> FindIntersectionPoints(Arc part1, Arc part2) {
        if (!part1.Bounds.IsIntersected(part2.Bounds))
            return [];
        
        var p1C = Geometry.ArcCenter(part1);
        var p2C = Geometry.ArcCenter(part2);
        
        var p1R = part1.Radius;
        var p2R = part2.Radius;

        var d = Math.Sqrt((p2C.X-p1C.X)*(p2C.X-p1C.X)+(p2C.Y-p1C.Y)*(p2C.Y-p1C.Y));
        if(d>p1R+p2R || d<Math.Abs(p1R-p2R) || d==0)
            return [];
        
        var a = (p1R*p1R - p2R*p2R + d*d)/(2d*d);
        var h = Math.Sqrt(p1R*p1R - a*a);

        var xm = p1C.X + (a / d) * (p2C.X - p1C.X);
        var ym = p1C.Y + (a / d) * (p2C.Y - p1C.Y);
        
        var x1 = xm+(h/d)*(p2C.Y - p1C.Y);
        var y1 = ym-(h/d)*(p2C.X - p1C.X);

        var x2 = xm-(h/d)*(p2C.Y - p1C.Y);
        var y2 = ym+(h/d)*(p2C.X - p1C.X);
        
        if (double.IsNaN(x1) || double.IsNaN(y1)) {
            return [];
        }

        return [new Point(x1, y1), new Point(x2, y2)];
    }
}