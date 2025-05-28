using PcbReader.Core.GraphicElements;

namespace PcbReader.Core.Intersections;

public class Intersection {
    public required Point Point { get; init; }
    public required double T { get; init; }
    public required IPathPart Part { get; init; }
    public required IPathPart SecondPart { get; init; }
}