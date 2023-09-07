using FileReading.FileTree.Parameters;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree;

[DebuggerDisplay(@"{Name} {ReadType} ({Children.Count})")]
public class TreeBuilder
{
    public string Name { get; }
    public bool IsReference { get; set; }
    public DataType? ReadType { get; set; }
    public IList<TreeBuilder> Children { get; }
    public TreeBuilder Index { get; private set; }
    public IList<FileTreeExpressionParameter> Parameters { get; }

    public TreeBuilder(string name)
    {
        Name = name;
        Children = new List<TreeBuilder>();
        Parameters = ImmutableList<FileTreeExpressionParameter>.Empty;
    }
    TreeBuilder(string name, DataType readType, FileTreeExpressionParameter[] parameters) : this(name)
    {
        ReadType = readType;
        Parameters = parameters.ToList();
    }

    public TreeBuilder AddChild<T>(string name, params FileTreeExpressionParameter[] parameters) where T : DataType, new() => AddChild(new T(), name, parameters);
    public TreeBuilder AddChild(DataType type, string name, params FileTreeExpressionParameter[] parameters)
    {
        var child = new TreeBuilder(name, type, parameters);
        if(type is ArrayDataType)
        {
            child.Index = new TreeBuilder("Index", IntDataType.Instance, Array.Empty<FileTreeExpressionParameter>());
        }
        Children.Add(child);
        return child;
    }

    public TreeBuilder AddChildReference<T>(string name, FileTreeExpressionParameter parameter) where T : DataType, new() => AddChildReference(new T(), name, parameter);
    public TreeBuilder AddChildReference(DataType type, string name, FileTreeExpressionParameter parameters)
    {
        var child = new TreeBuilder(name, type, new[] { parameters });
        child.IsReference = true;
        Children.Add(child);
        return child;
    }

    public TreeRoot BuildTree()
    {
        if (ReadType is not null) throw new Exception("Can only build the base bulder value.");
        TreeRoot root = new TreeRoot(Guid.NewGuid())
        {
            Name = Name,
        };
        Dictionary<TreeBuilder, TreeNode> mappings = new();
        foreach (var child in Children)
        {
            child.Build(root, root, mappings);
        }
        return root;
    }
    TreeNode Build(TreeNode nodeParent, TreeRoot root, Dictionary<TreeBuilder, TreeNode> mappings)
    {
        if (ReadType is null) throw new Exception("Cannot buld data type node without data type");

        TreeNode node;
        if (IsReference)
        {
            node = new TreeReference(Guid.NewGuid(), root, ReadType);
        }
        else
        {
            node = ReadType switch
            {
                ArrayDataType a => new TreeArray(Guid.NewGuid(), root, a),
                var t => new TreeDataType(Guid.NewGuid(), root, t)
            };
            if(node is TreeArray ad)
            {
                mappings[Index!] = ad.Index;
            }
        }

        nodeParent.AddChild(node);
        node.Name = Name;
        mappings[this] = node;

        if (node is TreeReference r)
        {
            SetParameter(r.Value, Parameters[0], mappings);
        }
        else if(node is TreeDataType d)
        {
            foreach (var parameter in Parameters)
            {
                var baseParameter = ReadType.GetParameter(parameter.Name);
                SetParameter(d.Parameters[baseParameter], parameter, mappings);
            }
        }

        foreach (var child in Children)
        {
            child.Build(node, root, mappings);
        }

        return node;
    }
    private void SetParameter(ITreeParameter parameter, FileTreeExpressionParameter builderParameter, Dictionary<TreeBuilder, TreeNode> mappings)
    {
        if(parameter is TreeParameter p)
        {
            p.StringValue = builderParameter.Value;
            if(builderParameter.NodeReference is null)
            {
                foreach(var bp in builderParameter.ReferenceParameters)
                {
                    var nodeParameter = new TreeNodeParameter(p.ParentNode, new CustomBaseParameter("", bp.NodeReference!.ReadType!), mappings[bp.NodeReference!]);
                    p.ReferenceParameters.Add(nodeParameter);
                    SetParameter(nodeParameter, bp, mappings);
                }
            }
            else
            {
                var nodeParameter = new TreeNodeParameter(p.ParentNode, new CustomBaseParameter("", builderParameter.NodeReference!.ReadType!), mappings[builderParameter.NodeReference]);
                p.ReferenceParameters.Add(nodeParameter);
                SetParameter(nodeParameter, builderParameter, mappings);
            }
        } else if(parameter is TreeNodeParameter n)
        {
            for (int i = 0; i < n.ReferenceParameters.Count; i++)
            {
                SetParameter(n.ReferenceParameters[i], builderParameter.ReferenceParameters[i], mappings);
            }
        }
    }

