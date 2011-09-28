using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public abstract class KmlContainer : KmlFeature, ISearchable  {
		protected List<KmlFeature> _features = new List<KmlFeature>();

		public KmlContainer() : base() {}
		public KmlContainer(XmlNode parent, Logger log) : base(parent, log) {}

		public List<KmlFeature> Features {
			get { return _features; }
			set { _features = value; }
		}
		
		#region helpers
		
		public new void handleNode(XmlNode node, Logger log) {
			string key = node.Name.ToLower();
			switch (key) {
				case "folder":
					_features.Add(new KmlFolder(node, log));
					break;
				case "document":
					_features.Add(new KmlDocument(node, log));
					break;
				case "placemark":
					_features.Add(new KmlPlacemark(node, log));
					break;
				case "networklink":
					_features.Add(new KmlNetworkLink(node, log));
					break;
				case "groundoverlay":
					_features.Add(new KmlGroundOverlay(node, log));
					break;
				case "photooverlay":
					_features.Add(new KmlPhotoOverlay(node, log));
					break;
				case "screenoverlay":
					_features.Add(new KmlScreenOverlay(node, log));
					break;
				default:
					// pass it down to Feature
					base.handleNode(node, log);
					break;
			};
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			foreach (KmlFeature feature in _features) {
				feature.findElementsOfType<T>(elements);
			}
		}
		#endregion helpers
	}//	class
}//	namespace
