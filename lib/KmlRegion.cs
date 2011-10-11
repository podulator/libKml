// 
//  KmlRegion.cs
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
	public class KmlRegion : ISearchable {
		private string _id = string.Empty;
		private KmlLatLonAltBox _latLonAltBox = null;
		private KmlLod _lod = null;
		
		public KmlRegion() {}
		public KmlRegion(XmlNode parent, Logger log) {
			Log += log;
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
			
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "latlonaltbox":
						_latLonAltBox = new KmlLatLonAltBox(node);
						break;
					case "lod":
						_lod = new KmlLod(node);
						break;
				};
			}
		}
		
		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public KmlLatLonAltBox LatLonAltBox {
			get { return _latLonAltBox; }
			set { _latLonAltBox = value; }
		}
		public KmlLod Lod {
			get { return _lod; }
			set { _lod = value; }
		}
		#endregion properties

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Region", string.Empty);
			if (_id.Length > 0) {
				XmlAttribute nodId = result.OwnerDocument.CreateAttribute("id");
				nodId.Value = _id;
				result.Attributes.Append(nodId);
			}
			// child nodes
			if (null != _latLonAltBox)
				result.AppendChild(_latLonAltBox.ToXml(result));
			if (null != _lod)
				result.AppendChild(_lod.ToXml(result));

			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			if (null != _latLonAltBox)
				_latLonAltBox.findElementsOfType<T>(elements);
			if (null != _lod)
				_lod.findElementsOfType<T>(elements);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers

	}//	class
}//	namespace
