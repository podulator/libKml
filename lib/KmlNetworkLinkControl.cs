using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlNetworkLinkControl : ISearchable {
		private float _minRefreshPeriod = 0.0f;
		private float _maxSessionLength = -1.0f;
		private string _cookie = string.Empty;
		private string _session = string.Empty;
		private string _message = string.Empty;
		private string _linkName = string.Empty;
		private string _linkDescription = string.Empty;
		private string _linkSnippet = string.Empty;
		private string _expires = string.Empty;
		private KmlUpdate _update = new KmlUpdate();
		private KmlAbstractView _view = null;
		
		public KmlNetworkLinkControl() {}
		public KmlNetworkLinkControl(XmlNode parent, Logger log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.NamespaceURI.ToLower();
				switch (key) {
					case "minrefreshperiod":
						_minRefreshPeriod = float.Parse(node.InnerText);
						break;
					case "maxsessionlength":
						_maxSessionLength = float.Parse(node.InnerText);
						break;
					case "cookie":
						_cookie = node.InnerText;
						break;
					case "messsage":
						_message = node.InnerText;
						break;
					case "linkname":
						_linkName = node.InnerText;
						break;
					case "linkdescription":
						_linkDescription = node.InnerText;
						break;
					case "linksnippet":
						_linkSnippet = node.InnerText;
						break;
					case "expires":
						_expires = node.InnerText;
						break;
					case "update":
						_update = new KmlUpdate(node, log);
						break;
					case "lookat":
						_view = new KmlLookAt(node, log);
						break;
					case "camera":
						_view = new KmlCamera(node, log);
						break;
				};
			}
		}

		#region properties
		public float MinRefreshPeriod {
			get { return _minRefreshPeriod; }
			set { _minRefreshPeriod = value; }
		}
		public float MaxSessionLength {
			get { return _maxSessionLength; }
			set { _maxSessionLength = value; }
		}
		public string Cookie {
			get { return _cookie; }
			set { _cookie = value; }
		}
		public string Session {
			get { return _session; }
			set { _session = value; }
		}
		public string Message {
			get { return _message; }
			set { _message = value; }
		}
		public string LinkName {
			get { return _linkName; }
			set { _linkName = value; }
		}
		public string LinkDescription {
			get { return _linkDescription; }
			set { _linkDescription = value; }
		}
		public string LinkSnippet {
			get { return _linkSnippet; }
			set { _linkSnippet = value; }
		}
		public string Expires {
			get { return _expires; }
			set { _expires = value; }
		}
		public KmlUpdate Update {
			get { return _update; }
			set { _update = value; }
		}
		public KmlAbstractView View {
			get { return _view; }
			set { _view = value; }
		}
		#endregion properties
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "NetworkLinkControl", string.Empty);
			// child nodes
			XmlNode nodMinRefreshPeriod = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "minRefreshPeriod", string.Empty);
			nodMinRefreshPeriod.InnerText = MinRefreshPeriod.ToString();
			result.AppendChild(nodMinRefreshPeriod);
			
			XmlNode nodMaxSessionLength = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "maxSessionLength", string.Empty);
			nodMaxSessionLength.InnerText = MaxSessionLength.ToString();
			result.AppendChild(nodMaxSessionLength);
			
			XmlNode nodCookie = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "cookie", string.Empty);
			nodCookie.InnerText = Cookie;
			result.AppendChild(nodCookie);
			
			XmlNode nodMessage = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "message", string.Empty);
			nodMessage.InnerText = Message;
			result.AppendChild(nodMessage);
			
			XmlNode nodLinkName = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "linkName", string.Empty);
			nodLinkName.InnerText = LinkName;
			result.AppendChild(nodLinkName);
			
			XmlNode nodLinkDescription = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "linkDescription", string.Empty);
			nodLinkDescription.InnerText = LinkDescription;
			result.AppendChild(nodLinkDescription);
			
			XmlNode nodLinkSnippet = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "linkSnippet", string.Empty);
			nodLinkSnippet.InnerText = LinkSnippet;
			result.AppendChild(nodLinkSnippet);
			
			XmlNode nodExpires = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "expires", string.Empty);
			nodExpires.InnerText = Expires;
			result.AppendChild(nodExpires);
			
			if (null != _update) {
				result.AppendChild(_update.ToXml(result));
			}
			if (null != _view) {
				result.AppendChild(_view.ToXml(result));
			}
			return result;
		}
		#region helpers
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			if (null != _update)
				_update.findElementsOfType<T>(elements);
			if (null != _view)
				_view.findElementsOfType<T>(elements);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
		
	}//	class
}//	namespace
