// 
//  KmlStyleMap.cs
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
	public class KmlStyleMap : KmlStyleSelector, ISearchable  {

		private List<KmlPair> _pairs = new List<KmlPair>();
		
		public KmlStyleMap() : base() {}
		public KmlStyleMap(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "pair":
						_pairs.Add(new KmlPair(node));
						break;
				};
			}
		}
		#region properties
		public List<KmlPair> Pairs {
			get { return _pairs; }
			set { _pairs = value; }
		}
		#endregion properties
		
		#region helpers
		public override XmlNode ToXml(XmlNode parent, Logger log) {

			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "StyleMap", string.Empty);
			base.ToXml(result, log);
			if (null != _pairs && _pairs.Count > 0) {
				foreach (KmlPair pair in _pairs) {
					result.AppendChild(pair.ToXml(result));
				}
			}
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _pairs) {
				foreach (KmlPair pair in _pairs) {
					pair.findElementsOfType<T>(elements);
				}
			}
		}
		#endregion helpers
	}//	class
}//	namespace
