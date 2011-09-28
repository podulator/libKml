using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlPolyStyle : KmlColourStyle, ISearchable  {
		private bool _fill = true;
		private bool _outline = true;

		public KmlPolyStyle() : base() {}
		public KmlPolyStyle(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "fill":
						_fill = node.InnerText.Equals("1") ? true : false;
						break;
					case "outline":
						_outline = node.InnerText.Equals("1") ? true : false;
						break;
				};
			}
		}

		#region properties
		
		public bool Fill {
			get { return _fill; }
			set { _fill = value; }
		}
		public bool Outline {
			get { return _outline; }
			set { _outline = value; }
		}

		#endregion properties
		
		#region helpers
		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "PolyStyle", string.Empty);
			base.ToXml(result);
			//child nodes
			XmlNode nodFill = result.OwnerDocument.CreateNode(XmlNodeType.Element, "fill", string.Empty);
			nodFill.InnerText = Fill ? "1" : "0";
			result.AppendChild(nodFill);

			XmlNode nodOutline = result.OwnerDocument.CreateNode(XmlNodeType.Element, "outline", string.Empty);
			nodOutline.InnerText = Outline ? "1" : "0";
			result.AppendChild(nodOutline);

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion helpers

	}//	class
}//	namespace
