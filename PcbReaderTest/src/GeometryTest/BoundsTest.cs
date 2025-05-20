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
    
    
}