using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ministry
{
    public class MinistrySummary: MinistrySummaryMinistryCount
    {
        public string DB_NAME { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string MINISTRY_NAME { get; set; }
        public string MINISTRY_NAMEBN { get; set; }
        public int NOC { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal PRN { get; set; }
        public decimal TOTAL { get; set; }
        public string DB_CODE { get; set; }
        public string ZONE_CODE { get; set; }
    }
    public class MinistrySummaryMinistryCount
    {
        public decimal CHITTAGONG_COUNT { get; set; }
        public decimal CHITTAGONG_PREV_ARREAR_AMT { get; set; }
        public decimal CHITTAGONG_CURR_MONTH_BILL { get; set; }
        public decimal CHITTAGONG_COLLECTION_AMOUNT { get; set; }
        public decimal CHITTAGONG_TOTAL_ARREAR_AMOUNT { get; set; }

        public decimal COMILLA_COUNT { get; set; }
        //public decimal COMILLA_PRN { get; set; }
        public decimal COMILLA_PREV_ARREAR_AMT { get; set; }
        public decimal COMILLA_CURR_MONTH_BILL { get; set; }
        public decimal COMILLA_COLLECTION_AMOUNT { get; set; }
        public decimal COMILLA_TOTAL_ARREAR_AMOUNT { get; set; }

        public decimal SYLHET_COUNT { get; set; }
        //public decimal SYLHET_PRN { get; set; }
        public decimal SYLHET_PREV_ARREAR_AMT { get; set; }
        public decimal SYLHET_CURR_MONTH_BILL { get; set; }
        public decimal SYLHET_COLLECTION_AMOUNT { get; set; }
        public decimal SYLHET_TOTAL_ARREAR_AMOUNT { get; set; }


        public decimal MYMENSINGH_COUNT { get; set; }
        //public decimal MYMENSINGH_PRN { get; set; }
        public decimal MYMENSINGH_PREV_ARREAR_AMT { get; set; }
        public decimal MYMENSINGH_CURR_MONTH_BILL { get; set; }
        public decimal MYMENSINGH_COLLECTION_AMOUNT { get; set; }
        public decimal MYMENSINGH_TOTAL_ARREAR_AMOUNT { get; set; }


        public decimal KISHOREGANJ_COUNT { get; set; }
        public decimal KISHOREGANJ_PRN { get; set; }

        public decimal MOULVIBAZAR_COUNT { get; set; }
        public decimal MOULVIBAZAR_PRN { get; set; }

        public decimal TANGAIL_COUNT { get; set; }
        public decimal TANGAIL_PRN { get; set; }

        public decimal JAMALPUR_COUNT { get; set; }
        public decimal JAMALPUR_PRN { get; set; }
    }
}
