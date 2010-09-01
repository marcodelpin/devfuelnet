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

namespace DevFuel.Core
{
    public class VersionPattern
    {
        int _Major = int.MaxValue;
        int _Minor = int.MaxValue;
        int _Build = int.MaxValue;
        int _Revision = int.MaxValue;
        public VersionPattern(string pattern)
        {
            pattern = pattern.Replace("-1", "*");
            string[] parts = pattern.Split('.');
            if (parts.Length != 4)
                throw new FormatException("pattern must be of the form 1.2.*.* where * represents any allowed numeric version");
            if (parts[0] == "*")
                _Major = -1;
            else
                _Major = int.Parse(parts[0]);

            if (parts[1] == "*")
                _Minor = -1;
            else
                _Minor = int.Parse(parts[1]);

            if (parts[2] == "*")
                _Build = -1;
            else
                _Build = int.Parse(parts[2]);

            if (parts[3] == "*")
                _Revision = -1;
            else
                _Revision = int.Parse(parts[3]);
        }

        public bool Matches(Version v)
        {
            if (_Major >= 0)
            {
                if (v.Major > _Major)
                    return false;
                else if (v.Major < _Major)
                    return true;
                if (_Minor >= 0)
                {
                    if (v.Minor > _Minor)
                        return false;
                    if (v.Minor < _Minor)
                        return true;
                    if (_Build >= 0)
                    {
                        if (v.Build < 0)
                            return true;
                        if (v.Build > _Build)
                            return false;
                        if (v.Build < _Build)
                            return true;
                        if (_Revision >= 0)
                        {
                            if (v.Revision < 0)
                                return true;
                            if (v.Revision > _Revision)
                                return false;
                        }
                    }
                }
            }
            return true; //Major version wildcard...everything matches
        }
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", _Major, _Minor, _Build, _Revision).Replace("-1","*");
        }
    }
}
