// 
//  KmlFile.cs
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
	public class KmlFile : ISearchable {

		private KmlFeature _feature = null;
		private KmlNetworkLinkControl _networkLinkControl = null;
		
		public KmlFile() {}
		public KmlFile (XmlDocument doc) : this() { fromXml(doc, null); }
		public KmlFile (XmlDocument doc, Logger log) : this() {
			Log += log;
			fromXml(doc, log);
		}

		#region properties
		public KmlFeature Feature {
			get { return _feature; }
			set { _feature = value; }
		}
		public KmlNetworkLinkControl NetworkLinkControl {
			get { return _networkLinkControl; }
			set { _networkLinkControl = value; }
		}
		#endregion properties

		#region helpers
		private void fromXml(XmlDocument doc, Logger log) {

			foreach (XmlNode node in doc.GetElementsByTagName("kml")[0].ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "networklinkcontrol":
						_networkLinkControl = new KmlNetworkLinkControl(node, log);
						break;
					case "networklink":
						_feature = new KmlNetworkLink(node, log);
						break;
					case "placemark":
						_feature = new KmlPlacemark(node, log);
						break;
					case "groundoverlay":
						_feature = new KmlGroundOverlay(node, log);
						break;
					case "photooverlay":
						_feature = new KmlPhotoOverlay(node, log);
						break;
					case "screenoverlay":
						_feature = new KmlScreenOverlay(node, log);
						break;
					case "document":
						_feature = new KmlDocument(node, log);
						break;
					case "folder":
						_feature = new KmlFolder(node, log);
						break;
					default:
						debug("Unknown tag :: " + key);
						break;
				};
			}
		}
		public XmlDocument ToXml() {
			XmlDocument result = new XmlDocument();
			result.AppendChild(result.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
			XmlNode kml = result.CreateNode(XmlNodeType.Element, "kml", "http://earth.google.com/kml/2.2");
			
			XmlAttribute atom = result.CreateAttribute("xmlns:atom");
			atom.Value = "http://www.w3.org/2005/Atom";
			kml.Attributes.Append(atom);
			
			XmlAttribute gx = result.CreateAttribute("xmlns:gx");
			gx.Value = "http://www.google.com/kml/ext/2.2";
			kml.Attributes.Append(gx);

			XmlAttribute xal = result.CreateAttribute("xmlns:xal");
			xal.Value = "http://openxal.org/ui";
			kml.Attributes.Append(xal);
			
			if (null != _networkLinkControl) 
				kml.AppendChild(_networkLinkControl.ToXml(kml));
			if (null != _feature) {
				kml.AppendChild(_feature.ToXml(kml));
			}
			result.AppendChild(kml);
			return result;
		}

		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);

			if (null != _networkLinkControl)
				_networkLinkControl.findElementsOfType<T>(elements);
			if (null != _feature)
				_feature.findElementsOfType<T>(elements);
			
		}//	findElementsOfType

		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;

		#endregion helpers

	}//	class
}//	namespace
