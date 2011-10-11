using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlPlacemark : KmlFeature, ISearchable  {
		private KmlGeometry _geometry;
		
		public KmlPlacemark() : base() { _geometry = null; }
		public KmlPlacemark(XmlNode parent, Logger log) : base(parent, log) { fromXml(parent, log); }

		public KmlGeometry Geometry {
			get { return _geometry; }
			set { _geometry = value; }
		}
		
		#region helpers

		private void fromXml (XmlNode parent, Logger log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				debug("KmlPlacemark handling key " + key);
				switch (key) {
					case "point":
						_geometry = new KmlPoint(node, log);
						break;
					case "linestring":
						_geometry = new KmlLineString(node, log);
						break;
					case "linearring":
						_geometry = new KmlLinearRing(node, log);
						break;
					case "polygon":
						_geometry = new KmlPolygon(node, log);
						break;
					case "multigeometry":
						_geometry = new KmlMultiGeometry(node, log);
						break;
					case "model":
						_geometry = new KmlModel(node, log);
						break;
					default:
						base.handleNode(node, log);
						break;
				};
			}
		}
		private new void debug (string message) {
			base.debug("KmlPlacemark :: " + message);
		}

		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Placemark", string.Empty);
			base.ToXml(result);
			if (null != _geometry) {
				result.AppendChild(_geometry.ToXml(result));
			}
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _geometry)
				_geometry.findElementsOfType<T>(elements);
		}
		#endregion helpers

	}//	class
}//	namespace
