using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NGitHub.Models {
    [JsonObject]
    public class Repository {
        [JsonProperty(PropertyName = "owner")]
        public User Owner { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        // TODO: Remove this property...
        [JsonIgnore]
        public string FullName {
            get {
                string owner;
                if (Owner != null &&
                    Owner.Login != null) {
                    owner = Owner.Login;
                }
                else {
                    owner = string.Empty;
                }
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

        [JsonProperty(PropertyName = "open_issues")]
        public int OpenIssues { get; set; }

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

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("clone_url")]
        public string CloneUrl { get; set; }

        [JsonProperty("git_url")]
        public string GitUrl { get; set; }

        [JsonProperty("ssh_url")]
        public string SshUrl { get; set; }

        [JsonProperty("svn_url")]
        public string SvnUrl { get; set; }

        [JsonProperty("homepage")]
        public string HomePage { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
