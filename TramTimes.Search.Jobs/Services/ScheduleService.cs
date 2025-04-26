using Quartz;
using TramTimes.Search.Jobs.Workers;

namespace TramTimes.Search.Jobs.Services;

public static class ScheduleService
{
    public static HostApplicationBuilder AddScheduleDefaults(this HostApplicationBuilder builder)
    {
        builder.Services.AddQuartz(configure: quartz =>
        {
            var _9400ZZSYABR1 = new JobKey(name: "9400ZZSYABR1");
            var _9400ZZSYABR2 = new JobKey(name: "9400ZZSYABR2");
            
            quartz.AddJob<_9400ZZSYABR1>(jobKey: _9400ZZSYABR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYABR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYABR2>(jobKey: _9400ZZSYABR2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYABR2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYATT1 = new JobKey(name: "9400ZZSYATT1");
            var _9400ZZSYATT2 = new JobKey(name: "9400ZZSYATT2");
            
            quartz.AddJob<_9400ZZSYATT1>(jobKey: _9400ZZSYATT1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYATT1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYATT2>(jobKey: _9400ZZSYATT2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYATT2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYBAM1 = new JobKey(name: "9400ZZSYBAM1");
            var _9400ZZSYBAM2 = new JobKey(name: "9400ZZSYBAM2");
            
            quartz.AddJob<_9400ZZSYBAM1>(jobKey: _9400ZZSYBAM1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYBAM1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYBAM2>(jobKey: _9400ZZSYBAM2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYBAM2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYBMR1 = new JobKey(name: "9400ZZSYBMR1");
            var _9400ZZSYBMR2 = new JobKey(name: "9400ZZSYBMR2");
            
            quartz.AddJob<_9400ZZSYBMR1>(jobKey: _9400ZZSYBMR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYBMR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYBMR2>(jobKey: _9400ZZSYBMR2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYBMR2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYBRL1 = new JobKey(name: "9400ZZSYBRL1");
            var _9400ZZSYBRL2 = new JobKey(name: "9400ZZSYBRL2");
            
            quartz.AddJob<_9400ZZSYBRL1>(jobKey: _9400ZZSYBRL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYBRL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYBRL2>(jobKey: _9400ZZSYBRL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYBRL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYCAS1 = new JobKey(name: "9400ZZSYCAS1");
            var _9400ZZSYCAS2 = new JobKey(name: "9400ZZSYCAS2");
            
            quartz.AddJob<_9400ZZSYCAS1>(jobKey: _9400ZZSYCAS1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCAS1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYCAS2>(jobKey: _9400ZZSYCAS2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCAS2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYCAT1 = new JobKey(name: "9400ZZSYCAT1");
            var _9400ZZSYCAT2 = new JobKey(name: "9400ZZSYCAT2");
            
            quartz.AddJob<_9400ZZSYCAT1>(jobKey: _9400ZZSYCAT1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCAT1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYCAT2>(jobKey: _9400ZZSYCAT2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCAT2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYCIR1 = new JobKey(name: "9400ZZSYCIR1");
            var _9400ZZSYCIR2 = new JobKey(name: "9400ZZSYCIR2");
            
            quartz.AddJob<_9400ZZSYCIR1>(jobKey: _9400ZZSYCIR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCIR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYCIR2>(jobKey: _9400ZZSYCIR2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCIR2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYCRB1 = new JobKey(name: "9400ZZSYCRB1");
            var _9400ZZSYCRB2 = new JobKey(name: "9400ZZSYCRB2");
            
            quartz.AddJob<_9400ZZSYCRB1>(jobKey: _9400ZZSYCRB1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCRB1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYCRB2>(jobKey: _9400ZZSYCRB2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCRB2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYCRY1 = new JobKey(name: "9400ZZSYCRY1");
            var _9400ZZSYCRY2 = new JobKey(name: "9400ZZSYCRY2");
            
            quartz.AddJob<_9400ZZSYCRY1>(jobKey: _9400ZZSYCRY1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCRY1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYCRY2>(jobKey: _9400ZZSYCRY2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCRY2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYCYH1 = new JobKey(name: "9400ZZSYCYH1");
            var _9400ZZSYCYH2 = new JobKey(name: "9400ZZSYCYH2");
            
            quartz.AddJob<_9400ZZSYCYH1>(jobKey: _9400ZZSYCYH1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCYH1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYCYH2>(jobKey: _9400ZZSYCYH2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYCYH2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYDHL1 = new JobKey(name: "9400ZZSYDHL1");
            var _9400ZZSYDHL2 = new JobKey(name: "9400ZZSYDHL2");
            
            quartz.AddJob<_9400ZZSYDHL1>(jobKey: _9400ZZSYDHL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYDHL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYDHL2>(jobKey: _9400ZZSYDHL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYDHL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYDON1 = new JobKey(name: "9400ZZSYDON1");
            var _9400ZZSYDON2 = new JobKey(name: "9400ZZSYDON2");
            
            quartz.AddJob<_9400ZZSYDON1>(jobKey: _9400ZZSYDON1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYDON1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYDON2>(jobKey: _9400ZZSYDON2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYDON2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYDVS1 = new JobKey(name: "9400ZZSYDVS1");
            var _9400ZZSYDVS2 = new JobKey(name: "9400ZZSYDVS2");
            
            quartz.AddJob<_9400ZZSYDVS1>(jobKey: _9400ZZSYDVS1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYDVS1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYDVS2>(jobKey: _9400ZZSYDVS2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYDVS2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYFIZ1 = new JobKey(name: "9400ZZSYFIZ1");
            var _9400ZZSYFIZ2 = new JobKey(name: "9400ZZSYFIZ2");
            
            quartz.AddJob<_9400ZZSYFIZ1>(jobKey: _9400ZZSYFIZ1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYFIZ1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYFIZ2>(jobKey: _9400ZZSYFIZ2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYFIZ2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYGLE1 = new JobKey(name: "9400ZZSYGLE1");
            var _9400ZZSYGLE2 = new JobKey(name: "9400ZZSYGLE2");
            
            quartz.AddJob<_9400ZZSYGLE1>(jobKey: _9400ZZSYGLE1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYGLE1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYGLE2>(jobKey: _9400ZZSYGLE2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYGLE2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYGRC1 = new JobKey(name: "9400ZZSYGRC1");
            var _9400ZZSYGRC2 = new JobKey(name: "9400ZZSYGRC2");
            
            quartz.AddJob<_9400ZZSYGRC1>(jobKey: _9400ZZSYGRC1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYGRC1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYGRC2>(jobKey: _9400ZZSYGRC2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYGRC2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYHAK1 = new JobKey(name: "9400ZZSYHAK1");
            var _9400ZZSYHAK2 = new JobKey(name: "9400ZZSYHAK2");
            
            quartz.AddJob<_9400ZZSYHAK1>(jobKey: _9400ZZSYHAK1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHAK1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYHAK2>(jobKey: _9400ZZSYHAK2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHAK2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYHDP1 = new JobKey(name: "9400ZZSYHDP1");
            
            quartz.AddJob<_9400ZZSYHDP1>(jobKey: _9400ZZSYHDP1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHDP1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            var _9400ZZSYHFW1 = new JobKey(name: "9400ZZSYHFW1");
            
            quartz.AddJob<_9400ZZSYHFW1>(jobKey: _9400ZZSYHFW1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHFW1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            var _9400ZZSYHIL1 = new JobKey(name: "9400ZZSYHIL1");
            var _9400ZZSYHIL2 = new JobKey(name: "9400ZZSYHIL2");
            
            quartz.AddJob<_9400ZZSYHIL1>(jobKey: _9400ZZSYHIL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHIL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYHIL2>(jobKey: _9400ZZSYHIL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHIL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYHLP1 = new JobKey(name: "9400ZZSYHLP1");
            var _9400ZZSYHLP2 = new JobKey(name: "9400ZZSYHLP2");
            
            quartz.AddJob<_9400ZZSYHLP1>(jobKey: _9400ZZSYHLP1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHLP1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYHLP2>(jobKey: _9400ZZSYHLP2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHLP2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYHLR1 = new JobKey(name: "9400ZZSYHLR1");
            var _9400ZZSYHLR2 = new JobKey(name: "9400ZZSYHLR2");
            
            quartz.AddJob<_9400ZZSYHLR1>(jobKey: _9400ZZSYHLR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHLR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYHLR2>(jobKey: _9400ZZSYHLR2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHLR2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYHOL1 = new JobKey(name: "9400ZZSYHOL1");
            var _9400ZZSYHOL2 = new JobKey(name: "9400ZZSYHOL2");
            
            quartz.AddJob<_9400ZZSYHOL1>(jobKey: _9400ZZSYHOL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHOL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYHOL2>(jobKey: _9400ZZSYHOL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHOL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYHYP1 = new JobKey(name: "9400ZZSYHYP1");
            var _9400ZZSYHYP2 = new JobKey(name: "9400ZZSYHYP2");
            
            quartz.AddJob<_9400ZZSYHYP1>(jobKey: _9400ZZSYHYP1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHYP1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYHYP2>(jobKey: _9400ZZSYHYP2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYHYP2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYINF1 = new JobKey(name: "9400ZZSYINF1");
            var _9400ZZSYINF2 = new JobKey(name: "9400ZZSYINF2");
            
            quartz.AddJob<_9400ZZSYINF1>(jobKey: _9400ZZSYINF1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYINF1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYINF2>(jobKey: _9400ZZSYINF2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYINF2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYLEP1 = new JobKey(name: "9400ZZSYLEP1");
            var _9400ZZSYLEP2 = new JobKey(name: "9400ZZSYLEP2");
            
            quartz.AddJob<_9400ZZSYLEP1>(jobKey: _9400ZZSYLEP1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYLEP1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYLEP2>(jobKey: _9400ZZSYLEP2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYLEP2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYLPH1 = new JobKey(name: "9400ZZSYLPH1");
            var _9400ZZSYLPH2 = new JobKey(name: "9400ZZSYLPH2");
            
            quartz.AddJob<_9400ZZSYLPH1>(jobKey: _9400ZZSYLPH1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYLPH1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYLPH2>(jobKey: _9400ZZSYLPH2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYLPH2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYMAL1 = new JobKey(name: "9400ZZSYMAL1");
            
            quartz.AddJob<_9400ZZSYMAL1>(jobKey: _9400ZZSYMAL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMAL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            var _9400ZZSYMHI1 = new JobKey(name: "9400ZZSYMHI1");
            var _9400ZZSYMHI2 = new JobKey(name: "9400ZZSYMHI2");
            
            quartz.AddJob<_9400ZZSYMHI1>(jobKey: _9400ZZSYMHI1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMHI1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYMHI2>(jobKey: _9400ZZSYMHI2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMHI2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYMHS1 = new JobKey(name: "9400ZZSYMHS1");
            var _9400ZZSYMHS2 = new JobKey(name: "9400ZZSYMHS2");
            
            quartz.AddJob<_9400ZZSYMHS1>(jobKey: _9400ZZSYMHS1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMHS1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYMHS2>(jobKey: _9400ZZSYMHS2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMHS2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYMID1 = new JobKey(name: "9400ZZSYMID1");
            
            quartz.AddJob<_9400ZZSYMID1>(jobKey: _9400ZZSYMID1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMID1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            var _9400ZZSYMRT1 = new JobKey(name: "9400ZZSYMRT1");
            var _9400ZZSYMRT2 = new JobKey(name: "9400ZZSYMRT2");
            
            quartz.AddJob<_9400ZZSYMRT1>(jobKey: _9400ZZSYMRT1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMRT1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYMRT2>(jobKey: _9400ZZSYMRT2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMRT2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYMWY1 = new JobKey(name: "9400ZZSYMWY1");
            var _9400ZZSYMWY2 = new JobKey(name: "9400ZZSYMWY2");
            
            quartz.AddJob<_9400ZZSYMWY1>(jobKey: _9400ZZSYMWY1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMWY1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYMWY2>(jobKey: _9400ZZSYMWY2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYMWY2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYNET1 = new JobKey(name: "9400ZZSYNET1");
            var _9400ZZSYNET2 = new JobKey(name: "9400ZZSYNET2");
            
            quartz.AddJob<_9400ZZSYNET1>(jobKey: _9400ZZSYNET1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYNET1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYNET2>(jobKey: _9400ZZSYNET2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYNET2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYNUN1 = new JobKey(name: "9400ZZSYNUN1");
            var _9400ZZSYNUN2 = new JobKey(name: "9400ZZSYNUN2");
            
            quartz.AddJob<_9400ZZSYNUN1>(jobKey: _9400ZZSYNUN1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYNUN1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYNUN2>(jobKey: _9400ZZSYNUN2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYNUN2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYPAR1 = new JobKey(name: "9400ZZSYPAR1");
            
            quartz.AddJob<_9400ZZSYPAR1>(jobKey: _9400ZZSYPAR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYPAR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            var _9400ZZSYPGC1 = new JobKey(name: "9400ZZSYPGC1");
            var _9400ZZSYPGC2 = new JobKey(name: "9400ZZSYPGC2");
            
            quartz.AddJob<_9400ZZSYPGC1>(jobKey: _9400ZZSYPGC1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYPGC1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYPGC2>(jobKey: _9400ZZSYPGC2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYPGC2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYPGR1 = new JobKey(name: "9400ZZSYPGR1");
            var _9400ZZSYPGR2 = new JobKey(name: "9400ZZSYPGR2");
            
            quartz.AddJob<_9400ZZSYPGR1>(jobKey: _9400ZZSYPGR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYPGR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYPGR2>(jobKey: _9400ZZSYPGR2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYPGR2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYRTH1 = new JobKey(name: "9400ZZSYRTH1");
            var _9400ZZSYRTH2 = new JobKey(name: "9400ZZSYRTH2");
            
            quartz.AddJob<_9400ZZSYRTH1>(jobKey: _9400ZZSYRTH1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYRTH1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYRTH2>(jobKey: _9400ZZSYRTH2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYRTH2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYSHL1 = new JobKey(name: "9400ZZSYSHL1");
            var _9400ZZSYSHL2 = new JobKey(name: "9400ZZSYSHL2");
            
            quartz.AddJob<_9400ZZSYSHL1>(jobKey: _9400ZZSYSHL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYSHL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYSHL2>(jobKey: _9400ZZSYSHL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYSHL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYSHU1 = new JobKey(name: "9400ZZSYSHU1");
            var _9400ZZSYSHU2 = new JobKey(name: "9400ZZSYSHU2");
            
            quartz.AddJob<_9400ZZSYSHU1>(jobKey: _9400ZZSYSHU1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYSHU1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYSHU2>(jobKey: _9400ZZSYSHU2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYSHU2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYSPL1 = new JobKey(name: "9400ZZSYSPL1");
            var _9400ZZSYSPL2 = new JobKey(name: "9400ZZSYSPL2");
            
            quartz.AddJob<_9400ZZSYSPL1>(jobKey: _9400ZZSYSPL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYSPL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYSPL2>(jobKey: _9400ZZSYSPL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYSPL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYUNI1 = new JobKey(name: "9400ZZSYUNI1");
            var _9400ZZSYUNI2 = new JobKey(name: "9400ZZSYUNI2");
            
            quartz.AddJob<_9400ZZSYUNI1>(jobKey: _9400ZZSYUNI1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYUNI1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYUNI2>(jobKey: _9400ZZSYUNI2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYUNI2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYVEN1 = new JobKey(name: "9400ZZSYVEN1");
            var _9400ZZSYVEN2 = new JobKey(name: "9400ZZSYVEN2");
            
            quartz.AddJob<_9400ZZSYVEN1>(jobKey: _9400ZZSYVEN1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYVEN1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYVEN2>(jobKey: _9400ZZSYVEN2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYVEN2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYWBN1 = new JobKey(name: "9400ZZSYWBN1");
            var _9400ZZSYWBN2 = new JobKey(name: "9400ZZSYWBN2");
            
            quartz.AddJob<_9400ZZSYWBN1>(jobKey: _9400ZZSYWBN1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWBN1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYWBN2>(jobKey: _9400ZZSYWBN2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWBN2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYWTF1 = new JobKey(name: "9400ZZSYWTF1");
            var _9400ZZSYWTF2 = new JobKey(name: "9400ZZSYWTF2");
            
            quartz.AddJob<_9400ZZSYWTF1>(jobKey: _9400ZZSYWTF1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTF1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYWTF2>(jobKey: _9400ZZSYWTF2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTF2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYWTL1 = new JobKey(name: "9400ZZSYWTL1");
            var _9400ZZSYWTL2 = new JobKey(name: "9400ZZSYWTL2");
            
            quartz.AddJob<_9400ZZSYWTL1>(jobKey: _9400ZZSYWTL1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTL1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYWTL2>(jobKey: _9400ZZSYWTL2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTL2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYWTR1 = new JobKey(name: "9400ZZSYWTR1");
            var _9400ZZSYWTR2 = new JobKey(name: "9400ZZSYWTR2");
            
            quartz.AddJob<_9400ZZSYWTR1>(jobKey: _9400ZZSYWTR1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTR1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYWTR2>(jobKey: _9400ZZSYWTR2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTR2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
            
            var _9400ZZSYWTS1 = new JobKey(name: "9400ZZSYWTS1");
            var _9400ZZSYWTS2 = new JobKey(name: "9400ZZSYWTS2");
            
            quartz.AddJob<_9400ZZSYWTS1>(jobKey: _9400ZZSYWTS1)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTS1);
                    trigger.WithCronSchedule(cronExpression: "0 * * * * ?");
                });
            
            quartz.AddJob<_9400ZZSYWTS2>(jobKey: _9400ZZSYWTS2)
                .AddTrigger(configure: trigger =>
                {
                    trigger.ForJob(jobKey: _9400ZZSYWTS2);
                    trigger.WithCronSchedule(cronExpression: "30 * * * * ?");
                });
        });
        
        builder.Services.AddQuartzHostedService(configure: options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        return builder;
    }
}