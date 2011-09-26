﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public enum listItemTypes : int {
		check = 0, 
		checkOffOnly, 
		checkHideChildren, 
		radioFolder
	};
	public class KmlListStyle : ISearchable {
		private string _id = string.Empty;
		private listItemTypes _listItemType = listItemTypes.check;
		private string _bgColour = "ffffffff";
		private KmlItemIcon _itemIcon = new KmlItemIcon();
		
		public KmlListStyle() {}
		public KmlListStyle(XmlNode parent, Logger log) {
			Log += log;
			if (null != parent.Attributes["id"]) 
				_id = parent.Attributes["id"].Value;
			
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "listitemtype":
						_listItemType = listItemTypeFromString(node.InnerText);
						break;
					case "bgcolor":
						_bgColour = node.InnerText;
						break;
					case "itemicon":
						_itemIcon = new KmlItemIcon(node, log);
						break;
				};
			}
		}
		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public string ListItemType {
			get { return listItemTypeToString(_listItemType); }
			//set { _listItemType = listItemTypeFromString(value); }
		}
		public void setListItemType(listItemTypes type) {
			_listItemType = type;
		}
		public string BgColour {
			get { return _bgColour; }
			set { _bgColour = value; }
		}
		public KmlItemIcon ItemIcon {
			get { return _itemIcon; }
			set { _itemIcon = value; }
		}
		#endregion properties

		#region helpers
		private string listItemTypeToString(listItemTypes value) {
			switch (value) {
				case listItemTypes.checkHideChildren:
					return "checkHideChildren";
				case listItemTypes.checkOffOnly:
					return "checkOffOnly";
				case listItemTypes.radioFolder:
					return "radioFolder";
				default:
					return "check";
			};
		}
		private listItemTypes listItemTypeFromString(string value) {
			switch (value.ToLower()) {
				case "checkhidechildren":
					return listItemTypes.checkHideChildren;
				case "checkoffonly":
					return listItemTypes.checkOffOnly;
				case "radiofolder":
					return listItemTypes.radioFolder;
				default:
					return listItemTypes.check;
			};
		}

		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "ListStyle", string.Empty);
			if (_id.Length > 0) {
				XmlAttribute attId = result.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				result.Attributes.Append(attId);
			}
			// child nodes
			XmlNode nodListItemType = result.OwnerDocument.CreateNode(XmlNodeType.Element, "listIemType", string.Empty);
			nodListItemType.InnerText = ListItemType;
			result.AppendChild(nodListItemType);
			
			XmlNode nodColour = result.OwnerDocument.CreateNode(XmlNodeType.Element, "bgColor", string.Empty);
			nodColour.InnerText = BgColour;
			result.AppendChild(nodColour);
			
			if (null != _itemIcon) {
				result.AppendChild(_itemIcon.ToXml(result));
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
		#endregion helpers

	}//	class
}//	kml