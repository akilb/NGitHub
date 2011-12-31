using NGitHub.Models;
using NGitHub.Utility;

namespace NGitHub.Web {
    public class Parameter {
        private readonly string _name;
        private readonly string _value;

        public Parameter(string name, string value) {
            Requires.ArgumentNotNull(name, "name");
            Requires.ArgumentNotNull(value, "value");

            _name = name;
            _value = value;
        }

        public string Name {
            get {
                return _name;
            }
        }

        public string Value {
            get {
                return _value;
            }
        }

        public static Parameter Page(int page) {
            return new Parameter("page", page.ToString());
        }

        public static Parameter SortBy(SortBy sortby) {
            return new Parameter("sortby", sortby.GetText());
        }

        public static Parameter State(State state) {
            return new Parameter("state", state.GetText());
        }

        public static Parameter OrderBy(OrderBy direction) {
            return new Parameter("direction", direction.GetText());
        }
    }
}