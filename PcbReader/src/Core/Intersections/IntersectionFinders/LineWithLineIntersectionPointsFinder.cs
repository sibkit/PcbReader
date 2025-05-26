using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections.IntersectionFinders;

public class LineWithLineIntersectionPointsFinder: IIntersectionPointsFinder<LinePathPart, LinePathPart> {
    public List<Point> FindIntersectionPoints(LinePathPart part1, LinePathPart part2) {
        var p1X1 = part1.PointFrom.X;
        var p1Y1 = part1.PointFrom.Y;
        var p1X2 = part1.PointTo.X;
        var p1Y2 = part1.PointTo.Y;
        var p2X1 = part2.PointFrom.X;
        var p2Y1 = part2.PointFrom.Y;
        var p2X2 = part2.PointTo.X;
        var p2Y2 = part2.PointTo.Y;
        
        var p1Dx = p1X1 - p1X2;
        var p1Dy = p1Y1 - p1Y2;
        var p2Dx = p2X1 - p2X2;
        var p2Dy = p2Y1 - p2Y2;

        var x = ((p1X1 * p1Y2 - p1Y1 * p1X2) * p2Dx - (p2X1 * p2Y2 - p2Y1 * p2X2) * p1Dx) / (p1Dx * p2Dy - p1Dy * p2Dx);
        var y = ((x - p2X1) * p2Dy / p2Dx) + p2Y1;

        if (!double.IsFinite(x)) {
            return [];
        }
        
        return[new Point(x, y)];
    }
}