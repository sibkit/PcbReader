using PcbReader.Converters.PathEdit;
using PcbReader.Geometry;
using PcbReader.Geometry.Intersections;

namespace PcbReaderTest.GeometryTest.IntersectionsTest;

public class IntersectionsTest {
    [Fact]
    public void TestIntersections() {
        var p1 = new PathPartsPainter<Contour>(10,10);
        p1.LineToInc(0,30);
        p1.LineToInc(30,0);
        p1.ArcToInc(-30,-30,30,RotationDirection.ClockWise,false);
        var c1 = p1.PartsOwner;
        
        var p2 = new PathPartsPainter<Contour>(20,10);
        p2.ArcToInc(20,20,20,RotationDirection.ClockWise,false);
        p2.LineToInc(0,-20);
        p2.LineToInc(-20,0);
        var c2 = p2.PartsOwner;
        
        var is1 = Intersections.FindIntersections(c1.Parts[2], c2.Parts[0]);
        Assert.Equal(2, is1.Count);
        
        Assert.True(is1[0].X>is1[1].X);
        Assert.True(is1[0].Y>is1[1].Y);
        
        var is2 = Intersections.FindIntersections(c2.Parts[0], c1.Parts[2]);
        Assert.Equal(2, is2.Count);
        
        Assert.True(is1[0].X<is2[1].X);
        Assert.True(is1[0].Y<is2[1].Y);
    }
}