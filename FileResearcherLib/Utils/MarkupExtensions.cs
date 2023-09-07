using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.Utils;
public static class MarkupExtensions
{
    public static T GetRef<T>(this T t, out T result)
    {
        result = t;
        return t;
    }
}
