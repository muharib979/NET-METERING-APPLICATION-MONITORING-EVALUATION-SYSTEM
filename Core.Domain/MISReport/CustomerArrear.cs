using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MISReport
{
    public class CustomerArrear
    {
        public string LOC { get; set; }
        public string OFFICE { get; set; }
        public string BG { get; set; }
        public string B_K { get; set; }
        public string CON_NO { get; set; }
        public string WLK_OR { get; set; }
        public string PV_AC { get; set; }
        public string NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string ADDR { get; set; }
        public string LIKELY { get; set; }
        public string TARIFF { get; set; }
        public int NOM { get; set; }
        public decimal PRINCIPAL_ARREAR { get; set; }
        public decimal LPS_ARREAR { get; set; }
        public decimal VAT_ARREAR { get; set; }
        public decimal TOTAL_ARREAR { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public DateTime DISC_DATE { get; set; }
        public string STATUS { get; set; }
 
    }
    public class AllCustoemrArrearSummary
    {
        public string CENTER { get; set; }
        public int NOC { get; set; }
        public double ARR_PRIN { get; set; }
        public double ARR_LPS { get; set; }
        public double ARR_VAT { get; set; }
        public double TOTAL_BILL { get; set; }
        public int ORDER { get; set; }
        public string LOC { get; set; }
        public string OFFICE { get; set; }
    }
    public class DbMappingDto
    {
        public int ID { get; set; }
        public string USERID { get; set; }
        public string DB_CODE { get; set; }
        public int FULLACCESS { get; set; }
    }

    public class PrepaidCustomerArrearBasedOnOffset 
    {
        
    }
}
