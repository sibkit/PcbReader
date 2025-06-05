namespace PcbReader.Core.Entities.GraphicElements;

public class Shape: IGraphicElement {
    
    public required Contour OuterContour { get; init; }
    public List<Contour> InnerContours { get; } = [];


    public Bounds Bounds => OuterContour.Bounds;

    public void UpdateBounds() {
        OuterContour.UpdateBounds();
        foreach(var ic in InnerContours)
            ic.UpdateBounds();
    }
}