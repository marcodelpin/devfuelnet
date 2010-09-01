using System;

namespace DevFuel.Core.ComponentModel
{
	/// <summary>
	/// A holder class for localizable strings that are used. Currently, these are not loaded from resources, but 
	/// just coded into this class. To make this library localizable, simply change this class to load the
	/// given strings from resources.
	/// </summary>
	internal static class Strings
	{
		public static readonly string PropGridNull = "[None]";
        public static readonly string LookupPropertyCaptionNotFound = "A Caption could not be found for \"{0}\" (\"{1}\")";
        public static readonly string LookupPropertyValueNotFound = "An {0} could not be found for the caption \"{1}\".";
    }
}
