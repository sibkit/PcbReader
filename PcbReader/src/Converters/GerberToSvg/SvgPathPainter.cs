using PcbReader.Geometry;
using PcbReader.Layers.Svg.Entities;

namespace PcbReader.Converters.GerberToSvg;

public class SvgPathPainter {
    private Point _curPoint;
    
    public SvgPath SvgPath { get; } = new SvgPath();
    
    public SvgPathPainter(Point startPoint) {
        _curPoint = startPoint;
    }
    
    public void LineTo(double x, double y) {
        _curPoint.X += x;
        _curPoint.Y += y;
        SvgPath.Parts.Add(new LineSvgPathPart {
            PointTo = _curPoint
        });
    }
    
    
}