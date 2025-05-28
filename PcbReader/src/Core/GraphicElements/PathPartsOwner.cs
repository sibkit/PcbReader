namespace PcbReader.Core;

public abstract class PathPartsOwner: IGraphicElement {
    private Bounds? _bounds = new Bounds();
    
    public List<IPathPart> Parts { get; }
    public Point StartPoint { get; set; }
    
    public void UpdateBounds() {
        foreach (var p in Parts) {
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

    public Bounds GetBounds() {
        if(_bounds==null)
            throw new Exception("Contour: GetBounds");
        return _bounds!.Value;
    }
}