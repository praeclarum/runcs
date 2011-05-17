
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public MemberInfo MemberInfo { get; set; }
        public CustomAttributeTypedArgument TypedValue { get; set; }
    }

    public class CustomAttributeData
    {
        public virtual ConstructorInfo Constructor
        {
            get
            {
                return null;
            }
        }
        public virtual IList<CustomAttributeNamedArgument> NamedArguments
        {
            get
            {
                return new List<CustomAttributeNamedArgument>();
            }
        }
        public IList<CustomAttributeTypedArgument> ConstructorArguments { get; private set; }
        public static IList<CustomAttributeData> GetCustomAttributes(Module target)
        {
            return new List<CustomAttributeData>();
        }
        public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
        {
            return new List<CustomAttributeData>();
        }
        public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
        {
            return new List<CustomAttributeData>();
        }
        public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
        {
            return new List<CustomAttributeData>();
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
