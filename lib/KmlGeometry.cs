using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public abstract class KmlGeometry : ISearchable {
		private string _id = string.Empty;
		
		public KmlGeometry() {}
		public KmlGeometry(XmlNode parent, Logger log) : this(parent) { Log += log; }
		public KmlGeometry(XmlNode parent) {
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
		}
		
		public string Id {
			get { return _id; }
			set { _id = value; }
		}

		#region helpers
		public virtual XmlNode ToXml(XmlNode parent) {
			if (_id.Length > 0) {
				XmlAttribute id = parent.OwnerDocument.CreateAttribute(string.Empty, "id", string.Empty);
				id.Value = _id;
				parent.Attributes.Append(id);
			}
			return null;
		}
		public virtual void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
	}//	class
}//	namespace
