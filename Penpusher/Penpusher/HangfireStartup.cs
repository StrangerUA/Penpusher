// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HangfireStartup.cs" company="Star team">
//   This class created for start timer job
// </copyright>
// <summary>
//   The start job.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Penpusher
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    using Hangfire;

    using Ninject;

    using Owin;

    using Penpusher.Services.ContentService;

    /// <summary>
    /// The start job.
    /// </summary>
    public static class HangfireStartup
    {
        /// <summary>
        /// The job for syncronize articles.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void InitHangfire(this IAppBuilder app)
        {
            var options = new DashboardOptions { AppPath = VirtualPathUtility.ToAbsolute("~") };
            app.UseHangfireDashboard("/jobs", options);
            app.UseHangfireServer(
                new BackgroundJobServerOptions { Activator = new NinjectJobActivator(NinjectWebCommon.Kernel) });

            var artService = NinjectWebCommon.Kernel.Get<IProviderTrackingService>();
            RecurringJob.AddOrUpdate(
                "test add new article",
                () => artService.UpdateArticlesFromNewsProviders(),
                Cron.Daily);
        }
    }
}