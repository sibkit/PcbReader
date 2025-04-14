using PcbReader.Layers.Gerber.Macro;

namespace PcbReader.Layers.Gerber.Entities.MacroApertures.Primitives;

public class Outline {
    public IExpression Exposure { get; set; }
    public IExpression Vertices { get; set; }
    public IExpression StartX { get; set; }
    public IExpression StartY { get; set; }
    public IExpression Rotation { get; set; }
}