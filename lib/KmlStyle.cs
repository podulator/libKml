// 
//  KmlStyle.cs
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
	public class KmlStyle : KmlStyleSelector, ISearchable {
		private KmlIconStyle _iconStyle;	// = new KmlIconStyle();
		private KmlLabelStyle _labelStyle;	// = new KmlLabelStyle();
		private KmlLineStyle _lineStyle;	// = new KmlLineStyle();
		private KmlPolyStyle _polyStyle;	// = new KmlPolyStyle();
		private KmlBalloonStyle _balloonStyle;	// = new KmlBalloonStyle();
		private KmlListStyle _listStyle;	// = new KmlListStyle();
		
		public KmlStyle() {}
		public KmlStyle(XmlNode parent, Logger log) : base(parent, log) {
			Log += log;
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "iconstyle":
						_iconStyle = new KmlIconStyle(node, log);
						break;
					case "labelstyle":
						_labelStyle = new KmlLabelStyle(node, log);
						break;
					case "linestyle":
						_lineStyle = new KmlLineStyle(node, log);
						break;
					case "polystyle":
						_polyStyle = new KmlPolyStyle(node, log);
						break;
					case "balloonstyle":
						_balloonStyle = new KmlBalloonStyle(node, log);
						break;
					case "liststyle":
						_listStyle = new KmlListStyle(node, log);
						break;
					default:
						debug("Skipped tag :: " + key);
						break;
				};
			}
		}

		#region properties
		public KmlIconStyle IconStyle {
			get { return _iconStyle; }
			set { _iconStyle = value; }
		}
		public KmlLabelStyle LabelStyle {
			get { return _labelStyle; }
			set { _labelStyle = value; }
		}
		public KmlLineStyle LineStyle {
			get { return _lineStyle; }
			set { _lineStyle = value; }
		}
		public KmlPolyStyle PolyStyle {
			get { return _polyStyle; }
			set { _polyStyle = value; }
		}
		public KmlBalloonStyle BalloonStyle {
			get { return _balloonStyle; }
			set { _balloonStyle = value; }
		}
		public KmlListStyle ListStyle {
			get { return _listStyle; }
			set { _listStyle = value; }
		}
		#endregion properties
		
		#region helpers
		protected new void debug(string message) {
			if (Log != null) Log(message);
		}
		public new event Logger Log;
		public override XmlNode ToXml(XmlNode parent, Logger log) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Style", string.Empty);
			base.ToXml(result, log);
			if (null != _iconStyle)
				result.AppendChild(_iconStyle.ToXml(result));
			if (null != _labelStyle)
				result.AppendChild(_labelStyle.ToXml(result));
			if (null != _lineStyle)
				result.AppendChild(_lineStyle.ToXml(result));
			if (null != _polyStyle)
				result.AppendChild(_polyStyle.ToXml(result));
			if (null != _balloonStyle)
				result.AppendChild(_balloonStyle.ToXml(result));
			if (null != _listStyle)
				result.AppendChild(_listStyle.ToXml(result));
			return result;
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _iconStyle)
				_iconStyle.findElementsOfType<T>(elements);
			if (null != _labelStyle)
				_labelStyle.findElementsOfType<T>(elements);
			if (null != _lineStyle)
				_lineStyle.findElementsOfType<T>(elements);
			if (null != _polyStyle)
				_polyStyle.findElementsOfType<T>(elements);
			if (null != _balloonStyle)
				_balloonStyle.findElementsOfType<T>(elements);
			if (null != _listStyle)
				_listStyle.findElementsOfType<T>(elements);
		}
		#endregion helpers
	}//	class
}//	namespace
