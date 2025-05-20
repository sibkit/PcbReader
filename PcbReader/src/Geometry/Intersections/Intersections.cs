using PcbReader.Geometry.PathParts;

namespace PcbReader.Geometry.Intersections;

public static class Intersections {
    
    public static List<Point> FindIntersections(IPathPart pp1, IPathPart pp2) {
        if (pp1.Owner == pp2.Owner)
            throw new Exception("Intersections: FindIntersections (PathParts of one owner)");
        
        var intersections = pp1 switch {
            LinePathPart part when pp2 is LinePathPart pathPart => FindLinesIntersections( part, pathPart),
            ArcPathPart part when pp2 is ArcPathPart pathPart => FindArcsIntersections(part, pathPart),
            ArcPathPart part when pp2 is LinePathPart pathPart => FindLineArcIntersections(pathPart, part),
            LinePathPart part when pp2 is ArcPathPart pathPart => FindLineArcIntersections(part, pathPart),
            _ => throw new Exception("Intersector: FindIntersections => Cannot define segment(s)")
        };

        var result = new List<Point>();
        var s1b = PathPartBounds.GetBounds(pp1);
        var s2b = PathPartBounds.GetBounds(pp2);
        foreach (var intersection in intersections) {
            if (!s1b.Contains(intersection))
                continue;
            if (!s2b.Contains(intersection))
                continue;
            result.Add(intersection);
        }

        //var result = intersections.Where(isp => isp != p2 and PathPartBounds.GetBounds()).ToList();
        return result;
    }

    private static List<Point> FindLinesIntersections(LinePathPart sa, LinePathPart sb) {

        var result = new List<Point>();

        var ax1 = sa.PointFrom.X;
        var ay1 = sa.PointFrom.Y;
        var ax2 = sa.PointTo.X;
        var ay2 = sa.PointTo.Y;

        var bx1 = sb.PointFrom.X;
        var by1 = sb.PointFrom.Y;
        var bx2 = sb.PointTo.X;
        var by2 = sb.PointTo.Y;

        var dax = ax2 - ax1;
        var day = ay2 - ay1;
        var dbx = bx2 - bx1;
        var dby = by2 - by1;

        var da = dax / day;
        var ha = da * ay1 - ax1;

        var db = dbx / dby;
        var hb = db * by1 - bx1;

        var x = (da * hb / db - ha) / (1 - da / db);
        var y = (x + hb) / db;

        if (x >= ax1 && x >= bx1 && x < ax2 && x < bx2 && y >= ay1 && y >= by1 && y < ay2 && y < by2)
            result.Add(new Point(x, y));

        return result;
    }

    private static List<Point> FindLineArcIntersections(LinePathPart sa,  ArcPathPart sb) {
        var result = new List<Point>();
        var cp = Geometry.ArcCenter(sb);

        var ax1 = sb.PointFrom.X;
        var ay1 = sb.PointFrom.Y;
        var ax2 = sa.PointTo.X;
        var ay2 = sa.PointTo.Y;
        
        var x0 = cp.X;
        var y0 = cp.Y;
        var r = sb.Radius;
        
        var da = (ax2 - ax1) / (ay2 - ay1);
        
        var k = 1 / da;
        var b = ay1 - da / ax1;
        
        var a1 = 1 + k * k;
        var b1 = -2 * x0 + 2 * k * b - 2 * k * y0;
        var c1 = b * b + x0 * x0 - 2 * b * x0 + y0 * y0 - r * r;
        
        //a1*x^2+b1*x+c1=0;
        var d = b1 * b1 - 4 * a1 * c1;
        var x1 = (-b1 + Math.Sqrt(d))/2*a1;
        var x2 = (-b1 - Math.Sqrt(d))/2*a1;

        var y1 = k * x1 + b;
        var y2 = k + x2 + b;
        
        var p1 = new Point(x1, y1);
        var p2 = new Point(x2, y2);

        if (p1 == p2)
            return [p1];

        return [p1, p2];
    }

    private static List<Point> FindArcsIntersections(ArcPathPart sa, ArcPathPart sb) {

        if (Math.Abs(sa.PointFrom.X - sb.PointFrom.X) < Geometry.Accuracy && Math.Abs(sa.PointFrom.Y - sb.PointFrom.Y) < Geometry.Accuracy) {
            return [];
        }
        
        var c1 = Geometry.ArcCenter(sa);
        var c2 = Geometry.ArcCenter(sb);
        
        var r1 = sa.Radius;
        var r2 = sb.Radius;

        var result = Math.Abs(c1.X - c2.X) > Geometry.Accuracy ? 
            FindCirclesIntersectionsByX(c1, r1, c2, r2) : 
            FindCirclesIntersectionsByY(c1, r1, c2, r2);
        
        return result;
    }

