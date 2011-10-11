// 
//  KmlStyleSelector.cs
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
	public abstract class KmlStyleSelector : ISearchable {
		private string _id;
		
		public KmlStyleSelector() {
			_id = string.Empty;
		}
		public KmlStyleSelector(XmlNode parent, Logger log) : this() {
			Log += log;
			if (null != parent.Attributes["id"])
				_id = parent.Attributes["id"].Value;
		}
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		
		public virtual XmlNode ToXml(XmlNode parent, Logger log) {
			Log += log;
			if (_id.Length > 0) {
				XmlAttribute attId = parent.OwnerDocument.CreateAttribute("id");
				attId.Value = _id;
				parent.Attributes.Append(attId);
			}
			return null;
		}
		
		public virtual void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		
		#region helpers
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
	}//	class
}//	namespace
