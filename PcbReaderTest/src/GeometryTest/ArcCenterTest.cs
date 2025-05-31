using PcbReader.Core;
using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReaderTest.GeometryTest;

public class ArcCenterTest {
    [Fact]
    public void TestArcCenter() {
        var arc = new ArcPathPart {
            PointFrom = new Point(0, 0),
            PointTo = new Point(5, 5),
            IsLargeArc = true,
            Radius = 5,
            RotationDirection = RotationDirection.Clockwise
        };
        var cp = Geometry.ArcCenter(arc);
        Assert.True(cp == new Point(0,5));
    }
}