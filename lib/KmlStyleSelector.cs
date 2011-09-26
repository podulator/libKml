using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public abstract class KmlStyleSelector : ISearchable {
		private string _id;
		
		public KmlStyleSelector() {
			_id = string.Empty;
		}
		public KmlStyleSelector(XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
		}
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		
		public virtual XmlNode ToXml(XmlNode parent, Logger log) {
			Log += log;
			if (_id.Length > 0) {
				XmlAttribute attId = parent.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				parent.Attributes.Append(attId);
			}
			return null;
		}
		
		public virtual void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		
		#region helpers
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
	}//	class
}//	namespace
