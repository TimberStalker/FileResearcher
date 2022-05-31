using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FileReading.Utils;

namespace FileReading.Types
{
    public class CustomType
    {
        public string name;
        public List<(string, SharedResource<Type>)> fields;

        public CustomType(string name)
        {
            this.name = name;
            fields = new List<(string, SharedResource<Type>)>();
        }

        public Type Create()
        {
            AssemblyName aName = new AssemblyName(name);
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName,AssemblyBuilderAccess.RunAndCollect);

            var mb = ab.DefineDynamicModule(name);

            var tb = mb.DefineType(name);

            foreach (var (fname, type) in fields)
            {
                var fb = tb.DefineField(fname, type.Value, FieldAttributes.Public);
            }

            return tb.CreateType()!;
        }
    }
}
