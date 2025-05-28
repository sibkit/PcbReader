using PcbReader.Core;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading;

public class GerberReadingContext : ReadingContext {
    public Point? CurCoordinate { get; set; }
    public int? CurApertureCode { get; set; }
    public PathPaintOperation? CurPathPaintOperation { get; set; }
    
    public NumberFormat? NumberFormat { get; set; }
    public CoordinatesMode? CoordinatesMode { get; set; }
    
    public LcMode? LcMode { get; set; }
} 