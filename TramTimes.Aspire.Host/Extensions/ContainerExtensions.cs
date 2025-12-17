using Aspire.Hosting.Pipelines;
using CliWrap;
using Microsoft.Extensions.Logging;

namespace TramTimes.Aspire.Host.Extensions;

public static class ContainerExtensions
{
    public static void AddPipeline(
        this IDistributedApplicationBuilder builder,
        string? resourceName,
        string? stepName) {

        ArgumentNullException.ThrowIfNull(argument: resourceName);
        ArgumentNullException.ThrowIfNull(argument: stepName);

        builder.Pipeline.AddStep(
            name: stepName,
            dependsOn: WellKnownPipelineSteps.Deploy,
            action: async context =>
            {
                #region build command

                var command = Cli
                    .Wrap(targetFilePath: "az")
                    .WithArguments(configure: arguments => arguments
                        .Add(value: "containerapp")
                        .Add(value: "job")
                        .Add(value: "start")
                        .Add(value: "--name")
                        .Add(value: resourceName)
                        .Add(value: "--resource-group")
                        .Add(value: builder.Configuration["Azure:ResourceGroup"] ?? "tramtimes-southyorkshire")
                    );

                #endregion

                #region build output

                command.WithStandardOutputPipe(target: PipeTarget.ToDelegate(handleLine: line =>
                    context.Logger.LogInformation(
                        message: "{output}",
                        args: line)));

                #endregion

                #region build error

                command.WithStandardErrorPipe(target: PipeTarget.ToDelegate(handleLine: line =>
                    context.Logger.LogError(
                        message: "{error}",
                        args: line)));

                #endregion

                await command.ExecuteAsync();
            }
        );
    }
}