using BC.Utilities.Extensions;
using BC11.Interfaces;
using System;
using System.Globalization;

namespace BC11.Entities
{
    public enum ClaimType
    {
        TotalLoss = 0
    }

    public class ClaimSettlement : ITransaction
    {
        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Mileage { get; set; }
        public ClaimType ClaimType { get; set; }

        public ClaimSettlement(string claimNumber, decimal settlementAmount, DateTime settlementDate, string carRegistration, int mileage, ClaimType claimType) =>
            (ClaimNumber, SettlementAmount, SettlementDate, carRegistration, mileage, claimType) 
            = (claimNumber, settlementAmount, settlementDate, CarRegistration, Mileage, ClaimType);
        
        public string ComputeTransactionHash() =>
            (   ClaimNumber + 
                SettlementAmount.ToString("C", CultureInfo.CurrentCulture) + 
                SettlementDate.ToString().DateTimeStringToEpoch()
            ).ComputeHashBySHA256();
    }
}
