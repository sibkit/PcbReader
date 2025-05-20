namespace PcbReader.Geometry.Intersections;

public interface IIntersectionsFinder<in TF, in TS> 
    where TF : IPathPart
    where TS: IPathPart {
    List<Point> FindIntersections(TF part1, TS part2, IntersectionsSorting sorting);
}