﻿namespace PcbReader.Layers.Gerber.Reading.Macro;

public class ParseExpressionException: ApplicationException {

    public int CharIndex { get; init; }
    
    public ParseExpressionException(string message, int charIndex) : base(message) {
        CharIndex = charIndex;
    }
}