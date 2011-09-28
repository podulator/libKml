using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
	public abstract class KmlAbstractView : ISearchable {
		private KmlTimePrimitive _timePrimitive;

		public KmlTimePrimitive TimePrimitive {
			get { return _timePrimitive; }
			set { _timePrimitive = value; }
		}

		public abstract XmlNode ToXml(XmlNode parent);

		public virtual void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
	}//	class
}//	namespace
