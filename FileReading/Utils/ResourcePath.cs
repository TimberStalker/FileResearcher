using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Utils;

public class ResourcePath
{
    List<SharedResource<string>> path;
    public ResourcePath(List<SharedResource<string>> path)
    {
        this.path = path;
    }
}
