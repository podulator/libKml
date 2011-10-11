// 
//  KmlFeature.cs
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
	public abstract class KmlFeature : KmlObject, ISearchable {
		
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
			initialise();
		}

		public KmlFeature (XmlNode parent) : base(parent) {
			initialise();
			//this.handleNode(parent, Log);
		}
		public KmlFeature (XmlNode parent, Logger log) : this(parent) { 
			Log += log;
		}
		#region properties

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
		private void initialise() {
			base.Id = string.Empty;
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
		
		public virtual void handleNode (XmlNode node, Logger log) {
			string key = node.Name.ToLower();
			debug("KmlFeature handling key " + key);
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

		protected new void debug (string message) {
			if (Log != null) Log(message);
		}
		public new event Logger Log;
		
		public virtual new XmlNode ToXml (XmlNode parent) {

			base.ToXml(parent);

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

		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) {
				elements.Add(this);
			} else {
				base.findElementsOfType<T>(elements);
				if (null != StyleSelector)
					StyleSelector.findElementsOfType<T>(elements);
				if (null != Region)
					Region.findElementsOfType<T>(elements);
				if (null != ExtendedData)
					ExtendedData.findElementsOfType<T>(elements);
			}
		}
		#endregion helpers
	}//	class
}//	namespace
