using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;
using PcbReader.Spv.Entities.GraphicElements.Curves;

namespace PcbReader.Spv.Handling;

public static class Curves {
    public static Vector GetCurveInVector(ICurve curve) {

        switch (curve) {
            case Line line:
                return new Vector(line.PointTo.X - line.PointFrom.X, line.PointTo.Y - line.PointFrom.Y);
            case Arc arc:
                var cp = Geometry.ArcCenter(arc);
                var vecA = new Vector(arc.PointFrom.X - cp.X, arc.PointFrom.Y - cp.Y);
                var qA = Quadrants.GetQuadrant(vecA.X, vecA.Y);

                double cwbX = 0;

                switch (qA) {
                    case Quadrant.I:
                    case Quadrant.II:
                        cwbX = 1;
                        break;
                    case Quadrant.III:
                    case Quadrant.IV:
                        cwbX = -1;
                        break;
                    case Quadrant.I_II:
                        return arc.RotationDirection == RotationDirection.Clockwise ? new Vector(1, 0) : new Vector(-1, 0);
                    case Quadrant.II_III:
                        return arc.RotationDirection == RotationDirection.Clockwise ? new Vector(0, 1) : new Vector(0, -1);
                    case Quadrant.III_IV:
                        return arc.RotationDirection == RotationDirection.Clockwise ? new Vector(-1, 0) : new Vector(1, 0);
                    case Quadrant.IV_I:
                        return arc.RotationDirection == RotationDirection.Clockwise ? new Vector(0, -1) : new Vector(0, 1);
                }

                var bx = arc.RotationDirection == RotationDirection.Clockwise ? cwbX! : -cwbX!;
                var by = -1d * vecA.X * bx / vecA.Y;
                return new Vector(bx, by);
            default:
                throw new Exception("Unknown curve type");

        }
    }
}