    private static List<Point> FindCirclesIntersectionsByY(Point c1, double r1, Point c2, double r2) {
        //[ 1] (x-xc1)^2+(y-yc1)^2 = r1^2
        //[ 2] (x-xc2)^2+(y-yc2)^2 = r2^2
        //-----------------------------------
        //смещаем ось координат на (xc1, yc1), потом вернем
        //[ 3] x^2+y^2 = r1^2
        //[ 4] (x-(xc2-xc1))^2+(y-(yc2-yc1))^2 = r2^2;
        //[ 5] dxc=xc2-xc1; dyc=yc2-yc1; //вводим упрощения dxc и dyc
        //[ 6] (x-dxc)^2+(y-dyc)^2 = r2^2; //переписываем с учетом упрощения

        //[ 7] x^2 - 2*x*dxc + dxc^2 + y^2 - 2*y*dyc + dyc^2 = r2^2; //раскрываем скобки
        //[ 8] -2*x*dxc + dxc^2 - 2*y*dyc + dyc^2 = r2^2 - r1^2; //[7]-[3]
        //[ 9] x*dxc + y*dyc = (r2^2 - dxc^2 - dyc^2 - r1^2)/-2; //оставляем переменные в левой части
        //[10] f1 = (r2^2 - dxc^2 - dyc^2 - r1^2)/-2; //вводим упрощение f1

        var dxc = c2.X - c1.X;
        var dyc = c2.Y - c1.Y;
        var f1 = (r2 * r2 - dxc * dxc - dyc * dyc - r1 * r1) / -2;

        //[11] x*dxc + y*dyc = f1; // [9] с учетом упрощения f1
        //[12] y = (f1 - x*dxc)/dyc; //выделяем y
        //[13] x^2+((f1-x*dxc)/dyc)^2 = r1^2; //подставляем [12] в [3]
        //[14] x^2 + (f1^2 - 2*f1*x*dxc + x^2*dxc^2)/dyc^2 = r1^2; //раскрываем скобки
        //[15] x^2*dyc^2 + f1^2 - 2*f1*x*dxc + x^2*dxc^2 = r1^2*dyc^2; //умножаем на dyc^2
        //[16] x^2 * (dyc^2 + dxc^2) + x * (-2 * f1 * dxc) + f1^2 - r1^2 * dyc^2;
        // a = dyc^2 + dxc^2;
        // b = -2 * f1 * dxc;
        // c = f1^2 - r1^2 * dyc^2;
        //[17] d = b^2 - 4 * a * c;

        var a = dyc * dyc + dxc * dxc;
        var b = -2 * f1 * dxc;
        var c = f1 * f1 - r1 * r1 * dyc * dyc;

        var d = b * b - 4 * a * c;
        if (d <= Geometry.Accuracy) return [];

        //[18] x = (-b +- sqrt(d))/2*a;

        var x1 = (-b + Math.Sqrt(d)) / (2 * a);
        var x2 = (-b - Math.Sqrt(d)) / (2 * a);
        var y1 = (f1 - x1 * dxc) / dyc;
        var y2 = (f1 - x2 * dxc) / dyc;

        var p1 = new Point(x1+c1.X, y1+c1.Y);
        var p2 = new Point(x2+c1.X, y2+c1.Y);

        if (p1 == p2)
            return [p1];

        return [p1, p2];
    }

    private static List<Point> FindCirclesIntersectionsByX(Point c1, double r1, Point c2, double r2) {
        //[ 1] (x-xc1)^2+(y-yc1)^2 = r1^2
        //[ 2] (x-xc2)^2+(y-yc2)^2 = r2^2
        //-----------------------------------
        //смещаем ось координат, потом вернем
        //[ 3] x^2+y^2 = r1^2
        //[ 4] (x-(xc2-xc1))^2+(y-(yc2-yc1))^2 = r2^2;
        //[ 5] dxc=xc2-xc1; dyc=yc2-yc1; //вводим упрощения dxc и dyc
        //[ 6] (x-dxc)^2+(y-dyc)^2 = r2^2; //переписываем с учетом упрощения

        //[ 7] x^2 - 2*x*dxc + dxc^2 + y^2 - 2*y*dyc + dyc^2 = r2^2; //раскрываем скобки
        //[ 8] -2*x*dxc + dxc^2 - 2*y*dyc + dyc^2 = r2^2 - r1^2; //[7]-[3]
        //[ 9] x*dxc + y*dyc = (r2^2 - dxc^2 - dyc^2 - r1^2)/-2; //оставляем переменные в левой части
        //[10] f1 = (r2^2 - dxc^2 - dyc^2 - r1^2)/-2; //вводим упрощение f1

        var dxc = c2.X - c1.X;
        var dyc = c2.Y - c1.Y;
        var f1 = (r2 * r2 - dxc * dxc - dyc * dyc - r1 * r1) / -2;

        //[11] x*dxc + y*dyc = f1; // [9] с учетом упрощения f1
        //[12] x = (f1 - y*dyc)/dxc; //выделяем x
        //[13] ((f1-y*dyc)/dxc)^2 + y^2 = r1^2; //подставляем [12] в [3]
        //[14] (f1^2 - 2*f1*y*dyc + y^2*dyc^2)/dxc^2 + y^2 = r1^2;
        //[15] f1^2 - 2*f1*y*dyc + Y^2*dyc^2 + y*2*dxc^2 = r1^2*dxc^2;
        //[16] y^2*(dyc^2+dxc^2) + y*(-2*f1*dyc) + (f1^2 - r1^2*dxc^2) = 0;
            // a = dyc^2+dxc^2;
            // b = -2*f1*dyc;
            // c = f1^2 - r1^2*dxc^2;
            
        var a = dyc * dyc + dxc * dxc;
        var b = -2 * f1 * dyc;
        var c = f1 * f1 - r1 * r1 * dxc * dxc;

        var d = b * b - 4 * a * c;
        if (d <= Geometry.Accuracy) return [];

        //[18] x = (-b +- sqrt(d))/2*a;

        var y1 = (-b + Math.Sqrt(d)) / (2 * a);
        var y2 = (-b - Math.Sqrt(d)) / (2 * a);
        var x1 = (f1 - y1 * dyc) / dxc;
        var x2 = (f1 - y2 * dyc) / dxc;
        
        var p1 = new Point(x1+c1.X, y1+c1.Y);
        var p2 = new Point(x2+c1.X, y2+c1.Y);

        if (p1 == p2)
            return [p1];

        return [p1, p2];
        
        // var result = new List<Point> {
        //     new(x1 + c1.X, y1 + c1.Y),
        //     new(x2 + c1.X, y2 + c1.Y)
        // };
    }
    
    
}