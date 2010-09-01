using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using System.Xml;
using System.Globalization;
using NAnt.Core.Tasks;
using Yahoo.Yui.Compressor;
using XamlTune;

namespace DevFuel.NAnt.Tasks
{
    [TaskName("setassemblyversion")]
    public class SetAssemblyVersionTask : Task
    {
        #region Private Fields
        private FileSet m_InputFileSet = new FileSet();
        private string m_Version = "0.0.0.0";
        private Version m_Vs;
        #endregion

        #region Public Properties

        [BuildElement("fileset", Required = true)]
        public virtual FileSet InputFileSet
        {
            get { return m_InputFileSet; }
            set { m_InputFileSet = value; }
        }

        [TaskAttribute("version", Required = false)]
        public virtual string Version
        {
            get { return m_Version; }
            set { m_Version = value; }
        }

        #endregion

        protected override void InitializeTask(XmlNode taskNode)
        {
            if (m_InputFileSet == null)
                throw new BuildException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The <fileset> element must be used to specify the AssemblyInfo.cs files to Generate Versions for."),
                    Location);

            try
            {
                m_Vs = new Version(m_Version);
            }
            catch
            {
                throw new BuildException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Version \"{0}\" is in an invalid format.",
                    m_Version));
            }
        }

        protected override void ExecuteTask()
        {
            if (m_InputFileSet.BaseDirectory == null)
                m_InputFileSet.BaseDirectory = new DirectoryInfo(Project.BaseDirectory);

            Log(Level.Info, "Setting {0} Version file(s).", m_InputFileSet.FileNames.Count);          

            foreach (string srcPath in m_InputFileSet.FileNames)
            {                
                FileInfo srcFile = new FileInfo(srcPath);

                if (srcFile.Exists)
                {
                    string pathVersion = Path.ChangeExtension(srcFile.FullName, "vs.cs");

                    if (Path.GetFileName(srcPath).ToLower() == "assemblyinfo.cs")
                    {
                        Log(Level.Info, "Generating '{0}'.", pathVersion);
                        File.WriteAllText(pathVersion, string.Format("using System.Reflection;{0}using System.Runtime.CompilerServices;{0}using System.Runtime.InteropServices;{0}[assembly: AssemblyFileVersion(\"{1}\")]{0}[assembly: AssemblyVersion(\"{1}\")]", Environment.NewLine, m_Version));
                    }
                    else
                    {
                        throw new BuildException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Expected an assemblyinfo.cs file. Got \"{0}\"",
                            Path.GetFileName(srcPath)),
                        Location);
                    }
                }
                else
                {
                    throw new BuildException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Could not find file '{0}' to convert.",
                            srcFile.FullName),
                        Location);
                }
            }
        }
    }
}
