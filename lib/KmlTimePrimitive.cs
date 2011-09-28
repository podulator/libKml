using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	
	public class KmlTimeSpan : KmlTimePrimitive, ISearchable  {
		private string _id = string.Empty;
		private Nullable<DateTime> _begin = null;
		private Nullable<DateTime> _end = null;
		
		public KmlTimeSpan() {}
		public KmlTimeSpan(XmlNode parent, Logger log) {
			Log += log;
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "begin":
						_begin = DateTime.Parse(node.InnerText);
						break;
					case "end":
						_end = DateTime.Parse(node.InnerText);
						break;
				};
			}
		}

		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public Nullable<DateTime> Begin {
			get { return _begin; }
			set { _begin = value; }
		}
		public Nullable<DateTime> End {
			get { return _end; }
			set { _end = value; }
		}
		#endregion properties
		
		#region helpers
		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "TimeSpan", string.Empty);
			if (_id.Length > 0) {
				XmlAttribute attId = result.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				result.Attributes.Append(attId);
			}
			// child nodes
			if (null != _begin) {
				XmlNode nodBegin = result.OwnerDocument.CreateNode(XmlNodeType.Element, "begin", string.Empty);
				nodBegin.InnerText = _begin.Value.ToString();
				result.AppendChild(nodBegin);
			}
			if (null != _end) {
				XmlNode nodEnd = result.OwnerDocument.CreateNode(XmlNodeType.Element, "end", string.Empty);
				nodEnd.InnerText = _end.Value.ToString();
				result.AppendChild(nodEnd);
			}
			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}

		#endregion helpers
	}
	public class KmlTimeStamp : KmlTimePrimitive, ISearchable  {
		private string _id = string.Empty;
		private string _when = string.Empty;
		
		public KmlTimeStamp() {}
		public KmlTimeStamp(XmlNode parent, Logger log) : this() {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "when":
						_when = node.InnerText;
						break;
				};
			}
		}
		
		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public string When {
			get { return _when; }
			set { _when = value; }
		}	
		#endregion properties
		
		#region helpers
		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "TimeStamp", string.Empty);
			if (_id.Length > 0) {
				XmlAttribute attId = result.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				result.Attributes.Append(attId);
			}
			// child nodes
			XmlNode nodWhen = result.OwnerDocument.CreateNode(XmlNodeType.Element, "when", string.Empty);
			nodWhen.InnerText = _when;
			result.AppendChild(nodWhen);
			return result;
		}
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		#endregion helpers
	}
	public abstract class KmlTimePrimitive {
		public virtual XmlNode ToXml(XmlNode parent) {return null;}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
	}
}//	namespace
