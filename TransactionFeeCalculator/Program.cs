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
        static void Main(string[] args)
        {
            Console.WriteLine("Amount To be Transferred:" );
            double inputtedAmount =Convert.ToDouble(Console.ReadLine());
            double expectedChargeOutput = CalculateTransactionFee(inputtedAmount);
            Console.WriteLine("Expected Charge Deduction: " + expectedChargeOutput);
            Console.ReadKey();
        }

        public static double CalculateTransactionFee(double preferredAmount)
        {
            var expectedCharge = 0;
            try
            {
                string jsonString = string.Empty;
                //using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath("~/Json/feesConfig.json")))
                //{
                //     jsonString = r.ReadToEnd();
                //}

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
                else if (preferredAmount > 50000 && preferredAmount <= 999999999)
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
    }

    
}
