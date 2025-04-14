using PcbReader.Layers.Gerber.Macro;

namespace PcbReader.Layers.Gerber.Entities.MacroApertures.Primitives;

public class VectorLine {
    public IExpression Exposure { get; set; }
    public IExpression Width { get; set; }
    public IExpression StartX { get; set; }
    public IExpression StartY { get; set; }
    public IExpression EndX { get; set; }
    public IExpression EndY { get; set; }
    public IExpression Rotation { get; set; }
}