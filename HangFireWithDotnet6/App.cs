using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireWithDotnet6
{
    public class App
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        public App(IBackgroundJobClient backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
        }
        public void Main()
        {
            _backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            _backgroundJobs.Schedule(() => Console.WriteLine("Hello world from Hangfire test!"), TimeSpan.FromMinutes(1));
            RecurringJob.AddOrUpdate("easyjob", () => Console.Write("Easy!"), Cron.Minutely);
        }
    }
}
