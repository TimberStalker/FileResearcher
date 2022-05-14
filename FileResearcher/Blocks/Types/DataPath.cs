using FileResearcher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Blocks.Types
{
    public class DataPath
    {
        public List<PathItem> Items { get; init; }

        public DataPath(List<PathItem> items)
        {
            this.Items = items;
        }

        public DataValue? GetFrom(DataValue value)
        {
            foreach(var item in Items)
            {
                var next = item.Get(value);
                
                if (next is null) return null;
                
                value = next;
            }
            return value;
        }
    }

    public class PathItem
    {
        public SharedResource<string> Name { get; set; }

        public PathItem(SharedResource<string> name)
        {
            this.Name = name;
        }

        public virtual DataValue? Get(DataValue value)
        {
            return value.GetValue(Name);
        }
    }

    public class PathArrayItem : PathItem
    {
        public int Index { get; set; }

        public PathArrayItem(SharedResource<string> name, int index) : base(name)
        {
            Index = index;
        }
        public override DataValue? Get(DataValue value)
        {
            throw new NotImplementedException();
        }
    }
}
