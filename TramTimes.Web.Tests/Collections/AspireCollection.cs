using TramTimes.Web.Tests.Managers;
using Xunit;

namespace TramTimes.Web.Tests.Collections;

[CollectionDefinition(name: "AspireCollection")]
public class AspireCollection : ICollectionFixture<AspireManager>;