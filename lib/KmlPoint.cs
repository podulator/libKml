// 
//  KmlPoint.cs
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
	public class KmlPoint : KmlGeometry, IComparable, ISearchable  {
		private bool _extrude = false;
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;
		private KmlCoordinate _coordinate;
		
		public KmlPoint() : base() {
			_coordinate = new KmlCoordinate();
		}
		public KmlPoint(XmlNode parent, Logger log) : base(parent, log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "extrude":
						_extrude = (node.InnerText.Equals("1") ? true : false);
						break;
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(node.InnerText);
						break;
					case "coordinates":
						_coordinate = new KmlCoordinate(node.InnerText, log);
						break;
				};
			}
		}//	constructor

		# region properties
		public bool Extrude {
			get { return _extrude; }
			set { _extrude = value; }
		}
		public string AltitudeMode {
			get { return KmlAltitudeModes.altitudeModeToString(_altitudeMode); }
			set { _altitudeMode = KmlAltitudeModes.altitudeModeFromString(value); }
		}
		public KmlCoordinate KmlCoordinate {
			get { return _coordinate; }
			set { _coordinate = value; }
		}
		#endregion properties
	
		public override string ToString() {
			return "Point = " + _coordinate.ToString();
		}

		#region IComparable Members

		int IComparable.CompareTo (object obj) {
			KmlPoint temp = obj as KmlPoint;
			return _coordinate.CompareTo(temp.KmlCoordinate);
		}

		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Point", string.Empty);
			base.ToXml(result);
			// child nodes
			XmlNode nodExtrude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "extrude", string.Empty);
			nodExtrude.InnerText = (Extrude ? "1" : "0");
			result.AppendChild(nodExtrude);
			XmlNode nodAltitudeMode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitudeMode", string.Empty);
			nodAltitudeMode.InnerText = AltitudeMode;
			result.AppendChild(nodAltitudeMode);
			XmlNode nodCoords = result.OwnerDocument.CreateNode(XmlNodeType.Element, "coordinates", string.Empty);
			nodCoords.InnerText = _coordinate.ToString();
			result.AppendChild(nodCoords);
			
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _coordinate)
				_coordinate.findElementsOfType<T>(elements);
		}
		#endregion


	}//	class
}//	namespace
