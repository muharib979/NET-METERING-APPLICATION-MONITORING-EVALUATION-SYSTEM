using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Response
{
    public class Content
    {
        public string status { get; set; }
        public List<Result> Result { get; set; }
    }
}
