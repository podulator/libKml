// 
//  KmlColour.cs
//  
//  Author:
//       mat rowlands <code-account@podulator.com>
//  
//  Copyright (c) 2011 mat rowlands
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlColour : ISearchable {
		private string _alpha = "ff";
		private string _r = "00";
		private string _g = "00";
		private string _b = "00";

		public KmlColour() {}
		public KmlColour (string value, Logger log) {
			Log += log;
			if (value.Length == 8) {
				value = value.ToLower();
				_alpha = value.Substring(0, 2);
				_b = value.Substring(2, 2);
				_g = value.Substring(4, 2);
				_r = value.Substring(6, 2);
			}
		}
		public KmlColour(XmlNode parent, Logger log) : this(parent.InnerText, log) {}
		
		#region properties
		public string Alpha {
			get { return _alpha; }
			set { _alpha = value; }
		}
		public string Red {
			get { return _r; }
			set { _r = value; }
		}
		public string Green {
			get { return _g; }
			set { _g = value; }
		}
		public string Blue {
			get { return _b; }
			set { _b = value; }
		}
		#endregion properties

		#region helpers
		public override string ToString() {
			return string.Format("{0}{1}{2}{3}", _alpha, _b, _g,  _r);
		}
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "color", string.Empty);
			result.InnerText = this.ToString();
			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers

	}//	class
}//	namespace
