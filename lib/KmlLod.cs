using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlLod : ISearchable {
		private float _minLodPixels = 0.0f;
		private float _maxLodPixels = -1.0f;
		private float _minFadeExtent = 0.0f;
		private float _maxFadeExtent = 0.0f;

		public KmlLod() {}
		public KmlLod(XmlNode parent) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "minlodpixels":
						_minLodPixels = float.Parse(node.InnerText);
						break;
					case "maxlodpixels":
						_maxLodPixels = float.Parse(node.InnerText);
						break;
					case "minfadeextent":
						_minFadeExtent = float.Parse(node.InnerText);
						break;
					case "maxfadeextent":
						_maxFadeExtent = float.Parse(node.InnerText);
						break;
				};
			}
		}

		#region properties
		public float MinLodPixels {
			get { return _minLodPixels; }
			set { _minLodPixels = value; }
		}
		public float MaxLodPixels {
			get { return _maxLodPixels; }
			set { _maxLodPixels = value; }
		}
		public float MinFadeExtent {
			get { return _minFadeExtent; }
			set { _minFadeExtent = value; }
		}
		public float MaxFadeExtent {
			get { return _maxFadeExtent; }
			set { _maxFadeExtent = value; }
		}
		#endregion properties

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Lod", string.Empty);
			// child nodes
			XmlNode nodMinPix = result.OwnerDocument.CreateNode(XmlNodeType.Element, "minLodPixels", string.Empty);
			nodMinPix.InnerText = MinLodPixels.ToString();
			result.AppendChild(nodMinPix);

			XmlNode nodMaxPix = result.OwnerDocument.CreateNode(XmlNodeType.Element, "maxLodPixels", string.Empty);
			nodMaxPix.InnerText = MaxLodPixels.ToString();
			result.AppendChild(nodMaxPix);

			XmlNode nodMinFade = result.OwnerDocument.CreateNode(XmlNodeType.Element, "minFadeExtent", string.Empty);
			nodMinFade.InnerText = MinFadeExtent.ToString();
			result.AppendChild(nodMinFade);

			XmlNode nodMaxFade = result.OwnerDocument.CreateNode(XmlNodeType.Element, "maxFadeExtent", string.Empty);
			nodMaxFade.InnerText = MaxFadeExtent.ToString();
			result.AppendChild(nodMaxFade);

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
