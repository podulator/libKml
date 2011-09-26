using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlSchemaData : ISearchable {
		private string _scheamUrl = string.Empty;
		private List<KeyValuePair<string, string>> _simpleData = new List<KeyValuePair<string,string>>();
		
		public KmlSchemaData() {}
		public KmlSchemaData(XmlNode parent) {
			if (null != parent.Attributes["schemaUrl"])
				_scheamUrl = parent.Attributes["schemaUrl"].Value;
			foreach (XmlNode node in parent.ChildNodes) {
				if (node.Name.ToLower().Equals("simpledata")) {
					if (null != node.Attributes["name"]) {
						_simpleData.Add(new KeyValuePair<string,string>(node.Attributes["name"].Value, node.InnerText));
					}
				}
			}
		}//	constructor

		#region properties
		public string SchemaUrl {
			get { return _scheamUrl; }
			set { _scheamUrl = value; }
		}
		public List<KeyValuePair<string, string>> SimpleData {
			get { return _simpleData; }
			set { _simpleData = value; }
		}
		#endregion properties
		
		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "SchemaData", string.Empty);
			foreach (KeyValuePair<string, string> item in _simpleData) {
				XmlNode nodSimple = result.OwnerDocument.CreateNode(XmlNodeType.Element, "SimpleData", string.Empty);
				if (item.Key.Length > 0) {
					XmlAttribute attName = result.OwnerDocument.CreateAttribute("name");
					attName.Value = item.Key;
					nodSimple.Attributes.Append(attName);
				}
				nodSimple.InnerText = item.Value;
				result.AppendChild(nodSimple);
			}
			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		#endregion helpers
		
	}//	class
}//	namespace
