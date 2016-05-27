﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ninject;
using NUnit.Framework;
using Penpusher.Services;

namespace Penpusher.Test.Services
{
    [TestFixture]
    public class UserArticlesServiceTest : TestBase
    {
        private readonly List<UsersArticle> testArticles = new List<UsersArticle>
            {
                new UsersArticle { Id = 1, UserId = 1, ArticleId = 5, IsRead = false, IsFavorite = true, IsToReadLater = true, Article = new Article { Id = 5 } },
                new UsersArticle { Id = 2, UserId = 1, ArticleId = 15, IsRead = true, IsFavorite = true, IsToReadLater = true, Article = new Article { Id = 15 } },
                new UsersArticle { Id = 3, UserId = 2, ArticleId = 15, IsRead = true, IsFavorite = false, IsToReadLater = false, Article = new Article { Id = 15 } },
                new UsersArticle { Id = 4, UserId = 2, ArticleId = 6, IsRead = false, IsFavorite = false, IsToReadLater = true,  Article = new Article { Id = 6 } },
                new UsersArticle { Id = 4, UserId = 3, ArticleId = 6, IsRead = false, IsFavorite = false, IsToReadLater = false, Article = new Article { Id = 6 } }
            };

        [SetUp]
        public override void Testinitialize()
        {
            base.Testinitialize();
            MockKernel.Bind<IUsersArticlesService>().To<UsersArticlesService>();
            MockKernel.GetMock<IRepository<UsersArticle>>().Reset();
        }

        [Category("UserArticlesService")]
        [TestCase(1, TestName = "For existing user, that has read articles")]
        [TestCase(3, TestName = "For existing user, that hasn't read articles")]
        [TestCase(5, TestName = "For not existing user")]
        public void GetUsersReadArticlesTest(int userId)
        {
            MockKernel.GetMock<IRepository<UsersArticle>>().Setup(usrv => usrv.GetAll()).Returns(testArticles);
            int expected = testArticles.Count(ua => ua.IsRead == true && ua.UserId == userId);
            int actual = MockKernel.Get<IUsersArticlesService>().GetUsersReadArticles().Count();
            Assert.AreEqual(expected, actual);
        }

        [Category("UserArticlesService")]
        [TestCase(1, 2, TestName = "For existing user, that has favorite articles")]
        [TestCase(3, 0, TestName = "Should get zero articles for existing user, that hasn't favorite articles")]
        [TestCase(5, 0, TestName = "Should get zero articles for not existing user")]
        public void GetUsersFavoriteArticlesTest(int userId, int expected)
        {
            var testArticles2 = new List<UsersArticle>
            {
                new UsersArticle
                {
                    Id = 1,
                    UserId = 1,
                    ArticleId = 5,
                    IsFavorite = false,
                    Article = new Article
                    {
                        Id = 5,
                        Title = "first",
                        Date = DateTime.Now,
                        Description = "firstfirstfirst",
                        Image = "link",
                        Link = "linklink"
                    }
                },
                new UsersArticle
                {
                    Id = 2,
                    UserId = 1,
                    ArticleId = 153,
                    IsFavorite = true,
                    Article = new Article
                    {
                        Id = 153,
                        Title = "second",
                        Date = DateTime.Now,
                        Description = "secondsecondsecondsecond",
                        Image = "link",
                        Link = "linklink"
                    }
                },
                new UsersArticle
                {
                    Id = 3,
                    UserId = 1,
                    ArticleId = 15,
                    IsFavorite = true,
                    Article = new Article
                    {
                        Id = 15,
                        Title = "second",
                        Date = DateTime.Now,
                        Description = "secondsecondsecondsecond",
                        Image = "link",
                        Link = "linklink"
                    }
                },
                new UsersArticle
                {
                    Id = 4,
                    UserId = 1,
                    ArticleId = 6,
                    IsFavorite = false,
                    Article = new Article
                    {
                        Id = 15,
                        Title = "third",
                        Date = DateTime.Now,
                        Description = "thirdthirdthirdthird",
                        Image = "link",
                        Link = "linklink"
                    }
                },
                new UsersArticle
                {
                    Id = 4,
                    UserId = 3,
                    ArticleId = 63,
                    IsFavorite = false,
                    Article = new Article
                    {
                        Id = 15,
                        Title = "fourth",
                        Date = DateTime.Now,
                        Description = "fourthfourthfourthfourth",
                        Image = "link",
                        Link = "linklink"
                    }
                }
            };

            MockKernel.GetMock<IRepository<UsersArticle>>().Setup(usrv => usrv.GetAll()).Returns(testArticles2);
            int actual = MockKernel.Get<IUsersArticlesService>().GetUsersFavoriteArticles().Count();
            Assert.AreEqual(expected, actual);
        }

        [Category("UserArticlesService")]
        [TestCase(1, 5, TestName = "For existin UserArticle item with IsRead==false")]
        [TestCase(1, 15, TestName = "For existin UserArticle item with IsRead==true")]
        [TestCase(1, 7, TestName = "For not existin UserArticle")]
        public void MarkAsReadTest(int userId, int articleId)
        {
            MockKernel.GetMock<IRepository<UsersArticle>>().Setup(usrv => usrv.GetAll()).Returns(testArticles);
            MockKernel.GetMock<IRepository<UsersArticle>>()
                .Setup(edit => edit.Edit(It.IsAny<UsersArticle>()))
                .Callback((UsersArticle article) => { testArticles.Add(article); });
            MockKernel.Get<IUsersArticlesService>().MarkAsRead(articleId);

            bool actual = MockKernel.Get<IUsersArticlesService>().GetUsersReadArticles().Any(ua => ua.Id == articleId);
            Assert.AreEqual(false, actual);
        }

        [Category("UserArticlesService")]
        [TestCase(2, 6, true, TestName = "Add to favorite for existing UserArticle item with IsFavorite==false")]
        [TestCase(1, 5, true, TestName = "Add to favorite for existing UserArticle item with IsFavorite==true")]
        [TestCase(2, 5, true, TestName = "Add to favorite for not existing UserArticle")]
        [TestCase(4, 3, false, TestName = "Remove from existing UserArticle item with IsFavorite==false")]
        [TestCase(4, 2, false, TestName = "Remove from existing UserArticle item with IsFavorite==true")]
        [TestCase(12, 5, false, TestName = "Remove from not existing UserArticle")]
        public void AddRemoveFavoritesTest(int userId, int articleId, bool favoriteFlag)
        {
            MockKernel.GetMock<IRepository<UsersArticle>>().Setup(usrv => usrv.GetAll()).Returns(testArticles);
            MockKernel.GetMock<IRepository<UsersArticle>>()
                .Setup(edit => edit.Edit(It.IsAny<UsersArticle>()))
                .Callback((UsersArticle article) => { testArticles.Add(article); });

            MockKernel.Get<IUsersArticlesService>().AddRemoveFavorites(articleId, favoriteFlag);
            UsersArticle actual = testArticles.First(ua => ua.UserId == userId && ua.ArticleId == articleId);
            Assert.AreEqual(favoriteFlag, actual.IsFavorite);
        }
    }
}