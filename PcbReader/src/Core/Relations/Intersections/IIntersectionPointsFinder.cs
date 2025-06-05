using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;

namespace PcbReader.Core.Location.Intersections;

public interface IIntersectionPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    List<Point> FindIntersectionPoints(TF part1, TS part2);
}