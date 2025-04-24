using System.Globalization;
using PcbReader.Layers.Common;
using PcbReader.Layers.Svg;
using PcbReader.Layers.Svg.Entities;
using PcbReader.Layers.Svg.Writing;
using Path = PcbReader.Layers.Svg.Entities.Path;

namespace ConsoleApp;

public static class Program {
    public static void Main(string[] args) {

        double d1 = 128;
        double d2 = 100;
        var d3 = d1 / d2;
        var d4 = Math.Round(d3, 2, MidpointRounding.ToZero);
        var d5 = 0.32;
        var s1 = d5.ToString("R");
        
        TestSvg();
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

    static void CalculateResult() {
        var callsCountPerDay = 30m;
        var avgSellPrice = 50000m;
        var clientOrdersPerDay = 0.03m;
        var resultChance = 0.25m;
        
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

    static void TestSvg() {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\test_files\svg");
        var files = di.GetFiles("svg_test_*");
        var num = files.Select(efi => efi.Name.Split(".")[0].Split("_").Last()).Select(int.Parse).Prepend(0).Max();
        num++;

        
        var 
        
        var doc = new SvgLayer();
        for (var i = 0; i < 10; i++) {
            var p = new Path {
                StartPoint = new Point((double)i * (1 + i) / 2 + i, 0)
            };

            var w = i + 1;

            p.Parts.Add(new LinePathPart {
                EndPoint = new Point(p.StartPoint.X, p.StartPoint.Y + w)
            });
            p.Parts.Add(new LinePathPart {
                EndPoint = new Point(p.StartPoint.X + w, p.StartPoint.Y + w)
            });
            p.Parts.Add(new LinePathPart {
                EndPoint = new Point(p.StartPoint.X + w, p.StartPoint.Y)
            });


            doc.Paths.Add(p);
        }

        SvgWriter.Write(doc, Directory.GetCurrentDirectory() + @"\test_files\svg\svg_test_" + num.ToString("D4") + ".svg");
    }

}