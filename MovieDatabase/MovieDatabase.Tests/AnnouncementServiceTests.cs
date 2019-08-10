using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class AnnouncementServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public AnnouncementServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AnnouncementsProfile());
            });
            this.mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllAnnouncementsShouldReturnEmptyListWithEmptyDb()
        {
            var expectedResult = new List<AnnouncementViewModel>();

            var announcementService = new AnnouncementService(dbContext, mapper);

            var actualResult = await announcementService.GetAllAnnouncementsAsync();

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task GetAllAnnouncementsShouldReturnAllAnnouncementsProperly()
        {
            var announcement1 = new Announcement
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = "image1",
                Date = DateTime.Parse("24 July 2019"),
                OfficialArticleLink = "article1"
            };
            var announcement2 = new Announcement
            {
                Creator = "creator2",
                Title = "title2",
                Content = "content2",
                ImageLink = "image2",
                Date = DateTime.Parse("25 July 2019"),
                OfficialArticleLink = "article2"
            };
            await dbContext.Announcements.AddAsync(announcement1);
            await dbContext.Announcements.AddAsync(announcement2);
            await dbContext.SaveChangesAsync();

            var expectedResult = new List<AnnouncementViewModel>()
            {
                new AnnouncementViewModel
                {
                    Creator = "creator1",
                    Title = "title1",
                    Content = "content1",
                    ImageLink = "image1",
                    Date = DateTime.Parse("24 July 2019"),
                    OfficialArticleLink = "article1"
                },
                new AnnouncementViewModel
                {
                    Creator = "creator2",
                    Title = "title2",
                    Content = "content2",
                    ImageLink = "image2",
                    Date = DateTime.Parse("25 July 2019"),
                    OfficialArticleLink = "article2"
                }
            };

            var announcementService = new AnnouncementService(dbContext, mapper);

            var actualResult = await announcementService.GetAllAnnouncementsAsync();

            for (int i = 0; i < expectedResult.Count(); i++)
            {
                Assert.Equal(expectedResult[i].Creator, actualResult[i].Creator);
                Assert.Equal(expectedResult[i].Title, actualResult[i].Title);
                Assert.Equal(expectedResult[i].Content, actualResult[i].Content);
                Assert.Equal(expectedResult[i].ImageLink, actualResult[i].ImageLink);
                Assert.Equal(expectedResult[i].Date, actualResult[i].Date);
                Assert.Equal(expectedResult[i].OfficialArticleLink, actualResult[i].OfficialArticleLink);
            }
        }

        [Fact]
        public void OrderAnnouncementsShouldReturnAnnouncementsOrderedByNewest()
        {
            var input = new List<AnnouncementViewModel>()
            {
                new AnnouncementViewModel
                {
                    Creator = "creator2",
                    Title = "title2",
                    Content = "content2",
                    ImageLink = "image2",
                    Date = DateTime.Parse("21 July 2019"),
                    OfficialArticleLink = "article2"
                },
                new AnnouncementViewModel
                {
                    Creator = "creator1",
                    Title = "title1",
                    Content = "content1",
                    ImageLink = "image1",
                    Date = DateTime.Parse("24 July 2019"),
                    OfficialArticleLink = "article1"
                },
            };

            var announcementService = new AnnouncementService(dbContext, mapper);

            var actualResult = announcementService.OrderAnnouncements(input, GlobalConstants.announcementsOrderByLatest);

            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderAnnouncementsShouldReturnAnnouncementsOrderedByOldest()
        {
            var input = new List<AnnouncementViewModel>()
            {
                new AnnouncementViewModel
                {
                    Creator = "creator1",
                    Title = "title1",
                    Content = "content1",
                    ImageLink = "image1",
                    Date = DateTime.Parse("24 July 2019"),
                    OfficialArticleLink = "article1"
                },
                new AnnouncementViewModel
                {
                    Creator = "creator2",
                    Title = "title2",
                    Content = "content2",
                    ImageLink = "image2",
                    Date = DateTime.Parse("21 July 2019"),
                    OfficialArticleLink = "article2"
                },
            };

            var announcementService = new AnnouncementService(dbContext, mapper);

            var actualResult = announcementService.OrderAnnouncements(input, GlobalConstants.announcementsOrderByOldest);

            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderAnnouncementsShouldReturnInputIfOrderByIsInvalid()
        {
            var input = new List<AnnouncementViewModel>()
            {
                new AnnouncementViewModel
                {
                    Creator = "creator1",
                    Title = "title1",
                    Content = "content1",
                    ImageLink = "image1",
                    Date = DateTime.Parse("24 July 2019"),
                    OfficialArticleLink = "article1"
                },
                new AnnouncementViewModel
                {
                    Creator = "creator2",
                    Title = "title2",
                    Content = "content2",
                    ImageLink = "image2",
                    Date = DateTime.Parse("21 July 2019"),
                    OfficialArticleLink = "article2"
                },
            };

            var announcementService = new AnnouncementService(dbContext, mapper);

            var actualResult = announcementService.OrderAnnouncements(input, "invalid input");

            Assert.Equal(input.First(), actualResult.First());
            Assert.Equal(input.Last(), actualResult.Last());
        }

        [Fact]
        public async Task CreateAnnouncementShouldAddAnnouncementToDbIfInputIsValid()
        {
            var input = new CreateAnnouncementInputModel
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = "image1",
                OfficialArticleLink = "article1",
            };

            var announcementService = new AnnouncementService(dbContext, mapper);

            Assert.True(await announcementService.CreateAnnouncementAsync(input));
            Assert.True(dbContext.Announcements.Count() == 1);
            Assert.True(dbContext.Announcements.First().Date != null);
        }

        [Fact]
        public async Task CreateAnnouncementShouldReturnFalseIfAnnouncementWithSameTitleAndContentAlreadyExists()
        {
            var input = new CreateAnnouncementInputModel
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = "image1",
                OfficialArticleLink = "article1",
            };

            await dbContext.Announcements.AddAsync(new Announcement
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = "image1",
                OfficialArticleLink = "article1",
                Date = DateTime.UtcNow,
            });
            await dbContext.SaveChangesAsync();

            var announcementService = new AnnouncementService(dbContext, mapper);

            Assert.False(await announcementService.CreateAnnouncementAsync(input));
            Assert.True(dbContext.Announcements.Count() == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public async Task CreateAnnouncementShouldSetImageLinkIfNoneIsProvided(string imageLink)
        {
            var input = new CreateAnnouncementInputModel
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = imageLink,
                OfficialArticleLink = "article1",
            };

            var announcementService = new AnnouncementService(dbContext, mapper);

            Assert.True(await announcementService.CreateAnnouncementAsync(input));
            Assert.True(dbContext.Announcements.Count() == 1);
            Assert.True(dbContext.Announcements.First().ImageLink == GlobalConstants.noImageLink);
        }

        [Fact]
        public async Task DeleteAnnouncementShouldReturnTrueIfValidIdIsGiven()
        {
            var announcement = new Announcement
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = "image1",
                OfficialArticleLink = "article1",
                Date = DateTime.UtcNow,
            };
            await dbContext.Announcements.AddAsync(announcement);
            await dbContext.SaveChangesAsync();

            var id = dbContext.Announcements.First().Id;

            var announcementService = new AnnouncementService(dbContext, mapper);

            Assert.True(dbContext.Announcements.Count() == 1);
            Assert.True(await announcementService.DeleteAnnouncementAsync(id));
            Assert.True(dbContext.Announcements.Count() == 0);
        }

        [Fact]
        public async Task DeleteAnnouncementShouldReturnFalseIfInvalidIdIsGiven()
        {
            var announcement = new Announcement
            {
                Creator = "creator1",
                Title = "title1",
                Content = "content1",
                ImageLink = "image1",
                OfficialArticleLink = "article1",
                Date = DateTime.UtcNow,
            };
            await dbContext.Announcements.AddAsync(announcement);
            await dbContext.SaveChangesAsync();

            var id = "invalid";

            var announcementService = new AnnouncementService(dbContext, mapper);

            Assert.True(dbContext.Announcements.Count() == 1);
            Assert.False(await announcementService.DeleteAnnouncementAsync(id));
            Assert.True(dbContext.Announcements.Count() == 1);
        }

        [Fact]
        public async Task DeleteAnnouncementShouldReturnFalseIfDbIsEmpty()
        {
            var id = "something";

            var announcementService = new AnnouncementService(dbContext, mapper);

            Assert.True(dbContext.Announcements.Count() == 0);
            Assert.False(await announcementService.DeleteAnnouncementAsync(id));
            Assert.True(dbContext.Announcements.Count() == 0);
        }
    }
}
