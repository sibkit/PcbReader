namespace PcbReader.Layers.Svg.Entities;

public class SvgPath {
    public double StrokeWidth { get; set; }
    public List<ISvgPathPart> Parts { get; } = [];
    // public Point StartPoint { get; set; }
}