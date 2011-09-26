using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public delegate void Logger (string message);
	public interface IChangeable {
		string Id { get; }
		XmlNode ToXml(XmlNode parent);
	}
	public interface ICreatable {
		string Id { get; }
		XmlNode ToXml(XmlNode parent);
	}
	public interface IDeleteable {
		string Id { get; }
		XmlNode ToXml(XmlNode parent);
	}
	public interface ISearchable {
		void findElementsOfType<T> (List<object> elements);
	}
}//	namespace
