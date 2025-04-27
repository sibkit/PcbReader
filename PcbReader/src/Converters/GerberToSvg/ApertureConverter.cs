using PcbReader.Geometry;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures;
using PcbReader.Layers.Svg.Entities;

namespace PcbReader.Converters.GerberToSvg;

public static class ApertureConverter {
    public static SvgPath ConvertAperture(Point coordinate, IAperture aperture) {
        return aperture switch {
            CircleAperture circle => ConvertCircleAperture(coordinate, circle),
            RectangleAperture rect => ConvertRectangleAperture(coordinate, rect),
            MacroAperture macro => ConvertMacroAperture(coordinate, macro),
            ObRoundAperture obRound => ConvertObRoundAperture(coordinate, obRound),
            // case RectangleAperture rectangle:
            //     break;
            // case ObRoundAperture obRound:
            //     break;
            // case PolygonAperture polygon:
            //     break;
            // case MacroAperture macro:
            //     break;
            _ => throw new Exception("Unknown aperture type")
        };
    }

    public static SvgPath ConvertCircleAperture(Point coordinate, CircleAperture circle) {
        var result = new SvgPath();
        
        return result;
    }
    
    public static SvgPath ConvertRectangleAperture(Point coordinate, RectangleAperture rect){
        var painter = new SvgPathPainter(new Point(coordinate.X - rect.XSize/2, coordinate.Y - rect.YSize/2));
        painter.LineTo(0, rect.YSize);
        painter.LineTo(rect.XSize, 0);
        painter.LineTo(0, -rect.YSize);
        painter.LineTo(-rect.XSize, 0);
        return painter.SvgPath;
    }
    
    public static SvgPath ConvertObRoundAperture(Point coordinate, ObRoundAperture obRound){
        var result = new SvgPath();
        Console.WriteLine("ObRoundAperture apertures not implemented");
        // result.StartPoint = coordinate - new Point(rect.XSize/2, rect.YSize/2);
        return result;
    }
    
    public static SvgPath ConvertMacroAperture(Point coordinate, MacroAperture macro){
        var result = new SvgPath();
        Console.WriteLine("Macro apertures not implemented");
       // result.StartPoint = coordinate - new Point(rect.XSize/2, rect.YSize/2);
        return result;
    }
}