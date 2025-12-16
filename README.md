# TramTimes.SouthYorkshire

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A comprehensive tram information system for South Yorkshire, built with .NET and Aspire. This solution provides
web and API services for accessing tram schedules, service updates, and stop information with caching, search
indexing, and database management capabilities.

## 📋 Table of Contents

- [Overview](#-overview)
- [Projects](#-projects)
- [Prerequisites](#-prerequisites)
- [Getting Started (Local)](#-getting-started-local)
- [Getting Started (Remote)](#-getting-started-remote)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [License](#-license)

## 🎯 Overview

This repository contains a distributed application architecture for managing and serving tram information:

- 🌐 Website and API for accessing tram schedules and stop information
- 🔄 Background jobs for data caching, database updates, and search indexing
- 🚀 Aspire orchestration for cloud-native deployment
- 🔍 Elasticsearch powered search functionality
- 💾 PostgreSQL database with Redis caching layer
- ☁️ Azure Storage integration for blob and table data

## 📦 Projects

### TramTimes.Aspire.Host

The Aspire orchestration host that configures and manages all services, infrastructure dependencies, and
distributed application architecture.

**Key Features:**

- 🚀 Aspire orchestration and service discovery
- 🔧 Infrastructure resource management (PostgreSQL, Redis, Elasticsearch, Azure Storage)
- ⚙️ Development, Testing and Production environment configurations

### TramTimes.Aspire.Services

Shared service definitions and extensions for Aspire integration across all projects.

**Key Features:**

- 🔌 Common service configurations
- 📡 Service defaults and telemetry
- 🛠 Reusable extensions

### TramTimes.Cache.Jobs

Background worker service that manages data caching operations using Quartz.NET schedulers. Ensures real-time tram
service and trip data is cached efficiently.

**Key Features:**

- ⏰ Scheduled cache refresh jobs
- 🔄 Real-time data synchronization
- 🚊 Stop-specific caching strategies

### TramTimes.Database.Jobs

Background worker service for database management tasks. Handles scheduled updates, data imports, and database
maintenance operations.

**Key Features:**

- 🗄️ Database update scheduling
- 📊 GTFS data creation and processing
- 🔄 Schedule synchronization

### TramTimes.Search.Jobs

Background worker service that manages Elasticsearch indexing operations. Keeps search indices up-to-date with the
latest stop and service information.

**Key Features:**

- 🔍 Search index management
- 📇 Stop and service indexing
- 🔄 Real-time index updates

### TramTimes.Web.Api

RESTful API service providing programmatic access to tram schedules and stop data.

**Key Features:**

- 🌐 RESTful endpoints for tram data
- 🔐 Health checks and monitoring
- 🚀 Fast API responses with caching

### TramTimes.Web.Site

Blazor web application providing an interactive user interface for accessing tram schedules and stop information.
Offers scheduled and stop updates with responsive design for an optimal user experience across devices.

**Key Features:**

- 🎨 Modern Blazor UI
- 📱 Responsive design
- 💾 Local storage cache

### TramTimes.Web.Tests

Comprehensive playwright test suite for website pages including validation of API endpoints and core
functionality. Screenshots uploaded to blob storage for review.

**Key Features:**

- ✅ Unit and integration tests
- 🧪 API endpoint testing
- 📸 Screenshot capture and storage

### TramTimes.Web.Utilities

Shared utilities, extensions, and helper classes used across web projects.

**Key Features:**

- 🛠 Common utility functions
- 📐 Extension methods
- 🔧 Reusable tools

## ✅ Prerequisites

- [Git](https://git-scm.com/downloads)
- [GitHub CLI](https://cli.github.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Aspire CLI](https://aspire.dev/get-started/install-cli/)
- [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell)
- [Telerik UI for Blazor](https://www.telerik.com/blazor-ui)
- [Traveline Data Set](https://www.travelinedata.org.uk/traveline-open-data/traveline-national-dataset/)

## 🚀 Getting Started (Local)

### Clone the Repository

```bash
git clone https://github.com/philvessey/TramTimes.SouthYorkshire.git
cd TramTimes.SouthYorkshire
```

### Add Custom Package Source

```bash
dotnet new nugetconfig
```

### Copy Telerik License

```bash
cp /path/to/your/telerik-license.txt TramTimes.SouthYorkshire/telerik-license.txt
```

### Add Custom Telerik Source

```bash
dotnet nuget add source "https://nuget.telerik.com/v3/index.json" --name "telerik" --username "api-key" --password "<your-api-key>" --configfile "./nuget.config" --store-password-in-clear-text
```

### Update Custom Package Source

```bash
sed -i '/<\/configuration>/e sed "s/^/  /" nuget.xml; echo' ./nuget.config
```

### Run Aspire Host

```bash
aspire run
```

## 🚀 Getting Started (Remote)

### Authenticate Infrastructure

```bash
ssh -i <public-key> root@<server-address>
```

### Setup Infrastructure

```bash
nano setup.sh
sudo bash setup.sh
```

### Authenticate Infrastructure

```bash
ssh -i <public-key> admin@<server-address>
```

### Build Cache Infrastructure

```bash
nano cache.yml
nano cache.sh
sudo bash cache.sh
```

### Build Search Infrastructure

```bash
nano search.yml
nano search.sh
sudo bash search.sh
```

### Build Server Infrastructure

```bash
nano server.yml
nano server.sh
sudo bash server.sh
```

### Deploy Aspire Host

```bash
aspire deploy
```

## 🛠 Technology Stack

### Core Framework

- **.NET 10.0** - Core framework
- **C# 13** - Programming language
- **Aspire 13** - Cloud-native orchestration

### Web Technologies

- **ASP.NET Core** - Web API and hosting
- **Blazor** - Interactive web UI
- **SignalR** - Real-time communication

### Data & Storage

- **PostgreSQL** - Primary database
- **Redis** - Caching layer
- **Elasticsearch** - Search indexing

### Background Processing

- **Quartz.NET** - Job scheduling and execution
- **Hosted Services** - Background workers
- **Azure Containers** - Cloud deployment

### Resilience & Quality

- **Polly** - Resilience and transient-fault-handling
- **Health Checks** - Service monitoring
- **Telemetry** - OpenTelemetry integration

### UI & Utilities

- **Telerik UI for Blazor** - UI components
- **Blazored.LocalStorage** - Local storage
- **Playwright** - Website end-to-end testing

## 📁 Project Structure

```
TramTimes.SouthYorkshire/
├── TramTimes.Aspire.Host/        # Aspire host application
├── TramTimes.Aspire.Services/    # Aspire service defaults
├── TramTimes.Cache.Jobs/         # Redis cache jobs
├── TramTimes.Database.Jobs/      # PostgreSQL database jobs
├── TramTimes.Search.Jobs/        # Elasticsearch index jobs
├── TramTimes.Web.Api/            # Web backend application
├── TramTimes.Web.Site/           # Web frontend application
├── TramTimes.Web.Tests/          # Web frontend playwright tests
├── TramTimes.Web.Utilities/      # Web utilities library
├── Directory.Build.props         # Shared build configuration
├── Directory.Packages.props      # Centralized package management
└── TramTimes.slnx                # Solution file
```

### Project Organization

Each project follows a consistent structure:

- `Program.cs` - Application entry point
- `Builders/` - Service and resource builders
- `Data/` - Data models and repositories
- `Services/` - Business logic services
- `Extensions/` - Extension methods and helpers
- `Models/` - Domain models and DTOs
- `Properties/` - Assembly information and settings

### Key Directories

- **`.docker/`** - Contains Docker Compose files for infrastructure services
- **`.scripts/`** - Helper scripts for starting services and deployment tasks

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.