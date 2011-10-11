// 
//  KmlObject.cs
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
using System.Xml;

namespace Pod.Kml {
	/// <summary>
	/// The most basic Kml class, based on
	/// http://code.google.com/apis/kml/documentation/kmlreference.html#Object
	/// </summary>
	public abstract class KmlObject : ISearchable {
		
		private string _id;
		private string _targetId;

		#region ctors
		public KmlObject (XmlNode parent, Logger log) : this(parent) { 
			Log += log;
		}

		public KmlObject (XmlNode parent) {
			_id = (null == parent.Attributes["id"]) ? string.Empty : parent.Attributes["id"].Value;
			_targetId = (null == parent.Attributes["targetId"]) ? string.Empty : parent.Attributes["targetId"].Value;
		}

		public KmlObject () {}
		#endregion ctors

		#region properties
		public string Id {
			get { return _id; }
			set { _id = value; }
		}
		public string TargetId {
			get { return _targetId; }
			set { _targetId = value; }
		}
		#endregion properties
		
		#region helpers

		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}

		public XmlNode ToXml (XmlNode parent) {
			// id tag
			if (Id.Length > 0) {
				XmlAttribute nodId = parent.OwnerDocument.CreateAttribute("id");
				nodId.Value = Id;
				parent.Attributes.Append(nodId);
			}
			if (TargetId.Length > 0) {
				XmlAttribute nodId = parent.OwnerDocument.CreateAttribute("targetId");
				nodId.Value = TargetId;
				parent.Attributes.Append(nodId);
			}
			return null;
		}
		#endregion helpers
		
	}//	class
}//	namespace

