using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class TVShowService : ITVShowService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public TVShowService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<TVShow> GetAllTVShows()
        {
            return this.dbContext.TVShows.ToList();
        }

        public TVShow GetTVShowById(string tvShowId)
        {
            return this.dbContext.TVShows.Find(tvShowId);
        }
    }
}
