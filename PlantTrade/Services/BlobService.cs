using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlantTrade.Services
{
	public class BlobService
	{

		private readonly BlobContainerClient _container;
		private readonly BlobServiceClient _blobServiceClient;
		private readonly IConfiguration _config;

		public BlobService( IConfiguration config)
		{
			this._config = config;
			this._blobServiceClient = new BlobServiceClient(this._config.GetConnectionString("AzureConnectionString"));
			this._container = _blobServiceClient.GetBlobContainerClient("pictures");
		}

		public async Task<string> Upload(IFormFile file, string ItemName)
		{
			string n = ItemName + file.ContentType.Split("/").Last();
			var blobReference = _container.GetBlobClient(n);
			BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
			{
				ContentType = file.ContentType
			};

			using (var fileStream = file.OpenReadStream()) 
			{
				await blobReference.UploadAsync(fileStream, httpHeaders);
			}

			return blobReference.Uri.AbsoluteUri;
		}

		public async Task Delete(string url)
		{
			string imageName = url.Split("/").Last();

			var blobReference = this._container.GetBlobClient(imageName);

			if (await blobReference.ExistsAsync())
			{
				await blobReference.DeleteAsync();
			}
		}
	}
}
