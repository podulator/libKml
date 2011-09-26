using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public abstract class KmlFeature : ISearchable {
		private string _id;
		private string _name;
		private bool _visibility;
		private bool _open;
		private string _atomAuthor;
		private string _atomLink;
		private string _address;
		private string _addressDetails;
		private string _phoneNumber;
		private string _snippet;
		private string _description;
		private KmlAbstractView _abstractView;
		private KmlTimePrimitive _timePrimitive;
		private string _styleUrl;
		private KmlStyleSelector _styleSelector;
		private KmlRegion _region;
		private KmlExtendedData _extendedData;

		public KmlFeature () {
			_id = string.Empty;
			_name = string.Empty;
			_visibility = true;
			_open = true;
			_atomAuthor = string.Empty;
			_atomLink = string.Empty;
			_address = string.Empty;
			_addressDetails = string.Empty;
			_phoneNumber = string.Empty;
			_snippet = string.Empty;
			_description = string.Empty;
			_abstractView = null;
			_timePrimitive = null;
			_styleUrl = string.Empty;
			_styleSelector = null;
			_region = null;
			_extendedData = null;
		}

		public KmlFeature (XmlNode parent) : this() {
			_id = (null == parent.Attributes["id"]) ? string.Empty : parent.Attributes["id"].Value;
		}
		public KmlFeature (XmlNode parent, Logger log) : this(parent) { Log += log; }
		#region properties

		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		public bool Visible {
			get { return _visibility; }
			set { _visibility = value; }
		}
		public bool Open {
			get { return _open; }
			set { _open = value; }
		}
		public string AtomAuthor {
			get { return _atomAuthor; }
			set { _atomAuthor = value; }
		}
		public string AtomLink {
			get { return _atomLink; }
			set { _atomLink = value; }
		}
		public string Address {
			get { return _address; }
			set { _address = value; }
		}
		public string AddressDetails {
			get { return _addressDetails; }
			set { _addressDetails = value; }
		}
		public string PhoneNumber {
			get { return _phoneNumber; }
			set { _phoneNumber = value; }
		}
		public string Snippet {
			get { return _snippet; }
			set { _snippet = value; }
		}
		public string Description {
			get { return _description; }
			set { _description = value; }
		}
		public KmlAbstractView AbstractView {
			get { return _abstractView; }
			set { _abstractView = value; }
		}
		public KmlTimePrimitive TimePrimitive {
			get { return _timePrimitive; }
			set { _timePrimitive = value; }
		}
		public string StyleUrl {
			get { return _styleUrl; }
			set { _styleUrl = value; }
		}
		public KmlStyleSelector StyleSelector {
			get { return _styleSelector; }
			set { _styleSelector = value; }
		}
		public KmlRegion Region {
			get { return _region; }
			set { _region = value; }
		}
		public KmlExtendedData ExtendedData {
			get { return _extendedData; }
			set { _extendedData = value; }
		}
		#endregion properties

		#region helpers

		public void handleNode (XmlNode node, Logger log) {
			string key = node.Name.ToLower();
			debug("handling key " + key);
			switch (key) {
				case "name":
					_name = node.InnerText;
					break;
				case "visibility":
					_visibility = (node.InnerText.Equals("1")) ? true : false;
					break;
				case "open":
					_open = (node.InnerText.Equals("1")) ? true : false;
					break;
				case "atom:author":
				case "author":
					_atomAuthor = node.InnerText;
					break;
				case "atom:link":
				case "link":
					_atomLink = node.InnerText;
					break;
				case "xal:addressdetails":
				case "addressdetails":
					_addressDetails = node.InnerText;
					break;
				case "phonenumber":
					_phoneNumber = node.InnerText;
					break;
				case "snippet":
					_snippet = (node.InnerXml.Length > 0) ? node.InnerXml : node.InnerText;
					break;
				case "description":
					_description = (node.InnerXml.Length > 0) ? node.InnerXml : node.InnerText;
					break;
				case "styleurl":
					_styleUrl = node.InnerText;
					break;
				case "timespan":
					_timePrimitive = new KmlTimeSpan(node, log);
					break;
				case "timestamp":
					_timePrimitive = new KmlTimeStamp(node, log);
					break;
				case "extendeddata":
				case "metadata":
					_extendedData = new KmlExtendedData(node, log);
					break;
				case "styleselector":
					throw new Exception("StyleSelector called rather than subclass");
				case "region":
					_region = new KmlRegion(node, log);
					break;
				case "lookat":
					_abstractView = new KmlLookAt(node, log);
					break;
				case "camera":
					_abstractView = new KmlCamera(node, log);
					break;
				case "style":
					_styleSelector = new KmlStyle(node, log);
					break;
				case "stylemap":
					_styleSelector = new KmlStyleMap(node, log);
					break;
				default:
					debug("skipping tag :: " + key);
					break;
			};
		}

		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		
		public virtual XmlNode ToXml (XmlNode parent) {
			// id tag
			if (_id.Length > 0) {
				XmlAttribute nodId = parent.OwnerDocument.CreateAttribute("id");
				nodId.Value = _id;
				parent.Attributes.Append(nodId);
			}
			// make the child nodes
			XmlNode nodName = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "name", string.Empty);
			nodName.InnerText = Name;
			parent.AppendChild(nodName);

			XmlNode nodVisibility = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "visibility", string.Empty);
			nodVisibility.InnerText = Visible ? "1" : "0";
			parent.AppendChild(nodVisibility);

			XmlNode nodOpen = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "open", string.Empty);
			nodOpen.InnerText = Open ? "1" : "0";
			parent.AppendChild(nodOpen);

			if (AtomAuthor.Length > 0) {
				XmlNode nodAuthor = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "author", "atom");
				nodAuthor.InnerText = AtomAuthor;
				parent.AppendChild(nodAuthor);
			}

			if (AtomLink.Length > 0) {
				XmlNode nodLink = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "link", "atom");
				nodLink.InnerText = AtomLink;
				parent.AppendChild(nodLink);
			}

			if (Address.Length > 0) {
				XmlNode nodAddress = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "address", string.Empty);
				nodAddress.InnerText = Address;
				parent.AppendChild(nodAddress);
			}

			if (AddressDetails.Length > 0) {
				XmlNode nodAddressDetails = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "AddressDetails", "xal");
				nodAddressDetails.InnerText = AddressDetails;
				parent.AppendChild(nodAddressDetails);
			}

			if (PhoneNumber.Length > 0) {
				XmlNode nodPhone = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "phoneNumber", string.Empty);
				nodPhone.InnerText = PhoneNumber;
				parent.AppendChild(nodPhone);
			}

			if (Snippet.Length > 0) {
				XmlNode nodSnippet = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Snippet", string.Empty);
				nodSnippet.InnerXml = Snippet;
				parent.AppendChild(nodSnippet);
			}

			if (Description.Length > 0) {
				XmlNode nodDescription = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "description", string.Empty);
				nodDescription.InnerXml = Description;
				parent.AppendChild(nodDescription);
			}

			if (null != AbstractView) {
				parent.AppendChild(AbstractView.ToXml(parent));
			}

			if (null != _timePrimitive) {
				parent.AppendChild(_timePrimitive.ToXml(parent));
			}

			if (StyleUrl.Length > 0) {
				XmlNode nodStyleUrl = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "styleUrl", string.Empty);
				nodStyleUrl.InnerText = StyleUrl;
				parent.AppendChild(nodStyleUrl);
			}

			if (null != StyleSelector) {
				parent.AppendChild(StyleSelector.ToXml(parent, null));
			}

			if (null != Region) {
				parent.AppendChild(Region.ToXml(parent));
			}

			if (null != ExtendedData) {
				parent.AppendChild(ExtendedData.ToXml(parent));
			}

			return null;
		}

		public virtual void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);

			if (null != StyleSelector)
				StyleSelector.findElementsOfType<T>(elements);
			if (null != Region)
				Region.findElementsOfType<T>(elements);
			if (null != ExtendedData)
				ExtendedData.findElementsOfType<T>(elements);
		}
		#endregion helpers
	}//	class
}//	namespace
