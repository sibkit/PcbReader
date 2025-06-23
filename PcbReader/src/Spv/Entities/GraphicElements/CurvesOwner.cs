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
                    if (_bounds == null) {
                        _bounds = p.Bounds;
                    } else {
                        _bounds = new Bounds(
                            p.Bounds.MinX < _bounds.Value.MinX ? p.Bounds.MinX : _bounds.Value.MinX,
                            p.Bounds.MinY < _bounds.Value.MinY ? p.Bounds.MinY : _bounds.Value.MinY,
                            p.Bounds.MaxX > _bounds.Value.MaxX ? p.Bounds.MaxX : _bounds.Value.MaxX,
                            p.Bounds.MaxY > _bounds.Value.MaxY ? p.Bounds.MaxY : _bounds.Value.MaxY
                        );
                    }
                }
            }

            return _bounds!.Value;
        }
    }
}