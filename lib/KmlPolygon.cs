using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlPolygon : KmlGeometry, ISearchable  {
		private bool _extrude = false;
		private bool _tessellate = false;
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;
		private KmlLinearRing _outerBoundary = new KmlLinearRing();
		private KmlLinearRing _innerBoundary = new KmlLinearRing();
		
		public KmlPolygon() {}
		public KmlPolygon(XmlNode parent, Logger log) : base(parent, log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "extrude":
						_extrude = node.InnerText.Equals("1") ? true : false;
						break;
					case "tessellate":
						_tessellate = node.InnerText.Equals("1") ? true : false;
						break;
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(node.InnerText);
						break;
					case "outerboundaryis":
						_outerBoundary = new KmlLinearRing(node.FirstChild, log);
						break;
					case "innerboundaryis":
						_innerBoundary = new KmlLinearRing(node.FirstChild, log);
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
		public KmlLinearRing OuterBoundary {
			get { return _outerBoundary; }
			set { _outerBoundary = value; }
		}
		public KmlLinearRing InnerBoundary {
			get { return _innerBoundary; }
			set { _innerBoundary = value; }
		}
		#endregion properties

		#region helpers
		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Polygon", string.Empty);
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
			
			if (null != _outerBoundary) {
				XmlNode nodOuter = result.OwnerDocument.CreateNode(XmlNodeType.Element, "outerBoundaryIs", string.Empty);
				nodOuter.AppendChild(_outerBoundary.ToXml(nodOuter));
				result.AppendChild(nodOuter);
			}
			
			if (null != _innerBoundary) {
				XmlNode nodInner = result.OwnerDocument.CreateNode(XmlNodeType.Element, "innerBoundaryIs", string.Empty);
				nodInner.AppendChild(_innerBoundary.ToXml(nodInner));
				result.AppendChild(nodInner);
			}

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _outerBoundary)
				_outerBoundary.findElementsOfType<T>(elements);
			if (null != _innerBoundary)
				_innerBoundary.findElementsOfType<T>(elements);
		}
		#endregion helpers
	}//	class
}//	namespace
