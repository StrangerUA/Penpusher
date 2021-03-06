﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ninject;
using NUnit.Framework;
using Penpusher.Models;
using Penpusher.Services;

namespace Penpusher.Test.Services
{
    [TestFixture]
    public class NewsProviderServiceTest : TestBase
    {
        [SetUp]
        public override void Testinitialize()
        {
            base.Testinitialize();
            MockKernel.Bind<INewsProviderService>().To<NewsProviderService>();
            MockKernel.GetMock<IRepository<UsersNewsProvider>>().Reset();
        }

        [Category("NewsProviderService")]
        [TestCase(2, TestName = "Should find 2 providers")]
        public void GetSubscriptionsByUserIdTest(int expected)
        {
            // arrange
            var usernewsprovider = new List<UsersNewsProvider>
            {
                new UsersNewsProvider { Id = 1, IdNewsProvider = 1, IdUser = 1, NewsProvider = new NewsProvider { Id = 1, Description = "firstfirstfirstfirstfirst", Name = "first" } },
                new UsersNewsProvider { Id = 2, IdNewsProvider = 1, IdUser = 2, NewsProvider = new NewsProvider { Id = 2, Description = "secondsecondsecondsecond", Name = "second" } },
                new UsersNewsProvider { Id = 3, IdNewsProvider = 3, IdUser = 1, NewsProvider = new NewsProvider { Id = 3, Description = "thirdthirdthirdthirdthird", Name = "third" } },
                new UsersNewsProvider { Id = 3, IdNewsProvider = 4, IdUser = 4, NewsProvider = new NewsProvider { Id = 3, Description = "thirdthirdthirdthirdthird", Name = "third" } },
                new UsersNewsProvider { Id = 3, IdNewsProvider = 8, IdUser = 5, NewsProvider = new NewsProvider { Id = 3, Description = "thirdthirdthirdthirdthird", Name = "third" } },
                new UsersNewsProvider { Id = 3, IdNewsProvider = 3, IdUser = 5, NewsProvider = new NewsProvider { Id = 3, Description = "thirdthirdthirdthirdthird", Name = "third" } }
            };

            MockKernel.GetMock<IRepository<UsersNewsProvider>>().Setup(rm => rm.GetAll()).Returns(usernewsprovider);

            // act
            IEnumerable<UserNewsProviderModels> result = MockKernel.Get<INewsProviderService>().GetSubscriptionsByUserId();

            // assert
            int actual = result.Count();
            Assert.AreEqual(actual, expected);
        }

        [Category("NewsProviderService")]
        [TestCase(0, TestName = "Delete subscription with id 0")]
        public void DeleteTest(int id)
        {
            // arrange
            var usernewsprovider = new List<UsersNewsProvider>()
            {
                new UsersNewsProvider { Id = 1, IdNewsProvider = 1, IdUser = 1 },
                new UsersNewsProvider { Id = 2, IdNewsProvider = 1, IdUser = 2 },
                new UsersNewsProvider { Id = 3, IdNewsProvider = 3, IdUser = 1 }
            };

            MockKernel.GetMock<IRepository<UsersNewsProvider>>().Setup(rm => rm.GetAll()).Returns(usernewsprovider);

            // act
            MockKernel.Get<INewsProviderService>().Unsubscription(id);

            IEnumerable<UserNewsProviderModels> result = MockKernel.Get<INewsProviderService>().GetSubscriptionsByUserId();

            // assert
            Assert.IsEmpty(result);
        }

