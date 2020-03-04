public async Task<List<FileUpload>> AddAws(List<IFormFile> files)
        {

            List<FileUpload> urls = null;
            RegionEndpoint bucketRegion = RegionEndpoint.GetBySystemName(_credentials.Value.BucketRegion);
            AWSCredentials myCredentials = new BasicAWSCredentials(_credentials.Value.AccessKey, _credentials.Value.Secret);
            s3Client = new AmazonS3Client(myCredentials, bucketRegion);

            foreach (var file in files)
            {
                Guid guid = Guid.NewGuid();
                string fileString = file.ContentType;
                string keyName = "saving_circles" + guid + "_@" + file.FileName;
              
                using (var fileTransferUtility = new TransferUtility(s3Client))
                {
                    using (Stream stream = file.OpenReadStream())
                    {
                        await fileTransferUtility.UploadAsync(stream, _credentials.Value.BucketName, keyName);

                    }

                    if (urls == null)
                    {
                        urls = new List<FileUpload>();
                    }

                    urls.Add(new FileUpload { Url = _credentials.Value.Domain + keyName, Name=file.FileName});
                    //urls.Add(file.FileName);

                };

            }
           

            return urls ;
            
        }
