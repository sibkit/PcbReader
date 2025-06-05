using System.Diagnostics;
using System.Runtime.Serialization;
using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Location;
using PcbReader.Core.Location.Intersections;

namespace PcbReaderTest.GeometryTest.IntersectionsTest;

public class IntersectionsFinderTest {
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

        var r1 = RelationManager.DefineRelation(c1.Curves[2], c2.Curves[0]);

        if (r1 is IntersectionRelation ir1) {
            Assert.Equal(2, ir1.Items.Count);

            Assert.True(ir1.Items[0].Point.X > ir1.Items[1].Point.X);
            Assert.True(ir1.Items[0].Point.Y > ir1.Items[1].Point.Y);
        } else {
            Assert.Fail("Пересечения не найдены");
        }

        var r2 = RelationManager.DefineRelation(c2.Curves[0], c1.Curves[2]);
        
        if (r2 is IntersectionRelation ir2) {
            Assert.Equal(2, ir2.Items.Count);

            Assert.True(ir2.Items[0].Point.X < ir2.Items[1].Point.X);
            Assert.True(ir2.Items[0].Point.Y < ir2.Items[1].Point.Y);
        } else {
            Assert.Fail("Пересечения не найдены");
        }
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

        var r1 = RelationManager.DefineRelation(c1.Curves[0], c2.Curves[3]);
        if (r1 is IntersectionRelation ir1) {

            Assert.Single(ir1.Items);
            Assert.True(Math.Abs(ir1.Items[0].Point.X - 12d) < Geometry.Accuracy && Math.Abs(ir1.Items[0].Point.Y - 12d) < Geometry.Accuracy);
        } else {
            Assert.Fail("Пересечения не найдены");
        }

        if (r1 is IntersectionRelation ir2) {
            Assert.Single(ir2.Items);
            Assert.True(Math.Abs(ir2.Items[0].Point.X - 12) < Geometry.Accuracy && Math.Abs(ir2.Items[0].Point.Y - 12) < Geometry.Accuracy);
        } else {
            Assert.Fail("Пересечения не найдены");
        }


        
        Assert.True(RelationManager.DefineRelation(c1.Curves[0], c1.Curves[2]) is NotRelation);
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
        
        var r1 = RelationManager.DefineRelation(c1.Curves[0], c2.Curves[2]);
        if(r1 is not IntersectionRelation)
            Assert.Fail("Пересечения не найдены");
        var ir1 = r1 as IntersectionRelation;
        //var is2 = IntersectionsFinder.FindIntersections(c1.Curves[0], c2.Curves[2]);
        Debug.Assert(ir1?.Items != null, "ir1?.Items != null");
        Assert.Single(ir1.Items);
        Assert.True(Math.Abs(ir1.Items[0].Point.X - 29.85) < 0.01);
    }

}