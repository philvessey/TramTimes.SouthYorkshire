using Quartz;
using TramTimes.Database.Jobs.Workers.Stops;

namespace TramTimes.Database.Jobs.Workers;

public class Test : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        #region arbourthorne road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYABR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartNow()
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYABR2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartNow()
                .Build());
        
        var interval = 10;
        
        #endregion
        
        #region attercliffe
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYATT1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYATT2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region bamforth street
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYBAM1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYBAM2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region birley moor road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYBMR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYBMR2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region birley lane
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYBRL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYBRL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region castle square
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCAS1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCAS2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region cathedral
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCAT1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCAT2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region cricket inn road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCIR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCIR2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region carbrook
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCRB1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCRB2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region crystal peaks
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCRY1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCRY2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region city hall
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCYH1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYCYH2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region drake house lane
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYDHL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYDHL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region donetsk way
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYDON1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYDON2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region arena / olympic legacy park
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYDVS1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYDVS2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region fitzalan sq - ponds forge
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYFIZ1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYFIZ2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region gleadless townend
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYGLE1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYGLE2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region granville rd - sheffield college
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYGRC1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYGRC2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region hackenthorpe
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHAK1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHAK2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region herdings park
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHDP1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region halfway
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHFW1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region hillsborough
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHIL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHIL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region hillsborough park
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHLP1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHLP2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region herdings - leighton road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHLR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHLR2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region hollinsend
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHOL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHOL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region hyde park
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHYP1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYHYP2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region infirmary road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYINF1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYINF2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region leppings lane
        
        await context.Scheduler.ScheduleJob(
            jobDetail:  JobBuilder
                .Create<_9400ZZSYLEP1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYLEP2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region langsett - primrose view
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYLPH1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYLPH2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region malin bridge
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMAL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region meadowhall interchange
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMHI1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMHI2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region meadowhall south - tinsley
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMHS1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMHS2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region middlewood
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMID1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region manor top
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMRT1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMRT2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region moss way
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMWY1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYMWY2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region netherthorpe road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYNET1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYNET2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region nunnery square
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYNUN1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYNUN2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region parkgate
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYPAR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region park grange croft
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYPGC1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYPGC2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region park grange
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYPGR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYPGR2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region rotherham station
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYRTH1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYRTH2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region shalesmoor
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYSHL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYSHL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region sheffield stn - hallam uni
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYSHU1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYSHU2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region spring lane
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYSPL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYSPL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region university of sheffield
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYUNI1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYUNI2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region valley centertainment
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYVEN1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYVEN2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region woodbourn road
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWBN1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWBN2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region westfield
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTF1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTF2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region white lane
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTL1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTL2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region waterthorpe
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTR1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTR2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        interval += 10;
        
        #endregion
        
        #region west street
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTS1>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        await context.Scheduler.ScheduleJob(
            jobDetail: JobBuilder
                .Create<_9400ZZSYWTS2>()
                .Build(),
            trigger: TriggerBuilder
                .Create()
                .StartAt(startTimeUtc: DateBuilder.FutureDate(
                    interval: interval,
                    unit: IntervalUnit.Second))
                .Build());
        
        #endregion
        
        #region delete job
        
        await context.Scheduler.DeleteJob(jobKey: context.JobDetail.Key);
        
        #endregion
    }
}