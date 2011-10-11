// 
//  KmlScale.cs
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
	public class KmlScale : ISearchable {
		private double _x = 0.0d;
		private double _y = 0.0d;
		private double _z = 0.0d;
		
		public KmlScale() {}
		public KmlScale(XmlNode parent, Logger log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "x":
						_x = double.Parse(node.InnerText);
						break;
					case "y":
						_y = double.Parse(node.InnerText);
						break;
					case "z":
						_z = double.Parse(node.InnerText);
						break;
				};
			}
		}
		#region properties
		public double X {
			get { return _x; }
			set { _x = value; }
		}
		public double Y {
			get { return _y; }
			set { _y = value; }
		}
		public double Z {
			get { return _z; }
			set { _z = value; }
		}
		#endregion properties
		
		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Scale", string.Empty);
			// child nodes
			XmlNode nodX = result.OwnerDocument.CreateNode(XmlNodeType.Element, "x", string.Empty);
			nodX.InnerText = X.ToString();
			result.AppendChild(nodX);

			XmlNode nodY = result.OwnerDocument.CreateNode(XmlNodeType.Element, "y", string.Empty);
			nodY.InnerText = Y.ToString();
			result.AppendChild(nodY);

			XmlNode nodZ = result.OwnerDocument.CreateNode(XmlNodeType.Element, "z", string.Empty);
			nodZ.InnerText = Z.ToString();
			result.AppendChild(nodZ);

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
}//namespace
