﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public class KmlOrientation : ISearchable {
		private float _heading;
		private float _tilt;
		private double _roll;

		#region constructors
		public KmlOrientation () : base() {
			_heading = 0;
			_tilt = 0;
			_roll = 100;
		}
		public KmlOrientation (XmlNode node, Logger log) : this() {
			Log += log;
			XmlNodeList nodes = node.ChildNodes;
			foreach (XmlNode child in nodes) {
				switch (child.Name.ToLower()) {
					case "tilt":
						_tilt = float.Parse(child.InnerText);
						break;
					case "heading":
						_heading = float.Parse(child.InnerText);
						break;
					case "roll":
						_roll = double.Parse(child.InnerText);
						break;
				};
			}
		}

		#endregion

		#region properties
		public float Heading {
			get { return _heading; }
			set { _heading = value; }
		}
		public float Tilt {
			get { return _tilt; }
			set { _tilt = value; }
		}
		public double Roll {
			get { return _roll; }
			set { _roll = value; }
		}
		#endregion properties

		#region helpers
		public XmlNode ToXml (XmlNode parent) {
			XmlNode result = parent.OwnerDocument.CreateNode(XmlNodeType.Element, "Orientation", string.Empty);
			// child nodes
			XmlNode nodHeading = result.OwnerDocument.CreateNode(XmlNodeType.Element, "heading", string.Empty);
			nodHeading.InnerText = Heading.ToString();
			result.AppendChild(nodHeading);

			XmlNode nodTilt = result.OwnerDocument.CreateNode(XmlNodeType.Element, "tilt", string.Empty);
			nodTilt.InnerText = Tilt.ToString();
			result.AppendChild(nodTilt);

			XmlNode nodRoll = result.OwnerDocument.CreateNode(XmlNodeType.Element, "roll", string.Empty);
			nodRoll.InnerText = Roll.ToString();
			result.AppendChild(nodRoll);

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
