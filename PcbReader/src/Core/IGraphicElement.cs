﻿namespace PcbReader.Core;

public interface IGraphicElement {
    Bounds GetBounds();
    void UpdateBounds();
} 