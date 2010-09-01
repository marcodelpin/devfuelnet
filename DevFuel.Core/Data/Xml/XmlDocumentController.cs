#region COPYRIGHT
/*
The following code was created
by E. Edwards of http://www.devfuel.com. It is released for use under the BSD license:
Copyright (c) 2007-2008, E. Edwards 

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of devfuel.com nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using DevFuel.Core.UI;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace DevFuel.Core.Data.Xml
{
    /// <summary>
    /// A Helper class that provides several standard functions for XML document support
    /// </summary>
    public class XmlDocumentController
    {
        public struct XsltParam
        {
            public string Name { get; set; }
            public string NamespaceUri { get; set; }
            public object Value { get; set; }
        }

        /// <summary>
        /// Validates the specified schema path.
        /// </summary>
        /// <param name="schemaPath">The schema path.</param>
        /// <param name="documentPath">The document path.</param>
        /// <param name="eventsOut">The events out.</param>
        /// <returns></returns>
        public static bool Validate(string schemaPath, string documentPath, out List<ValidationEventArgs> eventsOut)
        {
            XmlReader schemaReader = XmlReader.Create(schemaPath);
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(XmlSchema.Read(schemaReader, new ValidationEventHandler(
            delegate(Object sender, ValidationEventArgs e)
            {
            }    
            )));
            List<ValidationEventArgs> events = new List<ValidationEventArgs>();
            schemaReader.Close();
            XmlDocument d = new XmlDocument();
            d.Schemas = schemaSet;
            d.Load(documentPath);

            d.Validate(new ValidationEventHandler(
                delegate(Object sender, ValidationEventArgs e)
                {
                    events.Add(e);
                }
            ));
            eventsOut = events;
            if (events.Count > 0)
                return false;
            return true; 
        }

        /// <summary>
        /// Transforms XML document at inputPath via the XSLT document at xsltPath and saves the result at outputPath
        /// </summary>
        /// <param name="xsltPath">The XSLT path.</param>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        public static void Transform(string xsltPath, string inputPath, string outputPath)
        {
            List<string> list = new List<string>();
            Transform(xsltPath, inputPath, outputPath, ref list, null);
        }


        public static void Transform(string xsltPath, string inputPath, string outputPath, XsltParam[] parameters)
        {
            List<string> list = new List<string>();
            Transform(xsltPath, inputPath, outputPath, ref list, parameters);
        }

        
        /// <summary>
        /// Transforms XML document at inputPath via the XSLT document at xsltPath and saves the result at outputPath.
        /// </summary>
        /// <param name="xsltPath">The XSLT path.</param>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="messageList">The message list.</param>
        public static void Transform(string xsltPath, string inputPath, string outputPath, ref List<string> messageList)
        {
            Transform(xsltPath, inputPath, outputPath, ref messageList, null);
        }

        /// <summary>
        /// Transforms XML document at inputPath via the XSLT document at xsltPath and saves the result at outputPath.
        /// </summary>
        /// <param name="xsltPath">The XSLT path.</param>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="messageList">The message list.</param>
        public static void Transform(string xsltPath, string inputPath, string outputPath, ref List<string> messageList, XsltParam[] parameters)
        {
            XsltSettings settings = new XsltSettings(true, true);
            XmlUrlResolver resolver = new XmlUrlResolver();
            XslCompiledTransform t = new XslCompiledTransform();
            t.Load(xsltPath, settings, resolver);
            XmlReader r = XmlReader.Create(inputPath);
            XmlWriter w = XmlWriter.Create(outputPath);
            XsltArgumentList al = new XsltArgumentList();
            if (parameters != null)
            {
                foreach (XsltParam p in parameters)
                {
                    al.AddParam(p.Name, p.NamespaceUri, p.Value);
                }
            }
            List<string> _messageList = new List<string>();
            al.XsltMessageEncountered += new XsltMessageEncounteredEventHandler(delegate (object sender, XsltMessageEncounteredEventArgs e)
                {
                    _messageList.Add(e.Message);
                });
            try
            {
                t.Transform(r, al, w);
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (_messageList.Count > 0 && messageList != null)
                    messageList.AddRange(_messageList);
                r.Close();
                w.Close();
            }
        }

        /// <summary>
        /// Gets the XMLNS from the specified xml document.
        /// </summary>
        /// <param name="xmlDocumentPath">The XML document path.</param>
        /// <param name="suffix">The suffix. "" will get you the value of xmlns="", "tim" will get you xmlns:tim=""</param>
        /// <returns></returns>
        /// Updated 2/15/2008 by ee
        public static string GetXmlns(string xmlDocumentPath, string suffix)
        {
            XPathDocument d = new XPathDocument(xmlDocumentPath);
            XPathNavigator nav = d.CreateNavigator();
            nav.MoveToFollowing(XPathNodeType.Element);
            IDictionary<string, string> namespaces = nav.GetNamespacesInScope(XmlNamespaceScope.Local);
            string xmlns = namespaces[suffix];
            return xmlns;
        }

        /// <summary>
        /// Gets a value specified by the XPathQuery from the XML document specified in xmlDocumentPath.
        /// </summary>
        /// <param name="xmlDocumentPath">The XML document path.</param>
        /// <param name="xpathQuery">The XPath query.</param>
        /// <returns></returns>
        public static string GetXPathValue(string xmlDocumentPath, string xpathQuery)
        {
            XPathDocument d = new XPathDocument(xmlDocumentPath);
            XPathNavigator nav = d.CreateNavigator();
            XPathNodeIterator it = nav.Select(xpathQuery);
            while (it.MoveNext())
            {
                return it.Current.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets values specified by the XPathQuery from the XML document specified in xmlDocumentPath.
        /// </summary>
        /// <param name="xmlDocumentPath">The XML document path.</param>
        /// <param name="xpathQueries">The xpath queries.</param>
        /// <returns></returns>
        public static List<string> GetXPathValues(string xmlDocumentPath, params string[] xpathQueries)
        {
            List<string> values = new List<string>();
            XPathDocument d = new XPathDocument(xmlDocumentPath);
            foreach (string xpathQuery in xpathQueries)
            {
                XPathNavigator nav = d.CreateNavigator();
                XPathNodeIterator it = nav.Select(xpathQuery);
                if (it.MoveNext())
                {
                    values.Add(it.Current.Value);
                    continue;
                }
                else
                {
                    values.Add(string.Empty);
                }
            }
            return values;
        }
    }
}
