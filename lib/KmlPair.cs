// 
//  KmlPair.cs
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
	public enum PairKeyMode : int {
		none = 0, 
		normal = 1, 
		highlight = 2
	};
	public class KmlPair : IComparable, ISearchable  {
		private PairKeyMode _mode = PairKeyMode.none;
		private string _style = string.Empty;
		private bool _isUrl = false;

		public KmlPair() {}
		public KmlPair(XmlNode parent) {

			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "key": 
						_mode = pairKeyModeFromString(node.InnerText);
						break;
					case "style":
						_style = node.InnerText;
						break;
					case "styeurl":
						_style = node.InnerText;
						_isUrl = true;
						break;
				};
			}
		}

		#region properties
		public string Mode {
			get { return pairKeyModeToString(_mode); }
			//set { _mode = pairKeyModeFromString(value); }
		}
		public void setPairKeyMode(PairKeyMode mode) {
			_mode = mode;
		}
		public string Style {
			get { return _style; }
			set { _style = value; }
		}
		public bool IsUrl {
			get { return _isUrl; }
			set { _isUrl = value; }
		}
		#endregion properties

		#region helpers
		public string pairKeyModeToString(PairKeyMode mode) {
			switch (mode) {
				case PairKeyMode.highlight:
					return "highlight";
				default :
					return "normal";
			};
		}
		public PairKeyMode pairKeyModeFromString(string mode) {
			switch (mode.ToLower()) {
				case "highlight":
					return PairKeyMode.highlight;
				default:
					return PairKeyMode.normal;
			};
		}
		#endregion helpers

		#region IComparable Members

		public int CompareTo (object obj) {
			return Mode.CompareTo(((KmlPair)obj).Mode);
		}

		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Pair", string.Empty);
			// child nodes
			XmlNode nodKey = result.OwnerDocument.CreateNode(XmlNodeType.Element, "key", string.Empty);
			nodKey.InnerText = Mode;
			result.AppendChild(nodKey);
			
			XmlNode nodStyle = result.OwnerDocument.CreateNode(XmlNodeType.Element, (IsUrl ? "styleUrl" : "style"), string.Empty);
			nodStyle.InnerText = Style;
			result.AppendChild(nodStyle);

			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		#endregion
	}//	class
}//	namespace
