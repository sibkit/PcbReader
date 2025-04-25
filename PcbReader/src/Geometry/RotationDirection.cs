namespace PcbReader.Layers.Common;

public enum RotationDirection {
    ClockWise,
    CounterClockwise,
}

public static class RotationDirectionExtension {
    public static RotationDirection Invert(this RotationDirection direction) {
        //return direction;
        return direction switch {
            RotationDirection.ClockWise => RotationDirection.CounterClockwise,
            RotationDirection.CounterClockwise => RotationDirection.ClockWise,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}