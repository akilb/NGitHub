namespace NGitHub.Models {
    public enum State {
        Open,
        Closed
    }

    public enum Event {
        Closed,
        Reopened,
        Subscribed,
        Merged,
        Referenced,
        Mentioned,
        Assigned
    }

    public enum SortBy {
        Created,
        Updated,
        Comments
    }

    public enum OrderBy {
        Ascending,
        Descending
    }
}
