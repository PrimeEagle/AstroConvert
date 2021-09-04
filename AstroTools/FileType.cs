using System;
using System.Collections.Generic;
using System.Text;

namespace AstroTools
{
    class FileType
    {
        public string Filename { get; set; }
        public FileFormat Format { get; set; }

        public FileType(string filename, FileFormat format)
        {
            this.Filename = filename;
            this.Format = format;
        }
    }
}
