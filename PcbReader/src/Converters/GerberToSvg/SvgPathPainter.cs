using PcbReader.Geometry;
using PcbReader.Layers.Svg.Entities;

namespace PcbReader.Converters.GerberToSvg;

public class SvgPathPainter {
    private Point _curPoint;
    public SvgPath SvgPath { get; } = new SvgPath();
    
    public void MoveToInc(double x, double y) {
        _curPoint.X += x;
        _curPoint.Y += y;
        SvgPath.Parts.Add(new MoveSvgPathPart {
            PointTo = _curPoint
        });
    }
    
    public void LineToInc(double x, double y) {
        _curPoint.X += x;
        _curPoint.Y += y;
        SvgPath.Parts.Add(new LineSvgPathPart {
            PointTo = _curPoint
        });
    }

    public void ArcToInc(double x, double y, double radius, RotationDirection direction, bool isLarge) {
        _curPoint.X += x;
        _curPoint.Y += y;
        SvgPath.Parts.Add(new ArcSvgPathPart {
            PointTo = _curPoint,
            IsLargeArc = isLarge,
            Radius = radius,
            RotationDirection = direction
        });
    }
    
    public void MoveToAbs(double x, double y) {
        _curPoint.X = x;
        _curPoint.Y = y;
        SvgPath.Parts.Add(new MoveSvgPathPart {
            PointTo = _curPoint
        });
    }

    public void ClosePath() {
        SvgPath.Parts.Add(new CloseSvgPathPart());
    }
    
    
}