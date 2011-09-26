using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;

namespace TfL.Kml {
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
