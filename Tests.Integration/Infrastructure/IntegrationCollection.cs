namespace Tests.Integration.Infrastructure;

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
    // This class has no code, and is never created. Its purpose is just
    // to apply [CollectionDefinition] and tie the fixture to the name.
}