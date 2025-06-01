using PcbReader.Core;


namespace PcbReaderTest.GeometryTest;

public class CalculateAngleTest {

    void AssertWithAccuracy(double val1, double val2) {
        Assert.True(Math.Abs(val1-val2)<Geometry.Accuracy);
    }

    [Fact]
    public void TestCalculateAngle() {
        
        var sp = new Point(10, 10);
        var ep = new Point(20, 20);
        var cp = new Point(20, 10);
        var a1 = Geometry.CalculateAngle(sp, ep, cp);
        AssertWithAccuracy(Geometry.NegativeNormalize(a1), -Math.PI/2);

        var a2 = Geometry.CalculateAngle(
            new Point(10, 20),
            new Point(10, 0),
            new Point(10, 10)
        );
        AssertWithAccuracy(Geometry.PositiveNormalize(a2), Math.PI);
        
        var a3 = Geometry.CalculateAngle(
            new Point(10, 10),
            new Point(10, -10),
            new Point(0, 0)
        );
        AssertWithAccuracy(a3, -Math.PI/2);
        
        var a4 = Geometry.CalculateAngle(
            new Point(10, -10),
            new Point(10, 10),
            new Point(0, 0)
        );
        AssertWithAccuracy(Geometry.PositiveNormalize(a4), Math.PI/2);
        
        var a5 = Geometry.CalculateAngle(
            new Point(10, -10),
            new Point(10, 10),
            new Point(0, 0)
        );
        AssertWithAccuracy(Geometry.PositiveNormalize(a5), Math.PI/2);
        
    }
}