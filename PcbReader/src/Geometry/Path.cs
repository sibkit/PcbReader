﻿namespace PcbReader.Geometry;

public class Path: IPathPartsOwner {
    public Point StartPoint { get; set; }
    public double StrokeWidth { get; init; }
    public List<IPathPart> Parts { get; } = [];

}