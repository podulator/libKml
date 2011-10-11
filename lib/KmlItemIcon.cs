// 
//  KmlItemIcon.cs
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
	[Flags] 
	public enum IconStates : int {
		none = 0, 
		open = 1, 
		closed = 2, 
		error = 4, 
		fetching0 = 8, 
		fetching1 = 16, 
		fetching2 = 32
	};
	public class KmlItemIcon : KmlIcon, ISearchable  {
		private IconStates _iconState = IconStates.none;
		
		public KmlItemIcon() : base() {}
		public KmlItemIcon(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "state":
						_iconState = ItemIconFromString(node.InnerText);
						break;
				};
			}
		}
		
		#region properties
		public IconStates IconState {
			get { return _iconState; }
			set { _iconState = value; }
		}
		#endregion properties

		#region helpers
		public string ItemIconToString() {
			string result = string.Empty;
			if ((_iconState & IconStates.none) == IconStates.none) return string.Empty;
			if ((_iconState & IconStates.open) == IconStates.open) result += "open ";
			if ((_iconState & IconStates.closed) == IconStates.closed) result += "closed ";
			if ((_iconState & IconStates.error) == IconStates.error) result += "error ";
			if ((_iconState & IconStates.fetching0) == IconStates.fetching0) result += "fetching0 ";
			if ((_iconState & IconStates.fetching0) == IconStates.fetching1) result += "fetching1 ";
			if ((_iconState & IconStates.fetching0) == IconStates.fetching2) result += "fetching2 ";
			return result.Trim();
		}
		public IconStates ItemIconFromString(string state) {
			string[] parts = state.Split(' ');
			IconStates result = IconStates.none;
			foreach (string part in parts) {
				switch (part) {
					case "open":
						result &= IconStates.open;
						break;
					case "closed":
						result &= IconStates.closed;
						break;
					case "error":
						result &= IconStates.error;
						break;
					case "fetching0":
						result &= IconStates.fetching0;
						break;
					case "fetching1":
						result &= IconStates.fetching1;
						break;
					case "fetching2":
						result &= IconStates.fetching2;
						break;
				};
			}
			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		#endregion helpers

	}//	class
}//	namespace
