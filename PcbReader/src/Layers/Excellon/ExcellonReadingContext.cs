using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon;

public enum CoordinatesMode {
    Incremental,
    Absolute
}

public struct CoordinatesDefineState {
    public bool CalculateValueDetected = false;
    public bool AccurateValueDetected = false;
    public bool DifferentLineLengthsDetected = false;
    public bool UndefinedScaleDetected = false;
    public CoordinatesDefineState() { }
}

public class ExcellonReadingContext: ReadingContext {

    public bool UndefinedFormatDetected { get; set; } = false;
    public NumberFormat NumberFormat { get; set; } = new(null, null);
    
    public int? CurToolNumber { get; set; }
    public CoordinatesMode CoordinatesMode { get; set; } = CoordinatesMode.Absolute;
    public Pattern? CurPattern { get; set; }
    public Point CurPoint { get; set; }

    public MillOperation? CurMillOperation { get; set; } = null;
    public Uom? Uom {get; set;} = null;
    
    public CoordinatesDefineState CoordinatesDefineState = new();
}