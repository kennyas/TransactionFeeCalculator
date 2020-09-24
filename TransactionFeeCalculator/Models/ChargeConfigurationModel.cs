using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionFeeCalculator.Models
{
    public class ChargeConfigurationModel
    {
        public List<Fee> Fees { get; set; }
    }
    public class Fee
    {
        public int minAmount { get; set; }
        public int maxAmount { get; set; }
        public int feeAmount { get; set; }
    }
    
}
