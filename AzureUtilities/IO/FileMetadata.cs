using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureUtilities
{
    public class FileMetadata
    {
        public DateTimeOffset? LastModifiedOffset { get; set; }
        public string ETag { get; set; }
        public IDictionary<string,string> CustomMetadata { get; set; }
        public DateTime? LastModified
        {
            get
            {
                if (LastModifiedOffset.GetValueOrDefault() != null)
                    return LastModifiedOffset.Value.DateTime;
                else
                    return null;
            }
        }
    }
}
