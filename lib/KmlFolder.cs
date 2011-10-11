using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlFolder : KmlContainer, ISearchable  {
		
		public KmlFolder() {}
		public KmlFolder(XmlNode parent, Logger log) : base(parent, log) { fromXml(parent, log); }

		#region helpers

		private void fromXml (XmlNode parent, Logger log) {
			foreach (XmlNode child in parent.ChildNodes) {
				string key = child.Name.ToLower();
				debug("handling :: " + key);
				switch (key) {
					default:
						base.handleNode(child, log);
						break;
				};
			}
		}

		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Folder", string.Empty);
			base.ToXml(result);
			if (null != _features) {
				foreach(KmlFeature feature in _features) {
					result.AppendChild(feature.ToXml(result));
				}
			}
			return result;
		}
		private new void debug (string mesage) {
			base.debug("KmlFolder :: " + mesage);
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion helpers

	}//	class
}//	namespace
