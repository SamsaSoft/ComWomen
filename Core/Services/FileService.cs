using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Core.Enums;

namespace Core.Services
{
    public class FileService : IFileService
    {
        private readonly string wwwRootPath;

        const string wwwRoot = "wwwroot";
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            this.wwwRootPath = hostingEnvironment.WebRootPath;
        }

        public async Task<OperationResult<int>> UpdateFile(Media media)
        {
            if (!CheckAttachFilesFromMediaType(media, media.MediaType))
            {
                return new OperationResult<int>(false, $"Selected files must be of the type {media.MediaType}", media.Id);
            }
            var classMedia = media.MediaType;
            await CreateFilesFromMedia(media, classMedia);
            return new OperationResult<int>(true, string.Empty, media.Id);
        }

        public async Task<OperationResult<int>> CreateFile(Media media)
        {
            if (!CheckAttachFiles(media))
            {
                return new OperationResult<int>(false, "At least one file must be selected and all selected files must be of the same type", 0);
            }
            var classMedia = ClassNameToMediaTypeId(GetClass(media));
            media.MediaType = classMedia;
            await CreateFilesFromMedia(media, classMedia);
            UpdateEmptyUrl(media);
            return new OperationResult<int>(true, string.Empty, 0);
        }

        private static void UpdateEmptyUrl(Media media)
        {
            foreach (var item in media.Translations.Where(x => string.IsNullOrEmpty(x.Url)))
            {
                item.Url = media.Translations
                    .Where(x => !string.IsNullOrEmpty(x.Url))
                    .Select(x => x.Url)
                    .FirstOrDefault();
            }
        }

        private async Task CreateFilesFromMedia(Media media, MediaType classMedia)
        {
            var mediaPath = Path.Combine(wwwRootPath, classMedia.ToString());
            CreateDirectoryIfNotExists(mediaPath);
            foreach (var item in media.Translations)
            {
                if (item.File != null)
                {
                    var fileName = await GenerateNameFile(item.File);
                    var filePath = Path.Combine(mediaPath, fileName);
                    item.Url = fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        continue;
                    }
                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await item.File.CopyToAsync(stream);
                }
            }
        }

        private static void CreateDirectoryIfNotExists(string mediaPath)
        {
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
        }

        public static string MediaTypeIdToClassName(MediaType mediaType)
        {
            return mediaType switch
            {
                MediaType.Photo => "image",
                MediaType.Video => "video",
                MediaType.Audio => "audio",
                _ => throw new ArgumentException("Not supported type"),
            };
        }

        public static MediaType ClassNameToMediaTypeId(string className)
        {
            return className switch
            {
                "image" => MediaType.Photo,
                "video" => MediaType.Video,
                "audio" => MediaType.Audio,
                _ => throw new ArgumentException("Not supported type"),
            };
        }

        protected async Task<string> GenerateNameFile(IFormFile formFile)
        {
            System.Security.Cryptography.HashAlgorithm hashAlg = System.Security.Cryptography.MD5.Create();
            var stream = formFile.OpenReadStream();
            var hash = await hashAlg.ComputeHashAsync(stream);
            var strhash = BitConverter.ToString(hash).Replace("-", "");
            return strhash + Path.GetExtension(formFile.FileName);
        }

        string GetClass(Media media)
        {
            var first = media.Translations.FirstOrDefault(x => x.File != null);
            if (first == null)
                throw new Exception("Internal error");

            return first.File.ContentType.Split("/").First();
        }

        bool CheckAttachFilesFromMediaType(Media media, MediaType mediaType)
        {
            var mediaClass = GetClass(media);
            return !media.Translations.All(x => x.File == null)
                && media.Translations
                .Where(x => x.File != null)
                .Any(x => x.File.ContentType.Split("/").First() == mediaClass);
        }

        bool CheckAttachFiles(Media media)
        {
            return !media.Translations.All(x => x.File == null)
                && media.Translations
                .Where(x => x.File != null)
                .GroupBy(x => x.File.ContentType.Split("/").First())
                .Count() == 1;
        }
    }
}
