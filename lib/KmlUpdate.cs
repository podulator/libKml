// 
//  KmlUpdate.cs
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
	public class KmlUpdate : ISearchable {
		private string _targetHref = string.Empty;
		private IChangeable _change = null;
		private ICreatable _create = null;
		private IDeleteable _delete = null;
		
		public KmlUpdate() {}
		public KmlUpdate(XmlNode parent, Logger log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.NamespaceURI.ToLower();
				switch (key) {
					case "targethref":
						_targetHref = node.InnerText;
						break;
					case "change":
						break;
					case "create":
						break;
					case "delete":
						break;
				};
			}
		}

		#region properties
		public string TargetHref {
			get { return _targetHref; }
			set { _targetHref = value; }
		}
		public IChangeable Change {
			get { return _change; }
			set { _change = value; }
		}
		public ICreatable Create {
			get { return _create; }
			set { _create = value; }
		}
		public IDeleteable Deleteable {
			get { return _delete; }
			set { _delete = value; }
		}
		#endregion properties
		
		#region helpers
		public XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Update", string.Empty);
			// child nodes
			XmlNode nodTarget = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "targetHref", string.Empty);
			nodTarget.InnerText = TargetHref;
			result.AppendChild(nodTarget);
			
			if (null != _change) {
				result.AppendChild(_change.ToXml(result));
			}
			if (null != _create) {
				result.AppendChild(_create.ToXml(result));
			}
			if (null != _delete) {
				result.AppendChild(_delete.ToXml(result));
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
}//	namespace
