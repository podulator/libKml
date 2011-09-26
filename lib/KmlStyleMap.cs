using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlStyleMap : KmlStyleSelector, ISearchable  {

		private List<KmlPair> _pairs = new List<KmlPair>();
		
		public KmlStyleMap() : base() {}
		public KmlStyleMap(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "pair":
						_pairs.Add(new KmlPair(node));
						break;
				};
			}
		}
		#region properties
		public List<KmlPair> Pairs {
			get { return _pairs; }
			set { _pairs = value; }
		}
		#endregion properties
		
		#region helpers
		public override XmlNode ToXml(XmlNode parent, Logger log) {

			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "StyleMap", string.Empty);
			base.ToXml(result, log);
			if (null != _pairs && _pairs.Count > 0) {
				foreach (KmlPair pair in _pairs) {
					result.AppendChild(pair.ToXml(result));
				}
			}
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _pairs) {
				foreach (KmlPair pair in _pairs) {
					pair.findElementsOfType<T>(elements);
				}
			}
		}
		#endregion helpers
	}//	class
}//	namespace
