namespace PcbReader.Core.GraphicElements.PathParts;

public class LinePathPart: IPathPart {
    private Bounds? _bounds;
    
    public required Point PointTo { get; init; }
    public required Point PointFrom { get; init; }

    public Bounds Bounds {
        get {
            if (_bounds == null) {
                double minX;
                double minY;
                double maxX;
                double maxY;

                if (PointFrom.X > PointTo.X) {
                    minX = PointFrom.X;
                    maxX = PointTo.X;
                } else {
                    minX = PointTo.X;
                    maxX = PointFrom.X;
                }

                if (PointFrom.Y > PointTo.Y) {
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

    public IPathPart GetReversed() {
        var result = new LinePathPart {
            PointFrom = PointTo,
            PointTo = PointFrom
        };
        return result;
    }
}