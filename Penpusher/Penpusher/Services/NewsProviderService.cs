﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProviderService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Penpusher.Services.Base;
namespace Penpusher.Services
{


    /// <summary>
    /// The provider service.
    /// </summary>
    public class NewsProviderService : INewsProviderService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository<NewsProvider> newsProviderRepository;
        private readonly IRepository<UsersNewsProvider> usersNewsProvider;

        public NewsProviderService(IRepository<NewsProvider> repository, IRepository<UsersNewsProvider> usersNewsProvider)
        {
            this.newsProviderRepository = repository;
            this.usersNewsProvider = usersNewsProvider;
        }
        public IEnumerable<NewsProvider> GetAll()
        {
            IEnumerable<NewsProvider> newsprovider = newsProviderRepository.GetAll().Select(n => new NewsProvider
            {
                Id = n.Id,
                Name = n.Name,
                Description = n.Description,
                Link = n.Link,
                RssImage = n.RssImage,
                SubscriptionDate = n.SubscriptionDate
            });

            return newsprovider;
        }

        public IEnumerable<NewsProvider> GetByUserId(int id)
        {
            IEnumerable<NewsProvider> news = usersNewsProvider.GetAll().Where(_ => _.IdUser == id)
                .Select(un => new NewsProvider
                {
                    Id = un.Id,
                    Name = un.NewsProvider.Name,
                    Description = un.NewsProvider.Description,
                    Link = un.NewsProvider.Link,
                    RssImage = un.NewsProvider.RssImage,
                    SubscriptionDate = un.NewsProvider.SubscriptionDate
                });
            return news;
        }

        /// <summary>
        /// The add provider.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="NewsProvider"/>.
        /// </returns>
        public NewsProvider AddNewsProvider(NewsProvider provider)
        {

            return newsProviderRepository.Add(provider);
        }

        /// <summary>
        /// The delete news provider.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void DeleteNewsProvider(int id)
        {
            usersNewsProvider.Delete(id);  
        }

        /// <summary>
        /// The add subscription news provider.
        /// </summary>
        /// <param name="link">
        /// The link.
        /// </param>
        /// <returns>
        /// The <see cref="UsersNewsProvider"/>.
        /// </returns>
        public UsersNewsProvider AddSubscription(string link)
        {
            var channel = newsProviderRepository.GetAll().FirstOrDefault(rm => rm.Link == link);

            if (channel == null)
            {
                channel = new NewsProvider
                {
                    Link = link, Name = "test", Description = "test", SubscriptionDate = DateTime.Today
                };

                channel = AddNewsProvider(channel);
            }

            var subscription = usersNewsProvider.GetAll().Count(rm => rm.IdNewsProvider == channel.Id);

            if (subscription == 0)
            {
                return usersNewsProvider.Add(new UsersNewsProvider
                {
                    IdNewsProvider = channel.Id,
                    IdUser = 4
                });
          }
            return new UsersNewsProvider();
        }
    }

}