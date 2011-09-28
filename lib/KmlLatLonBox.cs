using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlLatLonBox : ISearchable {
		private float _north = 0.0f;
		private float _south = 0.0f;
		private float _east = 0.0f;
		private float _west = 0.0f;
		private float _rotation = 0.0f;

		public KmlLatLonBox () { }
		public KmlLatLonBox (XmlNode parent) {
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
					case "rotation":
						_rotation = float.Parse(node.InnerText);
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
		public float Rotation {
			get { return _rotation; }
			set { _rotation = value; }
		}
		#endregion properties

		#region helpers
		public XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "LatLoBox", string.Empty);
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

			XmlNode nodRotation = result.OwnerDocument.CreateNode(XmlNodeType.Element, "rotation", string.Empty);
			nodRotation.InnerText = Rotation.ToString();
			result.AppendChild(nodRotation);

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
