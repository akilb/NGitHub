using NGitHub.Models;

namespace NGitHub {
    public static class Constants {
        public const string ApiV3Url = "https://api.github.com";
        public const string ApiV2Url = "http://github.com/api/v2/json";
        public const SortBy DefaultSortBy = SortBy.Created;
        public const OrderBy DefaultOrderBy = OrderBy.Descending;

        public const string JsonApplicationContent = "application/json";
        public const string JsonTextContent = "text/json";
        public const string XJsonTextContent = "text/x-json";
    }
}
