using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAnt.Core;
using NAnt.Core.Attributes;
using System.IO;
using System.Security;

namespace DevFuel.NAnt.Tasks
{
    [FunctionSet("file", "file")]
    public class FunctionSet : FunctionSetBase
    {
        public FunctionSet(Project project, PropertyDictionary properties)
            : base(project, properties)
        {
        }
        [Function("read-all-text")]
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
        [Function("read-all-xml-escaped-text")]
        public static string ReadAllXmlEscapedText(string path)
        {
            return SecurityElement.Escape(File.ReadAllText(path));
        }
    }
}
