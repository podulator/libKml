using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlLabelStyle : KmlColourStyle, ISearchable  {
		private float _scale = 1.0f;
		
		public KmlLabelStyle() : base() {}

		public KmlLabelStyle(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "scale":
						_scale = float.Parse(node.InnerText);
						break;
				};
			}
		}
		
		#region properties
		public float Scale {
			get { return _scale; }
			set { _scale = value; }
		}
		#endregion properties
		
		#region helpers
		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "LabelStyle", string.Empty);
			base.ToXml(result);
			// child nodes
			XmlNode nodScale = result.OwnerDocument.CreateNode(XmlNodeType.Element, "scale", string.Empty);
			nodScale.InnerText = Scale.ToString();
			result.AppendChild(nodScale);

			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion helpers
	}//	class
}//	namespace