        [Category("NewsProviderService")]
        [TestCase("link1", "link1", "link1", TestName = "Add new subscription")]
        public void AddSubscriptionTest(string link, string name, string description)
        {
            var channel = new NewsProvider { Link = "link1", Id = 1 };
            var subscription = new UsersNewsProvider
            {
                Id = 2,
                IdNewsProvider = 1,
                IdUser = 4
            };

            MockKernel.GetMock<IRepository<NewsProvider>>().Setup(repo => repo.GetAll()).Returns(new List<NewsProvider>
            {
                channel
            });

            MockKernel.GetMock<IRepository<UsersNewsProvider>>()
                .Setup(repo => repo.GetAll())
                .Returns(new List<UsersNewsProvider>
                {
                    new UsersNewsProvider { Id = 1, IdNewsProvider = 2, IdUser = 1 }
                });
            MockKernel.GetMock<IRepository<UsersNewsProvider>>()
                .Setup(repos => repos.Add(It.IsAny<UsersNewsProvider>()))
                .Returns(subscription);
            //// act
            UsersNewsProvider actual = MockKernel.Get<INewsProviderService>().Subscription(link, name, description);

            ////assert
            Assert.AreEqual(actual.Id, subscription.Id);
            Assert.AreEqual(actual.IdNewsProvider, channel.Id);
        }

        [Category("NewsProviderService")]
        [TestCase("link1", "link1", "link1", TestName = "Add new subscription and channel")]
        public void AddSubscriptionAndChannelTest(string link, string name, string description)
        {
            var channel = new NewsProvider { Link = "link1", Id = 1 };
            var subscription = new UsersNewsProvider
            {
                Id = 2,
                IdNewsProvider = 1,
                IdUser = 4
            };

            Mock<IRepository<NewsProvider>> newsProviderRepositoryMock = MockKernel.GetMock<IRepository<NewsProvider>>();
            newsProviderRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<NewsProvider>());
            newsProviderRepositoryMock.Setup(repo => repo.Add(It.IsAny<NewsProvider>())).Returns(channel);

            MockKernel.GetMock<IRepository<UsersNewsProvider>>()
                .Setup(repo => repo.GetAll())
                .Returns(new List<UsersNewsProvider>
                {
                    new UsersNewsProvider { Id = 1, IdNewsProvider = 2, IdUser = 1 }
                });
            MockKernel.GetMock<IRepository<UsersNewsProvider>>()
                .Setup(repos => repos.Add(It.IsAny<UsersNewsProvider>()))
                .Returns(subscription);
            UsersNewsProvider actual = MockKernel.Get<INewsProviderService>().Subscription(link, name, description);

            Assert.AreEqual(actual.Id, subscription.Id);
            Assert.AreEqual(actual.IdNewsProvider, channel.Id);
            newsProviderRepositoryMock.Verify(repo => repo.Add(It.IsAny<NewsProvider>()), Times.Once);
        }

        [Category("NewsProviderService")]
        [TestCase(1, TestName = "Check if LastBuildDate is updated for newsProvider")]
        public void UpdateLastBuildDateForNewsProviderTest(int id)
        {
            var testDate = new DateTime(2016, 7, 1, 13, 2, 1);

            var newsproviders = new List<NewsProvider>
            {
                new NewsProvider { Id = 1, Description = "qwe", LastBuildDate = new DateTime(2016, 6, 1, 4, 5, 4), Link = "sdf" },
                new NewsProvider { Id = 3, Description = "desc1", LastBuildDate = new DateTime(2016, 5, 1, 3, 5, 14), Link = "sdf" },
                new NewsProvider { Id = 5, Description = "desc2", LastBuildDate = new DateTime(2016, 6, 2, 10, 5, 9), Link = "sdfqwe" }
            };
            MockKernel.GetMock<IRepository<NewsProvider>>().Setup(usrv => usrv.GetById(It.IsAny<int>())).Returns(newsproviders.First(np => np.Id == id));
            MockKernel.GetMock<IRepository<NewsProvider>>().Setup(usrv => usrv.Edit(It.IsAny<NewsProvider>())).Callback(
                (NewsProvider newsProvider) => { newsproviders.Find(np => np.Id == newsProvider.Id); });

            MockKernel.Get<INewsProviderService>().UpdateLastBuildDateForNewsProvider(id, testDate);
            Assert.AreEqual(newsproviders.First(np => np.Id == id).LastBuildDate, testDate);
        }
    }
}