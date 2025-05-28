namespace PcbReader.Core.GraphicElements;

public struct Dot: IGraphicElement {
    public Point CenterPoint{get;init;}
    public double Diameter{get;init;}
    public Bounds GetBounds() {
        return new Bounds(CenterPoint.X-Diameter/2, CenterPoint.Y-Diameter/2, CenterPoint.X+Diameter/2, CenterPoint.Y+Diameter/2);
    }
}