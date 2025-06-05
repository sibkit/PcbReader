using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Entities.GraphicElements.Curves;
using PcbReader.Core.Relations.Intersections;

namespace PcbReader.Core.Relations;

public static class RelationManager {
    
    private static readonly CircleCirclePointsFinder CircleCircleIPF = new();
    private static readonly LineLinePointsFinder LineLineIPF = new();
    private static readonly LineCirclePointsFinder LineCircleIPF = new();
    
    public static IRelation DefineRelation(ICurve curve, ICurve baseCurve) {
        if (!curve.Bounds.IsIntersected(baseCurve.Bounds))
            return new NotRelation();
        
        var intersectionPoints = curve switch {
            Line part1 when baseCurve is Line part2 => LineLineIPF.FindContactPoints(part1, part2),
            Arc part1 when baseCurve is Arc part2 => CircleCircleIPF.FindContactPoints(part1, part2),
            Arc part1 when baseCurve is Line part2 => LineCircleIPF.FindContactPoints(part2, part1),
            Line part1 when baseCurve is Arc part2 => LineCircleIPF.FindContactPoints(part1, part2),
            _ => throw new Exception("Intersector: FindIntersections => Cannot define segment(s)")
        };

        var intersections = new List<ContactPoint>();
        foreach (var pt in intersectionPoints.points) {
            var t1 = TCalculator.CalculateT(curve, pt);
            var t2 = TCalculator.CalculateT(baseCurve, pt);
            if (t1 > 0 && t1 <= 1 && t2 > 0 && t2 <= 1) {
                intersections.Add(new ContactPoint {
                    //Part = pp1,
                    //SecondPart = pp2,
                    T = t1,
                    Point = pt,
                    BaseT = t2
                });
            }
        }
        
        if (intersections.Count == 0) 
            return new NotRelation();
        
        intersections.Sort((a, b) => a.T.CompareTo(b.T));
        return new IntersectionRelation {
            Points = intersections
        };
    }
}