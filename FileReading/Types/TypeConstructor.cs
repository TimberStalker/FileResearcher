using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Types;

public static  class TypeConstructor
{
    static AssemblyBuilder ab;
    static ModuleBuilder mb;
    static TypeConstructor()
    {
        AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
        ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);

        // The module name is usually the same as the assembly name.
        mb = ab.DefineDynamicModule(aName.Name!);
    }
    public static Type CreateType(string name, List<(string name, Type type)> fields)
    {
        var tb = mb.DefineType(name, TypeAttributes.Public);

        foreach (var (fname, type) in fields)
        {
            tb.DefineField(fname, type, FieldAttributes.Public);
        }

        return tb.CreateType()!;
    }
}
