using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DevFuel.Core.UI.Forms;
using DevFuel.Core.UI;

namespace DevFuel.Core.Test.UI
{
    [TestFixture]
    public class ExceptionBoxTest
    {
        [Test]
        public void Show()
        {
            using (EnableThemingInScope e = new EnableThemingInScope(true))
            {
                //Optionally set the default dialog icon
                HostApplicationResources.DialogIcon = Properties.Resources.devfuel;
                try
                {
                    Exception realException = new Exception("Ugly Exception");
                    throw new Exception("User Friendly Message", realException);
                }
                catch (Exception x)
                {
                    ExceptionBox.Show(x);
                }
            }
        }

    }
}
