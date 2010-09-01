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
    [TaskName("svg2xaml")]
    public class SvgToXamlTask : Task
    {
        #region Private Fields

        private DirectoryInfo m_ToDir;
        private bool m_Flatten;
        private FileSet m_InputFileSet = new FileSet();

        #endregion

        #region Public Properties

        [TaskAttribute("todir", Required = true)]
        public virtual DirectoryInfo ToDirectory
        {
            get { return m_ToDir; }
            set { m_ToDir = value; }
        }

        [TaskAttribute("flatten")]
        [BooleanValidator()]
        public virtual bool Flatten
        {
            get { return m_Flatten; }
            set { m_Flatten = value; }
        }

        [BuildElement("fileset", Required = true)]
        public virtual FileSet InputFileSet
        {
            get { return m_InputFileSet; }
            set { m_InputFileSet = value; }
        }

        #endregion

        protected override void InitializeTask(XmlNode taskNode)
        {
            if (m_ToDir == null)
                throw new BuildException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The 'todir' attribute must be set to specify the output directory of the XAML files."),
                    this.Location);

            if (!m_ToDir.Exists)
                m_ToDir.Create();

            if (m_InputFileSet == null)
                throw new BuildException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The <fileset> element must be used to specify the SVG files to convert."),
                    Location);
        }

        protected override void ExecuteTask()
        {
            if (m_InputFileSet.BaseDirectory == null)
                m_InputFileSet.BaseDirectory = new DirectoryInfo(Project.BaseDirectory);

            Log(Level.Info, "Converting {0} SVG file(s) to '{1}'.", m_InputFileSet.FileNames.Count, m_ToDir.FullName);          

            foreach (string srcPath in m_InputFileSet.FileNames)
            {
                FileInfo srcFile = new FileInfo(srcPath);

                if (srcFile.Exists)
                {
                    string destPath = GetDestPath(m_InputFileSet.BaseDirectory, srcFile);

                    DirectoryInfo destDir = new DirectoryInfo(Path.GetDirectoryName(destPath));

                    if (!destDir.Exists)
                        destDir.Create();

                    Log(Level.Verbose, "Converting '{0}' to '{1}'.", srcPath, destPath);

                    string fileText = File.ReadAllText(srcPath);
                    if (Path.GetExtension(srcPath).ToLower() == ".svg")
                    {
                        ConvertUtility.Svg2Xaml(srcPath, destPath);
                    }
                    else
                    {
                        throw new BuildException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Expected a .svg extension. Got \"{0}\"",
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

        private string GetDestPath(DirectoryInfo srcBase, FileInfo srcFile)
        {
            string destPath = string.Empty;

            if (m_Flatten)
            {
                destPath = Path.Combine(m_ToDir.FullName, srcFile.Name);
            }
            else
            {
                if (srcFile.FullName.IndexOf(srcBase.FullName, 0) != -1)
                    destPath = srcFile.FullName.Substring(srcBase.FullName.Length);
                else
                    destPath = srcFile.Name;

                if (destPath[0] == Path.DirectorySeparatorChar)
                    destPath = destPath.Substring(1);

                destPath = Path.Combine(m_ToDir.FullName, destPath);
            }
            destPath = Path.ChangeExtension(destPath, "xaml");
            return destPath;
        }
    }
}
