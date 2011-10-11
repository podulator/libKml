// 
//  KmlGeometry.cs
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
	public abstract class KmlGeometry : ISearchable {
		private string _id = string.Empty;
		
		public KmlGeometry() {}
		public KmlGeometry(XmlNode parent, Logger log) : this(parent) { Log += log; }
		public KmlGeometry(XmlNode parent) {
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
		}
		
		public string Id {
			get { return _id; }
			set { _id = value; }
		}

		#region helpers
		public virtual XmlNode ToXml(XmlNode parent) {
			if (_id.Length > 0) {
				XmlAttribute id = parent.OwnerDocument.CreateAttribute(string.Empty, "id", string.Empty);
				id.Value = _id;
				parent.Attributes.Append(id);
			}
			return null;
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
}//	namespace
