using System.Runtime.Serialization;
using PcbReader.Core;
using PcbReader.Core.GraphicElements;
using PcbReader.Core.Intersections;
using PcbReader.Core.PathEdit;

namespace PcbReaderTest.GeometryTest.IntersectionsTest;

public class IntersectionsTest {
    [Fact]
    public void TestArcArcIntersection() {
        var p1 = new Painter<Contour>(10,10);
        p1.LineToInc(0,30);
        p1.LineToInc(30,0);
        p1.ArcToInc(-30,-30,30,RotationDirection.Clockwise,false);
        var c1 = p1.Root;
        
        var p2 = new Painter<Contour>(20,10);
        p2.ArcToInc(20,20,20,RotationDirection.Clockwise,false);
        p2.LineToInc(0,-20);
        p2.LineToInc(-20,0);
        var c2 = p2.Root;
        
        var is1 = Intersections.FindIntersections(c1.Curves[2], c2.Curves[0]);
        Assert.Equal(2, is1.Count);
        
        Assert.True(is1[0].Point.X>is1[1].Point.X);
        Assert.True(is1[0].Point.Y>is1[1].Point.Y);
        
        var is2 = Intersections.FindIntersections(c2.Curves[0], c1.Curves[2]);
        Assert.Equal(2, is2.Count);
        
        Assert.True(is2[0].Point.X<is2[1].Point.X);
        Assert.True(is2[0].Point.Y<is2[1].Point.Y);
    }

    [Fact]
    public void TestLineLineIntersections() {
        var p1 = new Painter<Contour>(10,20);
        p1.LineToInc(10,-40);
        p1.LineToInc(-40,-10);
        p1.LineToInc(-10,40);
        p1.LineToInc(40,10);
        var c1 = p1.Root;
        
        var p2 = new Painter<Contour>(20,10);
        p2.LineToInc(-10,-40);
        p2.LineToInc(-40,10);
        p2.LineToInc(10,40);
        p2.LineToInc(40,-10);
        var c2 = p2.Root;

        var is1 = Intersections.FindIntersections(c1.Curves[0], c2.Curves[3]);
        Assert.Single(is1);
        Assert.True(Math.Abs(is1[0].Point.X - 12d) < Geometry.Accuracy && Math.Abs(is1[0].Point.Y - 12d) < Geometry.Accuracy);
        
        var is2 = Intersections.FindIntersections(c1.Curves[0], c2.Curves[0]);
        Assert.Single(is2);
        Assert.True(Math.Abs(is2[0].Point.X - 16.25) < Geometry.Accuracy && Math.Abs(is2[0].Point.Y - (-5d)) < Geometry.Accuracy);
        
        Assert.Empty(Intersections.FindIntersections(c1.Curves[0], c1.Curves[2]));
    }

    [Fact]
    public void TestLineArcIntersections() {
        var p1 = new Painter<Contour>(10,10);
        p1.ArcToInc(20,0,10,RotationDirection.Clockwise,false);
        p1.ArcToInc(-20,0,10,RotationDirection.Clockwise,false);
        var c1 = p1.Root;
        
        var p2 = new Painter<Contour>(5,10);
        p2.LineToInc(0,5);
        p2.LineToInc(15,0);
        p2.LineToInc(15,-5);
        p2.LineToInc(-30,0);
        var c2 = p2.Root;
        
        var is2 = Intersections.FindIntersections(c1.Curves[0], c2.Curves[2]);
        Assert.Single(is2);
        Assert.True(Math.Abs(is2[0].Point.X - 29.85) < 0.01);
    }

}