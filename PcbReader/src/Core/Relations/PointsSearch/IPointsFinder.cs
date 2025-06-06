using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;

namespace PcbReader.Core.Relations.PointsSearch;

public interface IPointsFinder<in TF, in TS> 
    where TF : ICurve
    where TS: ICurve {
    (List<Point> points, bool isMatch) FindContactPoints(TF curve1, TS curve2);
}