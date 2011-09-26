using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlLookAt : KmlAbstractView, ISearchable  {
		private KmlCoordinate _coordinate = new KmlCoordinate();
		private float _heading = 0;
		private float _tilt = 0;
		private double _range = 100;
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;

		#region constructors
		public KmlLookAt() : base() {}
		public KmlLookAt(XmlNode node, Logger log) : this() {
			Log += log;
			XmlNodeList nodes = node.ChildNodes;
			foreach (XmlNode child in nodes) {
				switch (child.Name.ToLower()) {
					case "longitude":
						_coordinate.Longitude = float.Parse(child.InnerText);
						break;
					case "latitude":
						_coordinate.Latitude = float.Parse(child.InnerText);
						break;
					case "altitude":
						_coordinate.Altitude = float.Parse(child.InnerText);
						break;
					case "tilt":
						_tilt = float.Parse(child.InnerText);
						break;
					case "heading":
						_heading = float.Parse(child.InnerText);
						break;
					case "range":
						_range = double.Parse(child.InnerText);
						break;
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(child.InnerText);
						break;
					default:
						break;
				};
			}
		}
		
		#endregion

		#region properties
		public double Longitude {
			get { return _coordinate.Longitude; }
			set { _coordinate.Longitude = value; }
		}
		public double Latitude {
			get { return _coordinate.Latitude; }
			set { _coordinate.Latitude = value; }
		}
		public double Altitude {
			get { return _coordinate.Altitude; }
			set { _coordinate.Altitude = value; }
		}
		public float Heading {
			get { return _heading; }
			set { _heading = value; }
		}
		public float Tilt {
			get { return _tilt; }
			set { _tilt = value; }
		}
		public double Range {
			get { return _range; }
			set { _range = value; }
		}
		public string AltitudeMode {
			get { return KmlAltitudeModes.altitudeModeToString(_altitudeMode); }
			set { _altitudeMode = KmlAltitudeModes.altitudeModeFromString(value); }
		}
		#endregion properties

		#region interfaces
		public override string ToString () {
			return String.Format("<LookAt>\n" +
						"\t<longitude>{0}</longitude>\n" +
						"\t<latitude>{1}</latitude>\n" +
						"\t<altitude>{2}</altitude>\n" + 
						"\t<range>{3}</range>\n" +
						"\t<tilt>{4}</tilt>\n" +
						"\t<heading>{5}</heading>\n" +
						"\t<altitudeMode>{6}</altitudeMode>\n" + 
						"</LookAt>\n", Longitude, Latitude, Altitude, Range, Tilt, Heading, AltitudeMode);
		}

		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "LookAt", string.Empty);
			//create child nodes
			XmlNode nodLong = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "longitude", string.Empty);
			nodLong.InnerText = Longitude.ToString();
			result.AppendChild(nodLong);
			
			XmlNode nodLat = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "latitude", string.Empty);
			nodLat.InnerText = Latitude.ToString();
			result.AppendChild(nodLat);
			
			XmlNode nodAlt = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "altitude", string.Empty);
			nodAlt.InnerText = Altitude.ToString();
			result.AppendChild(nodAlt);
			
			XmlNode nodRange = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "range", string.Empty);
			nodRange.InnerText = Range.ToString();
			result.AppendChild(nodRange);
			
			XmlNode nodTilt = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "tilt", string.Empty);
			nodTilt.InnerText = Tilt.ToString();
			result.AppendChild(nodTilt);
			
			XmlNode nodHeading = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "heading", string.Empty);
			nodHeading.InnerText = Heading.ToString();
			result.AppendChild(nodHeading);
			
			XmlNode nodAltitudeMode = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "altitudeMode", string.Empty);
			nodAltitudeMode.InnerText = AltitudeMode;
			result.AppendChild(nodAltitudeMode);

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion interfaces

		#region helpers
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
		
	}//	class
}//	namespace
