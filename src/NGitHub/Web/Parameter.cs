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

        public Parameter(SortBy sortby)
            : this("sortby", sortby.GetText()) {
        }

        public Parameter(State state)
            : this("state", state.GetText()) {
        }

        public Parameter(OrderBy direction)
            : this("direction", direction.GetText()) {
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
    }
}