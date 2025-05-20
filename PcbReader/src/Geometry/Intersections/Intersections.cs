using PcbReader.Geometry.Intersections.IntersectionFinders;
using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections;

public static class Intersections {
    
    private static readonly ArcArcIntersectionsFinder ArcArcIntersectionsFinder = new ArcArcIntersectionsFinder();
    private static readonly LineLineIntersectionsFinder LineLineIntersectionsFinder = new LineLineIntersectionsFinder();
    private static readonly LineArcIntersectionsFinder LineArcIntersectionsFinder = new LineArcIntersectionsFinder();
    
    public static List<Point> FindIntersections(IPathPart pp1, IPathPart pp2) {
        if (pp1.Owner == pp2.Owner)
            throw new Exception("Intersections: FindIntersections (PathParts of one owner)");
        
        var intersections = pp1 switch {
            LinePathPart part1 when pp2 is LinePathPart part2 => LineLineIntersectionsFinder.FindIntersections( part1, part2, IntersectionsSorting.ByFirstPart),
            ArcPathPart part1 when pp2 is ArcPathPart part2 => ArcArcIntersectionsFinder.FindIntersections( part1, part2, IntersectionsSorting.ByFirstPart),
            ArcPathPart part1 when pp2 is LinePathPart part2 => LineArcIntersectionsFinder.FindIntersections(part2, part1, IntersectionsSorting.BySecondPart),
            LinePathPart part1 when pp2 is ArcPathPart part2 => LineArcIntersectionsFinder.FindIntersections( part1, part2, IntersectionsSorting.ByFirstPart),
            _ => throw new Exception("Intersector: FindIntersections => Cannot define segment(s)")
        };

        return intersections;
    }
    
}