    public class FileTreeExpressionParameter
    {
        public string Name { get; }
        public string Value { get; }
        public TreeBuilder? NodeReference { get; }
        public FileTreeExpressionParameter[] ReferenceParameters { get; }
        FileTreeExpressionParameter(string name, string value)
        {
            Name = name;
            Value = value;
            ReferenceParameters = Array.Empty<FileTreeExpressionParameter>();
        }
        FileTreeExpressionParameter(string name, TreeBuilder reference)
        {
            Name = name;
            Value = "\x1A";
            NodeReference = reference;
            ReferenceParameters = Array.Empty<FileTreeExpressionParameter>();
        }
        FileTreeExpressionParameter(string name, string value, IEnumerable<FileTreeExpressionParameter> referenceParameters)
        {
            Name = name;
            Value = value;
            ReferenceParameters = referenceParameters.ToArray();
        }
        FileTreeExpressionParameter(string name, TreeBuilder reference, IEnumerable<FileTreeExpressionParameter> referenceParameters)
        {
            Name = name;
            Value = "\x1A";
            NodeReference = reference;
            ReferenceParameters = referenceParameters.ToArray();
        }

        public static implicit operator FileTreeExpressionParameter(TreeBuilder value) => new FileTreeExpressionParameter("", value);
        public static implicit operator FileTreeExpressionParameter(ExpressionInterpolatedStringHandler handler) => new FileTreeExpressionParameter("", handler.GetTextPart(), handler.Refrences);

        public static implicit operator FileTreeExpressionParameter((TreeBuilder value, FileTreeExpressionParameter p1) input) => 
            new FileTreeExpressionParameter("", input.value, new []{ input.p1 });
        public static implicit operator FileTreeExpressionParameter((TreeBuilder value, FileTreeExpressionParameter p1, FileTreeExpressionParameter p2) input) => 
            new FileTreeExpressionParameter("", input.value, new[] { input.p1, input.p2 });
        public static implicit operator FileTreeExpressionParameter((TreeBuilder value, FileTreeExpressionParameter p1, FileTreeExpressionParameter p2, FileTreeExpressionParameter p3) input) => 
            new FileTreeExpressionParameter("", input.value, new[] { input.p1, input.p2, input.p3 });
        public static implicit operator FileTreeExpressionParameter((TreeBuilder value, IEnumerable<FileTreeExpressionParameter> parameter) input) => 
            new FileTreeExpressionParameter("", input.value, input.parameter);

        public static implicit operator FileTreeExpressionParameter((string name, TreeBuilder value) input) => new FileTreeExpressionParameter(input.name, input.value);
        public static implicit operator FileTreeExpressionParameter((string name, ExpressionInterpolatedStringHandler handler) input) => new FileTreeExpressionParameter(input.name, input.handler.GetTextPart(), input.handler.Refrences);

        public static implicit operator FileTreeExpressionParameter((string name, TreeBuilder value, FileTreeExpressionParameter p1) input) =>
            new FileTreeExpressionParameter(input.name, input.value, new[] { input.p1 });
        public static implicit operator FileTreeExpressionParameter((string name, TreeBuilder value, FileTreeExpressionParameter p1, FileTreeExpressionParameter p2) input) =>
            new FileTreeExpressionParameter(input.name, input.value, new[] { input.p1, input.p2 });
        public static implicit operator FileTreeExpressionParameter((string name, TreeBuilder value, FileTreeExpressionParameter p1, FileTreeExpressionParameter p2, FileTreeExpressionParameter p3) input) =>
            new FileTreeExpressionParameter(input.name, input.value, new[] { input.p1, input.p2, input.p3 });
        public static implicit operator FileTreeExpressionParameter((string name, TreeBuilder value, IEnumerable<FileTreeExpressionParameter> parameter) input) =>
            new FileTreeExpressionParameter(input.name, input.value, input.parameter);

    }
}
