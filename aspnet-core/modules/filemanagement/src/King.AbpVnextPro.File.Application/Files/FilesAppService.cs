﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using Volo.Abp;
using King.AbpVnextPro.File.Settings;

namespace King.AbpVnextPro.File.Files
{
    public class FilesAppService : FileAppService, IFileAppService
    {
        protected IFileManager FileManager { get; }

        public FilesAppService(IFileManager fileManager)
        {
            FileManager = fileManager;
        }

        public virtual async Task<FileDto> FindByBlobNameAsync(string blobName)
        {
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            var file = await FileManager.FindByBlobNameAsync(blobName);
            var bytes = await FileManager.GetBlobAsync(blobName);

            return new FileDto
            {
                Bytes = bytes,
                FileName = file.FileName
            };
        }

        [Authorize]
        public virtual async Task<string> CreateAsync(FileDto input)
        {
            await CheckFile(input);

            var file = await FileManager.CreateAsync(input.FileName, input.Bytes);

            return file.BlobName;
        }

        protected virtual async Task CheckFile(FileDto input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                throw new AbpValidationException("Bytes can not be null or empty!",
                    new List<ValidationResult>
                    {
                        new ValidationResult("Bytes can not be null or empty!", new[] {"Bytes"})
                    });
            }

            var allowedMaxFileSize = await SettingProvider.GetAsync<int>(FileSettings.AllowedMaxFileSize);//kb
            var allowedUploadFormats = (await SettingProvider.GetOrNullAsync(FileSettings.AllowedUploadFormats))
                ?.Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (input.Bytes.Length > allowedMaxFileSize * 1024)
            {
                throw new UserFriendlyException(L["FileManagement.ExceedsTheMaximumSize", allowedMaxFileSize]);
            }

            if (allowedUploadFormats == null || !allowedUploadFormats.Contains(Path.GetExtension(input.FileName)))
            {
                throw new UserFriendlyException(L["FileManagement.NotValidFormat"]);
            }
        }

    }
}
