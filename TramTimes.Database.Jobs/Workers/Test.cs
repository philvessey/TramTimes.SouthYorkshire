using Quartz;
using TramTimes.Database.Jobs.Workers.Stops;

namespace TramTimes.Database.Jobs.Workers;

public class Test : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        #region Arbourthorne Road (To City)
        
        var jobDetail = JobBuilder.Create<_9400ZZSYABR1>()
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .StartNow()
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        var interval = 10;
        
        #endregion
        
        #region Arbourthorne Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYABR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Attercliffe (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYATT1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Attercliffe (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYATT2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Bamforth Street (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYBAM1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Bamforth Street (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYBAM2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Birley Moor Road (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYBMR1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Birley Moor Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYBMR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Birley Lane (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYBRL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Birley Lane (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYBRL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Castle Square (To Cathedral)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCAS1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Castle Square (From Cathedral)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCAS2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Cathedral (To City Hall)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCAT1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Cathedral (From City Hall)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCAT2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Cricket Inn Road (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCIR1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Cricket Inn Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCIR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Carbrook (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCRB1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Carbrook (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCRB2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Crystal Peaks (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCRY1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Crystal Peaks (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCRY2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region City Hall (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCYH1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region City Hall (To Cathedral)
        
        jobDetail = JobBuilder.Create<_9400ZZSYCYH2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Drake House Lane (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYDHL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Drake House Lane (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYDHL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Donetsk Way (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYDON1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Donetsk Way (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYDON2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Arena/Olympic Legacy Park (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYDVS1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Arena/Olympic Legacy Park (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYDVS2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Fitzalan Sq - Ponds Forge (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYFIZ1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Fitzalan Sq - Ponds Forge (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYFIZ2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Gleadless Townend (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYGLE1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Gleadless Townend (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYGLE2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Granville Rd - Sheffield College (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYGRC1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Granville Rd - Sheffield College (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYGRC2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hackenthorpe (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHAK1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hackenthorpe (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHAK2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Herdings Park (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHDP1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Halfway (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHFW1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hillsborough (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHIL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hillsborough (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHIL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hillsborough Park (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHLP1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hillsborough Park (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHLP2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Herdings - Leighton Road (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHLR1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Herdings - Leighton Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHLR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hollinsend (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHOL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hollinsend (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHOL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hyde Park (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHYP1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Hyde Park (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYHYP2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Infirmary Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYINF1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Infirmary Road (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYINF2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Leppings Lane (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYLEP1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Leppings Lane (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYLEP2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Langsett - Primrose View (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYLPH1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Langsett - Primrose View (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYLPH2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Malin Bridge (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMAL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Meadowhall Interchange (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMHI1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Meadowhall Interchange (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMHI2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Meadowhall South - Tinsley (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMHS1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Meadowhall South - Tinsley (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMHS2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Middlewood (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMID1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Manor Top (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMRT1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Manor Top (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMRT2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Moss Way (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMWY1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Moss Way (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYMWY2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Netherthorpe Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYNET1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Netherthorpe Road (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYNET2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Nunnery Square (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYNUN1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Nunnery Square (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYNUN2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Parkgate (To Sheffield)
        
        jobDetail = JobBuilder.Create<_9400ZZSYPAR1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Park Grange Croft (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYPGC1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Park Grange Croft (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYPGC2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Park Grange (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYPGR1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Park Grange (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYPGR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Rotherham Station (To Sheffield)
        
        jobDetail = JobBuilder.Create<_9400ZZSYRTH1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Rotherham Station (To Parkgate)
        
        jobDetail = JobBuilder.Create<_9400ZZSYRTH2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Shalesmoor (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYSHL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Shalesmoor (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYSHL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Sheffield Stn - Hallam Uni (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYSHU1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Sheffield Stn - Hallam Uni (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYSHU2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Spring Lane (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYSPL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Spring Lane (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYSPL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region University of Sheffield (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYUNI1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region University of Sheffield (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYUNI2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Valley Centertainment (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYVEN1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Valley Centertainment (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYVEN2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Woodbourn Road (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWBN1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Woodbourn Road (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWBN2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Westfield (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTF1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Westfield (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTF2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region White Lane (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTL1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region White Lane (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTL2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Waterthorpe (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTR1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region Waterthorpe (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region West Street (From City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTS1>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        interval += 10;
        
        #endregion
        
        #region West Street (To City)
        
        jobDetail = JobBuilder.Create<_9400ZZSYWTS2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartAt(startTimeUtc: DateBuilder.FutureDate(
                interval: interval,
                unit: IntervalUnit.Second))
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        #endregion
        
        #region Delete Job
        
        await context.Scheduler.DeleteJob(jobKey: context.JobDetail.Key);
        
        #endregion
    }
}