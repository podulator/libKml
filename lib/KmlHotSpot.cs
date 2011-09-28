using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlHotSpot : ISearchable {
		private float _x;
		private float _y;
		private string _xUnits;
		private string _yUnits;
		
		#region constructors
		public KmlHotSpot() {
			_x = 0;
			_y = 0;
			_xUnits = string.Empty;
			_yUnits = string.Empty;
		}
		public KmlHotSpot (float x, float y, string xUnits, string yUnits) {
			_x = x;
			_y = y;
			_xUnits = xUnits;
			_yUnits = yUnits;
		}
		public KmlHotSpot(XmlNode node) {
			foreach (XmlAttribute attribute in node.Attributes) {
				switch (attribute.Name.ToLower()) {
					case "x":
						_x = float.Parse(attribute.InnerText);
						break;
					case "y":
						_y = float.Parse(attribute.InnerText);
						break;
					case "xunits":
						_xUnits = attribute.InnerText;
						break;
					case "yunits":
						_yUnits = attribute.InnerText;
						break;
					default:
						break;
				};
			}
		}
		#endregion constructors

		#region properties
		public float X {
			get { return _x; }
			set { _x = value; }
		}
		public float Y {
			get { return _y; }
			set { _y = value; }
		}
		public string XUnits {
			get { return _xUnits; }
			set { _xUnits = value; }
		}
		public string YUnits {
			get { return _yUnits; }
			set { _yUnits = value; }
		}
		#endregion properties

		#region interfaces
		public override string ToString () {
			return string.Format(	@"<hotspot x=""{0}"" y=""{1}"" xunits=""{2}"" yunits=""{3}"" />\n", 
											_x, _y, _xUnits, _yUnits);
		}
		#endregion interfaces

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "hotSpot", string.Empty);
			XmlAttribute attX = parent.OwnerDocument.CreateAttribute("x");
			attX.Value = X.ToString();
			result.Attributes.Append(attX);

			XmlAttribute attY = parent.OwnerDocument.CreateAttribute("y");
			attY.Value = Y.ToString();
			result.Attributes.Append(attY);

			XmlAttribute attXunits = parent.OwnerDocument.CreateAttribute("xunits");
			attXunits.Value = XUnits;
			result.Attributes.Append(attXunits);

			XmlAttribute attYunits = parent.OwnerDocument.CreateAttribute("yunits");
			attYunits.Value = YUnits;
			result.Attributes.Append(attYunits);

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
}//	namespace
