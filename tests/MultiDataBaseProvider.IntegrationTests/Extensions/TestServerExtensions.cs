namespace Microsoft.AspNetCore.TestHost;

public static class TestServerExtensions
{
    public static Given Given(this TestServer server)
        => new(server);
}
