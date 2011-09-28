using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlAlias : ISearchable {
		private string _targetHref = string.Empty;
		private string _sourceHref = string.Empty;
		
		public KmlAlias() {}
		public KmlAlias(XmlNode parent, Logger log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "targethref":
						_targetHref = node.InnerText;
						break;
					case "sourcehref":
						_sourceHref = node.InnerText;
						break;
				};
			}
		}
		#region properties
		public string TargetHref {
			get { return _targetHref; }
			set { _targetHref = value; }
		}
		public string SourceHref {
			get { return _sourceHref; }
			set { _sourceHref = value; }
		}
		#endregion properties
		
		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Alias", string.Empty);
			// child nodes
			XmlNode nodTarget = result.OwnerDocument.CreateNode(XmlNodeType.Element, "targetHref", string.Empty);
			nodTarget.InnerText = TargetHref;
			result.AppendChild(nodTarget);

			XmlNode nodSource = result.OwnerDocument.CreateNode(XmlNodeType.Element, "sourceHref", string.Empty);
			nodSource.InnerText = SourceHref;
			result.AppendChild(nodSource);

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
