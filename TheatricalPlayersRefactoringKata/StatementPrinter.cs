using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private Dictionary<string, Play> plays;

        public StatementPrinter(Dictionary<string, Play> inputPlays)
        {
            plays = inputPlays;
        }

        Play playFor(Performance performance)
        {
            return plays[performance.PlayID];
        }

        public int amountFor(Performance performance)
        {
            var result = 0;
            switch (playFor(performance).Type)
            {
                case "tragedy":
                    result = 40000;
                    if (performance.Audience > 30)
                    {
                        result += 1000 * (performance.Audience - 30);
                    }
                    break;
                case "comedy":
                    result = 30000;
                    if (performance.Audience > 20)
                    {
                        result += 10000 + 500 * (performance.Audience - 20);
                    }
                    result += 300 * performance.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + playFor(performance).Type);
            }
            return result;
        }

        public int volumeCreditsFor(Performance performance)
        {
            var result = 0;
            result += Math.Max(performance.Audience - 30, 0);
            if ("comedy" == playFor(performance).Type) result += (int)Math.Floor((decimal)performance.Audience / 5);
            return result;
        }

        public int totalVolumeCredits(Invoice invoice)
        {
            var result = 0;
            foreach (var performance in invoice.Performances)
            {
                result += volumeCreditsFor(performance);
            }

            return result;
        }

        public string Print(Invoice invoice)
        {
            var totalAmount = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var performance in invoice.Performances) 
            {
                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", playFor(performance).Name, Convert.ToDecimal(amountFor(performance) / 100), performance.Audience);
                totalAmount += amountFor(performance);
            }

            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", totalVolumeCredits(invoice));
            return result;
        }
    }
}
