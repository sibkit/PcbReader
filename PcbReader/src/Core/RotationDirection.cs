namespace PcbReader.Core;

public enum RotationDirection {
    Clockwise,
    CounterClockwise,
}

public static class RotationDirectionExtension {
    public static RotationDirection Invert(this RotationDirection direction) {
        //return direction;
        return direction switch {
            RotationDirection.Clockwise => RotationDirection.CounterClockwise,
            RotationDirection.CounterClockwise => RotationDirection.Clockwise,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}