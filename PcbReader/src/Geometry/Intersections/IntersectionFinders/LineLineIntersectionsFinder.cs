using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections.IntersectionFinders;

public class LineLineIntersectionsFinder: IIntersectionsFinder<ArcPathPart, ArcPathPart> {
    public List<Point> FindIntersections(ArcPathPart part1, ArcPathPart part2, IntersectionsSorting sorting) {
        var x10 = part1.PointFrom.X;
        //var y10 = part1.PointFrom.Y;
        var x11 = part1.PointTo.X;
        //var y11 = part1.PointTo.Y;
        var x20 = part2.PointFrom.X;
        var y20 = part2.PointFrom.Y;
        var x21 = part2.PointTo.X;
        var y21 = part2.PointTo.Y;
        var t = (x10 - x20) / (x10 + x11 - x20 - x21);
        if (t - Geometry.Accuracy < 0 || t + Geometry.Accuracy > 1) {
            return [];
        }

        var x = (-x10 * x21 + x11 * x20) / (x10 + x11 - x20 - x21);
        var y = (-x10 * y21 + x11 * y20 + x20 * y21 - x21 * y20) / (x10 + x11 - x20 - x21);
        return [
            new Point(x, y)
        ];
    }
}