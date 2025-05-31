using PcbReader.Converters.LvfToSvg;
using PcbReader.Core;
using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;
using PcbReader.Core.PathEdit;
using PcbReader.Layers.Svg.Entities;
using PcbReader.Layers.Svg.Writing;

namespace PcbReaderTest.GeometryTest;

public class ArcMiddlePointTest {

    internal static void BuildSvgs() {
        var area = new Area();

        var painter = new PathPartsPainter<Contour>(10,10);
        painter.LineToInc(0,90);
        painter.ArcToInc(10,10,10,RotationDirection.Clockwise, true);
        painter.LineToInc(50,0);
        painter.ArcToInc(10,-10,10,RotationDirection.Clockwise, true);
        painter.LineToInc(0,-90);
        painter.ArcToInc(-10,-10,10,RotationDirection.Clockwise, false);
        painter.LineToInc(-50,0);
        painter.ArcToInc(-10,10,10,RotationDirection.Clockwise, false);

        var c = painter.Root;
        area.GraphicElements.Add(c);
        area.GraphicElements.Add(new Dot {
            CenterPoint = Geometry.ArcMiddlePoint((ArcPathPart)c.Parts[1]),
            Diameter = 2
        }); 
        area.GraphicElements.Add(new Dot {
            CenterPoint = Geometry.ArcMiddlePoint((ArcPathPart)c.Parts[3]),
            Diameter = 2
        }); 
        area.GraphicElements.Add(new Dot {
            CenterPoint = Geometry.ArcMiddlePoint((ArcPathPart)c.Parts[5]),
            Diameter = 2
        }); 
        area.GraphicElements.Add(new Dot {
            CenterPoint = Geometry.ArcMiddlePoint((ArcPathPart)c.Parts[7]),
            Diameter = 2
        }); 
        
       
        
        
        area.InvertYAxe();

        var svg = LvfToSvgConverter.Convert(area);
        SvgWriter.Write(svg,"tvg.svg");
    }
    
    internal static void CheckArcMiddlePoint(Point from, Point to, Point mp) {
        BuildSvgs();
        var arc1 = new ArcPathPart {
            PointFrom = from,
            PointTo = to,
            Radius = 1d,
            RotationDirection = RotationDirection.CounterClockwise,
            IsLargeArc = false
        };
        var mp1 = Geometry.ArcMiddlePoint(arc1);
        Assert.True(Math.Abs(mp1.X-mp.X)<Geometry.Accuracy);
        Assert.True(Math.Abs(mp1.Y-mp.Y)<Geometry.Accuracy);
    }

    [Fact]
    public void TestArcMiddlePoint() {
        CheckArcMiddlePoint(new Point(1, 0), new Point(-1,0), new Point(0,1));
    }
}