// 
//  KmlSchemaData.cs
//  
//  Author:
//       mat rowlands <code-account@podulator.com>
//  
//  Copyright (c) 2011 mat rowlands
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
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
