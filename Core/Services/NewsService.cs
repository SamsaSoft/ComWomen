using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;
        private readonly MediaService _mediaService;

        public NewsService(ApplicationDbContext context, MediaService mediaService)
        {
            _context = context;
            _mediaService = mediaService;
        }

        public async Task<OperationResult<int>> Create(News news)
        {
            news.CreatedAt = DateTime.UtcNow;

            foreach (var medias in news.Translations.Select(x => x.Media))
            {
                var result = await _mediaService.CreateList(medias);
                if (!result.IsSuccess)
                    return new OperationResult<int>(false, result.Message, 0);
            }

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return new OperationResult<int>(true, "The news created successfully", news.Id);
        }

        public async Task<OperationResult<bool>> DeleteById(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news is null)
                return new OperationResult<bool>(false, $"News entry with id {id} not found", false);
            
            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return new OperationResult<bool>(true, $"The news deleted successfuly!", true);
        }

        public async Task<List<News>> GetAllByLanguage(Language language)
        {
            return await _context.News.Include(x => x.Translations.Where(t => t.Language == language)).ToListAsync();
        }
        
        public async Task<News> GetAllById(int id)
        {
            return await _context.News.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<OperationResult<bool>> Update(News news)
        {
            var entity = await _context.News.FindAsync(news.Id);
            
            entity.EditedAt = DateTime.UtcNow;
            entity.Translations = new List<NewsTranslation>(news.Translations);

            await _context.SaveChangesAsync();

            return new OperationResult<bool>(true, "The news updated successfully", true);
        }

        public async Task<OperationResult<News>> GetById(int id)
        {
            var news = await _context.News.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);

            if (news is null)
                return new OperationResult<News>(false, "News not found", null);
            else
                return new OperationResult<News>(true, null, news);

        }
    }
}
