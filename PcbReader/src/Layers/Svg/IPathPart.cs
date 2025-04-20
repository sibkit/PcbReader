using PcbReader.Project;

namespace PcbReader.Layers.Svg;

public interface IPathPart {
    Point EndPoint { get; set; }
}