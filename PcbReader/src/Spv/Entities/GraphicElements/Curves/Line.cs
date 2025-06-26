namespace PcbReader.Spv.Entities.GraphicElements.Curves;

public class Line: ICurve {
    private Bounds? _bounds;
    
    public CurvesOwner Owner { get; set; }
    
    public required Point PointTo { get; set; }
    
    public required Point PointFrom { get; set; }

    public void UpdateBounds() {
        _bounds = null;
    }
    
    public Bounds Bounds {
        get {
            if (_bounds == null) {
                double minX;
                double minY;
                double maxX;
                double maxY;

                if (PointFrom.X < PointTo.X) {
                    minX = PointFrom.X;
                    maxX = PointTo.X;
                } else {
                    minX = PointTo.X;
                    maxX = PointFrom.X;
                }

                if (PointFrom.Y < PointTo.Y) {
                    minY = PointFrom.Y;
                    maxY = PointTo.Y;
                } else {
                    minY = PointTo.Y;
                    maxY = PointFrom.Y;
                }

                _bounds = new Bounds(minX, minY,maxX, maxY);
            }
            return _bounds.Value;
        }
    }

    public ICurve GetReversed() {
        var result = new Line {
            PointFrom = PointTo,
            PointTo = PointFrom
        };
        return result;
    }

    public void Reverse() {
        (PointFrom, PointTo) = (PointTo, PointFrom);
    }

    public object Clone() {
        return MemberwiseClone();
    }
}