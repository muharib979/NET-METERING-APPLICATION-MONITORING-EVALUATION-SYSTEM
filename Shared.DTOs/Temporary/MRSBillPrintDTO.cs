using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Temporary
{
    public class MRSBillPrintDTO
    {
        public string? LocationCode { get; set; }
        public string? locationName { get; set; }
        public string? BillMonth { get; set; }
        public string? BillNumber { get; set; }
        public string? BillNumberCheckDigit { get; set; }
        public string?  CustomerNumber { get; set; }
        public string? BillIssueDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Area { get; set; }
        public string? BillGroup { get; set; }
        public string? WalkOrder { get; set; }
        public string? PrevAcNo { get; set; }
        public string? DueDate { get; set; }
        public string? Tarrif { get; set; }
        public string? BusCode { get; set; }
        public string? MeterNumber { get; set; }
        public string? MeterType { get; set; }
        public string? MeterCondition { get; set; }
        public string? Phone { get; set; }
        public string? Nid { get; set; }
        public string? PowerFactor { get; set; }
        public int? UnitSr { get; set; }
        public int? UnitPeak { get; set; }
        public int? UnitOffPeak { get; set; }
        public int? TotalUnit { get; set; }
        public int? OpenUnitSr { get; set; }
        public int? OpenUnitPeak { get; set; }
        public int? OpenUnitOffPeak { get; set; }
        public int? ClsUnitSr { get; set; }
        public int? ClsUnitPeak { get; set; }
        public int? ClsUnitOffPeak { get; set; }
        public decimal? EnergyAmountSr { get; set; }
        public decimal? EnergyAmountPeak { get; set; }
        public decimal? EnergyAmountOffPeak { get; set; }
        public decimal? PfcAmount { get; set; }
        public string? DemandCharge { get; set; }
        public string? CurrentLps { get; set; }
        public string? CurrentVat { get; set; }
        public string? CurrentPrinciple { get; set; }
        public string? ArrPrinciple { get; set; }
        public string? ArrVat { get; set; }
        public string? ArrLps { get; set; }
        public decimal?TotalPrincipleAmount{get; set; }
        public decimal?TotalVatAmount { get; set; }
        public decimal? TotalLpsAmount { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public string? PresentDate { get; set; }
        public string? PresentRdg { get; set; }
        public string? PreviousRdg { get; set; }










    }

}
