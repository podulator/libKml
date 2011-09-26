using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlModel : KmlGeometry, ISearchable  {
		private AltitudeModes _altitudeMode = AltitudeModes.clampToGround;
		private KmlLocation _location = new KmlLocation();
		private KmlOrientation _orientation = new KmlOrientation();
		private KmlScale _scale = new KmlScale();
		private string _link = string.Empty;
		private List<KmlAlias> _resourceMap = new List<KmlAlias>();

		public KmlModel() {}
		public KmlModel(XmlNode parent, Logger log) : base(parent, log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "altitudemode":
						_altitudeMode = KmlAltitudeModes.altitudeModeFromString(node.InnerText);
						break;
					case "location":
						_location = new KmlLocation(node, log);
						break;
					case "orientation":
						_orientation = new KmlOrientation(node, log);
						break;
					case "scale":
						_scale = new KmlScale(node, log);
						break;
					case "link":
						_link = node.InnerText;
						break;
					case "resourcemap":
						foreach (XmlNode alias in node.ChildNodes) {
							_resourceMap.Add(new KmlAlias(alias, log));
						}
						break;
				};
			}
		}
		
		#region properties

		public string AltitudeMode {
			get { return KmlAltitudeModes.altitudeModeToString(_altitudeMode); }
			set { _altitudeMode = KmlAltitudeModes.altitudeModeFromString(value); }
		}
		public KmlLocation Location {
			get { return _location; }
			set { _location = value; }
		}
		public KmlOrientation Orientation {
			get { return _orientation; }
			set { _orientation = value; }
		}
		public KmlScale Scale {
			get { return _scale; }
			set { _scale = value; }
		}
		public string Link {
			get { return _link; }
			set { _link = value; }
		}
		public List<KmlAlias>ResourceMap {
			get { return _resourceMap; }
			set { _resourceMap = value; }
		}
		#endregion properties

		#region helpers
		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Model", string.Empty);
			base.ToXml(result);
			// child nodes
			XmlNode nodAltitudeMode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "altitudeMode", string.Empty);
			nodAltitudeMode.InnerText = AltitudeMode;
			result.AppendChild(nodAltitudeMode);

			if (null != _location) {
				result.AppendChild(_location.ToXml(result));
			}
			if (null != _orientation) {
				result.AppendChild(_orientation.ToXml(result));
			}
			if (null != _scale) {
				result.AppendChild(_scale.ToXml(result));
			}
			XmlNode nodLink = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Link", string.Empty);
			nodLink.InnerText = Link;
			result.AppendChild(nodLink);

			if (null != _resourceMap && _resourceMap.Count > 0) {
				XmlNode nodResource = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "ResourceMap", string.Empty);
				foreach (KmlAlias alias in _resourceMap) {
					nodResource.AppendChild(alias.ToXml(nodResource));
				}
				result.AppendChild(nodResource);
			}
			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _location)
				_location.findElementsOfType<T>(elements);
			if (null != _orientation)
				_orientation.findElementsOfType<T>(elements);
			if (null != _scale)
				_scale.findElementsOfType<T>(elements);
			if (null != _resourceMap) {
				foreach (KmlAlias alias in _resourceMap) {
					alias.findElementsOfType<T>(elements);
				}
			}
		}
		#endregion helpers

	}//	class
}//	namespace
