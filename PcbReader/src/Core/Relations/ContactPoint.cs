using PcbReader.Core.Entities;

namespace PcbReader.Core.Relations;

public class ContactPoint {
    public required Point Point { get; init; }
    public required double T { get; init; }
    public required double BaseT { get; init; }
}