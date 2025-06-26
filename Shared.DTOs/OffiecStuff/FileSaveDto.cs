using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OffiecStuff
{
    public class FileSaveDto
    {
        public List<byte[]> FileList { get; set; }
        public List<string> FileTypes { get; set; }
        public List<string> FileNames { get; set; }
    }

    public class FileDto
    {
        public byte[] FileList { get; set; }
        public string FileTypes { get; set; }
        public string FileNames { get; set; }
    }
}
