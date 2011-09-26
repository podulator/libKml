using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {

	public class KmlLinearRing : KmlGeometry, ISearchable  {
		private bool _extrude = false;
		private bool _tessellate = false;
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;
		private List<KmlCoordinate> _coordinates = new List<KmlCoordinate>();

		public KmlLinearRing () : base() {}
		public KmlLinearRing (XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["id"]) Id = parent.Attributes["id"].Value;

			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "extrude":
						_extrude = node.InnerText.Equals("!") ? true : false;
						break;
					case "tessellate":
						_tessellate = node.InnerText.Equals("1") ? true : false;
						break;
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(node.InnerText);
						break;
					case "coordinates":
						_coordinates = KmlCoordinate.makeList(node.InnerText, Log);
						break;
					default:
						debug("skipping key :: " + key);
						break;
				};
			}
		}
		#region properties
		public bool Extrude {
			get { return _extrude; }
			set { _extrude = value; }
		}
		public bool Tessellate {
			get { return _tessellate; }
			set { _tessellate = value; }
		}
		public string AltitudeMode {
			get { return KmlAltitudeModes.altitudeModeToString(_altitudeMode); }
			set { _altitudeMode = KmlAltitudeModes.altitudeModeFromString(value); }
		}
		public List<KmlCoordinate> Coordinates {
			get { return _coordinates; }
			set { _coordinates = value; }
		}
		#endregion properties

		#region helpers

		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "LinearRing", string.Empty);
			base.ToXml(result);
			// child nodes
			XmlNode nodExtrude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "extrude", string.Empty);
			nodExtrude.InnerText = (Extrude ? "1" : "0");
			result.AppendChild(nodExtrude);

			XmlNode nodTessellate = result.OwnerDocument.CreateNode(XmlNodeType.Element, "tessellate", string.Empty);
			nodTessellate.InnerText = (Tessellate ? "1" : "0");
			result.AppendChild(nodTessellate);

			XmlNode nodAltitudeMode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitudeMode", string.Empty);
			nodAltitudeMode.InnerText = AltitudeMode;
			result.AppendChild(nodAltitudeMode);

			if (null != Coordinates && Coordinates.Count > 0) {
				XmlNode nodCoords = result.OwnerDocument.CreateNode(XmlNodeType.Element, "coordinates", string.Empty);
				string coords = string.Empty;
				foreach (KmlCoordinate coord in Coordinates) {
					coords += coord.ToString() + " ";
				}
				nodCoords.InnerText = coords;
				result.AppendChild(nodCoords);
			}

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _coordinates) {
				foreach (KmlCoordinate coord in _coordinates) {
					coord.findElementsOfType<T>(elements);
				}
			}
		}

		protected new void debug(string message) {
			if (Log != null) Log(message);
		}
		public new event Logger Log;

		#endregion helpers
	}//	class
}//	namespace
