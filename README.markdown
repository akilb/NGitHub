# NGitHub
Simple GitHub API library for .NET, Silverlight and Windows Phone applications

#### Hello GitHub
```csharp
using NGitHub;

var githubClient = new GitHubClient();

githubClient.Users.GetUserAsync("akilb",
                                user => Console.WriteLine("{0} has {1} repositories!",
                                                            user.Login,
                                                            user.PublicRepos),
                                onError: e => { });
```