using Core.Enums;
using Core.Interfaces;
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
                .Include(x => x.MediaTranslations)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (media is null)
                throw new KeyNotFoundException($"Медиа с таким ключем не найдено");
            return media;
        }

        public async Task<List<Media>> GetAllWithType(MediaTypeEnum type) =>
            await _context.Medias
            .Where(x => x.MediaTypeId == type)
            .Include(x => x.Author)
            .Include(x => x.MediaTranslations)
            .ToListAsync();

        public async Task DelteById(int id)
        {
            var media = await GetById(id);
            _context.Medias.Remove(media);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Media media)
        {
            _context.Update(media);
            await _context.SaveChangesAsync();
        }

        public string LanguageIdToCode(LanguageEnum language)
        {
            var lang = _context.Languages.Find(language);
            if (lang == null)
                throw new KeyNotFoundException("This language is not in the database");
            return lang.LanguageCode;
        }
    }
}
