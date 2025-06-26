using System.Text.RegularExpressions;
using PcbReader.Spv;
using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements.Curves;
using PcbReader.Spv.Handling;

namespace PcbReaderTest.GeometryTest;

public class VectorsTest {
    [Fact]
    public void TestVectorsAngle() {
        Assert.Equal(-Math.PI/2d, Vectors.Angle(new Vector(0,10), new Vector(10,0)));
    }

    [Fact]
    public void TestStartCurveVector() {
        var cos45 = Math.Cos(Math.PI / 4d);


        
        var arc1 = new Arc {
            PointFrom = new Point(2d + cos45, 2d + cos45),
            PointTo = new Point(2d - cos45, 2d + cos45),
            RotationDirection = RotationDirection.CounterClockwise,
            Radius = 1d,
            IsLargeArc = false
        };
        
        var arc2 = new Arc {
            PointFrom = new Point(2d - cos45, 2d + cos45),
            PointTo = new Point(2d + cos45, 2d + cos45),
            RotationDirection = RotationDirection.Clockwise,
            Radius = 1d,
            IsLargeArc = false
        };
        
        var arc3 = new Arc {
            PointFrom = new Point(2d, 3d),
            PointTo = new Point(1d, 2d),
            RotationDirection = RotationDirection.CounterClockwise,
            Radius = 1d,
            IsLargeArc = false
        };
        
        var arc4 = new Arc {
            PointFrom = new Point(2d, 1d),
            PointTo = new Point(2d , 3d),
            RotationDirection = RotationDirection.Clockwise,
            Radius = 1d,
            IsLargeArc = true
        };
        
        var arc5= new Arc {
            PointFrom = new Point(2d, 1d),
            PointTo = new Point(1d, 2d),
            RotationDirection = RotationDirection.CounterClockwise,
            Radius = 1d,
            IsLargeArc = true
        };
        
        var arc6 = new Arc {
            PointFrom = new Point(2d - cos45, 2d - cos45),
            PointTo = new Point(2d + cos45, 2d - cos45),
            RotationDirection = RotationDirection.CounterClockwise,
            Radius = 1d,
            IsLargeArc = false
        };




        var v1 = Curves.GetCurveInVector(arc1);
        var v2 = Curves.GetCurveInVector(arc2);
        var v3 = Curves.GetCurveInVector(arc3);
        var v4 = Curves.GetCurveInVector(arc4);
        var v5 = Curves.GetCurveInVector(arc5);
        var v6 = Curves.GetCurveInVector(arc6);
        

    }
}