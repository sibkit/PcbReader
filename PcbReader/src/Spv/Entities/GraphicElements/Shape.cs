using PcbReader.Spv.Handling;

namespace PcbReader.Spv.Entities.GraphicElements;

public class Shape: IGraphicElement {

    public List<Contour> OuterContours { get; } = [];
    public List<Contour> InnerContours { get; } = [];


    public Shape(Contour contour) {
        OuterContours.Add(contour);
    }
    
    
    private Bounds? _bounds;
    public Bounds Bounds {
        get {
            if (_bounds == null) {
                _bounds = Bounds.Empty();
                foreach (var contour in OuterContours) {
                    _bounds = Bounds.ExtendBounds(contour.Bounds);
                }
            }
            return _bounds.Value;
        }
    }

    public void NormalizeRotation() {
        foreach (Contour contour in OuterContours) {
            var rd = Contours.GetRotationDirection(contour);
            if (rd == RotationDirection.CounterClockwise) {
                
            }
        }
    }
    
    public void UpdateBounds() {
        _bounds = null;
        foreach(var ic in InnerContours)
            ic.UpdateBounds();
        foreach(var ic in OuterContours)
            ic.UpdateBounds();
    }
}