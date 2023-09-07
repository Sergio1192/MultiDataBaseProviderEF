namespace MultiDataBaseProvider.IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(ApiCollection), DisableParallelization = true)]
public class ApiCollection : ICollectionFixture<ApiFixture>
{ }
