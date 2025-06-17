﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using PcbReader.Converters;
using PcbReader.Converters.GerberToSvg;
using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Reading;
using PcbReader.Layers.Svg;
using PcbReader.Layers.Svg.Entities;
using PcbReader.Layers.Svg.Writing;

namespace ConsoleApp;

public static class Program {

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void Main(string[] args) {
        
        PointsAccuracyHashing.HashPoints();
        
        CalculateResult();
        //MacroTest.MacroAmTest();
        return;
        TestGeometry();
        
        double d1 = 128;
        double d2 = 100;
        var d3 = d1 / d2;
        var d4 = Math.Round(d3, 2, MidpointRounding.ToZero);
        var d5 = 0.32;
        var s1 = d5.ToString("R");
        
        TestGerberToSvg();
        //ExpressionsTest.Test("2+3*4*5/(6+7)");
        //CalculateResult();
        // ExpressionsTest.Test();
        // Console.WriteLine("ok");
        // while (true) {
        //     var line = Console.ReadLine();
        //     if (line == null) {
        //         return;
        //     }
        //     ExpressionsTest.Test(line);
        // }
        
    }

    static void TestGeometry() {
        
        //upRight
        //  cp  ep
        //  sp
        
        
        // var sp = new Point(1, 1);
        // var ep = new Point(2, 2);
        // var cp = new Point(1, 2);
        // var aw = Geometry.ArcWay(sp, ep, cp, AxisLayout.YUpXRight);
        // if(aw.IsLarge)
        //     throw new Exception("TestGeometry: large arc way");
        // if(aw.RotationDirection != RotationDirection.CounterClockwise)
        //     throw new Exception("TestGeometry: ClockWise Arc Way");
        //
        // aw = Geometry.ArcWay(sp, ep, cp, AxisLayout.YDownXRight);
        //
        // if(aw.IsLarge)
        //     throw new Exception("TestGeometry: large arc way");
        // if(aw.RotationDirection != RotationDirection.ClockWise)
        //     throw new Exception("TestGeometry: CounterClockwise Arc Way");
        
        
        
        //downRight
        
        //  cp  ep
        //  sp

        
        var aw = Geometry.ArcWay(new Point(1,2), new Point(2,1), new Point(1,1));
        Console.WriteLine("d");
    }

    static string GetText(string text, int charsCount) {
        if (text.Length < charsCount) {
            var d = charsCount - text.Length;
            for (var i = 0; i < d; i++)
                text += " ";
            return text;
        } else {
            return text[..charsCount];
        }
    }
    
    static void CalculateResult() {
        var callsCountPerDay = 60m;
        var avgSellPrice = 1000m;
        var clientOrdersPerDay = 0.12m;
        var resultChance = 0.5m;
        var clientsCount = 0m;
        var chanceLowingPerDay = 0.0001m;
        
        var nfi = new NumberFormatInfo {
            CurrencyDecimalSeparator = ".",
            CurrencyGroupSeparator = " ",
            NumberGroupSizes = [3],
            NumberDecimalDigits = 0
        };

        var perMonth = 0m;
        for (int d = 1; d < 1201; d++) {
            var newClients = Math.Floor((decimal)Random.Shared.NextDouble()*2 * callsCountPerDay * resultChance);
            clientsCount+=newClients;
            var orders = Math.Floor((decimal)Random.Shared.NextDouble()*2*clientsCount * clientOrdersPerDay);
            var sum = (decimal)Random.Shared.NextDouble()*2* orders * avgSellPrice;

            // Console.WriteLine(
            //     GetText($"Day: {d}", 10) +
            //     GetText($"Clients: {clientsCount.ToString("N", nfi)}", 18) +
            //     GetText($"Sum: {sum.ToString("N", nfi)}", 18) +
            //     GetText($"PerMonth: {(sum * 21m).ToString("N", nfi)}", 24)
            // );
            
            perMonth += sum;
            if (d!=0 && d % 20 == 0) {
                Console.WriteLine("Всего за месяц: "+perMonth.ToString("N",nfi));
                perMonth = 0;
            }

            if (resultChance > chanceLowingPerDay) {
                resultChance -= chanceLowingPerDay;
            }
            
            
        }
        
    }

    public static GerberLayer ReadGerber(FileInfo fileInfo) {
        // try {
            var p = GerberReader.Instance.Read(fileInfo);
            if (p.Item2.Errors.Count > 0) {
                Console.WriteLine("  ...ERROR");
                Console.WriteLine("Обнаружены ошибки:");
                foreach (var error in p.Item2.Errors) {
                    Console.WriteLine(error);
                }
                Console.WriteLine();
            }
            
            if (p.Item2.Warnings.Count > 0) {
                Console.WriteLine("Обнаружены предупреждения:");
                foreach (var warning in p.Item2.Warnings) {
                    Console.WriteLine(warning);
                }
            }

            if (p.Item2.Errors.Count == 0 && p.Item2.Warnings.Count == 0) {
                Console.WriteLine("  ...OK");
            }
            return p.Item1;
    }


    
    private static void TestGerberToSvg() {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\test_files\gerber\03");
        var files = di.GetFiles("*");
        var i = 0;
        foreach (var fi in files) {
            // if(!fi.Name.Contains("COMPMASK"))
            //     continue;
            // if(i>3)
            //     break;
            // i++;
            if(fi.Extension.ToUpper() is ".DRL" or ".TC" or ".TOL")
                continue;
            Console.WriteLine("-----");
            Console.WriteLine("Обработка файла: " + fi.Name);

            var gl = ReadGerber(fi);
            var svg = GerberToSvgConverter.Convert(gl);
            WriteSvg(svg);
            Console.WriteLine("-----");
            Console.WriteLine("");
        }
    }
    
    public static void WriteSvg(SvgLayer layer) {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\test_files\svg");
        var files = di.GetFiles("svg_test_*");
        var num = files.Select(efi => efi.Name.Split(".")[0].Split("_").Last()).Select(int.Parse).Prepend(0).Max();
        num++;

        
        
        
        // var doc = new SvgLayer();
        // for (var i = 0; i < 10; i++) {
        //     var p = new Path {
        //         StartPoint = new Point((double)i * (1 + i) / 2 + i, 0)
        //     };
        //
        //     var w = i + 1;
        //
        //     p.Parts.Add(new LinePathPart {
        //         EndPoint = new Point(p.StartPoint.X, p.StartPoint.Y + w)
        //     });
        //     p.Parts.Add(new LinePathPart {
        //         EndPoint = new Point(p.StartPoint.X + w, p.StartPoint.Y + w)
        //     });
        //     p.Parts.Add(new LinePathPart {
        //         EndPoint = new Point(p.StartPoint.X + w, p.StartPoint.Y)
        //     });
        //
        //
        //     doc.Paths.Add(p);
        // }

        SvgWriter.Write(layer, Directory.GetCurrentDirectory() + @"\test_files\svg\svg_test_" + num.ToString("D4") + ".svg");
    }

}