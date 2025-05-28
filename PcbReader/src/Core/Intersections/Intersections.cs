using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;
using PcbReader.Core.Intersections.IntersectionFinders;

namespace PcbReader.Core.Intersections;

public static class Intersections {
    
    private static readonly CircleWithCircleIntersectionPointsFinder CircleWithCircleIntersectionPointsFinder = new();
    private static readonly LineWithLineIntersectionPointsFinder LineWithLineIntersectionPointsFinder = new();
    private static readonly LineWithCircleIntersectionPointsFinder LineWithCircleIntersectionPointsFinder = new();
    
    public static List<Intersection> FindIntersections(IPathPart pp1, IPathPart pp2) {
        // if (pp1.Owner == pp2.Owner)
        //     throw new Exception("Intersections: FindIntersections (PathParts of one owner)");
        
        var intersectionPoints = pp1 switch {
            LinePathPart part1 when pp2 is LinePathPart part2 => LineWithLineIntersectionPointsFinder.FindIntersectionPoints(part1, part2),
            ArcPathPart part1 when pp2 is ArcPathPart part2 => CircleWithCircleIntersectionPointsFinder.FindIntersectionPoints(part1, part2),
            ArcPathPart part1 when pp2 is LinePathPart part2 => LineWithCircleIntersectionPointsFinder.FindIntersectionPoints(part2, part1),
            LinePathPart part1 when pp2 is ArcPathPart part2 => LineWithCircleIntersectionPointsFinder.FindIntersectionPoints(part1, part2),
            _ => throw new Exception("Intersector: FindIntersections => Cannot define segment(s)")
        };

        var intersections = new List<Intersection>();
        foreach (var pt in intersectionPoints) {
            var t1 = TCalculator.CalculateT(pp1, pt);
            var t2 = TCalculator.CalculateT(pp2, pt);
            if (t1 > 0 && t1 <= 1 && t2 > 0 && t2 <= 1) {
                intersections.Add(new Intersection {
                    Part = pp1,
                    SecondPart = pp2,
                    T = t1,
                    Point = pt
                });
            }
        }
        
        intersections.Sort((a, b) => a.T.CompareTo(b.T));
        return intersections;
    }
}