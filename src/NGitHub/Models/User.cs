using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NGitHub.Models {
    [JsonObject]
    public class User {
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        //[JsonProperty(PropertyName = "id")]
        // TODO: Ignoring Id for now since API V2 returns a string but API v3
        // returns an int...
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "gravatar_url")]
        public string GravatarUrl { get; set; }

        // NOTE: API V2 only uses gravatar id's
        [JsonProperty(PropertyName = "gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "blog")]
        public string BlogUrl { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "hireable")]
        public bool Hireable { get; set; }

        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }

        [JsonProperty(PropertyName = "public_repo_count")]
        public int PublicRepos { get; set; }

        [JsonProperty(PropertyName = "public_gists")]
        public int PublicGists { get; set; }

        [JsonProperty(PropertyName = "following_count")]
        public int Following { get; set; }

        [JsonProperty(PropertyName = "followers_count")]
        public int Followers { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonIgnore]
        public bool IsOrganization {
            get {
                return Type != null &&
                       Type.ToLower() == "organization";
            }
        }

        [JsonProperty(PropertyName = "total_private_repos")]
        public int TotalPrivateRepos { get; set; }

        [JsonProperty(PropertyName = "owned_private_repos")]
        public int OwnedPrivateRepos { get; set; }

        [JsonProperty(PropertyName = "private_gists")]
        public int PrivateGists { get; set; }

        [JsonProperty(PropertyName = "collaborators")]
        public int Collaborators { get; set; }

        [JsonProperty(PropertyName = "disk_usage")]
        public int DiskUsage { get; set; }

        [JsonProperty(PropertyName = "plan")]
        public Plan Plan { get; set; }
    }

    [JsonObject]
    public class UserResult {
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }

    [JsonObject]
    public class UsersResult {
        [JsonProperty(PropertyName = "users")]
        public List<User> Users { get; set; }
    }

    [JsonObject]
    public class WatchersResult {
        [JsonProperty(PropertyName = "watchers")]
        public List<User> Watchers { get; set; }
    }

    [JsonObject]
    public class OrganizationsResult {
        [JsonProperty(PropertyName = "organizations")]
        public List<User> Organizations { get; set; }
    }
}
