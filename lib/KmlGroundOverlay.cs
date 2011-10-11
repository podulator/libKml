using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlGroundOverlay : KmlOverlay, ISearchable  {
		private double _altitude = 0.0d;
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;
		private KmlLatLonBox _latLonBox = new KmlLatLonBox();
		
		public KmlGroundOverlay() {}
		public KmlGroundOverlay(XmlNode parent, Logger log) : base(parent, log) { fromXml(parent, log); }

		#region properties
		public double Altitude {
			get { return _altitude; }
			set { _altitude = value; }
		}
		public string AltitudeMode {
			get { return KmlAltitudeModes.altitudeModeToString(_altitudeMode); }
			set { _altitudeMode = KmlAltitudeModes.altitudeModeFromString(value); }
		}
		public KmlLatLonBox LatLonBox {
			get { return _latLonBox; }
			set { _latLonBox = value; }
		}
		#endregion properties
		
		#region helpers

		private void fromXml (XmlNode parent, Logger log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "altitude":
						_altitude = double.Parse(node.InnerText);
						break;
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(node.InnerText);
						break;
					case "latlonbox":
						_latLonBox = new KmlLatLonBox(node);
						break;
					default:
						base.handleNode(node, log);
						break;
				};
			}
		}

		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "GroundOverlay", string.Empty);
			base.ToXml(result);
			// child nodes
			XmlNode nodAltitude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitude", string.Empty);
			nodAltitude.InnerText = Altitude.ToString();
			result.AppendChild(nodAltitude);
			
			XmlNode nodAltitudeNode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitudeMode", string.Empty);
			nodAltitudeNode.InnerText = AltitudeMode;
			result.AppendChild(nodAltitudeNode);
			
			if (null != _latLonBox)
				result.AppendChild(_latLonBox.ToXml(result));
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _latLonBox)
				_latLonBox.findElementsOfType<T>(elements);
		}
		#endregion helpers
		
	}//	class
}//	Kml
