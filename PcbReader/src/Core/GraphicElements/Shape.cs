namespace PcbReader.Core.GraphicElements;

public class Shape: IGraphicElement {
    
    public required Contour OuterContour { get; init; }
    public List<Contour> InnerContours { get; } = [];


    public Bounds GetBounds() {
        return OuterContour.GetBounds();
    }

    public void UpdateBounds() {
        OuterContour.UpdateBounds();
        foreach(var ic in InnerContours)
            ic.UpdateBounds();
    }
}