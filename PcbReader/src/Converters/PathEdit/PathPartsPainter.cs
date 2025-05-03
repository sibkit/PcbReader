using PcbReader.Geometry;
using PcbReader.Geometry.PathParts;
using Path = PcbReader.Geometry.Path;

namespace PcbReader.Converters.PathEdit;

public class PathPartsPainter(double x, double y) {
    
   private Point _curPoint = new Point(x,y);
   public readonly List<IPathPart> PathParts = [];
    
    public void LineToInc(double x, double y) {
        _curPoint.X += x;
        _curPoint.Y += y;
        PathParts.Add(new LinePathPart {
            PointTo = _curPoint
        });
    }

    public void ArcToInc(double x, double y, double radius, RotationDirection direction, bool isLarge) {
        _curPoint.X += x;
        _curPoint.Y += y;
        PathParts.Add(new ArcPathPart {
            PointTo = _curPoint,
            IsLargeArc = isLarge,
            Radius = radius,
            RotationDirection = direction
        });
    }

    public Contour CreateContour() {
        var result = new Contour {
            StartPoint = new Point(x, y),
        };
        result.Parts.AddRange(PathParts);
        return result;
    }
    
    public Path CreatePath() {
        var result = new Path {
            StartPoint = new Point(x, y),
        };
        result.Parts.AddRange(PathParts);
        return result;
    }
}