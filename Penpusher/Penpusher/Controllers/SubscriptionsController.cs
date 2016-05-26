﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionsController.cs" company="Sigma software">
//   Subscription
// </copyright>
// <summary>
//   Defines the SubscriptionsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Penpusher.Models;

namespace Penpusher.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Services;

    /// <summary>
    /// The subscriptions controller.
    /// </summary>
    [RoutePrefix("api")]
    public class SubscriptionsController : ApiController
    {
        /// <summary>
        /// The _news provider service.
        /// </summary>
        private readonly INewsProviderService _newsProviderService;
        private readonly IUserProviderService _userProviderService;
        public SubscriptionsController(INewsProviderService newsProviderService, IUserProviderService _userProviderService)
        {
            _newsProviderService = newsProviderService;
            this._userProviderService = _userProviderService;
        }

        // GET api/<controller>

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>NewsProvider[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        [Route("getall")]
        //DEFECT: violation dependency inversion principle. method type must be more abstract and return is more specific
        public NewsProvider[] Get()
        {
            return _newsProviderService.GetAll().ToArray();
        }

        // GET api/<controller>/5
        [Route("getallsubscription/{id}")]
        //DEFECT: violation dependency inversion principle. method type must be more abstract and return is more specific
        public UserNewsProviderModels[] GetByUser(int id)
        {
            return _newsProviderService.GetSubscriptionsByUserId(id).ToArray();
        }

        public  NewsProvider GetProviderDetails(int providerId)
        {
           return  _newsProviderService.GetAll().First(np => np.Id == providerId);
        }

        // POST api/<controller>

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="newsProvider">
        /// The news provider.
        /// </param>
        [Route("add")]
        public void Post(NewsProvider newsProvider)
        {
            string link = newsProvider.Link;
            _newsProviderService.Subscription(link);
        }

        [HttpPost]
        public bool SubscribeUserToProvider(int providerId)
        {
            return _userProviderService.SubscribeUserToProvider(providerId, true);
        }

        [HttpPost]
        public bool UnsubscribeUserToProvider(int providerId)
        {
            return _userProviderService.SubscribeUserToProvider(providerId, false);
        }

        [HttpGet]
        public bool IsUserSubscriberToProvider(int providerId)
        {
            var p= _userProviderService.IsUserSubscribedOnProvider(providerId);
            return p;
        }

        // DELETE api/<controller>/5

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.w
        /// </param>
        //DEFECT: its bad practice to return void in API. return bool at least to check does action perform successfully
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _newsProviderService.Unsubscription(id);
        }
    }
}