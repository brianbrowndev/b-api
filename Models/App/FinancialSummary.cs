using System;
using System.Collections.Generic;

namespace B.API.Models
{
    public partial class FinancialSummary
    {
        public virtual Asset Asset { get; set; }
        public virtual Investment Investment { get; set; }
        public virtual Debt Debt { get; set; }

        public virtual Earning Earnings { get; set;}

        public long NetWorth { 
            get {
                return AssetTotal - DebtTotal;
            }
        }

        public long AssetTotal {
            get {
                return Asset.Auto + Asset.Home + Asset.Hsa + Asset.Retirement + Asset.Saving + Asset.Stock;

            }
        }

        public long DebtTotal { 
            get {
                return Debt.Auto + Debt.Home;
            }
        }
    }
}
