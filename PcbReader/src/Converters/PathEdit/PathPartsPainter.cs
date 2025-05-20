using PcbReader.Geometry;
using PcbReader.Geometry.PathParts;
using Path = PcbReader.Geometry.Path;

namespace PcbReader.Converters.PathEdit;

public class PathPartsPainter<T> where T : class, IPathPartsOwner, new() {

    public T PartsOwner { get; } = new T();

    public PathPartsPainter(double x, double y) {
        PartsOwner.StartPoint = new Point(x, y);
        _curPoint = new Point(x, y);
    }
    
    private Point _curPoint;
   //public readonly List<IPathPart> PathParts = [];

   public void LineToAbs(double x, double y) {

       PartsOwner.Parts.Add(new LinePathPart {
           PointFrom = _curPoint,
           PointTo = new Point(x,y)
       });
       _curPoint = new Point(x, y);
   }
    public void LineToInc(double x, double y) {

        PartsOwner.Parts.Add(new LinePathPart {
            PointFrom = _curPoint,
            PointTo = new Point(_curPoint.X + x, _curPoint.Y + y)
        });
        _curPoint= new Point(_curPoint.X + x, _curPoint.Y + y);
    }

    public void ArcToInc(double x, double y, double radius, RotationDirection direction, bool isLarge) {
        _curPoint = new Point(_curPoint.X + x, _curPoint.Y + y);
        
        PartsOwner.Parts.Add(new ArcPathPart {
            PointTo = _curPoint,
            PointFrom = new Point(_curPoint.X - x,
                _curPoint.Y - y),
            IsLargeArc = isLarge,
            Radius = radius,
            RotationDirection = direction,
            Owner = PartsOwner
        });
    }

    private Point RotatePoint(Point rotationCenter, Point point, double angle, RotationDirection direction) {
        var (nx, ny) = RotatePoint(rotationCenter, point.X, point.Y, angle, direction);
        return new Point(nx, ny);
    }

    private (double nx, double ny) RotatePoint(Point rotationCenter, double x, double y, double angle, RotationDirection direction) {
        var r = Math.Sqrt(Math.Pow(x - rotationCenter.X, 2) + Math.Pow(y - rotationCenter.Y, 2));
        var betta = Math.Acos((x - rotationCenter.X) / r);

        switch (direction) {
            case RotationDirection.ClockWise:
                return (x + r * Math.Cos(angle), y - r * Math.Sin(angle));
            case RotationDirection.CounterClockwise:
                return (rotationCenter.X + x, rotationCenter.Y - y);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    // public void Rotate(Point rotationCenter, double angle, RotationDirection direction) {
    //     //var (nx, ny) = RotatePoint(rotationCenter, rotationCenter.X, rotationCenter.Y);
    //     foreach (var pathPart in PathParts) {
    //         switch (pathPart) {
    //             case LinePathPart lp:
    //                 lp.PointTo = RotatePoint(rotationCenter, lp.PointTo, angle, direction);
    //                 break;
    //             case ArcPathPart ap:
    //                 ap.PointTo = RotatePoint(rotationCenter, ap.PointTo, angle, direction);
    //                 break;
    //         }
    //     }
    // }
    
    // public Contour CreateContour() {
    //     var result = new Contour {
    //         StartPoint = new Point(x, y),
    //     };
    //     result.Parts.AddRange(PathParts);
    //     return result;
    // }
    //
    // public Path CreatePath() {
    //     var result = new Path {
    //         StartPoint = new Point(x, y),
    //     };
    //     result.Parts.AddRange(PathParts);
    //     return result;
    // }
}