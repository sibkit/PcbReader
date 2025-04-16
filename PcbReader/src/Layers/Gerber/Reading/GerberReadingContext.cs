using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Reading;

public class GerberReadingContext : ReadingContext {
    public Coordinate? CurCoordinate { get; set; }
    public int? CurApertureCode { get; set; }
    public PathPaintOperation? CurPathPaintOperation { get; set; }
    
    public NumberFormat? NumberFormat { get; set; }
    public CoordinatesMode? CoordinatesMode { get; set; }
    
    public LcMode? LcMode { get; set; }
}