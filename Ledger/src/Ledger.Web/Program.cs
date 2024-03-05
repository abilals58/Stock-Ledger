using System.Threading.Tasks;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace Ledger.Ledger.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            /*var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            IJobDetail tradeJob = JobBuilder.Create<TradeJob>()
                .WithIdentity("tradeJob", "group1")
                .Build();
            //tradeJob.JobDataMap["sellOrderService"] = _sellOrderService;
            var tradeTrigger = TriggerBuilder.Create()
                .WithIdentity("tradeTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(tradeJob, tradeTrigger);
            await builder.RunAsync();*/
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}