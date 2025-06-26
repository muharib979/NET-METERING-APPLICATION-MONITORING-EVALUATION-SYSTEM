using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class ProgramDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameBn { get; set; }
        public string? Code { get; set; }
        public string? ObjectiveCode { get; set; }
        public string? Objective   { get; set; }
        public int? OrderBy { get; set; }
    }
}
