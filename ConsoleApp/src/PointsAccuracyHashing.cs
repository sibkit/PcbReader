

using PcbReader.Core.Entities;
using PcbReader.Core.Handling;

namespace ConsoleApp;

public static class PointsAccuracyHashing {
    public static void HashPoints() {
        var p1 = new Point(10, 25);
        var p2 = new Point(10.01, 24.96);
        var map = new Dictionary<Point, string>();
        map.Add(p1, "fffttt");
        Console.WriteLine(map[p2]);
    }
   
}