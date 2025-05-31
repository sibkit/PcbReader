using System.Runtime.Intrinsics;
using PcbReader.Core;
using PcbReader.Core.GraphicElements.PathParts;


namespace PcbReaderTest.GeometryTest;

public class BoundsTest {
    [Fact]
    public void TestBoundsIntersections() {
        var b1 = new Bounds(0,0,5,5);
        
        Assert.False(b1.IsIntersected(new Bounds(6,6,7,7)));
        Assert.True(b1.IsIntersected(new Bounds(-1,-1,7,7)));
        Assert.True(b1.IsIntersected(new Bounds(-1,-1,1,1)));
    }
    
    [Fact]
    public void TestContainsPoint() {
        var b1 = new Bounds(0,0,5,5);
        
        Assert.True(b1.Contains(new Point(0,0)));
        Assert.True(b1.Contains(new Point(5,5)));
        Assert.True(b1.Contains(new Point(2,3)));
        
        Assert.False(b1.Contains(new Point(-1,0)));
        Assert.False(b1.Contains(new Point(0,-1)));
        Assert.False(b1.Contains(new Point(6,5)));
        Assert.False(b1.Contains(new Point(2,5.0001)));
        Assert.False(b1.Contains(new Point(-0.000001,2)));

    }

    [Fact]
    public void TestArcBounds() {
        var arc1 = new ArcPathPart {
            PointFrom = new Point(0, 0),
            PointTo = new Point(5, 5),
            RotationDirection = RotationDirection.Clockwise,
            IsLargeArc = false,
            Radius = 5
        };

        var b1 = arc1.Bounds;
        Assert.True(Math.Abs(b1.GetWidth() - 5) < Geometry.Accuracy);
        Assert.True(Math.Abs(b1.GetHeight() - 5) < Geometry.Accuracy);
        
        arc1.IsLargeArc = true;
        arc1.UpdateBounds();
        var b2 = arc1.Bounds;
        Assert.True(Math.Abs(b2.GetWidth() - 10) < Geometry.Accuracy);
        Assert.True(Math.Abs(b2.GetHeight() - 10) < Geometry.Accuracy);
    }
}