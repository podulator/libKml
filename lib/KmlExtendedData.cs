// 
//  KmlExtendedData.cs
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
	public class KmlData : ISearchable {
		private string _name = string.Empty;
		private string _displayName = string.Empty;
		private string _value = string.Empty;
		
		public KmlData() {}
		public KmlData(XmlNode parent) {
			if (null != parent.Attributes["name"])
				_name = parent.Attributes["name"].Value;

			foreach ( XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "displayname":
						_displayName = node.InnerText;
						break;
					case "value":
						_value = node.InnerText;
						break;
				};
			}
		}
		
		#region properties
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		public string DisplayName {
			get { return _displayName; }
			set { _displayName = value; }
		}
		public string Value {
			get { return _value; }
			set { _value = value; }
		}
		#endregion properties
		
		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			if (Value.Equals(string.Empty)) return null;

			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Data", string.Empty);
			if (Name.Length > 0) {
				XmlAttribute attName = result.OwnerDocument.CreateAttribute("name");
				attName.Value = Name;
				result.Attributes.Append(attName);
			}
			if (!DisplayName.Equals(string.Empty)) {
				XmlNode nodName = result.OwnerDocument.CreateNode(XmlNodeType.Element, "displayName", string.Empty);
				nodName.InnerText = DisplayName;
				result.AppendChild(nodName);
			}

			XmlNode nodValue = result.OwnerDocument.CreateNode(XmlNodeType.Element, "value", string.Empty);
			nodValue.InnerText = Value;
			result.AppendChild(nodValue);

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

	public class KmlExtendedData : ISearchable {
		private List<KmlData> _datum;
		private List<KmlSchemaData> _schemaData;

		public KmlExtendedData() {
			_datum = new List<KmlData>();
			_schemaData = new List<KmlSchemaData>();
		}
		public KmlExtendedData(XmlNode parent, Logger log) : this() {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "data":
						_datum.Add(new KmlData(node));
						break;
					case "schemadata":
						_schemaData.Add(new KmlSchemaData(node));
						break;
				};
			}
		}
		#region properties
		
		public List<KmlData> Datum {
			get { return _datum; }
			set { _datum = value; }
		}
		public List<KmlSchemaData> SchemaData {
			get { return _schemaData; }
			set { _schemaData = value; }
		}
		
		#endregion properties

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "ExtendedData", string.Empty);
			foreach (KmlData data in _datum) {
				XmlNode temp = data.ToXml(result);
				if (temp != null) 
					result.AppendChild(data.ToXml(result));
			}
			foreach (KmlSchemaData data in _schemaData) {
				XmlNode temp = data.ToXml(result);
				if (temp != null)
					result.AppendChild(data.ToXml(result));
			}
			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			foreach (KmlData data in _datum) {
				data.findElementsOfType<T>(elements);
			}
			foreach (KmlSchemaData data in _schemaData) {
				data.findElementsOfType<T>(elements);
			}
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
		
	}//	class
}//	namespace
