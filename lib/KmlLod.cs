// 
//  KmlLod.cs
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
	public class KmlLod : ISearchable {
		private float _minLodPixels = 0.0f;
		private float _maxLodPixels = -1.0f;
		private float _minFadeExtent = 0.0f;
		private float _maxFadeExtent = 0.0f;

		public KmlLod() {}
		public KmlLod(XmlNode parent) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "minlodpixels":
						_minLodPixels = float.Parse(node.InnerText);
						break;
					case "maxlodpixels":
						_maxLodPixels = float.Parse(node.InnerText);
						break;
					case "minfadeextent":
						_minFadeExtent = float.Parse(node.InnerText);
						break;
					case "maxfadeextent":
						_maxFadeExtent = float.Parse(node.InnerText);
						break;
				};
			}
		}

		#region properties
		public float MinLodPixels {
			get { return _minLodPixels; }
			set { _minLodPixels = value; }
		}
		public float MaxLodPixels {
			get { return _maxLodPixels; }
			set { _maxLodPixels = value; }
		}
		public float MinFadeExtent {
			get { return _minFadeExtent; }
			set { _minFadeExtent = value; }
		}
		public float MaxFadeExtent {
			get { return _maxFadeExtent; }
			set { _maxFadeExtent = value; }
		}
		#endregion properties

		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Lod", string.Empty);
			// child nodes
			XmlNode nodMinPix = result.OwnerDocument.CreateNode(XmlNodeType.Element, "minLodPixels", string.Empty);
			nodMinPix.InnerText = MinLodPixels.ToString();
			result.AppendChild(nodMinPix);

			XmlNode nodMaxPix = result.OwnerDocument.CreateNode(XmlNodeType.Element, "maxLodPixels", string.Empty);
			nodMaxPix.InnerText = MaxLodPixels.ToString();
			result.AppendChild(nodMaxPix);

			XmlNode nodMinFade = result.OwnerDocument.CreateNode(XmlNodeType.Element, "minFadeExtent", string.Empty);
			nodMinFade.InnerText = MinFadeExtent.ToString();
			result.AppendChild(nodMinFade);

			XmlNode nodMaxFade = result.OwnerDocument.CreateNode(XmlNodeType.Element, "maxFadeExtent", string.Empty);
			nodMaxFade.InnerText = MaxFadeExtent.ToString();
			result.AppendChild(nodMaxFade);

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
}//	namespace
