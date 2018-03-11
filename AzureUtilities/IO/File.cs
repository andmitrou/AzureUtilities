using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace AzureUtilities.IO
{
    public class AzureFile
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient client;

        public AzureFile(string storageConnectionString)
        {
            CloudStorageAccount.TryParse(storageConnectionString, out _storageAccount);

            if (this._storageAccount == null)
                throw new NullReferenceException("Storage Account has not been instantiated");

            client = _storageAccount.CreateCloudBlobClient();
        }

        public void UploadAsBlob(string localfilename, string blobContainer)
        {
            var blobFileName = Path.GetFileName(localfilename);
            UploadAsBlob(localfilename, blobContainer, blobFileName);
        }

        public void UploadAsBlob(string localfilename, string blobContainer, string newBlobFileName)
        {
            if (System.IO.File.Exists(localfilename))
                throw new FileNotFoundException();

            CloudBlobContainer container = client.GetContainerReference(blobContainer);

            if (container == null)
                throw new NullReferenceException("Container not found");

            CloudBlockBlob blockblob = container.GetBlockBlobReference(newBlobFileName);

            blockblob.UploadFromFile(localfilename, FileMode.Open);
        }

        public void DeleteFile(string blobContainer, string blobFile)
        {
            CloudBlobContainer container = client.GetContainerReference(blobContainer);
            CloudBlockBlob _blockBlob = container.GetBlockBlobReference(blobFile);
            _blockBlob.Delete();
        }

    }
}
