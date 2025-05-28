namespace PcbReader.Core.Intersections;

public interface IIntersectionPointsFinder<in TF, in TS> 
    where TF : IPathPart
    where TS: IPathPart {
    List<Point> FindIntersectionPoints(TF part1, TS part2);
}