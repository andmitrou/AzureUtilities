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
        /// <summary>
        /// Upload File to Azure using same filename as input
        /// </summary>
        /// <param name="localfilename"></param>
        /// <param name="blobContainer"></param>
        public void UploadAsBlob(string localfilename, string blobContainer)
        {
            var blobFileName = Path.GetFileName(localfilename);
            UploadAsBlob(localfilename, blobContainer, blobFileName);
        }
        /// <summary>
        /// Upload File to Azure using with using different filename as input
        /// </summary>
        /// <param name="localfilename"></param>
        /// <param name="blobContainer"></param>
        /// <param name="newBlobFileName"></param>
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

        public void DownloadFile(string blobContainer, string blobFile, string destFile)
        {
            CloudBlobContainer container = client.GetContainerReference(blobContainer);

            CloudBlockBlob _blockBlob = container.GetBlockBlobReference(blobFile);

            _blockBlob.DownloadToFile(destFile, FileMode.Create);
        }

        public FileMetadata GetFileProperties(string blobContainer, string blobFile)
        {
            CloudBlobContainer container = client.GetContainerReference(blobContainer);
            CloudBlockBlob _blockBlob = container.GetBlockBlobReference(blobFile);
            _blockBlob.FetchAttributes();

            var metadata = new FileMetadata();
            metadata.LastModifiedOffset = _blockBlob.Properties.LastModified;
            metadata.ETag = _blockBlob.Properties.ETag;
            metadata.CustomMetadata = _blockBlob.Metadata;

            return metadata;
        }
    }
}
