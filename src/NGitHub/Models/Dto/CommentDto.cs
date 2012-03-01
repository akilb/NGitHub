using Newtonsoft.Json;

namespace NGitHub.Models.Dto {
    [JsonObject]
    public class CommentDto {
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    [JsonObject]
    public class CommitCommentDto : CommentDto {
        [JsonProperty("commit_id")]
        public string CommitId { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }
    }

    [JsonObject]
    public class ReplyCommentDto : CommentDto {
        [JsonProperty("in_reply_to")]
        public int InReplyTo { get; set; }
    }
}
