// 
//  KmlBalloonStyle.cs
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
	public class KmlBalloonStyle : ISearchable {
		private string _id = string.Empty;
		private KmlColour _colour = null;
		private KmlColour _textColour = null;
		private string _text = string.Empty;
		private bool _visible = true;
		
		public KmlBalloonStyle() {}

		public KmlBalloonStyle(XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["id"]) 
				_id = parent.Attributes["id"].Value;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "bgcolor":
						_colour = new KmlColour(node.InnerText, log);
						break;
					case "textcolor":
						_textColour = new KmlColour(node.InnerText, log);
						break;
					case "text":
						_text = node.InnerText;
						break;
					case "displaymode":
						_visible = (node.InnerText.ToLower().Equals("hide") ? false : true);
						break;
				};
			}
		}
		
		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public KmlColour BgColour {
			get { return _colour; }
			set { _colour = value; }
		}
		public KmlColour TextColour {
			get { return _textColour; }
			set { _textColour = value; }
		}
		public string Text {
			get { return _text; }
			set { _text = value; }
		}
		public bool Visible {
			get { return _visible; }
			set { _visible = value; }
		}
		#endregion

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "BalloonStyle", string.Empty);
			if (_id.Length > 0) {
				XmlAttribute attId = result.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				result.Attributes.Append(attId);
			}
			if (null != BgColour) {
				XmlNode nodColour = BgColour.ToXml(result);
				result.AppendChild(nodColour);
			}
			if (null != TextColour) {
				XmlNode nodTextColour = TextColour.ToXml(result);
				result.AppendChild(nodTextColour);
			}
			XmlNode nodText = result.OwnerDocument.CreateNode(XmlNodeType.Element, "text", string.Empty);
			nodText.InnerText = Text;
			result.AppendChild(nodText);

			XmlNode nodDisplayMode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "displayMode", string.Empty);
			nodDisplayMode.InnerText = (Visible ? "default" : "hide");
			result.AppendChild(nodDisplayMode);

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
