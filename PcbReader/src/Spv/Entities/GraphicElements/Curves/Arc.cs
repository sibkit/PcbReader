using PcbReader.Spv.Handling;

namespace PcbReader.Spv.Entities.GraphicElements.Curves;



public class Arc : ICurve {
    private Bounds? _bounds;

    public CurvesOwner Owner { get; set; }
    
    public required Point PointTo { get; set; }
    public required Point PointFrom { get; set; }

    
    public required double Radius { get; init; }
    public required bool IsLargeArc { get; set; } = false;
    public required RotationDirection RotationDirection { get; set; } = 0;
    
    
    private static Quadrant GetQuadrant(Point centerPoint, Point point) {
        if (centerPoint.Y <= point.Y) {
            return point.X <= centerPoint.X ? Quadrant.II : Quadrant.I;
        }
        return point.X <= centerPoint.X ? Quadrant.III : Quadrant.IV;
    }

    public void UpdateBounds() {
        _bounds = null;
    }
    
    public Bounds Bounds {
        get {

            if (_bounds == null) {
                var cp = Geometry.ArcCenter(this);

                var sp = PointFrom;
                var ep = PointTo;

                var spq = GetQuadrant(cp,sp);
                var epq = GetQuadrant(cp, ep);

                var qts = Quadrants.GetTransitions(spq, epq, RotationDirection);
                
                double minX, minY, maxX, maxY;

                if ((qts & QuadrantTransition.I_II) == QuadrantTransition.I_II) {
                    maxY = cp.Y + Radius;
                } else {
                    maxY = sp.Y > ep.Y ? sp.Y : ep.Y;
                }
                
                if ((qts & QuadrantTransition.II_III) == QuadrantTransition.II_III) {
                    minX = cp.X - Radius;
                } else {
                    minX = sp.X < ep.X ? sp.X : ep.X;
                }
                
                if ((qts & QuadrantTransition.III_IV) == QuadrantTransition.III_IV) {
                    minY = cp.Y - Radius;
                } else {
                    minY = sp.Y < ep.Y ? sp.Y : ep.Y;
                }

                if ((qts & QuadrantTransition.IV_I) == QuadrantTransition.IV_I) {
                    maxX = cp.X + Radius;
                }
                else {
                    maxX = sp.X > ep.X ? sp.X : ep.X;
                }

                _bounds = new Bounds(minX, minY,maxX, maxY);   
            }
            return _bounds.Value;
        }
    }

    public ICurve GetReversed() {
        var result = new Arc {
            PointTo = PointFrom,
            PointFrom = PointTo,
            Radius = Radius,
            IsLargeArc = IsLargeArc,
            RotationDirection = RotationDirection.Invert()
        };
        return result;
    }


    public object Clone() {
        return MemberwiseClone();
    }
}



