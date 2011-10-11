// 
//  KmlNetworkLink.cs
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
	public class KmlNetworkLink : KmlFeature, ISearchable  {
		private bool _refreshVisibility = false;
		private bool _flyToView = false;
		private string _link = string.Empty;
		
		public KmlNetworkLink() : base() {}
		public KmlNetworkLink(XmlNode parent, Logger log) : base(parent, log) { fromXml(parent, log); }

		#region properties
		public bool RefreshVisibility {
			get { return _refreshVisibility; }
			set { _refreshVisibility = value; }
		}
		public bool FlyToView {
			get { return _flyToView; }
			set { _flyToView = value; }
		}
		public string Link {
			get { return _link; }
			set { _link = value; }
		}
		#endregion properties
		
		#region helpers
		private void fromXml (XmlNode parent, Logger log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "refreshvisibility":
						_refreshVisibility = node.InnerText.Equals("1") ? true : false;
						break;
					case "flytoview":
						_flyToView = node.InnerText.Equals("1") ? true : false;
						break;
					case "link":
						_link = node.InnerText;
						break;
					default:
						base.handleNode(node, log);
						break;
				};
			}
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		public override XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "NetworkLink", string.Empty);
			base.ToXml(result);

			XmlNode nodRefreshVisibility = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "refreshVisibility", string.Empty);
			nodRefreshVisibility.InnerText = RefreshVisibility ? "1" : "0";
			result.AppendChild(nodRefreshVisibility);

			XmlNode nodFlyToView = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "flyToView", string.Empty);
			nodFlyToView.InnerText = FlyToView ? "1" : "0";
			result.AppendChild(nodFlyToView);

			XmlNode nodLink = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Link", string.Empty);
			nodLink.InnerText = Link;
			result.AppendChild(nodLink);

			return result;
		}
		#endregion helpers
	}//	class
}//	namespace
