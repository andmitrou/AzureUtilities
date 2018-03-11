using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AzureUtilities.IO
{
   public static class File
    {
        public static void UploadAsBlob(string filename, string blobContainer, string blobFileName)
        {
            if (System.IO.File.Exists(filename))
                throw new FileNotFoundException();


        }

        public static void UploadAsTable()
        {//From Devel

        }
    }
}
