using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections.IntersectionFinders;

public class LineArcIntersectionsFinder : IIntersectionsFinder<LinePathPart, ArcPathPart> {

    private static double CalculateX1(double p1X1, double p1Y1, double p1X2, double p1Y2, double p2Xc, double p2Yc, double p2R) {
        return (-p1X1 * p1Y2 +
            p1X1 * (p1X1*p1X1 * p1Y2 - p1X1 * p1X2 * p1Y1 - p1X1 * p1X2 * p1Y2 + p1X1 * p2Xc * p1Y1 - p1X1 * p2Xc * p1Y2 + p1X2*p1X2 * p1Y1 - p1X2 * p2Xc * p1Y1 + p1X2 * p2Xc * p1Y2 +
                   p1Y1*p1Y1 * p2Yc - 2 * p1Y1 * p1Y2 * p2Yc +
                   p1Y1 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                       2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                       p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                       2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                       2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                       2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2) + p1Y2*p1Y2 * p2Yc -
                   p1Y2 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                       2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                       p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                       2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                       2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                       2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2)) /
            (p1X1*p1X1 - 2 * p1X1 * p1X2 + p1X2*p1X2 + p1Y1*p1Y1 - 2 * p1Y1 * p1Y2 + p1Y2*p1Y2) + p1X2 * p1Y1 - p1X2 *
            (p1X1*p1X1 * p1Y2 - p1X1 * p1X2 * p1Y1 - p1X1 * p1X2 * p1Y2 + p1X1 * p2Xc * p1Y1 - p1X1 * p2Xc * p1Y2 + p1X2*p1X2 * p1Y1 - p1X2 * p2Xc * p1Y1 + p1X2 * p2Xc * p1Y2 + p1Y1*p1Y1 * p2Yc -
                2 * p1Y1 * p1Y2 * p2Yc +
                p1Y1 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                    2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                    p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                    2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                    2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                    2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2) + p1Y2*p1Y2 * p2Yc - p1Y2 *
                Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                    2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                    p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                    2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                    2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                    2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2)) /
            (p1X1*p1X1 - 2 * p1X1 * p1X2 + p1X2*p1X2 + p1Y1*p1Y1 - 2 * p1Y1 * p1Y2 + p1Y2*p1Y2)) / (p1Y1 - p1Y2);
    }

    private static double CalculateX2(double p1X1, double p1Y1, double p1X2, double p1Y2, double p2Xc, double p2Yc, double p2R) {
        return (-p1X1 * p1Y2 +
            p1X1 * (p1X1*p1X1 * p1Y2 - p1X1 * p1X2 * p1Y1 - p1X1 * p1X2 * p1Y2 + p1X1 * p2Xc * p1Y1 - p1X1 * p2Xc * p1Y2 + p1X2*p1X2 * p1Y1 - p1X2 * p2Xc * p1Y1 + p1X2 * p2Xc * p1Y2 +
                   p1Y1*p1Y1 * p2Yc - 2 * p1Y1 * p1Y2 * p2Yc -
                   p1Y1 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                       2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                       p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                       2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                       2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                       2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2) + p1Y2*p1Y2 * p2Yc +
                   p1Y2 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                       2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                       p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                       2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                       2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                       2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2)) /
            (p1X1*p1X1 - 2 * p1X1 * p1X2 + p1X2*p1X2 + p1Y1*p1Y1 - 2 * p1Y1 * p1Y2 + p1Y2*p1Y2) + p1X2 * p1Y1 - p1X2 *
            (p1X1*p1X1 * p1Y2 - p1X1 * p1X2 * p1Y1 - p1X1 * p1X2 * p1Y2 + p1X1 * p2Xc * p1Y1 - p1X1 * p2Xc * p1Y2 + p1X2*p1X2 * p1Y1 - p1X2 * p2Xc * p1Y1 + p1X2 * p2Xc * p1Y2 + p1Y1*p1Y1 * p2Yc -
                2 * p1Y1 * p1Y2 * p2Yc -
                p1Y1 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                    2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                    p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                    2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                    2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                    2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2) + p1Y2*p1Y2 * p2Yc + p1Y2 *
                Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                    2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                    p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc -
                    2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 +
                    2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc +
                    2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2)) /
            (p1X1*p1X1 - 2 * p1X1 * p1X2 + p1X2*p1X2 + p1Y1*p1Y1 - 2 * p1Y1 * p1Y2 + p1Y2*p1Y2)) / (p1Y1 - p1Y2);
    }

    private static double CalculateY1(double p1X1, double p1Y1, double p1X2, double p1Y2, double p2Xc, double p2Yc, double p2R) {
        return (p1X1*p1X1 * p1Y2 - p1X1 * p1X2 * p1Y1 - p1X1 * p1X2 * p1Y2 + p1X1 * p2Xc * p1Y1 - p1X1 * p2Xc * p1Y2 + p1X2*p1X2 * p1Y1 - p1X2 * p2Xc * p1Y1 + p1X2 * p2Xc * p1Y2 + p1Y1*p1Y1 * p2Yc -
                     2 * p1Y1 * p1Y2 * p2Yc +
                     p1Y1 * Math.Sqrt(p2R*p2R * p1X1*p1X1 - 2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 -
                         2 * p2R*p2R * p1Y1 * p1Y2 + p2R*p2R * p1Y2*p1Y2 - p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc -
                         p1X1*p1X1 * p2Yc*p2Yc +
                         2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc - 2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc +
                         2 * p1X1 * p2Xc * p1Y2*p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2*p1X2 * p1Y1*p1Y1 + 2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc +
                         2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc + 2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 +
                         2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2) + p1Y2*p1Y2 * p2Yc - p1Y2 * Math.Sqrt(p2R*p2R * p1X1*p1X1 -
                         2 * p2R*p2R * p1X1 * p1X2 + p2R*p2R * p1X2*p1X2 + p2R*p2R * p1Y1*p1Y1 - 2 * p2R*p2R * p1Y1 * p1Y2 +
                         p2R*p2R * p1Y2*p1Y2 -
                         p1X1*p1X1 * p1Y2*p1Y2 + 2 * p1X1*p1X1 * p1Y2 * p2Yc - p1X1*p1X1 * p2Yc*p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc -
                         2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc*p2Yc - 2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2*p1Y2 -
                         2 * p1X1 * p2Xc * p1Y2 * p2Yc -
                         p1X2*p1X2 * p1Y1*p1Y1 + 2 * p1X2*p1X2 * p1Y1 * p2Yc - p1X2*p1X2 * p2Yc*p2Yc + 2 * p1X2 * p2Xc * p1Y1*p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 -
                         2 * p1X2 * p2Xc * p1Y1 * p2Yc + 2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc*p2Xc * p1Y1*p1Y1 + 2 * p2Xc*p2Xc * p1Y1 * p1Y2 - p2Xc*p2Xc * p1Y2*p1Y2)) /
                 (p1X1*p1X1 - 2 * p1X1 * p1X2 + p1X2*p1X2 + p1Y1*p1Y1 - 2 * p1Y1 * p1Y2 + p1Y2*p1Y2);
    }

    private static double CalculateY2(double p1X1, double p1Y1, double p1X2, double p1Y2, double p2Xc, double p2Yc, double p2R) {
        return (p1X1 * p1X1 * p1Y2 - p1X1 * p1X2 * p1Y1 - p1X1 * p1X2 * p1Y2 + p1X1 * p2Xc * p1Y1 - p1X1 * p2Xc * p1Y2 + p1X2 * p1X2 * p1Y1 - p1X2 * p2Xc * p1Y1 + p1X2 * p2Xc * p1Y2 +
                   p1Y1 * p1Y1 * p2Yc -
                   2 * p1Y1 * p1Y2 * p2Yc -
                   p1Y1 * Math.Sqrt(p2R * p2R * p1X1 * p1X1 - 2 * p2R * p2R * p1X1 * p1X2 + p2R * p2R * p1X2 * p1X2 + p2R * p2R * p1Y1 * p1Y1 -
                       2 * p2R * p2R * p1Y1 * p1Y2 + p2R * p2R * p1Y2 * p1Y2 - p1X1 * p1X1 * p1Y2 * p1Y2 + 2 * p1X1 * p1X1 * p1Y2 * p2Yc -
                       p1X1 * p1X1 * p2Yc * p2Yc +
                       2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc - 2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc * p2Yc - 2 * p1X1 * p2Xc * p1Y1 * p1Y2 +
                       2 * p1X1 * p2Xc * p1Y1 * p2Yc +
                       2 * p1X1 * p2Xc * p1Y2 * p1Y2 - 2 * p1X1 * p2Xc * p1Y2 * p2Yc - p1X2 * p1X2 * p1Y1 * p1Y1 + 2 * p1X2 * p1X2 * p1Y1 * p2Yc - p1X2 * p1X2 * p2Yc * p2Yc +
                       2 * p1X2 * p2Xc * p1Y1 * p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 - 2 * p1X2 * p2Xc * p1Y1 * p2Yc + 2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc * p2Xc * p1Y1 * p1Y1 +
                       2 * p2Xc * p2Xc * p1Y1 * p1Y2 - p2Xc * p2Xc * p1Y2 * p1Y2) + p1Y2 * p1Y2 * p2Yc + p1Y2 * Math.Sqrt(p2R * p2R * p1X1 * p1X1 -
                       2 * p2R * p2R * p1X1 * p1X2 + p2R * p2R * p1X2 * p1X2 + p2R * p2R * p1Y1 * p1Y1 - 2 * p2R * p2R * p1Y1 * p1Y2 +
                       p2R * p2R * p1Y2 * p1Y2 -
                       p1X1 * p1X1 * p1Y2 * p1Y2 + 2 * p1X1 * p1X1 * p1Y2 * p2Yc - p1X1 * p1X1 * p2Yc * p2Yc + 2 * p1X1 * p1X2 * p1Y1 * p1Y2 - 2 * p1X1 * p1X2 * p1Y1 * p2Yc -
                       2 * p1X1 * p1X2 * p1Y2 * p2Yc + 2 * p1X1 * p1X2 * p2Yc * p2Yc - 2 * p1X1 * p2Xc * p1Y1 * p1Y2 + 2 * p1X1 * p2Xc * p1Y1 * p2Yc + 2 * p1X1 * p2Xc * p1Y2 * p1Y2 -
                       2 * p1X1 * p2Xc * p1Y2 * p2Yc -
                       p1X2 * p1X2 * p1Y1 * p1Y1 + 2 * p1X2 * p1X2 * p1Y1 * p2Yc - p1X2 * p1X2 * p2Yc * p2Yc + 2 * p1X2 * p2Xc * p1Y1 * p1Y1 - 2 * p1X2 * p2Xc * p1Y1 * p1Y2 -
                       2 * p1X2 * p2Xc * p1Y1 * p2Yc + 2 * p1X2 * p2Xc * p1Y2 * p2Yc - p2Xc * p2Xc * p1Y1 * p1Y1 + 2 * p2Xc * p2Xc * p1Y1 * p1Y2 - p2Xc * p2Xc * p1Y2 * p1Y2)) /
               (p1X1 * p1X1 - 2 * p1X1 * p1X2 + p1X2 * p1X2 + p1Y1 * p1Y1 - 2 * p1Y1 * p1Y2 + p1Y2 * p1Y2);
    }

    public List<Point> FindIntersections(LinePathPart part1, ArcPathPart part2, IntersectionsSorting sorting) {
        
        List<Point> result = [];
        
        var p1X1 = part1.PointFrom.X;
        var p1Y1 = part1.PointFrom.Y;
        var p1X2 = part1.PointTo.X;
        var p1Y2 = part1.PointTo.Y;
        var p2C = Geometry.ArcCenter(part2);
        var p2Xc = p2C.X;
        var p2Yc = p2C.Y;
        var p2R = part2.Radius;
        
        var x1 = CalculateX1(p1X1, p1Y1, p1X2, p1Y2, p2Xc, p2Yc, p2R);
        var x2 = CalculateX2(p1X1, p1Y1, p1X2, p1Y2, p2Xc, p2Yc, p2R);

        var y1 = CalculateY1(p1X1, p1Y1, p1X2, p1Y2, p2Xc, p2Yc, p2R);
        var y2 = CalculateY2(p1X1, p1Y1, p1X2, p1Y2, p2Xc, p2Yc, p2R);

        if (!double.IsNaN(x1)) {
            if (x1 >= p1X1 && x1 <= p1X2) {
                result.Add(new Point(x1, y1));
            }
        }
        
        if (!double.IsNaN(x2)) {
            if (x2 >= p1X1 && x2 <= p1X2) {
                result.Add(new Point(x2, y2));
            }
        }
        
        return result;
    }
}