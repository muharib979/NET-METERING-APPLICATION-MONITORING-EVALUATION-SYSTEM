﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.CustomeEntity
{
    public class CustomerCategory
    {
        public int USAGE_CAT_ID { get; set; }
        public string? USAGE_CAT_CODE { get; set; }
        public string? USAGE_CAT_DESC { get; set; }
        public string? REMARKS { get; set; }
        public string? STATUS { get; set; }
        public string? CREATE_BY { get; set; }
        public string? UPDATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
}
