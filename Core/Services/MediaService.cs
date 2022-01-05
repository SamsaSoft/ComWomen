using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class MediaService : IMediaService
    {
        private readonly ApplicationDbContext _context;

        public MediaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Media> Upload(Media media)
        {
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

        public async Task<List<Media>> GetAllWithType(MediaType type) =>
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

        public async Task Update(Media media)
        {
            var dbMedia = await GetById(media.Id);
            foreach (var item in media.Translations)
            {
                var translate = dbMedia[item.LanguageId];
                if (translate != null) 
                {
                    translate.Url = item.Url;
                    translate.Title = item.Title;
                    translate.Description = item.Description;
                }
            }
            _context.Update(dbMedia);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Media>> GetAll() =>
            await _context.Medias
                    .Include(x => x.Author)
                    .Include(x => x.Translations)
                    .ToListAsync();

        public Task<OperationResult<int>> Create(Media media)
        {
            throw new NotImplementedException();
        }
    }
}
