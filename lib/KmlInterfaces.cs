// 
//  KmlInterfaces.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pod.Kml {
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
