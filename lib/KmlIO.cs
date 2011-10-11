// 
//  KmlIO.cs
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
using System.Net;

namespace Pod.Kml {
	public static class KmlIO {

		/// <summary>
		/// Loads a kml from file
		/// </summary>
		/// <param name="address">the url to load the kml file from</param>
		/// <returns>Kml on success, null on failure</returns>
		public static KmlFile fromFile(string filename) {
			return KmlIO.fromFile(filename, null);
		}

		public static KmlFile fromFile(string filename, Logger log) {
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			KmlFile result = new KmlFile(doc, log);
			return result;
		}
		/// <summary>
		/// Loads a kml file from a url
		/// </summary>
		/// <param name="address">the url to load the kml file from</param>
		/// <returns>Kml on success, null on failure</returns>
		public static KmlFile fromUrl(string address) {
			WebRequest request = System.Net.FileWebRequest.Create(address);
			WebResponse response = request.GetResponse();
			long fileSize = response.ContentLength;
			byte[] buf = new byte[fileSize];
			System.IO.Stream stream = response.GetResponseStream();
			for (int x = 0; x < fileSize; x++) {
				buf[x] = (byte)stream.ReadByte();
			}
			string content = System.Text.Encoding.ASCII.GetString(buf);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(content);
			return new KmlFile(doc);
		}

		public static bool toFile(KmlFile doc, string filename) {
			try {
				XmlDocument result = doc.ToXml();
				if (null != result) {
					result.Save(filename);
					return true;
				} return false;
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return false;
			}
		}
		
		public static List<T> findElementsOfType<T>(KmlFile file, Type t) {
			List<object> tempList = new List<object>();
			file.findElementsOfType<T>(tempList);
			List<T> result = tempList.ConvertAll(delegate(object x) { return (T)x; });
			return result;
		}
	}//	class
}//	namespcce
