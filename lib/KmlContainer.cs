// 
//  KmlContainer.cs
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
	public abstract class KmlContainer : KmlFeature, ISearchable  {
		protected List<KmlFeature> _features = new List<KmlFeature>();

		public KmlContainer() : base() {}
		public KmlContainer(XmlNode parent, Logger log) : base(parent, log) {}

		public List<KmlFeature> Features {
			get { return _features; }
			set { _features = value; }
		}
		
		#region helpers
		
		public new void handleNode(XmlNode node, Logger log) {
			string key = node.Name.ToLower();
			switch (key) {
				case "folder":
					_features.Add(new KmlFolder(node, log));
					break;
				case "document":
					_features.Add(new KmlDocument(node, log));
					break;
				case "placemark":
					_features.Add(new KmlPlacemark(node, log));
					break;
				case "networklink":
					_features.Add(new KmlNetworkLink(node, log));
					break;
				case "groundoverlay":
					_features.Add(new KmlGroundOverlay(node, log));
					break;
				case "photooverlay":
					_features.Add(new KmlPhotoOverlay(node, log));
					break;
				case "screenoverlay":
					_features.Add(new KmlScreenOverlay(node, log));
					break;
				default:
					// pass it down to Feature
					base.handleNode(node, log);
					break;
			};
		}
		public new void findElementsOfType<T> (List<object> elements) {
			
			base.findElementsOfType<T>(elements);

			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			foreach (KmlFeature feature in _features) {
				feature.findElementsOfType<T>(elements);
			}
		}
		#endregion helpers
	}//	class
}//	namespace
