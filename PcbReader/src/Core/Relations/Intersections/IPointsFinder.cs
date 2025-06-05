using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;

namespace PcbReader.Core.Relations.Intersections;

public interface IPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    (List<Point> points, bool isIntersection) FindContactPoints(TF curve1, TS curve2);
}