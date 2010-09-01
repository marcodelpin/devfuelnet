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

namespace DevFuel.NAnt.Tasks
{
    [TaskName("readversion")]
	public class ReadVersionTask : Task 
	{
		private string m_Path;
		[TaskAttribute("path", Required=true)]
		public string Path 
		{
			get { return m_Path; }
			set { m_Path = value; }
		}

        [TaskAttribute("incrementmajor")]
        public bool IncrementMajor { get; set; }

        [TaskAttribute("incrementminor")]
        public bool IncrementMinor { get; set; }

        [TaskAttribute("incrementbuild")]
        public bool IncrementBuild { get; set; }

        [TaskAttribute("incrementrevision")]
        public bool IncrementRevision { get; set; }

		protected override void ExecuteTask() 
		{
			Version version = new Version("0.0.0.0");
			if (File.Exists(Path))
			{
                if (Verbose)
				    Log(Level.Info, String.Format("Reading Version from {0}", Path));
				version = new Version(File.ReadAllText(Path));
			}
			else
			{
                if (Verbose)
                    Log(Level.Info, String.Format("Defaulted to Version {0}", version));
			}

            if (IncrementBuild | IncrementMajor | IncrementMinor | IncrementRevision)
            {
                version = new Version(
                    version.Major + (IncrementMajor ? 1 : 0),
                    version.Minor + (IncrementMinor ? 1 : 0),
                    version.Build + (IncrementBuild ? 1 : 0), 
                    version.Revision + (IncrementRevision ? 1 : 0));
                if (Verbose)
                    Log(Level.Info, String.Format("Writing Incremented Version ({0}) to {1}", version, Path));
                File.WriteAllText(Path, version.ToString());
            }
            Project.Properties["vs.major"] = version.Major.ToString();
            Project.Properties["vs.minor"] = version.Minor.ToString();
            Project.Properties["vs.build"] = version.Build.ToString();
            Project.Properties["vs.revision"] = version.Revision.ToString();
            Project.Properties["vs"] = version.ToString();
		}
	}
}
