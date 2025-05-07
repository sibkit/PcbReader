namespace PcbReader.Geometry.Vectors;

public static class Vectors {
    public static double DotProduct(Vector a, Vector b) {
        return a.X * b.X + a.Y * b.Y;
    }

    // public static Vector CrossProduct(Vector a, Vector b) {
    //     return a.Y * b.X - a.X * b.Y;
    // }
}