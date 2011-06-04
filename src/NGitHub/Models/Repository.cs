using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NGitHub.Models {
    [JsonObject]
    public class Repository {
        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        // TODO: Remove this property...
        [JsonIgnore]
        public string FullName {
            get {
                var owner = Owner ?? string.Empty;
                var repoName = Name ?? string.Empty;

                return owner + " / " + repoName;
            }
        }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        // TODO: Remove this property...
        [JsonIgnore]
        private const int ShortDescriptionLength = 100;
        public string ShortDescription {
            get {
                if (String.IsNullOrEmpty(Description)) {
                    return string.Empty;
                }

                return Description.Length >= ShortDescriptionLength ?
                    Description.Substring(0, ShortDescriptionLength) + "..." :
                    Description;
            }
        }

        [JsonProperty(PropertyName = "private")]
        public bool IsPrivate { get; set; }

        [JsonProperty(PropertyName = "fork")]
        public bool IsFork { get; set; }

        [JsonProperty(PropertyName = "has_issues")]
        public bool HasIssues { get; set; }

        [JsonProperty(PropertyName = "watchers")]
        public int NumberOfWatchers { get; set; }

        [JsonProperty(PropertyName = "forks")]
        public int NumberOfForks { get; set; }

        [JsonProperty(PropertyName = "pushed_at")]
        public DateTime LastUpdatedDate { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedDate { get; set; }

        private string _masterBranch;
        [JsonProperty(PropertyName = "master_branch")]
        public string MasterBranch {
            get {
                if (string.IsNullOrEmpty(_masterBranch)) {
                    // The github API will only provide a value if there is a
                    // custom master branch. Otherwise we should assume
                    // "master"
                    return "master";
                }

                return _masterBranch;
            }
            set {
                _masterBranch = value;
            }
        }
    }

    [JsonObject]
    public class RepositoriesResult {
        [JsonProperty(PropertyName = "repositories")]
        public List<Repository> Repositories { get; set; }
    }

    [JsonObject]
    public class NetworkResult {
        [JsonProperty(PropertyName = "network")]
        public List<Repository> Forks { get; set; }
    }

    [JsonObject]
    public class RepositoryResult {
        [JsonProperty(PropertyName = "repository")]
        public Repository Repository { get; set; }
    }
}
