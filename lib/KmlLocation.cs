using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlLocation : KmlCoordinate, ISearchable  {

		#region constructors
		public KmlLocation () : base() {}
		public KmlLocation (XmlNode node, Logger log) : base(log) {
			XmlNodeList nodes = node.ChildNodes;
			foreach (XmlNode child in nodes) {
				switch (child.Name.ToLower()) {
					case "longitude":
						Longitude = float.Parse(child.InnerText);
						break;
					case "latitiude":
						Latitude = float.Parse(child.InnerText);
						break;
					case "altitude":
						Altitude = float.Parse(child.InnerText);
						break;
				};
			}
		}

		#endregion

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Location", string.Empty);
			// child nodes
			XmlNode nodLongitude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "longitude", string.Empty);
			nodLongitude.InnerText = Longitude.ToString();
			result.AppendChild(nodLongitude);

			XmlNode nodLatitude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "latitude", string.Empty);
			nodLatitude.InnerText = Latitude.ToString();
			result.AppendChild(nodLatitude);

			XmlNode nodAltitude = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitude", string.Empty);
			nodAltitude.InnerText = Altitude.ToString();
			result.AppendChild(nodAltitude);

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion helpers
	}//	class
}//	namespace
