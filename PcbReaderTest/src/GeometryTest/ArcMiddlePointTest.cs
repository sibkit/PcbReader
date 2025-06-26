using PcbReader.Converters.SpvToSvg;
using PcbReader.Layers.Svg.Entities;
using PcbReader.Layers.Svg.Writing;
using PcbReader.Spv;
using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;
using PcbReader.Spv.Entities.GraphicElements.Curves;
using PcbReader.Spv.Handling;

namespace PcbReaderTest.GeometryTest;

public class ArcMiddlePointTest {

    internal static void BuildSvgs() {
        var area = new Area();

        var painter = new Painter<Contour>(0,0);
        // painter.LineToInc(0,90);
        // painter.ArcToInc(10,10,10,RotationDirection.Clockwise, true);
        // painter.LineToInc(50,0);
        // painter.ArcToInc(10,-10,10,RotationDirection.Clockwise, true);
        // painter.LineToInc(0,-90);
        // painter.ArcToInc(-10,-10,10,RotationDirection.Clockwise, false);
        // painter.LineToInc(-50,0);
        // painter.ArcToInc(-10,10,10,RotationDirection.Clockwise, false);

        painter.ArcToInc(50,0,25,RotationDirection.Clockwise,false);
        painter.ArcToInc(-50,0,25,RotationDirection.Clockwise,false);
        
        var c = painter.Root;
        area.GraphicElements.Add(c);
        area.GraphicElements.Add(new Dot {
            CenterPoint = Geometry.ArcMiddlePoint((Arc)c.Curves[0]),
            Diameter = 2
        }); 
        area.GraphicElements.Add(new Dot {
            CenterPoint = Geometry.ArcMiddlePoint((Arc)c.Curves[1]),
            Diameter = 2
        }); 
        // area.GraphicElements.Add(new Dot {
        //     CenterPoint = Geometry.ArcMiddlePoint((ArcPathPart)c.Parts[5]),
        //     Diameter = 2
        // }); 
        // area.GraphicElements.Add(new Dot {
        //     CenterPoint = Geometry.ArcMiddlePoint((ArcPathPart)c.Parts[7]),
        //     Diameter = 2
        // }); 

        area.InvertYAxe();

        var svg = SpvToSvgConverter.Convert(area);
        SvgWriter.Write(svg,"tvg.svg");
    }
    
    internal static void CheckArcMiddlePoint(Arc arc, Point mp) {
        var mp1 = Geometry.ArcMiddlePoint(arc);
        Assert.True(Math.Abs(mp1.X-mp.X)<0.015);
        Assert.True(Math.Abs(mp1.Y-mp.Y)<0.015);
    }

    [Fact]
    public void TestArcMiddlePoint() {
        BuildSvgs();
        CheckArcMiddlePoint(
            new Arc {
                PointFrom = new Point(1, 0),
                PointTo = new Point(-1, 0),
                Radius = 1,
                IsLargeArc = false,
                RotationDirection = RotationDirection.CounterClockwise,

            },
            new Point(0, 1)
        );
        
        CheckArcMiddlePoint(
            new Arc {
                PointFrom = new Point(10, 100),
                PointTo = new Point(20, 110),
                Radius = 10,
                IsLargeArc = true,
                RotationDirection = RotationDirection.Clockwise,
            },
            new Point(2.93, 117.07)
        );

    }
}