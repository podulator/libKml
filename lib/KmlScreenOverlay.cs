// 
//  KmlScreenOverlay.cs
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
	public enum ScreenOverlayUnits : int {
		fraction = 0, 
		pixels = 1
	};
	public abstract class abstractXY {
		private double _x = 0;
		private double _y = 0;
		private ScreenOverlayUnits _xUnits = ScreenOverlayUnits.fraction;
		private ScreenOverlayUnits _yUnits = ScreenOverlayUnits.fraction;
		
		public abstractXY() {}
		public abstractXY(XmlNode parent) {
			if (null != parent.Attributes["x"])
				_x = double.Parse(parent.Attributes["x"].Value);
			if (null != parent.Attributes["y"])
				_y = double.Parse(parent.Attributes["y"].Value);
			if (null != parent.Attributes["xunits"]) {
				string xValue = parent.Attributes["xunits"].Value.ToLower();
				_xUnits = xValue.Equals("fraction") ? ScreenOverlayUnits.fraction : ScreenOverlayUnits.pixels;
			}
			if (null != parent.Attributes["yunits"]) {
				string yValue = parent.Attributes["yunits"].Value.ToLower();
				_yUnits = yValue.Equals("fraction") ? ScreenOverlayUnits.fraction : ScreenOverlayUnits.pixels;
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
		public string XUnits {
			get { return _xUnits == ScreenOverlayUnits.fraction ? "fraction" : "pixels"; }
			set {
				_xUnits = value.Equals("fraction") ? ScreenOverlayUnits.fraction : ScreenOverlayUnits.pixels;
			}
		}
		public string YUnits {
			get { return _yUnits == ScreenOverlayUnits.fraction ? "fraction" : "pixels"; }
			set {
				_yUnits = value.Equals("fraction") ? ScreenOverlayUnits.fraction : ScreenOverlayUnits.pixels;
			}
		}
		#endregion
	}//	class

	public class KmlOverlayXY : abstractXY {
		public KmlOverlayXY() : base() {}
		public KmlOverlayXY(XmlNode parent) : base(parent) {}
	}
	public class KmlScreenXY : abstractXY {
		public KmlScreenXY() : base() {}
		public KmlScreenXY(XmlNode parent) : base(parent) {}
	}
	public class KmlRotationXY : abstractXY {
		public KmlRotationXY() : base() {}
		public KmlRotationXY(XmlNode parent) : base(parent) {}
	}
	public class KmlSizeXY : abstractXY {
		public KmlSizeXY() : base() {}
		public KmlSizeXY (XmlNode parent) : base(parent) { }
	}

	public class KmlScreenOverlay : KmlOverlay, ISearchable  {
		private KmlOverlayXY _overlayXY = new KmlOverlayXY();
		private KmlScreenXY _screenXY = new KmlScreenXY();
		private KmlRotationXY _rotationXY = new KmlRotationXY();
		private KmlSizeXY _sizeXY = new KmlSizeXY();
		private float _rotation = 0.0f;

		public KmlScreenOverlay() : base() {}
		public KmlScreenOverlay(XmlNode parent, Logger log) : base(parent, log) { fromXml(parent, log); }
		
		#region properties
		public KmlOverlayXY OverlayXY {
			get { return _overlayXY; }
			set { _overlayXY = value; }
		}
		public KmlScreenXY ScreenXY {
			get { return _screenXY; }
			set { _screenXY = value; }
		}
		public KmlRotationXY RotationXY {
			get { return _rotationXY; }
			set { _rotationXY = value; }
		}
		public KmlSizeXY SizeXY {
			get { return _sizeXY; }
			set { _sizeXY = value; }
		}
		#endregion properties
		
		#region helpers
		private void fromXml (XmlNode parent, Logger log) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "overlayxy":
						_overlayXY = new KmlOverlayXY(node);
						break;
					case "screenxy":
						_screenXY = new KmlScreenXY(node);
						break;
					case "rotationxy":
						_rotationXY = new KmlRotationXY(node);
						break;
					case "size":
						_sizeXY = new KmlSizeXY(node);
						break;
					case "rotation":
						_rotation = float.Parse(node.InnerText);
						break;
					default:
						base.handleNode(node, log);
						break;
				};
			}
		}

		public override XmlNode ToXml(XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "ScreenOverlay", string.Empty);
			base.ToXml(result);
			
			return result;
		}
		public new void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
		}
		#endregion helpers
		
	}//	class
}//	namespace
