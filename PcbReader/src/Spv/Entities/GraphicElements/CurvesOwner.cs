namespace PcbReader.Spv.Entities.GraphicElements;

public abstract class CurvesOwner: IGraphicElement {
    private Bounds? _bounds = new Bounds();

    public List<ICurve> Curves { get; } = [];
    
    public void UpdateBounds() {
        _bounds = null;
        foreach (var p in Curves)
            p.UpdateBounds();
    }
    
    public Bounds Bounds {
        get {
            if (_bounds == null) {
                foreach (var p in Curves) {
                    _bounds = _bounds?.ExtendBounds(p.Bounds) ?? p.Bounds;
                }
            }
            return _bounds!.Value;
        }
    }
}