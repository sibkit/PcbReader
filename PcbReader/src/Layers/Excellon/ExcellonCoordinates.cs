﻿using System.Globalization;
using System.Text.RegularExpressions;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Project;

namespace PcbReader.Layers.Excellon;

internal class StringCoordinate {
    public string? X{get;set;}
    public string? Y{get;set;}
}

public static partial class ExcellonCoordinates {
    
    private static readonly Regex ReCoordinates = MyRegex();
    private static readonly IFormatProvider Formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
    
    private static bool FillCoordinate(string? axe, string? val, StringCoordinate coordinate) {
        if (axe == null || val == null)
            return false;
        switch (axe) {
            case "X":
                coordinate.X = val;
                return true;
            case "Y":
                coordinate.Y = val;
                return true;
        }
        return false;
    }
    
    private static StringCoordinate? ReadStringCoordinate(string line) {
        var match = ReCoordinates.Match(line);
        if (!match.Success) return null;
        var sc = new StringCoordinate();
        var found = FillCoordinate(match.Groups[1].Value, match.Groups[2].Value, sc) || 
                    FillCoordinate(match.Groups[3].Value, match.Groups[4].Value, sc);
        return found ? sc : null;
    }
    

    
    private static decimal ReadValue(string value, ExcellonReadingContext ctx) {
        
        if (value.Contains('.')) {
            ctx.CoordinatesDefineState.AccurateValueDetected = true;
            return decimal.Parse(value, Formatter);
        } 

        ctx.CoordinatesDefineState.CalculateValueDetected = true;
        
        if (ctx.NumberFormat is { Left: not null, Right: not null } &&
            value.Length != ctx.NumberFormat.Left + ctx.NumberFormat.Right) {
            ctx.CoordinatesDefineState.DifferentLineLengthsDetected = true;
        }
        
        if (ctx.NumberFormat.Zeros != null && (ctx.NumberFormat.Left != null || ctx.NumberFormat.Right != null)) {
            return Coordinates.ReadValue(ctx.NumberFormat, value);
        }

        if (ctx.NumberFormat.Zeros != null) {
            ctx.CoordinatesDefineState.UndefinedScaleDetected = true;
            return Coordinates.ReadValue(new NumberFormat {
                Left = 4,
                Right = 2,
                Zeros = ctx.NumberFormat.Zeros
            }, value);
        }

        if (ctx.NumberFormat is {Zeros: null, Left: not null, Right: not null }) {
            if(value.Trim('+').Trim('-').Length == ctx.NumberFormat.Left + ctx.NumberFormat.Right)
                return Coordinates.ReadValue(new NumberFormat {
                    Left = ctx.NumberFormat.Left,
                    Right = ctx.NumberFormat.Right,
                    Zeros = Zeros.All
                }, value);
            ctx.WriteError("Не удалось определить значение координаты: \"" + value + "\"");
        }

        if (ctx.NumberFormat is { Left: null, Right: null, Zeros: null }) {
            if (value.Length == 6)
                return Coordinates.ReadValue(new NumberFormat {
                    Left = 4,
                    Right = 2,
                    Zeros = Zeros.All
                }, value);
        }
        
        return 0;

        // if (ctx.NumberFormat.Left == null && ctx.NumberFormat.Right == null) {
        //     switch (format.Zeros) {
        //         case Zeros.Leading:
        //             break;
        //         case Zeros.Trailing:
        //         case null:
        //             left = 3;
        //             right = 3;
        //             break;
        //         default:
        //             throw new Exception("Coordinates: ReadValue");
        //     }
        // } else {
        //     left = format.Left!.Value;
        //     right = format.Right!.Value;
        // }
        //
        // var stringValue = value;
        // var multiplier = 1;
        // if (stringValue.First() == '-') {
        //     multiplier = -1;
        //     stringValue = stringValue[1..];
        // }
        //
        // string frac;
        //
        // if (format.Zeros == Zeros.Leading) {
        //     if (stringValue.Length < left) {
        //         var zeroesToAdd = left - stringValue.Length;
        //         return double.Parse(stringValue) * multiplier * Math.Pow(10.0, zeroesToAdd);
        //     }
        //
        //     frac = stringValue[left..];
        // } else {
        //     frac = stringValue.Length < right ? stringValue : stringValue[^right..];
        // }
        //
        // var dValue = double.Parse(stringValue);
        // return dValue * multiplier / Math.Pow(10.0, frac.Length);
    }
    
    public static Coordinate? ReadCoordinate(string line, ExcellonReadingContext ctx) {
        var sc = ReadStringCoordinate(line);
        if (sc == null) 
            return null;
        decimal x;
        decimal y;
        x = !string.IsNullOrEmpty(sc.X) ? ReadValue(sc.X, ctx) : 0;;
        y = !string.IsNullOrEmpty(sc.Y) ? ReadValue(sc.Y, ctx) : 0;
        return new Coordinate(x,y);
    }

    public static bool IsCoordinate(string line) {
        return ReCoordinates.IsMatch(line);
    }
    
    [GeneratedRegex("^(?:([XY])([+-]?[0-9.]+))?(?:([XY])([+-]?[0-9.]+))?$")]
    private static partial Regex MyRegex();
}