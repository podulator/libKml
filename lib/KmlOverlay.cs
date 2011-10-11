// 
//  KmlOverlay.cs
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
	public abstract class KmlOverlay : KmlFeature, ISearchable  {

		private KmlColour _colour = null;
		private int _drawOrder = 0;
		private KmlIcon _icon = new KmlIcon();
		
		public KmlOverlay() : base() {}
		public KmlOverlay(XmlNode parent, Logger log) : base(parent) { 
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "color": 
						_colour = new KmlColour(node, log);
						break;
					case "drawOrder":
						_drawOrder = Int32.Parse(node.InnerText);
						break;
					case "Icon":
						_icon = new KmlIcon(node, log);
						break;
				};
			}
		}

		public KmlColour Colour {
			get { return _colour; }
			set { _colour = value; }
		}
		public int DrawOrder {
			get { return _drawOrder; }
			set { _drawOrder = value; }
		}
		public KmlIcon Icon {
			get { return _icon; }
			set { _icon = value; }
		}

		#region helpers
		
		public new void handleNode(XmlNode node, Logger log) {
			string key = node.Name.ToLower();
			switch (key) {
				case "color":
					_colour = new KmlColour(node.InnerText, log);
					break;
				case "draworder":
					_drawOrder = int.Parse(node.InnerText);
					break;
				case "icon":
					_icon = new KmlIcon(node, log);
					break;
				default:
					base.handleNode(node, log);
					break;
			};
		}

		public override XmlNode ToXml (XmlNode parent) {
			// add the feature stuff
			base.ToXml(parent);
			
			// child nodes
			if (null != Colour) {
				XmlNode nodColour = Colour.ToXml(parent);
				parent.AppendChild(nodColour);
			}
			
			XmlNode nodDrawOrder = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "drawOrder", string.Empty);
			nodDrawOrder.InnerText = DrawOrder.ToString();
			parent.AppendChild(nodDrawOrder);
			
			if (null != _icon)
				parent.AppendChild(_icon.ToXml(parent));

			return null;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _icon)
				_icon.findElementsOfType<T>(elements);
		}
		#endregion helpers

	}//	class
}//	namespace
