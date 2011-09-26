using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {

	public enum KmlRefreshModes : int {
		onChange = 0,
		onInterval = 1,
		onExpire = 2
	};
	public enum KmlViewRefreshModes : int {
		never = 0,
		onStop = 1,
		onRequest = 2,
		onRegion = 4
	};

	public class KmlLink : KmlIcon {
		public KmlLink () {
			ElementName = "Link";
		}
	}
	public class KmlIcon : ISearchable {
		protected string ElementName = "Icon";
		private string _id = string.Empty;
		private string _href = string.Empty;
		private KmlRefreshModes _refreshMode = KmlRefreshModes.onChange;
		private float _refreshInterval = 0.0f;
		private KmlViewRefreshModes _viewRefreshMode = KmlViewRefreshModes.never;
		private float _viewRefreshTime = 4.0f;
		private float _viewBoundScale = 1.0f;
		private string _viewFormat = string.Empty;
		private string _httpQuery = string.Empty;

		public KmlIcon () { }
		public KmlIcon (XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "href":
						_href = node.InnerText;
						break;
					case "refreshmode":
						_refreshMode = refreshModeFromString(node.InnerText);
						break;
					case "refreshinterval":
						_refreshInterval = float.Parse(node.InnerText);
						break;
					case "viewrefreshmode":
						_viewRefreshMode = viewRefreshModeFromString(node.InnerText);
						break;
					case "viewrefreshtime":
						_viewRefreshTime = float.Parse(node.InnerText);
						break;
					case "viewboundscale":
						_viewBoundScale = float.Parse(node.InnerText);
						break;
					case "viewformat":
						_viewFormat = node.InnerText;
						break;
					case "httpquery":
						_httpQuery = node.InnerText;
						break;
				};
			}
		}
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public string Href {
			get { return _href; }
			set { _href = value; }
		}
		public string RefreshMode {
			get { return refreshModeToString(_refreshMode); }
			//set { _refreshMode = refreshModeFromString(value); }
		}
		public void setRefreshMode(KmlRefreshModes mode) {
			_refreshMode = mode;
		}
		public float RefreshInterval {
			get { return _refreshInterval; }
			set { _refreshInterval = value; }
		}
		public string ViewRefreshMode {
			get { return viewRefreshModeToString(_viewRefreshMode); }
			//set { _viewRefreshMode = viewRefreshModeFromString(value); }
		}
		public void setViewRefreshMode(KmlViewRefreshModes mode) {
			_viewRefreshMode = mode;
		}
		public float ViewRefreshTime {
			get { return _viewRefreshTime; }
			set { _viewRefreshTime = value; }
		}
		public float ViewBoundScale {
			get { return _viewBoundScale; }
			set { _viewBoundScale = value; }
		}
		public string ViewFormat {
			get { return _viewFormat; }
			set { _viewFormat = value; }
		}
		public string HttpQuery {
			get { return _httpQuery; }
			set { _httpQuery = value; }
		}

		#region helpers
		protected string refreshModeToString (KmlRefreshModes value) {
			switch (value) {
				case KmlRefreshModes.onInterval:
					return "onInterval";
				case KmlRefreshModes.onExpire:
					return "onExpire";
				default:
					return "onChange";
			};
		}
		protected KmlRefreshModes refreshModeFromString (string value) {
			switch (value.ToLower()) {
				case "onexpire":
					return KmlRefreshModes.onExpire;
				case "oninterval":
					return KmlRefreshModes.onInterval;
				default:
					return KmlRefreshModes.onChange;
			};
		}
		protected string viewRefreshModeToString (KmlViewRefreshModes value) {
			switch (value) {
				case KmlViewRefreshModes.onRegion:
					return "onRegion";
				case KmlViewRefreshModes.onRequest:
					return "onRequest";
				case KmlViewRefreshModes.onStop:
					return "onStop";
				default:
					return "never";
			};
		}
		protected KmlViewRefreshModes viewRefreshModeFromString (string value) {
			switch (value.ToLower()) {
				case "onregion":
					return KmlViewRefreshModes.onRegion;
				case "onrequest":
					return KmlViewRefreshModes.onRequest;
				case "onstop":
					return KmlViewRefreshModes.onStop;
				default:
					return KmlViewRefreshModes.never;
			};
		}
		public override string ToString () {
			return string.Format(@"
			<Icon>
				<href>{0}</href>
			</icon>{1}", Href, Environment.NewLine);
		}
		public XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, ElementName, string.Empty);
			if (_id.Length > 0) {
				XmlAttribute attId = result.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				result.Attributes.Append(attId);
			}
			// child nodes
			XmlNode nodHref = result.OwnerDocument.CreateNode(XmlNodeType.Element, "href", string.Empty);
			nodHref.InnerText = Href;
			result.AppendChild(nodHref);

			if (RefreshMode.Length > 0) {
				XmlNode nodRefreshNode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "refreshMode", string.Empty);
				nodRefreshNode.InnerText = RefreshMode;
				result.AppendChild(nodRefreshNode);
			}

			XmlNode nodRefreshInterval = result.OwnerDocument.CreateNode(XmlNodeType.Element, "refreshInterval", string.Empty);
			nodRefreshInterval.InnerText = RefreshInterval.ToString();
			result.AppendChild(nodRefreshInterval);

			if (ViewRefreshMode.Length > 0) {
				XmlNode nodViewRefreshMode = result.OwnerDocument.CreateNode(XmlNodeType.Element, "viewRefreshMode", string.Empty);
				nodViewRefreshMode.InnerText = ViewRefreshMode;
				result.AppendChild(nodViewRefreshMode);
			}
			XmlNode nodViewRefreshTime = result.OwnerDocument.CreateNode(XmlNodeType.Element, "viewRefreshTime", string.Empty);
			nodViewRefreshTime.InnerText = ViewRefreshTime.ToString();
			result.AppendChild(nodViewRefreshTime);

			XmlNode nodViewBoundScale = result.OwnerDocument.CreateNode(XmlNodeType.Element, "viewBoundScale", string.Empty);
			nodViewBoundScale.InnerText = ViewBoundScale.ToString();
			result.AppendChild(nodViewBoundScale);

			if (ViewFormat.Length > 0) {
				XmlNode nodViewFormat = result.OwnerDocument.CreateNode(XmlNodeType.Element, "viewFormat", string.Empty);
				nodViewFormat.InnerText = ViewFormat;
				result.AppendChild(nodViewFormat);
			}

			if (HttpQuery.Length > 0) {
				XmlNode nodHttpQuery = result.OwnerDocument.CreateNode(XmlNodeType.Element, "httpQuery", string.Empty);
				nodHttpQuery.InnerText = HttpQuery;
				result.AppendChild(nodHttpQuery);
			}

			return result;
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
}//	kml
