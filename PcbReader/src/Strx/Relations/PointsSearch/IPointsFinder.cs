using PcbReader.Strx.Entities;
using PcbReader.Strx.Entities.GraphicElements;

namespace PcbReader.Strx.Relations.PointsSearch;

public interface IPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    (List<Point> points, bool isMatch) FindContactPoints(TF curve1, TS curve2);
}