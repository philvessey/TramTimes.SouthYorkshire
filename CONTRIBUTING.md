# Contributing to TramTimes.SouthYorkshire

Thank you for your interest in contributing to TramTimes.SouthYorkshire! We welcome contributions from the community
and are grateful for your support.

## 📋 Table of Contents

- [Code of Conduct](#-code-of-conduct)
- [Getting Started](#-getting-started)
- [How to Contribute](#-how-to-contribute)
- [Development Setup](#-development-setup)
- [Coding Standards](#-coding-standards)
- [Commit Guidelines](#-commit-guidelines)
- [Pull Request Process](#-pull-request-process)
- [Reporting Bugs](#-reporting-bugs)
- [Suggesting Enhancements](#-suggesting-enhancements)
- [Documentation](#-documentation)
- [Community](#-community)

## 📜 Code of Conduct

This project adheres to a [Code of Conduct](CODE_OF_CONDUCT.md) that all contributors are expected to follow.
Please read the full [Code of Conduct](CODE_OF_CONDUCT.md) to understand the expectations for all participants
in our community. Please be respectful, inclusive, and considerate in all interactions.

### Our Pledge

We are committed to providing a welcoming and inspiring community for all. We pledge to make participation in
our project a harassment-free experience for everyone, regardless of:

- Age, body size, disability, ethnicity, gender identity and expression
- Level of experience, education, socio-economic status
- Nationality, personal appearance, race, religion
- Sexual identity and orientation

### Expected Behavior

- Use welcoming and inclusive language
- Be respectful of differing viewpoints and experiences
- Gracefully accept constructive criticism
- Focus on what is best for the community
- Show empathy towards other community members

## 🚀 Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- A code editor:
    - [JetBrains Rider](https://www.jetbrains.com/rider/)
    - [Visual Studio](https://visualstudio.microsoft.com/)
    - [Visual Studio Code](https://code.visualstudio.com/)


- [Git](https://git-scm.com/downloads)
- [GitHub CLI](https://cli.github.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Aspire CLI](https://aspire.dev/get-started/install-cli/)
- [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell)
- [Telerik UI for Blazor](https://www.telerik.com/blazor-ui)
- [Traveline Data Set](https://www.travelinedata.org.uk/traveline-open-data/traveline-national-dataset/)

### First Time Setup

1. **Fork the repository** on GitHub via Web or using GitHub CLI:
   ```bash
   gh repo fork philvessey/TramTimes.SouthYorkshire
   ```

2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/<your-username>/TramTimes.SouthYorkshire.git
   cd TramTimes.SouthYorkshire
   ```

3. **Add upstream remote**:
   ```bash
   git remote add upstream https://github.com/philvessey/TramTimes.SouthYorkshire.git
   ```

4. **Verify remotes**:
   ```bash
   git remote -v
   ```

5. **Add custom package source**:
   ```bash
   dotnet new nugetconfig
   ```

6. **Copy telerik-license.txt** to the root of the repository if you have a Telerik license:
   ```bash
   cp /path/to/your/telerik-license.txt TramTimes.SouthYorkshire/telerik-license.txt
   ```

7. **Add custom telerik source** if you have a Telerik license:
   ```bash
   dotnet nuget add source "https://nuget.telerik.com/v3/index.json" --name "telerik" --username "api-key" --password "<your-api-key>" --configfile "./nuget.config" --store-password-in-clear-text
   ```

8. **Copy nuget.xml** to `./nuget.config` between `<configuration>` tags if you have a Telerik license:
    ```bash
    sed -i '/<\/configuration>/e sed "s/^/  /" nuget.xml; echo' ./nuget.config
    ```

9. **Run the solution** to verify everything works:
   ```bash
   aspire run
   ```

## 🤝 How to Contribute

### Types of Contributions

We welcome many types of contributions:

- 🐛 **Bug fixes** - Fix issues in existing code
- ✨ **New features** - Add new functionality
- 📝 **Documentation** - Improve or add documentation
- 🎨 **Code quality** - Refactoring, performance improvements
- ✅ **Tests** - Add or improve test coverage
- 💡 **Ideas** - Suggest enhancements or new features

### Before You Start

1. **Check existing issues** to see if your idea or bug is already being discussed
2. **Create an issue** to discuss your proposed changes (for significant changes)
3. **Wait for feedback** before starting major work
4. **Keep changes focused** - one feature or fix per pull request

## 💻 Development Setup

### Project Structure

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

### Building the Project

```bash
# Build all projects
dotnet build

# Build a specific project
dotnet build TramTimes.Web.Site/TramTimes.Web.Site.csproj

# Build in Release mode
dotnet build -c Release
```

### Debugging

- **JetBrains Rider**: Open the solution file and use the built-in debugger
- **Visual Studio**: Open the solution file and press F5
- **VS Code**: Install the C# extension and use the debugger

## 📏 Coding Standards

### .NET Conventions

Follow the official [.NET coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions):

#### Naming Conventions

```csharp
// PascalCase for classes, methods, properties
public class StopBuilder { }
public void BuildStop() { }
public string StopId { get; set; }

// camelCase for parameters and local variables
public void ProcessStop(string stopId)
{
    var stopName = GetStopName(stopId);
}

// UPPER_CASE for constants
private const string DEFAULT_NETWORK = "SOUTHYORKSHIRE";

// Prefix interfaces with 'I'
public interface IStopBuilder { }

// Use meaningful names
var stopBuilder = new StopBuilder(); // ✓ Good
var sb = new StopBuilder();          // ✗ Avoid
```

#### Code Style

```csharp
// Use expression-bodied members when appropriate
public string GetName() => _name;

// Use implicit typing (var) when type is obvious
var stopId = "9400ZZSYMAL1";
var builder = new StopBuilder();

// Use explicit typing when type is not obvious
IStopBuilder builder = CreateBuilder();

// Use nullable reference types
public string? OptionalProperty { get; set; }

// Use modern C# features
var list = new List<string> { "item1", "item2" };
var point = new { X = 1, Y = 2 };

// Prefer pattern matching
if (value is not null)
{
    ProcessValue(value);
}
```

#### File Organization

```csharp
// Order: usings, namespace, class
using System;
using System.Collections.Generic;

namespace TramTimes.Web.Site.Builders;

public class StopBuilder
{
    // Fields
    private readonly ILogger _logger;
    
    // Constructors
    public StopBuilder(ILogger logger)
    {
        _logger = logger;
    }
    
    // Properties
    public string StopId { get; set; }
    
    // Methods
    public void Build() { }
}
```

#### Comments and Documentation

```csharp
// Use XML documentation for public APIs
/// <summary>
/// Builds a stop job from the specified template.
/// </summary>
/// <param name="stopId">The unique identifier for the stop.</param>
/// <returns>The generated job code as a string.</returns>
public string BuildStop(string stopId)
{
    // Use comments to explain WHY, not WHAT
    // Skip validation if stop is in cache (performance optimization)
    if (_cache.Contains(stopId))
        return _cache[stopId];
        
    return GenerateFromTemplate(stopId);
}
```

### Code Quality

- **DRY (Don't Repeat Yourself)**: Extract repeated code into methods
- **SOLID Principles**: Follow object-oriented design principles
- **Error Handling**: Always handle exceptions appropriately
- **Async/Await**: Use async programming for I/O operations
- **Using Statements**: Properly dispose of resources

## 📝 Commit Guidelines

### Commit Message Format

We follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

#### Types

- `feat`: A new feature
- `fix`: A bug fix
- `docs`: Documentation only changes
- `style`: Code style changes (formatting, missing semicolons, etc.)
- `refactor`: Code refactoring without changing functionality
- `perf`: Performance improvements
- `test`: Adding or updating tests
- `build`: Changes to build system or dependencies
- `ci`: Changes to CI configuration
- `chore`: Other changes that don't modify src or test files

#### Examples

```bash
# Feature
feat(site): add support for real-time tram locations

# Bug fix
fix(schedules): correct timezone handling for weekend schedules

# Documentation
docs(readme): update installation instructions for macOS

# Multiple changes
feat(search): add geolocation filtering
- Implement distance calculation
- Add latitude/longitude validation
- Update Elasticsearch mapping
```

### Commit Best Practices

- Write clear, concise commit messages
- Use present tense ("add feature" not "added feature")
- Keep the subject line under 72 characters
- Separate subject from body with a blank line
- Reference issues and pull requests in the footer

## 🔄 Pull Request Process

### Before Submitting

1. **Update from upstream**:
   ```bash
   git fetch upstream
   git rebase upstream/master
   ```

2. **Build solution**:
   ```bash
   dotnet build
   ```

3. **Test solution**:
   ```bash
   pwsh test.ps1                                # Runs all tests
   pwsh test.ps1 --filter Home.Dark             # Runs specific page tests
   pwsh test.ps1 --filter Home.Dark --run 1     # Runs specific page data set test
   ```

4. **Run code formatting** (if applicable):
   ```bash
   dotnet format
   ```

5. **Commit your changes** following the commit guidelines:
   ```bash
   git add .
   git commit -m "feat(site): add support for real-time tram locations"
   ```

6. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```

### Submitting the PR

1. Go to your fork on GitHub
2. Click "New Pull Request"
3. Select the base repository and branch
4. Fill out the PR template with:
    - **Description**: What does this PR do?
    - **Motivation**: Why is this change needed?
    - **Related Issues**: Link to related issues
    - **Testing**: How was this tested?
    - **Screenshots**: If applicable
    - **Breaking Changes**: Any breaking changes?

### PR Title Format

Use the same format as commit messages:

```
feat(site): add support for real-time tram locations
fix(schedules): correct timezone handling
docs(contributing): add commit message guidelines
```

### Review Process

- Maintainers will review your PR
- Address any feedback or requested changes
- Once approved, a maintainer will merge your PR
- Your contribution will be included in the next release!

### PR Checklist

- [ ] Code builds successfully (`dotnet build`)
- [ ] Code follows the project's style guidelines
- [ ] Self-review of code completed
- [ ] Comments added for complex logic
- [ ] Documentation updated (if needed)
- [ ] No new warnings introduced
- [ ] Commit messages follow guidelines
- [ ] PR description is clear and complete

## 🐛 Reporting Bugs

### Before Submitting a Bug Report

- Check the [existing issues](https://github.com/philvessey/TramTimes.SouthYorkshire/issues)
- Verify you're using the latest version
- Collect relevant information about your environment

### How to Submit a Bug Report

Create an issue with the following information:

**Title**: Clear, descriptive title

**Description**:

- **Expected behavior**: What should happen?
- **Actual behavior**: What actually happens?
- **Steps to reproduce**: Detailed steps to reproduce the issue
- **Environment**:
    - .NET Version: (run `dotnet --version`)
    - Aspire Version: (run `aspire --version`)
    - Browser Version: (if applicable)
- **Error messages**: Full error messages and stack traces
- **Screenshots**: If applicable

### Bug Report Template

```markdown
**Describe the bug**
A clear and concise description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:

1. Go to '...'
2. Run '...'
3. See error

**Expected behavior**
A clear and concise description of what you expected to happen.

**Environment:**

- .NET Version: [e.g., 10.0.0]
- Aspire Version: [e.g., 13.0.2]
- Browser Version: [e.g., Chrome 90]

**Additional context**
Add any other context about the problem here.
```

## 💡 Suggesting Enhancements

We welcome feature suggestions! Here's how to suggest an enhancement:

### Before Submitting

- Check if the enhancement has already been suggested
- Consider if it aligns with the project's goals
- Think about how it would benefit other users

### How to Submit an Enhancement

Create an issue with:

- **Clear title**: Describe the enhancement concisely
- **Motivation**: Why is this enhancement needed?
- **Proposed solution**: How should it work?
- **Alternatives**: Have you considered alternatives?
- **Additional context**: Any other relevant information

### Enhancement Template

```markdown
**Is your feature request related to a problem?**
A clear and concise description of what the problem is.

**Describe the solution you'd like**
A clear and concise description of what you want to happen.

**Describe alternatives you've considered**
A clear and concise description of alternative solutions.

**Additional context**
Add any other context or screenshots about the feature request.
```

## 📚 Documentation

### Types of Documentation

- **README files**: Overview and getting started
- **Code comments**: Explain complex logic
- **XML documentation**: Public API documentation
- **Wiki**: Detailed guides and tutorials

### Documentation Guidelines

- Write in clear, simple English
- Use proper grammar and spelling
- Include code examples where helpful
- Keep documentation up to date with code changes
- Use Markdown formatting consistently

### Updating Documentation

When you change functionality:

1. Update relevant README files
2. Update XML documentation comments
3. Update code examples if needed
4. Add to CHANGELOG.md (if applicable)

## 👥 Community

### Getting Help

- **GitHub Issues**: For bugs and feature requests
- **GitHub Discussions**: For questions and general discussion

### Stay Updated

- Watch the repository for notifications
- Star the repository to show support
- Follow the project for updates

## 🎉 Recognition

Contributors will be recognized in:

- Release notes
- Project README (Contributors section)
- GitHub's contributor graph

Thank you for contributing to TramTimes.SouthYorkshire! Your efforts help make this project better for everyone.

---

**Questions?** Feel free to open an issue or start a discussion!

**Last Updated**: December 15, 2025