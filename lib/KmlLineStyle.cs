using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlLineStyle : KmlColourStyle, ISearchable  {
		private float _width = 1.0f;
		
		public KmlLineStyle() : base() {}
		public KmlLineStyle(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "width":
						_width = float.Parse(node.InnerText);
						break;
				};
			}
		}
		
		#region properties
		public float Width {
			get { return _width; }
			set { _width = value; }
		}
		#endregion properties
		
		#region helpers
		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "LineStyle", string.Empty);
			base.ToXml(result);
			//child nodes
			XmlNode nodWidth = result.OwnerDocument.CreateNode(XmlNodeType.Element, "width", string.Empty);
			nodWidth.InnerText = Width.ToString();
			result.AppendChild(nodWidth);
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion helpers

	}//	class
}//	namespace
