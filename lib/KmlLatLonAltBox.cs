using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlLatLonAltBox : ISearchable {
		private float _north = 0.0f;
		private float _south = 0.0f;
		private float _east = 0.0f;
		private float _west = 0.0f;
		private float _minAltitude = 0.0f;
		private float _maxAltitude = 0.0f;
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;
		
		public KmlLatLonAltBox() {}
		public KmlLatLonAltBox(XmlNode parent) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "north":
						_north = float.Parse(node.InnerText);
						break;
					case "south":
						_south = float.Parse(node.InnerText);
						break;
					case "east":
						_east = float.Parse(node.InnerText);
						break;
					case "west":
						_west = float.Parse(node.InnerText);
						break;
					case "minaltitude":
						_minAltitude = float.Parse(node.InnerText);
						break;
					case "maxaltitude":
						_maxAltitude = float.Parse(node.InnerText);
						break;
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(node.InnerText);
						break;
				};
			}
		}
		#region properties
		public float North {
			get { return _north; }
			set { _north = value; }
		}
		public float South {
			get { return _south; }
			set { _south = value; }
		}
		public float East {
			get { return _east; }
			set { _east = value; }
		}
		public float West {
			get { return _west; }
			set { _west = value; }
		}
		public float MinAltitude {
			get { return _minAltitude; }
			set { _minAltitude = value; }
		}
		public float MaxAltitude {
			get { return _maxAltitude; }
			set { _maxAltitude = value; }
		}
		public string AltitudeMode {
			get { return KmlAltitudeModes.altitudeModeToString(_altitudeMode); }
			set { _altitudeMode = KmlAltitudeModes.altitudeModeFromString(value); }
		}
		#endregion properties

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "LatLonAltBox", string.Empty);
			// child nodes
			XmlNode nodNorth = result.OwnerDocument.CreateNode(XmlNodeType.Element, "north", string.Empty);
			nodNorth.InnerText = North.ToString();
			result.AppendChild(nodNorth);

			XmlNode nodSouth = result.OwnerDocument.CreateNode(XmlNodeType.Element, "south", string.Empty);
			nodSouth.InnerText = South.ToString();
			result.AppendChild(nodSouth);

			XmlNode nodEast = result.OwnerDocument.CreateNode(XmlNodeType.Element, "east", string.Empty);
			nodEast.InnerText = East.ToString();
			result.AppendChild(nodEast);

			XmlNode nodWest = result.OwnerDocument.CreateNode(XmlNodeType.Element, "west", string.Empty);
			nodWest.InnerText = West.ToString();
			result.AppendChild(nodWest);

			XmlNode nodMinAltitude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "minAltitude", string.Empty);
			nodMinAltitude.InnerText = MinAltitude.ToString();
			result.AppendChild(nodMinAltitude);

			XmlNode nodMaxAltitude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "maxAltitude", string.Empty);
			nodMaxAltitude.InnerText = MaxAltitude.ToString();
			result.AppendChild(nodMaxAltitude);

			XmlNode nodMode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitudeMode", string.Empty);
			nodMode.InnerText = AltitudeMode;
			result.AppendChild(nodMode);

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
}//	kml
