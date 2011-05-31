using NGitHub.Models;

namespace NGitHub.Utility {
    internal static class ApiHelpers {
        public static string GetParametersString(int page, SortBy sort, OrderBy direction) {
            return string.Format("page={0}&sort={1}&direction={2}",
                                 page,
                                 sort.GetText(),
                                 direction.GetText());
        }

        public static string GetParametersString(int page, State state, SortBy sort, OrderBy direction) {
            return GetParametersString(page, sort, direction) + "&state=" + state.GetText();
        }
    }
}
