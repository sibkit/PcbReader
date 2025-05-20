using PcbReader.Geometry;

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
}