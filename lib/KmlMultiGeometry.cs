using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlMultiGeometry : KmlGeometry, ISearchable  {
		private List<KmlGeometry> _elements = new List<KmlGeometry>();
		
		public KmlMultiGeometry() {}
		public KmlMultiGeometry(XmlNode parent, Logger log) : base(parent, log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "point":
						_elements.Add(new KmlPoint(node, log));
						break;
					case "linestring":
						_elements.Add(new KmlLineString(node, log));
						break;
					case "linearring":
						_elements.Add(new KmlLinearRing(node, log));
						break;
					case "polygon":
						_elements.Add(new KmlPolygon(node, log));
						break;
					case "multigeometry":
						_elements.Add(new KmlMultiGeometry(node, log));
						break;
					case "model":
						_elements.Add(new KmlModel(node, log));
						break;
					}
				}
		}
		
		#region properties
		public List<KmlGeometry> Elements {
			get { return _elements; }
			set { _elements = value; }
		}
		#endregion properties

		#region helpers
		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "MultiGeometry", string.Empty);
			base.ToXml(result);
			if (null != _elements) {
				foreach (KmlGeometry element in _elements) {
					result.AppendChild(element.ToXml(result));
				}
			}
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _elements) {
				foreach (KmlGeometry element in _elements) {
					element.findElementsOfType<T>(elements);
				}
			}
		}
		#endregion helpers
	}//	class
}//	namespace
