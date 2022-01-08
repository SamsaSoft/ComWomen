using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class MediaService : IMediaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public MediaService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Media> Upload(Media media)
        {
            media.CreatedAt = DateTime.UtcNow;

            _context.Medias.Add(media);
            await _context.SaveChangesAsync();

            return media;
        }

        public async Task<Media> GetById(int id)
        {
            var media = await _context.Medias
                .Include(x => x.Author)
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (media is null)
                throw new KeyNotFoundException($"Медиа с таким ключем не найдено");
            return media;
        }

        public async Task<List<Media>> GetAllByType(MediaType type) =>
            await _context.Medias
            .Where(x => x.MediaType == type)
            .Include(x => x.Author)
            .Include(x => x.Translations)
            .ToListAsync();

        public async Task DelteById(int id)
        {
            var media = await GetById(id);
            _context.Medias.Remove(media);
            await _context.SaveChangesAsync();
        }
        public async Task<OperationResult<int>> Update(Media media)
        {
            try
            {
                var result = await _fileService.UpdateFile(media);
                if (!result.IsSuccess)
                {
                    return result;
                }
                var dbMedia = await GetById(media.Id);
                foreach (var item in media.Translations)
                {
                    var translate = dbMedia[item.Language];
                    if (translate != null)
                    {
                        translate.Url = item.Url;
                        translate.Title = item.Title;
                        translate.Description = item.Description;
                    }
                }
                _context.Update(dbMedia);
                await _context.SaveChangesAsync();
                return new OperationResult<int>(true, string.Empty, dbMedia.Id);
            }
            catch (Exception ex) 
            {
                return new OperationResult<int>(false, ex.Message, 0);
            }
        }

        public async Task<IEnumerable<Media>> GetAll() =>
            await _context.Medias
                    .Include(x => x.Author)
                    .Include(x => x.Translations)
                    .ToListAsync();

        public async Task<OperationResult<int>> Create(Media media)
        {
            var result = await _fileService.CreateFile(media);
            if (!result.IsSuccess)
            {
                return result;
            }
            _context.Add(media);
            await _context.SaveChangesAsync();
            return new OperationResult<int>(true, string.Empty, media.Id);
        }

        public async Task<OperationResult<List<int>>> CreateList(List<Media> medias)
        {
            for(int i = 0; i < medias.Count; i++)
            {
                var result = await Create(medias[i]);
                if(!result.IsSuccess)
                {
                    for (int j = i - 1; j >= 0; j--)
                        await _fileService.DeleteFile(medias[j]);
                    return new OperationResult<List<int>>(false, result.Message, null);
                }
            }
            return new OperationResult<List<int>>(true, "Medias has been created successfully", medias.Select(x => x.Id).ToList());
        }
    }
}
