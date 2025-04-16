using PcbReader.Layers.Gerber.Macro;

namespace PcbReader.Layers.Gerber.Entities.MacroApertures.Primitives;

public class Circle: IPrimitive {
    public IExpression Exposure { get; set; }
    public IExpression Diameter { get; set; }
    public IExpression CenterX { get; set; }
    public IExpression CenterY { get; set; }
    public IExpression Rotation { get; set; }
}