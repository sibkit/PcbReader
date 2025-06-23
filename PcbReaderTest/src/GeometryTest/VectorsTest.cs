using PcbReader.Spv;

namespace PcbReaderTest.GeometryTest;

public class VectorsTest {
    [Fact]
    public void TestVectorsAngle() {
        Assert.Equal(Math.PI/2d, Vectors.Angle(new Vector(0,10), new Vector(10,0)));
    }
}