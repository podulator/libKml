using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public enum KmlShapes : int {
		rectangle = 0, 
		cylinder = 1, 
		sphere = 2
	}
	public enum KmlGridOrigins : int {
		lowerLeft = 0, 
		upperLeft = 1
	};
	public class KmlImagePyramid {
		private int _tileSize = 256;
		private int _maxWidth = 0;
		private int _maxHeight = 0;
		private KmlGridOrigins _gridOrigin = KmlGridOrigins.lowerLeft;
		
		public KmlImagePyramid() {}
		public KmlImagePyramid(XmlNode parent) : this() {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "tilesize":
						int.Parse(node.InnerText);
						break;
					case "maxwidth":
						int.Parse(node.InnerText);
						break;
					case "maxheight":
						int.Parse(node.InnerText);
						break;
					case "gridorigin":
						_gridOrigin = gridOriginFromString(node.InnerText);
						break;
				};
			}
		}
		
		#region properties
		public int TileSize {
			get { return _tileSize; }
			set { _tileSize = value; }
		}
		public int MaxWidth {
			get { return _maxWidth; }
			set { _maxWidth = value; }
		}
		public int MaxHeight {
			get { return _maxHeight; }
			set { _maxHeight = value; }
		}
		public string GridOrigin {
			get { return gridOriginToString(_gridOrigin); }
			//set { _gridOrigin = gridOriginFromString(value); }
		}
		public void setGridOrigin(KmlGridOrigins origin) {
			_gridOrigin = origin;
		}
		#endregion properties

		#region helpers
		public string gridOriginToString(KmlGridOrigins value) {
			switch (value) {
				case KmlGridOrigins.upperLeft:
					return "upperLeft";
				default:
					return "lowerLeft";
			};
		}
		public KmlGridOrigins gridOriginFromString(string value) {
			switch (value.ToLower()) {
				case "upperleft":
					return KmlGridOrigins.upperLeft;
				default:
					return KmlGridOrigins.lowerLeft;
			};
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
	public class KmlViewVolume {
		private float _leftFov = 0.0f;
		private float _rightFov = 0.0f;
		private float _bottomFov = 0.0f;
		private float _topFov = 0.0f;
		private double _near = 0.0d;
		
		public KmlViewVolume() {}
		public KmlViewVolume(XmlNode parent) : this() {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "leftfov":
						_leftFov = float.Parse(node.InnerText);
						break;
					case "rightfov":
						_rightFov = float.Parse(node.InnerText);
						break;
					case "bottomfov":
						_bottomFov = float.Parse(node.InnerText);
						break;
					case "topfov":
						_topFov = float.Parse(node.InnerText);
						break;
					case "near":
						_near = double.Parse(node.InnerText);
						break;
				};
			}
		}//	constructor
		
		#region properties
		public float LeftFov {
			get { return _leftFov; }
			set { _leftFov = value; }
		}
		public float RightFov {
			get { return _rightFov; }
			set { _rightFov = value; }
		}	
		public float BottomFov {
			get { return _bottomFov; }
			set { _bottomFov = value; }
		}
		public float TopFov {
			get { return _topFov; }
			set { _topFov = value; }
		}
		public double Near {
			get { return _near; }
			set { _near = value; }
		}
		#endregion properties

		#region helpers
		public void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers

	}//	class

	public class KmlPhotoOverlay : KmlOverlay, ISearchable  {
		private float _rotation = 0.0f;
		private KmlViewVolume _viewVolume = new KmlViewVolume();
		private KmlImagePyramid _imagePyramid = new KmlImagePyramid();
		private KmlPoint _point = new KmlPoint();
		private KmlShapes _shape = KmlShapes.rectangle;
		
		public KmlPhotoOverlay() : base() {}
		public KmlPhotoOverlay(XmlNode parent) : base(parent) { fromXml(parent); }
		public KmlPhotoOverlay(XmlNode parent, Logger log) : base(parent, log) { fromXml(parent); }

		#region properties
		public float Rotation {
			get { return _rotation; }
			set { _rotation = value; }
		}
		public KmlViewVolume ViewVolume {
			get { return _viewVolume; }
			set { _viewVolume = value; }
		}
		public KmlImagePyramid ImagePyramid {
			get { return _imagePyramid; }
			set { _imagePyramid = value; }
		}
		public KmlPoint Point {
			get { return _point; }
			set { _point = value; }
		}
		public string Shape {
			get { return shapeToString(_shape); }
			set { _shape = shapeFromString(value); }
		}
		#endregion properties

		#region helpers
		private void fromXml (XmlNode parent) {
			foreach (XmlNode node in parent.ChildNodes) {
				string key = node.Name.ToLower();
				switch (key) {
					case "rotation":
						_rotation = float.Parse(node.InnerText);
						break;
					case "viewvolume":
						_viewVolume = new KmlViewVolume(node);
						break;
					case "imagepyramid":
						_imagePyramid = new KmlImagePyramid(node);
						break;
					case "point":
						_point = new KmlPoint(node, Log);
						break;
					case "shape":
						_shape = shapeFromString(node.InnerText);
						break;
				};
			}
		}

		public string shapeToString(KmlShapes value) {
			switch (value) {
				case KmlShapes.sphere:
					return "sphere";
				case KmlShapes.cylinder:
					return "cylinder";
				default:
					return "rectangle";
			};
		}
		public KmlShapes shapeFromString(string value) {
			switch (value.ToLower()) {
				case "cylinder":
					return KmlShapes.cylinder;
				case "sphere":
					return KmlShapes.sphere;
				default:
					return KmlShapes.rectangle;
			};
		}
		public override void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
			else base.findElementsOfType<T>(elements);
			if (null != _point)
				_point.findElementsOfType<T>(elements);
			if (null != _viewVolume)
				_viewVolume.findElementsOfType<T>(elements);
			if (null != _imagePyramid)
				_imagePyramid.findElementsOfType<T>(elements);
		}
		protected new void debug(string message) {
			if (Log != null) Log(message);
		}
		public new event Logger Log;
		#endregion helpers
	}//	class
}//	namespace
