﻿namespace PcbReader.Layers.Gerber.Entities.Apertures;

public class MacroAperture: IAperture {
    public MacroAperture(string templateName) {
        TemplateName = templateName;
    }

    public string TemplateName {get;}
    public List<double> ParameterValues {get;} = [];
}