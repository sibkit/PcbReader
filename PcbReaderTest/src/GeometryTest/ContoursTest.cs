using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;

namespace PcbReaderTest.GeometryTest;

public class ContoursTest {
    [Fact]
    public void ContourSplitTest() {
        var p1 = new Painter<Contour>(10, 10);
        p1.LineToInc(0, 60);
        p1.LineToInc(40, 0);
        p1.LineToInc(0, -60);
        p1.LineToInc(-40, 0);
        var c1 = p1.Root;
        
        var p2 = new Painter<Contour>(40, 30);
        p2.LineToInc(20, 20);
        p2.LineToInc(-30, 30);
        p2.LineToInc(50,0);
        p2.LineToInc(0,-50);
        p2.LineToInc(-40,0);
        var c2 = p2.Root;

        var cs1 = Contours.SplitByRelationPoints(c1, c2);
        Assert.True(cs1.Curves[1].PointTo == new Point(40,70));
        Assert.True(cs1.Curves[2].PointTo == new Point(50,70));
        Assert.True(cs1.Curves[3].PointTo == new Point(50,60));
        Assert.True(cs1.Curves[4].PointTo == new Point(50,40));
        Assert.True(cs1.Curves[5].PointTo == new Point(50,30));

        var cs2 = Contours.SplitByRelationPoints(c2, c1);
        Assert.True(cs2.Curves[0].PointTo == new Point(50,40));
        Assert.True(cs2.Curves[7].PointTo == new Point(50,30));
    }
}