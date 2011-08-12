using NGitHub.Models;
using NGitHub.Utility;

namespace NGitHub {
    public class Parameter {
        private readonly string _key;
        private readonly string _value;

        public Parameter(SortBy sortby)
            : this("sortby", sortby.GetText()) {
        }

        public Parameter(State state)
            : this("state", state.GetText()) {
        }

        public Parameter(OrderBy direction)
            : this("direction", direction.GetText()) {
        }

        public Parameter(string key, string value) {
            Requires.ArgumentNotNull(key, "key");
            Requires.ArgumentNotNull(value, "value");

            _key = key;
            _value = value;
        }

        public string Key {
            get {
                return _key;
            }
        }

        public string Value {
            get {
                return _value;
            }
        }
    }
}