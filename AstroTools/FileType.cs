using System;
using System.Collections.Generic;
using System.Text;

namespace AstroTools
{
    public class FileType
    {
        public string Filename { get; set; }
        public InputFileFormat Format { get; set; }

        public FileType(string filename, InputFileFormat format)
        {
            this.Filename = filename;
            this.Format = format;
        }
    }
}
