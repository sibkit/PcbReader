using System.Diagnostics;
using System.Globalization;
using PcbReader.Converters;
using PcbReader.Converters.GerberToSvg;
using PcbReader.Converters.PathEdit;
using PcbReader.Geometry;
using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Reading;
using PcbReader.Layers.Svg;
using PcbReader.Layers.Svg.Entities;
using PcbReader.Layers.Svg.Writing;

namespace ConsoleApp;

public static class Program {
    public static void Main(string[] args) {

        TestIntersections();
        
        //CalculateResult();
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
    
    static void CalculateResult() {
        var callsCountPerDay = 60m;
        var avgSellPrice = 30000m;
        var clientOrdersPerDay = 0.03m;
        var resultChance = 0.75m;
        
        var clientsCount = 0m;

        var nfi = new NumberFormatInfo {
            CurrencyDecimalSeparator = ".",
            CurrencyGroupSeparator = " ",
            NumberGroupSizes = [3],
            NumberDecimalDigits = 0
        };

        var perMonth = 0m;
        for (int d = 1; d < 180; d++) {
            var newClients = Math.Floor((decimal)Random.Shared.NextDouble()*2 * callsCountPerDay * resultChance);
            clientsCount+=newClients;
            var orders = Math.Floor((decimal)Random.Shared.NextDouble()*2*clientsCount * clientOrdersPerDay);
            var sum = (decimal)Random.Shared.NextDouble()*2* orders * avgSellPrice;

            Console.WriteLine($"Day: {d}; clients: {clientsCount.ToString("N",nfi)}; sum: {sum.ToString("N",nfi)}; perMonth: {(sum*21m).ToString("N",nfi)}");
            
            perMonth += sum;
            if (d!=0 && d % 20 == 0) {
                Console.WriteLine("Всего за месяц: "+perMonth.ToString("N",nfi));
                perMonth = 0;
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

    private static void TestIntersections() {
        var layer = new SvgLayer();
        var painter1 = new PathPartsPainter(10, 10);
        painter1.ArcToInc(0,20,10,RotationDirection.ClockWise,false);
        painter1.ArcToInc(0,-20,10,RotationDirection.ClockWise,false);
        layer.Elements.Add(painter1.CreateContour());
        
        var painter2 = new PathPartsPainter(20, 20);
        painter2.ArcToInc(0,20,10,RotationDirection.ClockWise,false);
        painter2.ArcToInc(0,-20,10,RotationDirection.ClockWise,false);
        layer.Elements.Add(painter2.CreateContour());

        var intersections = new List<Point>();
        
        foreach (var e1 in layer.Elements) {
            if (e1 is Contour c1) {
                var p1 = c1.StartPoint;
                foreach (var pp1 in c1.Parts) {
                    foreach (var e2 in layer.Elements) {
                        if (e2 is Contour c2 && c1 != c2) {
                            var p2 = c2.StartPoint;
                            foreach (var pp2 in c2.Parts) {
                                intersections.AddRange(Intersections.FindIntersections(p1, pp1, p2, pp2));
                                p2 = pp2.PointTo;
                            }
                        }
                    }

                    p1 = pp1.PointTo;
                }
            }
        }

        foreach (var point in intersections) {
            layer.Elements.Add(new Dot {
                Diameter = 1,
                Point = point,
            });
        }
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