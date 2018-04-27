using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureWorkshop.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureWorkshop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(AddPerson person, HttpPostedFileBase file)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


                CloudBlobContainer container = blobClient.GetContainerReference("azure-workshop");
                container.CreateIfNotExists();
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(person.Name + " " + person.Surname + "-" + Guid.NewGuid());
                blockBlob.Metadata.Add("surname", person.Surname);
                blockBlob.Metadata.Add("firstName", person.Name);
                if(person.Phone != null) blockBlob.Metadata.Add("phone", person.Phone);
                if (person.Mail != null) blockBlob.Metadata.Add("mail", person.Mail);
                blockBlob.UploadFromStream(file.InputStream);

            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            return View();
        }
    }
}