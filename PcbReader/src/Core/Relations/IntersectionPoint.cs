using PcbReader.Core.Entities;

namespace PcbReader.Core.Location;

public class IntersectionPoint {
    public required Point Point { get; init; }
    public required double T { get; init; }
    public required double BaseT { get; init; }
}