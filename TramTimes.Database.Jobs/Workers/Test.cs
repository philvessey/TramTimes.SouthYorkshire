using Quartz;
using TramTimes.Database.Jobs.Workers.Stops;

namespace TramTimes.Database.Jobs.Workers;

public class Test : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        #region Arbourthorne Road
        
        var jobDetail = JobBuilder.Create<_9400ZZSYABR1>()
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .StartNow()
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        jobDetail = JobBuilder.Create<_9400ZZSYABR2>()
            .Build();
        
        trigger = TriggerBuilder.Create()
            .StartNow()
            .Build();
        
        await context.Scheduler.ScheduleJob(
            jobDetail: jobDetail,
            trigger: trigger);
        
        var interval = 10;
        
        #endregion
        
        #region Attercliffe
        
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
        
        #region Bamforth Street
        
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
        
        #region Birley Moor Road
        
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
        
        #region Birley Lane
        
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
        
        #region Castle Square
        
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
        
        #region Cathedral
        
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
        
        #region Cricket Inn Road
        
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
        
        #region Carbrook
        
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
        
        #region Crystal Peaks
        
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
        
        #region City Hall
        
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
        
        #region Drake House Lane
        
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
        
        #region Donetsk Way
        
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
        
        #region Arena/Olympic Legacy Park
        
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
        
        #region Fitzalan Sq - Ponds Forge
        
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
        
        #region Gleadless Townend
        
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
        
        #region Granville Rd - Sheffield College
        
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
        
        #region Hackenthorpe
        
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
        
        #region Herdings Park
        
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
        
        #region Halfway
        
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
        
        #region Hillsborough
        
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
        
        #region Hillsborough Park
        
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
        
        #region Herdings - Leighton Road
        
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
        
        #region Hollinsend
        
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
        
        #region Hyde Park
        
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
        
        #region Infirmary Road
        
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
        
        #region Leppings Lane
        
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
        
        #region Langsett - Primrose View
        
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
        
        #region Malin Bridge
        
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
        
        #region Meadowhall Interchange
        
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
        
        #region Meadowhall South - Tinsley
        
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
        
        #region Middlewood
        
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
        
        #region Manor Top
        
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
        
        #region Moss Way
        
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
        
        #region Netherthorpe Road
        
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
        
        #region Nunnery Square
        
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
        
        #region Parkgate
        
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
        
        #region Park Grange Croft
        
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
        
        #region Park Grange
        
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
        
        #region Rotherham Station
        
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
        
        #region Shalesmoor
        
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
        
        #region Sheffield Stn - Hallam Uni
        
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
        
        #region Spring Lane
        
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
        
        #region University of Sheffield
        
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
        
        #region Valley Centertainment
        
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
        
        #region Woodbourn Road
        
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
        
        #region Westfield
        
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
        
        #region White Lane
        
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
        
        #region Waterthorpe
        
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
        
        #region West Street
        
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