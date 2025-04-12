﻿namespace PcbReader.Layers.Excellon;

public enum ExcellonCommandType
{
    StartHeader,
    EndHeader,
    Comment,

    ToolDefine,
    DefineAxisLayout,
    CheckCompatibility,

    DrillOperation,
    RoutOperation,
    LinearMillOperation,
    ArcMillOperation,
    
    StartMill,
    EndMill,
    
    SetTool,
    SetUomFormat,
    SetCoordinatesMode,
    SetDrillMode,
    SetRoutMode,
    BeginPattern,
    EndPattern,
    RepeatPattern,
    EndProgram,
    Ignored
}