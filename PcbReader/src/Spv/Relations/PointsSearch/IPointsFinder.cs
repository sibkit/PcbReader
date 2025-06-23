using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;

namespace PcbReader.Spv.Relations.PointsSearch;

public interface IPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    (List<Point> points, bool isMatch) FindContactPoints(TF curve1, TS curve2);
}