using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Linq
{
    public class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }

        public TypeSwitch CaseType(Type T, Action action) { matches.Add(T, (x) => action()); return this; }
        public void SwitchType(Type x) { matches[x](x); }
    }
}
