
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;

namespace System
{
    public class MarshalByRefObject
    {
    }
}

namespace System.Reflection
{
    public struct CustomAttributeTypedArgument
    {
        public Type ArgumentType;
        public object Value;
    }

    public struct CustomAttributeNamedArgument
    {
        public MemberInfo MemberInfo;
        public CustomAttributeTypedArgument TypedValue;
    }

    public class CustomAttributeData
    {
        public virtual ConstructorInfo Constructor { get; set; }

        public virtual IList<CustomAttributeNamedArgument> NamedArguments { get; set; }

        public IList<CustomAttributeTypedArgument> ConstructorArguments { get; private set; }

        static List<CustomAttributeTypedArgument> GetCtorArgs(object o, ConstructorInfo ctor)
        {
            var args = new List<CustomAttributeTypedArgument>();
            var props = ctor.DeclaringType.GetProperties ();

            if (props.Length == args.Count && args.Count == 1)
            {

            }

            foreach (var p in ctor.GetParameters())
            {
                var val = default(object);
                var ty = typeof(object);

                var prop = props.FirstOrDefault(x => x.Name.ToLowerInvariant() == p.Name.ToLowerInvariant());
                if (prop == null)
                {
                    prop = props.LastOrDefault();
                }

                if (prop != null)
                {
                    val = prop.GetValue(o, null);
                    ty = prop.PropertyType;                    
                }
                args.Add(new CustomAttributeTypedArgument()
                {
                    ArgumentType = ty,
                    Value = val
                });
            }
            return args;
        }

        static CustomAttributeData GetData (object o) {
            var ty = o.GetType();
            var ctor = ty.GetConstructors()[0];
            return new CustomAttributeData()
            {
                Constructor = ctor,
                ConstructorArguments = GetCtorArgs(o, ctor), 
            }; 
        }

        static IList<CustomAttributeData> GetData(object[] objs)
        {
            var attrs = from r in objs
                        select GetData (r);
            return attrs.ToList();
        }

        public static IList<CustomAttributeData> GetCustomAttributes(Module target)
        {
            return GetData(target.GetCustomAttributes(true));
        }
        public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
        {
            return GetData(target.GetCustomAttributes(true));
        }
        public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
        {
            return GetData(target.GetCustomAttributes(true));
        }
        public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
        {
            return GetData(target.GetCustomAttributes(true));
        }
    }

    public enum PortableExecutableKinds
    {
        ILOnly,
        Required32Bit
    }

    public enum ImageFileMachine
    {
        I386,
        IA64,
        AMD64
    }
}

namespace System.Diagnostics
{
    public class Stopwatch
    {
        DateTime _startTime;
        DateTime? _stopTime;
        public void Start()
        {
            _startTime = DateTime.UtcNow;
            _stopTime = null;
        }
        public void Stop()
        {
            _stopTime = DateTime.UtcNow;
        }
        public TimeSpan Elapsed
        {
            get
            {
                if (_stopTime.HasValue)
                {
                    return (_stopTime.Value - _startTime);
                }
                else
                {
                    return (DateTime.UtcNow - _startTime);
                }
            }
        }
        public long ElapsedMilliseconds
        {
            get
            {
                return (long)Elapsed.TotalMilliseconds;
            }
        }
    }
}

namespace System.Xml
{
    public class XmlNode
    {
    }

    public class XmlElement : XmlNode
    {
        XElement _e;

        public XmlNode ParentNode { get; private set; }
        public XmlDocument OwnerDocument { get; private set; }

        public string InnerXml { get { return _e.Nodes().Aggregate("", (b, node) => b += node.ToString()); } }

        public XmlElement(XmlDocument doc, XmlNode parent)
        {
            OwnerDocument = doc;
            ParentNode = parent;
        }

        public string GetAttribute(string name)
        {
            return _e.Attribute(name).Value;
        }
    }

    public class XmlDocument
    {

    }

    public class XmlTextWriter
    {

    }
}

namespace Mono.CSharp
{
    public class IndentedTextWriter : TextWriter
    {
        TextWriter _inner;
        string _indentString;

        public int Indent { get; set; }

        public IndentedTextWriter(TextWriter inner, string indent)
        {
            _inner = inner;
            _indentString = indent;
        }

        public override System.Text.Encoding Encoding
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
