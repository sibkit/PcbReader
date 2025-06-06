using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements.Curves;

namespace PcbReader.Core.Relations.PointsSearch;

public class LineLinePointsFinder: IPointsFinder<Line, Line> {
    public (List<Point> points, bool isMatch) FindContactPoints(Line line, Line arc) {
        var p1X1 = line.PointFrom.X;
        var p1Y1 = line.PointFrom.Y;
        var p1X2 = line.PointTo.X;
        var p1Y2 = line.PointTo.Y;
        var p2X1 = arc.PointFrom.X;
        var p2Y1 = arc.PointFrom.Y;
        var p2X2 = arc.PointTo.X;
        var p2Y2 = arc.PointTo.Y;
        
        var p1Dx = p1X1 - p1X2;
        var p1Dy = p1Y1 - p1Y2;
        var p2Dx = p2X1 - p2X2;
        var p2Dy = p2Y1 - p2Y2;

        var x = ((p1X1 * p1Y2 - p1Y1 * p1X2) * p2Dx - (p2X1 * p2Y2 - p2Y1 * p2X2) * p1Dx) / (p1Dx * p2Dy - p1Dy * p2Dx);
        var y = ((x - p2X1) * p2Dy / p2Dx) + p2Y1;

        
        
        if (!double.IsFinite(x)) {

            var k1 = (p1Y1 - p1Y2) / (p1X1 - p1X2);
            var k2 = (p2Y1 - p2Y2) / (p2X1 - p2X2);

            if (double.IsInfinity(k1)) {
                if (double.IsInfinity(k2)) {
                    if (Math.Abs(p1X1 - p2X1) < Geometry.Accuracy) {
                        return ([], true);
                    } else {
                        return ([], false);
                    }
                }
            } 
            
            if (Math.Abs(k1 - k2) < Geometry.Accuracy) {
                var b1 = p1Y2 - k1 * p1X2;
                var b2 = p1Y2 - k1 * p1X2;
                if (Math.Abs(b1 - b2) < Geometry.Accuracy) {
                    return ([], true);
                }
            }
            return ([], false);
        }
        
        return([new Point(x, y)], false);
    }
}