using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public enum KmlSimpleFieldTypes : int {
		tString = 0, 
		tInt = 1, 
		tUint = 2, 
		tShort = 4, 
		tUshort = 8, 
		tFloat = 16, 
		tDouble = 32, 
		tBool = 64
	}

	public class KmlSimpleField : ISearchable {
		private KmlSimpleFieldTypes _type = KmlSimpleFieldTypes.tString;
		private string _name = string.Empty;
		private string _displayName = string.Empty;
		
		public KmlSimpleField() {}
		public KmlSimpleField(XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["name"]) 
				_name = parent.Attributes["name"].Value;
			if (null != parent.Attributes["type"]) 
				_type = simpleFieldTypeFromString(parent.Attributes["type"].Value);

			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "displayname":
						_displayName = node.InnerText;
						break;
				};
			}
		}//	constructor

		#region properties
		public string Type {
			get { return simpleFieldTypeToString(_type); }
			//set { _type = simpleFieldTypeFromString(value); }
		}
		public void setType(KmlSimpleFieldTypes type) {
			_type = type;
		}
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		public string DisplayName {
			get { return _displayName; }
			set { _displayName = value; }
		}
		#endregion properties
		
		#region helpers
		public KmlSimpleFieldTypes simpleFieldTypeFromString(string value) {
			switch (value.ToLower()) {
				case "bool":
					return KmlSimpleFieldTypes.tBool;
				case "double":
					return KmlSimpleFieldTypes.tDouble;
				case "float":
					return KmlSimpleFieldTypes.tFloat;
				case "ushort":
					return KmlSimpleFieldTypes.tUshort;
				case "short":
					return KmlSimpleFieldTypes.tShort;
				case "uint":
					return KmlSimpleFieldTypes.tUint;
				case "int":
					return KmlSimpleFieldTypes.tInt;
				default:
					return KmlSimpleFieldTypes.tString;
			};
		}
		public string simpleFieldTypeToString(KmlSimpleFieldTypes value) {
			switch (value) {
				case KmlSimpleFieldTypes.tBool:
					return "bool";
				case KmlSimpleFieldTypes.tDouble:
					return "double";
				case KmlSimpleFieldTypes.tFloat:
					return "float";
				case KmlSimpleFieldTypes.tUshort:
					return "ushort";
				case KmlSimpleFieldTypes.tShort:
					return "short";
				case KmlSimpleFieldTypes.tUint:
					return "uint";
				case KmlSimpleFieldTypes.tInt:
					return "int";
				default:
					return "string";
			};
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
	}
	
	public class KmlSchema {
		private string _id = string.Empty;
		private string _name = string.Empty;
		private List<KmlSimpleField> _simpleFields = new List<KmlSimpleField>();
		
		public KmlSchema() {}
		public KmlSchema(XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
			if (null != parent.Attributes["name"])
				_name = parent.Attributes["name"].Value;
				
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "simplefield":
						_simpleFields.Add(new KmlSimpleField(node, log));
						break;
				};
			}
		}
		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		public List<KmlSimpleField> SimpleFields {
			get { return _simpleFields; }
			set { _simpleFields = value; }
		}
		#endregion properties
		
		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Schema", string.Empty);
			if (Id.Length > 0) {
				XmlAttribute attId = parent.OwnerDocument.CreateAttribute("id");
				attId.Value = Id;
				result.Attributes.Append(attId);
			}
			if (Name.Length > 0) {
				XmlAttribute attName = parent.OwnerDocument.CreateAttribute("name");
				attName.Value = Name;
				result.Attributes.Append(attName);
			}

			foreach (KmlSimpleField field in _simpleFields) {
				if (field.Name.Length == 0 && field.DisplayName.Length == 0) continue;
				XmlNode fieldNode = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "SimpleField", string.Empty);

				XmlAttribute attType = parent.OwnerDocument.CreateAttribute("type");
				attType.Value = field.Type;
				fieldNode.Attributes.Append(attType);

				if (field.Name.Length > 0) {
					XmlAttribute attName = parent.OwnerDocument.CreateAttribute("name");
					attName.Value = field.Name;
					fieldNode.Attributes.Append(attName);
				}
				if (field.DisplayName.Length > 0) {
					XmlAttribute attName = parent.OwnerDocument.CreateAttribute("displayName");
					attName.Value = field.DisplayName;
					fieldNode.Attributes.Append(attName);
				}
				result.AppendChild(fieldNode);
			}
			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion
	}//	class
}//	namespace
