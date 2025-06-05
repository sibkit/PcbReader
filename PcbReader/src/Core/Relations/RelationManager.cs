using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Entities.GraphicElements.Curves;
using PcbReader.Core.Location.Intersections.IntersectionFinders;

namespace PcbReader.Core.Location;

public static class RelationManager {
    
    private static readonly CircleWithCircleIntersectionPointsFinder CircleCircleIPF = new();
    private static readonly LineWithLineIntersectionPointsFinder LineLineIPF = new();
    private static readonly LineWithCircleIntersectionPointsFinder LineCircleIPF = new();
    
    public static IRelation DefineRelation(ICurve curve, ICurve baseCurve) {
        if (!curve.Bounds.IsIntersected(baseCurve.Bounds))
            return new NotRelation();
        
        var intersectionPoints = curve switch {
            Line part1 when baseCurve is Line part2 => LineLineIPF.FindIntersectionPoints(part1, part2),
            Arc part1 when baseCurve is Arc part2 => CircleCircleIPF.FindIntersectionPoints(part1, part2),
            Arc part1 when baseCurve is Line part2 => LineCircleIPF.FindIntersectionPoints(part2, part1),
            Line part1 when baseCurve is Arc part2 => LineCircleIPF.FindIntersectionPoints(part1, part2),
            _ => throw new Exception("Intersector: FindIntersections => Cannot define segment(s)")
        };

        var intersections = new List<IntersectionPoint>();
        foreach (var pt in intersectionPoints) {
            var t1 = TCalculator.CalculateT(curve, pt);
            var t2 = TCalculator.CalculateT(baseCurve, pt);
            if (t1 > 0 && t1 <= 1 && t2 > 0 && t2 <= 1) {
                intersections.Add(new IntersectionPoint {
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
            Items = intersections
        };
    }
}