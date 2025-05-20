namespace ConsoleApp;

public static class SymPyTest {
    public static string Test(double i) {
        var r1 = 10.0 * i + 1;
        var r2 = 20.0;
        var xc1 = 10.0 * i + 1;
        var xc2 = 20.0;
        var yc1 = 20.0;
        var yc2 = 30.0;

//[(
        var x1 = -1f / 2 * (r1 * r1 - r2 * r2 - xc1 * xc1 + xc2 * xc2 - yc1 * yc1 + yc2 * yc2 + (2 * yc1 - 2 * yc2) *
            (-1f / 2 * Math.Sqrt(-(r1 * r1 - 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2) *
                                 (r1 * r1 + 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2)) *
                (xc1 - xc2) / (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2) - 1f / 2 *
                (r1 * r1 * yc1 - r1 * r1 * yc2 - r2 * r2 * yc1 + r2 * r2 * yc2 - xc1 * xc1 * yc1 - xc1 * xc1 * yc2 + 2 * xc1 * xc2 * yc1 +
                    2 * xc1 * xc2 * yc2 - xc2 * xc2 * yc1 - xc2 * xc2 * yc2 - yc1 * yc1 * yc1 + yc1 * yc1 * yc2 + yc1 * yc2 * yc2 - yc2 * yc2 * yc2) /
                (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2))) / (xc1 - xc2);
        var y1 =
            -1f / 2 * Math.Sqrt(-(r1 * r1 - 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2) *
                                (r1 * r1 + 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2)) *
            (xc1 - xc2) / (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2) - 1f / 2 *
            (r1 * r1 * yc1 - r1 * r1 * yc2 - r2 * r2 * yc1 + r2 * r2 * yc2 - xc1 * xc1 * yc1 - xc1 * xc1 * yc2 + 2 * xc1 * xc2 * yc1 +
                2f * xc1 * xc2 * yc2 - xc2 * xc2 * yc1 - xc2 * xc2 * yc2 - yc1 * yc1 * yc1 + yc1 * yc1 * yc2 + yc1 * yc2 * yc2 - yc2 * yc2 * yc2) /
            (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2);
        var x2 = (-1f / 2 * (r1 * r1 - r2 * r2 - xc1 * xc1 + xc2 * xc2 - yc1 * yc1 + yc2 * yc2 + (2 * yc1 - 2 * yc2) *
            ((1f / 2) * Math.Sqrt(-(r1 * r1 - 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2) *
                                  (r1 * r1 + 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2)) *
                (xc1 - xc2) /
                (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2) - 1f / 2 *
                (r1 * r1 * yc1 - r1 * r1 * yc2 - r2 * r2 * yc1 + r2 * r2 * yc2 - xc1 * xc1 * yc1 - xc1 * xc1 * yc2 + 2 * xc1 * xc2 * yc1 +
                    2 * xc1 * xc2 * yc2 -
                    xc2 * xc2 * yc1 - xc2 * xc2 * yc2 - yc1 * yc1 * yc1 + yc1 * yc1 * yc2 + yc1 * yc2 * yc2 - yc2 * yc2 * yc2) /
                (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2))) / (xc1 - xc2));
        var y2 =
            (1f / 2) * Math.Sqrt(-(r1 * r1 - 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2) *
                                 (r1 * r1 + 2 * r1 * r2 + r2 * r2 - xc1 * xc1 + 2 * xc1 * xc2 - xc2 * xc2 - yc1 * yc1 + 2 * yc1 * yc2 - yc2 * yc2)) *
            (xc1 - xc2) /
            (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2) - 1f / 2 *
            (r1 * r1 * yc1 - r1 * r1 * yc2 - r2 * r2 * yc1 + r2 * r2 * yc2 - xc1 * xc1 * yc1 - xc1 * xc1 * yc2 + 2 * xc1 * xc2 * yc1 +
                2 * xc1 * xc2 * yc2 -
                xc2 * xc2 * yc1 - xc2 * xc2 * yc2 - yc1 * yc1 * yc1 + yc1 * yc1 * yc2 + yc1 * yc2 * yc2 - yc2 * yc2 * yc2) /
            (xc1 * xc1 - 2 * xc1 * xc2 + xc2 * xc2 + yc1 * yc1 - 2 * yc1 * yc2 + yc2 * yc2);

        return $"Result: {x1}, {y1}; {x2}, {y2}";
    }
}