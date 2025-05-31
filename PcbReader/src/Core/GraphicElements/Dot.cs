namespace PcbReader.Core.GraphicElements;

public class Dot: IGraphicElement {
    public Point CenterPoint{get; set; }
    public double Diameter{get; init; }
    public Bounds GetBounds() {
        return new Bounds(CenterPoint.X-Diameter/2, CenterPoint.Y-Diameter/2, CenterPoint.X+Diameter/2, CenterPoint.Y+Diameter/2);
    }

    public void UpdateBounds() { }
}