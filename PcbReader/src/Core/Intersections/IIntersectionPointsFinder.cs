using PcbReader.Core.GraphicElements;

namespace PcbReader.Core.Intersections;

public interface IIntersectionPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    List<Point> FindIntersectionPoints(TF part1, TS part2);
}