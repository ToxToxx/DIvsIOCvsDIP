var stars = new GitHubServiceDI().GetStars("throw");



#region DIP vs DI vs IOC classes
//DI - the bigger object don't rely on concrete smaller object - they use abstraction
//IOC - giving some control over application or part to framework or container
//
internal class GitHubServiceDI(IGithubClient gitHubClient)
{                                         //ctor injection
    private IGithubClient _gitHubClient = gitHubClient; //DI

    public int GetStars(string repoName)
    {
        return _gitHubClient.GetRepo(repoName).stars;//DI
    }
}

internal class GitHubServiceSetter
{                                         //ctor injection
    private IGithubClient _gitHubClient; //DI

    //setter injection
    public void SetGithubClient(IGithubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
    }

    public int GetStars(string repoName)
    {
        return _gitHubClient.GetRepo(repoName).stars;//DI
    }
}
internal class GitHubServiceInterface : IGithubClientSetter
{
    private IGithubClient _gitHubClient;

    public void SetGithubClient(IGithubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
    }

    public int GetStars(string repoName)
    {
        return _gitHubClient.GetRepo(repoName).stars;//DI
    }
}

internal interface IGithubClientSetter
{
    void SetGithubClient(IGithubClient githubClient);
}

internal interface IGithubClient //DI
{
    (string repoName, int stars) GetRepo(string repoName);
}

internal class GithubClient : IGithubClient //DI
{
   public (string repoName, int stars) GetRepo(string repoName)
    {
        return (repoName, repoName.Length);
    }
}

#endregion