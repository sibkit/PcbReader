using System.Runtime.InteropServices.ComTypes;
using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Handling;

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

        var cs1 = ContoursHandler.SplitByRelationPoints(c1, c2);
        Assert.True(cs1.Curves[1].PointTo == new Point(40,70));
        Assert.True(cs1.Curves[2].PointTo == new Point(50,70));
        Assert.True(cs1.Curves[3].PointTo == new Point(50,60));
        Assert.True(cs1.Curves[4].PointTo == new Point(50,40));
        Assert.True(cs1.Curves[5].PointTo == new Point(50,30));

        var cs2 = ContoursHandler.SplitByRelationPoints(c2, c1);
        Assert.True(cs2.Curves[0].PointTo == new Point(50,40));
        Assert.True(cs2.Curves[7].PointTo == new Point(50,30));
    }


    [Fact]
    public void RoundPointTest() {
        var r = new Random();
        var x1 = r.NextDouble();
        var x2 = x1 + 0.000_000_000_001;
        
        var y1 = r.NextDouble();
        var y2 = y1 + 0.000_000_000_001;
        
        var p1 = ContoursHandler.RoundPoint(new Point(x1,y1));
        var p2 = ContoursHandler.RoundPoint(new Point(x2,y2));
        var p3 = ContoursHandler.RoundPoint(new Point(x1 + 0.000_000_000_1,y2 + 0.000_000_000_1));
        Assert.Equal(p1,p2);
        Assert.NotEqual(p1,p3);
    }
}