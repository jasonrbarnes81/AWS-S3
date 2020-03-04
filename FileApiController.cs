[HttpPost("aws")]
        public async Task<ActionResult<ItemResponse<object>>> UploadAWS(List<IFormFile> files)
        {
            BaseResponse response = null;
            FileAddRequest _file = null;
            List<FileUpload> list = new List<FileUpload>();
            int id = 0;
            int iCode = 200;
           


            try
            {
                int userId = _authService.GetCurrentUserId();
                IUserAuthData name = _authService.GetCurrentUser();

                List<FileUpload> urls = await _service.AddAws(files);

                
                _file = new FileAddRequest();
                foreach (var item in urls)
                {
                   
                    _file.Url = item.Url;
                    _file.FileTypeId = 1;
                    _file.FirstName = name.Name;
                    _file.LastName = name.Name;
                    _file.Name = item.Name;
                    id = _service.Add(_file, userId);

                    list.Add(new FileUpload { Id = id,
                                            Url = _file.Url,
                                            Name = _file.Name,
                                            LastName = _file.LastName,
                                            FirstName = _file.FirstName,
                                            FileTypeId = _file.FileTypeId,
                                            CreatedBy = userId,
                                            DateCreated = DateTime.UtcNow });
                }

                    
                    response = new ItemResponse<List<FileUpload>> { Item = list };
               
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Errors: ${ex.Message}");
            }

            return StatusCode(iCode, response);

            
        }
