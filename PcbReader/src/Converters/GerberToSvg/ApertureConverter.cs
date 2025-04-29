using PcbReader.Geometry;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures;
using PcbReader.Layers.Gerber.Entities.Apertures.Macro;
using PcbReader.Layers.Svg.Entities;

namespace PcbReader.Converters.GerberToSvg;

public class ApertureConverter(GerberLayer layer) {
    
    public SvgPath ConvertAperture(Point coordinate, IAperture aperture) {
        return aperture switch {
            CircleAperture circle => ConvertCircleAperture(coordinate, circle),
            RectangleAperture rect => ConvertRectangleAperture(coordinate, rect),
            MacroAperture macro => ConvertMacroAperture(coordinate, macro),
            ObRoundAperture obRound => ConvertObRoundAperture(coordinate, obRound),
            // case PolygonAperture polygon:
            //     break;
            _ => throw new Exception("Unknown aperture type")
        };
    }

    public SvgPath ConvertCircleAperture(Point coordinate, CircleAperture circle) {

        var painter = new SvgPathPainter();
        var r = circle.Diameter / 2;
        painter.MoveToAbs(coordinate.X-r , coordinate.Y);
        painter.ArcToInc(2*r, 0, r, RotationDirection.CounterClockwise, false);
        painter.ArcToInc(-2*r, 0, r, RotationDirection.CounterClockwise, false);
        painter.ClosePath();

        if (circle.HoleDiameter is { } hd and > 0.000001) {
            var hr = hd / 2;
            painter.MoveToAbs(coordinate.X - r, coordinate.Y);
            painter.ArcToInc(2*r, 0, r, RotationDirection.ClockWise, false);
            painter.ArcToInc(-2*r, 0, r, RotationDirection.ClockWise, false);
            painter.ClosePath();
        }

        return painter.SvgPath;
    }

    public SvgPath ConvertRectangleAperture(Point coordinate, RectangleAperture rect) {
        var painter = new SvgPathPainter();
        painter.MoveToAbs(coordinate.X - rect.XSize / 2, coordinate.Y - rect.YSize / 2);
        painter.LineToInc(rect.XSize, 0);
        painter.LineToInc(0, rect.YSize);
        painter.LineToInc(-rect.XSize, 0);
        painter.LineToInc(0, -rect.YSize);
        painter.ClosePath();
        
        if (rect.HoleDiameter is { } hd and > 0.000001) {
            var r = hd / 2;
            painter.MoveToAbs(coordinate.X - r, coordinate.Y);
            painter.ArcToInc(2*r, 0, r, RotationDirection.ClockWise, false);
            painter.ArcToInc(-2*r, 0, r, RotationDirection.ClockWise, false);
            painter.ClosePath();
        }
        return painter.SvgPath;
    }

    public SvgPath ConvertObRoundAperture(Point coordinate, ObRoundAperture obRound){
            var painter = new SvgPathPainter();

            if (obRound.XSize >= obRound.YSize) {
                var br = obRound.YSize / 2;
                painter.MoveToAbs(coordinate.X - br , coordinate.Y - obRound.YSize/2 + br);
            
                painter.ArcToInc(2*br, 0, br, RotationDirection.CounterClockwise, false);
                painter.LineToInc(0, obRound.YSize - 2*br);
                painter.ArcToInc(-2*br, 0, br, RotationDirection.CounterClockwise, false);
                painter.LineToInc(0, -(obRound.YSize - 2*br));
                painter.ClosePath();
            } else {
                var br = obRound.XSize / 2;
                painter.MoveToAbs(coordinate.X - (obRound.XSize/2 - br), coordinate.Y + br);
            
                painter.ArcToInc(0, -2*br, br, RotationDirection.CounterClockwise, false);
                painter.LineToInc(obRound.XSize - 2*br, 0);
                painter.ArcToInc(0, 2*br, br, RotationDirection.CounterClockwise, false);
                painter.LineToInc(-(obRound.XSize - 2*br), 0);
                painter.ClosePath();
            }
            


            if (obRound.HoleDiameter is { } hd and > 0.000001) {
                var r = hd / 2;
                painter.MoveToAbs(coordinate.X - r, coordinate.Y);
                painter.ArcToInc(r, r, r, RotationDirection.ClockWise, false);
                painter.ArcToInc(r, -r, r, RotationDirection.ClockWise, false);
                painter.ArcToInc(-r, -r, r, RotationDirection.ClockWise, false);
                painter.ArcToInc(-r, r, r, RotationDirection.ClockWise, false);
                painter.ClosePath();
            }

            return painter.SvgPath;
        }
    
        public SvgPath ConvertMacroAperture(Point coordinate, MacroAperture macro){
            var result = new SvgPath();
            if (!layer.MacroApertureTemplates.TryGetValue(macro.TemplateName, out var template))
                throw new ApplicationException("Не найден шаблон для макроаппертуры: \"" + macro.TemplateName + "\"");

            foreach (IPrimitive primitive in template.Primitives) {
                
            }
            
            Console.WriteLine("GerberToSvg.ConvertMacroAperture: Macro apertures not implemented");
            // result.StartPoint = coordinate - new Point(rect.XSize/2, rect.YSize/2);
            return result;
        }
}