using System.Threading.Tasks;
using Ledger.Ledger.Web.Jobs;
using Quartz;
using Quartz.Impl;

namespace Ledger.Ledger.Web.Services
{
    public class SchedulerService 
    {
        public async Task StartAsync()
        {
            IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<SellTradeJob>()
                .WithIdentity("tradeJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithDailyTimeIntervalSchedule(x =>
                    x.OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(9, 0))
                        .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 0)))
                .Build();
            
            await scheduler.ScheduleJob(job, trigger);
            
        }
    }
}