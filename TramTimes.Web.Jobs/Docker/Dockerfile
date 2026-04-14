FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS builder
WORKDIR /src

COPY ["Directory.Build.props", "Directory.Packages.props", "./"]
COPY ["TramTimes.Web.Jobs/TramTimes.Web.Jobs.csproj", "TramTimes.Web.Jobs/"]
RUN dotnet restore "TramTimes.Web.Jobs/TramTimes.Web.Jobs.csproj"

COPY . .
RUN dotnet build "TramTimes.Web.Jobs/TramTimes.Web.Jobs.csproj" -c Release -o /app/build
RUN dotnet publish "TramTimes.Web.Jobs/TramTimes.Web.Jobs.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/playwright/dotnet:v1.59.0-noble AS runtime

RUN apt-get update
RUN apt-get install -y dotnet-runtime-10.0
RUN apt-get install -y xvfb
RUN rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=builder /app/publish .

RUN chown -R pwuser:pwuser /app
USER pwuser

ENTRYPOINT ["sh", "-c", "xvfb-run -a -s '-screen 0 1920x1080x24' dotnet TramTimes.Web.Jobs.dll"]