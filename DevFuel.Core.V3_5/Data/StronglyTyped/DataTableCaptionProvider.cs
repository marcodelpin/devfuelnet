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
using System.Resources;
using System.Data;
using DevFuel.Core.Collections.Generic;
namespace DevFuel.Core.Data.StronglyTyped
{
    /// <summary>
    /// Interface that represents an object capable of providing a caption for a DataTable
    /// </summary>
    public interface IDataTableCaptionProvider
    {
        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetCaption(Type type);
        /// <summary>
        /// Gets the caption plural.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetCaptionPlural(Type type);
    }

    /// <summary>
    /// Implementation of the IDataTableCaptionProvider Interface
    /// </summary>
    public class DataTableCaptionProvider : IDataTableCaptionProvider
    {
        /// <summary>
        /// Dictionary of DataTable Caption Pairs (singular and plural)
        /// </summary>
        public SortedDictionary<string, Tuple<string, string>> m_DataTableCaptions = new SortedDictionary<string, Tuple<string, string>>();

        /// <summary>
        /// Adds the captions.
        /// </summary>
        /// <param name="tDataTable">The t data table.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="captionPlural">The caption plural.</param>
        public void AddCaptions(Type tDataTable, string caption, string captionPlural)
        {
            m_DataTableCaptions.Add(tDataTable.Name, new Tuple<string, string>(caption, captionPlural));
        }

        /// <summary>
        /// Adds the captions.
        /// </summary>
        /// <param name="captions">The captions.</param>
        /// <param name="captionsPlural">The captions plural.</param>
        /// <param name="dataSet">The data set.</param>
        public void AddCaptions(ResourceManager captions, ResourceManager captionsPlural, DataSet dataSet)
        {
            string caption;
            string captionPlural;
            foreach (DataTable table in dataSet.Tables)
            {
                caption = captions.GetString(table.TableName);
                captionPlural = captionsPlural.GetString(table.TableName);
                
                if (string.IsNullOrEmpty(caption) || string.IsNullOrEmpty(captionPlural))
                {
                    caption = captionPlural = table.TableName;
                }
                AddCaptions(table.GetType(), caption, captionPlural);
            }
        }


        #region IDataTableCaptionProvider Members

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetCaption(Type type)
        {
            Tuple<string, string> captions;
            if (!m_DataTableCaptions.TryGetValue(type.Name, out captions))
            {
                DefaultDataTableCaptionProvider p = new DefaultDataTableCaptionProvider();
                return p.GetCaption(type);
            }
            return captions.First;
        }

        /// <summary>
        /// Gets the caption plural.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetCaptionPlural(Type type)
        {
            Tuple<string, string> captions;
            if (!m_DataTableCaptions.TryGetValue(type.Name, out captions))
            {
                DefaultDataTableCaptionProvider p = new DefaultDataTableCaptionProvider();
                return p.GetCaptionPlural(type);
            }
            return captions.Second;
        }

        #endregion
    }

    /// <summary>
    /// The Default Implementation of IDataTableCaption Provider (if a custom one cannot be found)
    /// </summary>
    public class DefaultDataTableCaptionProvider : IDataTableCaptionProvider
    {

        #region IDataTableCaptionProvider Members

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetCaption(Type type)
        {
            if (type.Name.EndsWith("DataTable"))
                return type.Name.Substring(0, type.Name.Length - 9);
            return type.Name;
        }

        /// <summary>
        /// Gets the caption plural.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetCaptionPlural(Type type)
        {
            return string.Format("{0}(s)", GetCaption(type));
        }

        #endregion
    }
}
