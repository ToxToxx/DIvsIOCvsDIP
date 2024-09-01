using Microsoft.Extensions.DependencyInjection;

#region IOC realization
// Setting up the dependency injection (IoC) container
var serviceCollection = new ServiceCollection();

// Registering dependencies in the IoC container
serviceCollection.AddTransient<IGithubClient, GithubClient>();  // Transient IGithubClient with concrete implementation GithubClient
serviceCollection.AddTransient<GitHubServiceDI>();              // Transient GitHubServiceDI service

// Building the service provider to resolve dependencies
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolving GitHubServiceDI from the service provider
var gitHubService = serviceProvider.GetRequiredService<GitHubServiceDI>();

// Using the resolved service
var result = gitHubService.GetStars("throw");
Console.WriteLine("throw has this many stars with service provider " + result);

#endregion

#region Classic DI realization
// Classic dependency injection without an IoC container

// Manually creating the GithubClient instance
var client = new GithubClient();

// Passing the dependency directly to the GitHubServiceDI constructor
var stars = new GitHubServiceDI(client).GetStars("throw");

Console.WriteLine("throw has this many stars with classic way " + stars);

#endregion

#region Different types of injection
// DI - Dependency Injection where the bigger object does not rely on concrete smaller objects, but on abstractions
// IoC - Inversion of Control where the control over the application or part of it is handed over to a framework or container

/// <summary>
/// Example of Constructor Injection using the IGithubClient abstraction.
/// </summary>
internal class GitHubServiceDI
{
    private readonly IGithubClient _gitHubClient;

    // Constructor Injection: Dependency is provided through the constructor
    public GitHubServiceDI(IGithubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
    }

    /// <summary>
    /// Returns the number of stars of the specified repository.
    /// </summary>
    public int GetStars(string repoName)
    {
        return _gitHubClient.GetRepo(repoName).stars;
    }
}

/// <summary>
/// Example of Setter Injection using the IGithubClient abstraction.
/// </summary>
internal class GitHubServiceSetter
{
    private IGithubClient _gitHubClient;

    // Setter Injection: Dependency is provided through a setter method
    public void SetGithubClient(IGithubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
    }

    /// <summary>
    /// Returns the number of stars of the specified repository.
    /// </summary>
    public int GetStars(string repoName)
    {
        return _gitHubClient.GetRepo(repoName).stars;
    }
}

/// <summary>
/// Example of Interface Injection using the IGithubClientSetter abstraction.
/// </summary>
internal class GitHubServiceInterface : IGithubClientSetter
{
    private IGithubClient _gitHubClient;

    // Interface Injection: Dependency is provided through an interface method
    public void SetGithubClient(IGithubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
    }

    /// <summary>
    /// Returns the number of stars of the specified repository.
    /// </summary>
    public int GetStars(string repoName)
    {
        return _gitHubClient.GetRepo(repoName).stars;
    }
}

/// <summary>
/// Interface defining a method for setting a GithubClient dependency.
/// Used for Interface Injection.
/// </summary>
internal interface IGithubClientSetter
{
    void SetGithubClient(IGithubClient githubClient);
}

/// <summary>
/// Abstraction for a Github client.
/// Defines a method to retrieve repository details.
/// </summary>
internal interface IGithubClient
{
    (string repoName, int stars) GetRepo(string repoName);
}

/// <summary>
/// Concrete implementation of the IGithubClient interface.
/// Returns repository details based on the repo name.
/// </summary>
internal class GithubClient : IGithubClient
{
    public (string repoName, int stars) GetRepo(string repoName)
    {
        // Dummy implementation: returns repo name and its length as stars
        return (repoName, repoName.Length);
    }
}

#endregion
