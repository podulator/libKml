// 
//  KmlIconStyle.cs
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
	public class KmlIconStyle : KmlColourStyle, ISearchable  {
		private float _scale;
		private float _heading;
		private KmlIcon _icon;
		private KmlHotSpot _hotSpot;

		public KmlIconStyle() : base() {
			_scale = 1;
			_heading = 0.0f;
			_icon = new KmlIcon();
			_hotSpot = new KmlHotSpot();
		}
		public KmlIconStyle(XmlNode parent, Logger log) : this() {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "color":
						Colour = new KmlColour(node.InnerText, log);
						break;
					case "scale":
						Scale = float.Parse(node.InnerText);
						break;
					case "icon":
						_icon = new KmlIcon(node, log);
						break;
					default:
						break;
				};
			}
		}
		#region properties
		public float Scale {
			get { return _scale; }
			set { _scale = value; }
		}
		public float Heading {
			get { return _heading; }
			set { _heading = value; }
		}
		public KmlIcon Icon {
			get { return _icon; }
			set { _icon = value; }
		}
		public KmlHotSpot HotSpot {
			get { return _hotSpot; }
			set { _hotSpot = value; }
		}
		#endregion properties

		#region helpers
		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "IconStyle", string.Empty);
			base.ToXml(result);
			//child nodes
			XmlNode nodScale = result.OwnerDocument.CreateNode(XmlNodeType.Element, "scale", string.Empty);
			nodScale.InnerText = Scale.ToString();
			result.AppendChild(nodScale);
			
			XmlNode nodHeading = result.OwnerDocument.CreateNode(XmlNodeType.Element, "heading", string.Empty);
			nodHeading.InnerText = Heading.ToString();
			result.AppendChild(nodHeading);

			if (null != _icon)
				result.AppendChild(_icon.ToXml(result));
			
			if (null != _hotSpot) 
				result.AppendChild(_hotSpot.ToXml(result));

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _icon)
				_icon.findElementsOfType<T>(elements);
			if (null != _hotSpot)
				_hotSpot.findElementsOfType<T>(elements);
		}
		protected new void debug(string message) {
			if (Log != null) Log(message);
		}
		public new event Logger Log;
		#endregion helpers
	}//	class
}//	namespace
