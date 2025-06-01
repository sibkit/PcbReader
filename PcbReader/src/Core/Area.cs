﻿using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReader.Core;

public class Area {
    public List<IGraphicElement> GraphicElements { get; } = [];

    public void InvertYAxe() {
        foreach (var e in GraphicElements) {
            switch (e) {

                case CurvesOwner ctr:
                    InvertYAxe(ctr);
                    break;
                case Shape shape:
                    InvertYAxe(shape.OuterContour);
                    foreach (var ic in shape.InnerContours)
                        InvertYAxe(ic);
                    break;
                case Dot dot:
                    dot.CenterPoint = new Point(dot.CenterPoint.X, -1*dot.CenterPoint.Y);
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: InvertAxis");
            }
            e.UpdateBounds();
        }
    }
    
    static void InvertYAxe(ICurve curve) {

        switch (curve) {
            case Line line:
                line.PointFrom = new Point(line.PointFrom.X, -1*line.PointFrom.Y);
                line.PointTo = new Point(line.PointTo.X, -1*line.PointTo.Y);
                break;
            case Arc arc:
                arc.PointFrom = new Point(arc.PointFrom.X, -1*arc.PointFrom.Y);
                arc.PointTo = new Point(arc.PointTo.X, -1*arc.PointTo.Y);
                arc.RotationDirection = arc.RotationDirection.Invert();
                break;
            default:
                throw new Exception("Area: InvertAxis");
        }
    }

    static void InvertYAxe(CurvesOwner ctx) {
        //ctx.StartPoint = ctx.StartPoint.WithNewY(-ctx.StartPoint.Y);
        foreach (var p in ctx.Parts) {
            InvertYAxe(p);
        }
    }
    
}