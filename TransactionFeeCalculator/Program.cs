using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TransactionFeeCalculator.Models;

namespace TransactionFeeCalculator
{
    class Program
    {
        static readonly int tableWidth = 100;
        static void Main(string[] args)
        {
            

            Console.WriteLine("Amount To be Transferred:" );
            double inputtedAmount =Convert.ToDouble(Console.ReadLine());
            double expectedChargeOutput = CalculateTransactionFee(inputtedAmount);
            double transferAmount = inputtedAmount - expectedChargeOutput;
            string inputAmount = inputtedAmount.ToString();
            double debitAmount = transferAmount + expectedChargeOutput;
            string advisedTransferAmt = transferAmount.ToString();
            string debittedAmount = debitAmount.ToString();
            string charge = expectedChargeOutput.ToString();
            Console.Clear();
            PrintLine();
            PrintRow("Amount", "Transfer Amount", "Charge", "Debit Amount(Transfer Amount + Charge)");
            PrintLine();
            PrintRow(inputAmount, advisedTransferAmt, charge, debittedAmount);
            PrintRow("", "", "", "");
            PrintLine();
            Console.ReadLine();

            //Console.WriteLine("Amount: " + debitAmount);
            //Console.WriteLine("Transfer Amount: " + inputtedAmount);
            //Console.WriteLine("Charge : " + expectedChargeOutput);
            //Console.WriteLine("Debit Amount(Transfer Amount + Charge) : " + debitAmount);
            //Console.ReadKey();
        }

        public static double CalculateTransactionFee(double preferredAmount)
        {
            var expectedCharge = 0;
            try
            {
                string jsonString = string.Empty;
                
                string filePath = string.Empty;
                
                filePath = ConfigurationManager.AppSettings["JsonConfigFile"].ToString() + "feesConfig.json";
                using (StreamReader reader = new StreamReader(filePath))
                {

                    jsonString = reader.ReadToEnd();

                }
                //jsonString = filePath; //System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Json/feesConfig.json"));
                var configuredCharges = JsonConvert.DeserializeObject<ChargeConfigurationModel>(jsonString);
                var configuredCharge = new Fee();
                
                if (preferredAmount > 0 && preferredAmount <= 5000)
                {
                    configuredCharge = configuredCharges.Fees[0];
                    expectedCharge = configuredCharge.feeAmount;
                    return expectedCharge;
                }
                else if (preferredAmount > 5000 && preferredAmount <= 50000)
                {
                    configuredCharge = configuredCharges.Fees[1];
                    expectedCharge = configuredCharge.feeAmount;
                    return expectedCharge;

                }
                else if (preferredAmount > 50000)
                {
                    configuredCharge = configuredCharges.Fees[2];
                    expectedCharge = configuredCharge.feeAmount;
                    return expectedCharge;
                }
                else
                {
                    return expectedCharge;
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return expectedCharge;
            }
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }

    
}
