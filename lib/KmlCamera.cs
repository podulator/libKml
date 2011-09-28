using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlCamera : KmlAbstractView, ISearchable  {
		private KmlCoordinate _coordinate;
		private float _heading;
		private float _tilt;
		private double _roll;
		private string _altitudeMode;

		#region constructors
		public KmlCamera ()
			: base() {
			_coordinate = new KmlCoordinate();
			_heading = 0;
			_tilt = 0;
			_roll = 100;
			_altitudeMode = string.Empty;
		}
		public KmlCamera (XmlNode node, Logger log) : base() {
			Log += log;
			XmlNodeList nodes = node.ChildNodes;
			foreach (XmlNode child in nodes) {
				switch (child.Name.ToLower()) {
					case "longitude":
						_coordinate.Longitude = float.Parse(child.InnerText);
						break;
					case "latitiude":
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
					case "roll":
						_roll = double.Parse(child.InnerText);
						break;
					case "altitudemode":
						_altitudeMode = child.InnerText;
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
		public double Roll {
			get { return _roll; }
			set { _roll = value; }
		}
		public string AltitudeMode {
			get { return _altitudeMode; }
			set { _altitudeMode = value; }
		}
		#endregion properties

		#region interfaces
		public override string ToString () {
			return String.Format("<Camera>\n" +
						"\t<longitude>{0}</longitude>\n" +
						"\t<latitude>{1}</latitude>\n" +
						"\t<altitude>{2}</altitude>\n" +
						"\t<roll>{3}</roll>\n" +
						"\t<tilt>{4}</tilt>\n" +
						"\t<heading>{5}</heading>\n" +
						"\t<altitudeMode>{6}</altitudeMode>\n" +
						"</Camera>\n", Longitude, Latitude, Altitude, Roll, Tilt, Heading, AltitudeMode);
		}

		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Camera", string.Empty);

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

			XmlNode nodRoll = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "roll", string.Empty);
			nodRoll.InnerText = Roll.ToString();
			result.AppendChild(nodRoll);

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
			if (null != _coordinate)
				_coordinate.findElementsOfType<T>(elements);
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
