namespace PcbReader.Core;

public class Shape: IGraphicElement {
    
    public required Contour OuterContour { get; init; }
    public List<Contour> InnerContours { get; } = [];


    public Bounds GetBounds() {
        return OuterContour.GetBounds();
    }
}