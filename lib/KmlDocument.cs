using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlDocument : KmlContainer, IDeleteable, ICreatable {

		private List<KmlSchema> _schemas = new List<KmlSchema>();
			
		public KmlDocument() : base() {}
		public KmlDocument(XmlNode parent, Logger log) : this() {
			Log += log;
			fromXml(parent, log);
		}
		
		#region properties
		public List<KmlSchema> Schemas {
			get { return _schemas; }
			set { _schemas = value; }
		}
		#endregion properties

		#region helpers

		private void fromXml(XmlNode parent, Logger log) {
			foreach (XmlNode childNode in parent.ChildNodes) {
				handleNode(childNode, log);
			}
		}
		public new void handleNode (XmlNode node, Logger log) {
			string nodeKey = node.Name.ToLower();
			debug("handling :: " + nodeKey);
			switch (nodeKey) {
				// handle the feature nodes
				case "placemark":
					_features.Add(new KmlPlacemark(node, log));
					break;
				case "document":
					_features.Add(new KmlDocument(node, log));
					break;
				case "folder":
					_features.Add(new KmlFolder(node, log));
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
				// or a schema node
				case "schema":
					_schemas.Add(new KmlSchema(node, log));
					break;
				default:
					base.handleNode(node, log);
					break;
			};
		}

		public override XmlNode ToXml(XmlNode parent) {

				XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Document", string.Empty);

				if (null != _schemas && _schemas.Count > 0) {
					foreach (KmlSchema schema in _schemas) {
						result.AppendChild(schema.ToXml(result));
					}
				}

				base.ToXml(result);

				if (null != _features && _features.Count > 0) {
					foreach (KmlFeature feature in _features) {
						result.AppendChild(feature.ToXml(result));
					}
				}
				return result;
		}

		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _schemas) {
				foreach(KmlSchema schema in _schemas) {
					schema.findElementsOfType<T>(elements);
				}
			}
		}
		#endregion helpers
	}//	class
}//	namespace
