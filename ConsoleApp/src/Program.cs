using System.Globalization;

namespace ConsoleApp;

public static class Program {
    public static void Main(string[] args) {
        ExpressionsTest.Test("2+3*4*5/(6+7)");
        //CalculateResult();
        // ExpressionsTest.Test();
        // Console.WriteLine("ok");
        while (true) {
            var line = Console.ReadLine();
            if (line == null) {
                return;
            }
            ExpressionsTest.Test(line);
        }
        
    }

    static void CalculateResult() {
        var callsCountPerDay = 30m;
        var avgSellPrice = 20000m;
        var clientOrdersPerDay = 0.02m;
        var resultChance = 0.33m;
        
        var clientsCount = 1m;

        var nfi = new NumberFormatInfo {
            CurrencyDecimalSeparator = ".",
            CurrencyGroupSeparator = " ",
            NumberGroupSizes = [3],
            NumberDecimalDigits = 0
        };

        var perMonth = 0m;
        for (int d = 0; d < 180; d++) {
            var newClients = (decimal)Random.Shared.NextDouble()*2 * callsCountPerDay * resultChance;
            clientsCount+=newClients;
            var orders = (decimal)Random.Shared.NextDouble()*2*clientsCount * clientOrdersPerDay;
            var sum = (decimal)Random.Shared.NextDouble()*2* orders * avgSellPrice;

            Console.WriteLine($"Day: {d}; clients: {clientsCount.ToString("N",nfi)}; sum: {sum.ToString("N",nfi)}; perMonth: {(sum*21m).ToString("N",nfi)}");
            
            perMonth += sum;
            if (d!=0 && d % 20 == 0) {
                Console.WriteLine("Всего за месяц: "+perMonth.ToString("N",nfi));
                perMonth = 0;
            }
        }
        
    }
    
}