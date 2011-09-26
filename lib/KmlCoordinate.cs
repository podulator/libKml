using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfL.Kml {
	public class KmlCoordinate : IComparable, ISearchable  {

		#region fields
		private double _longitude;
		private double _latitude;
		private double _altitude;
		#endregion fields

		#region constructors
		public KmlCoordinate() {
			_longitude = 0;
			_latitude = 0;
			_altitude = 0;
		}
		public KmlCoordinate(Logger log) : this() { Log += log; }
		/// <summary>
		/// Loads a KmlCoordinate from a kml coordinates string
		/// </summary>
		/// <param name="node">The coordinates string</param>
		public KmlCoordinate(string contents, Logger log) : this() {
			try {
				Log += log;
				string[] parts = contents.Trim().Split(',');
				if (parts.Length == 3) {
					_longitude = double.Parse(parts[0].Trim());
					_latitude = double.Parse(parts[1].Trim());
					_altitude = double.Parse(parts[2].Trim());
				}
			} catch (Exception ex) {
				debug("ERROR =:: " + contents + " ::= " + ex.Message);
			}
		}
		#endregion

		#region properties
		public double Longitude {
			get { return _longitude; }
			set { _longitude = value; }
		}
		public double Latitude {
			get { return _latitude; }
			set { _latitude = value; }
		}
		public double Altitude {
			get { return _altitude; }
			set { _altitude = value; }
		}
		#endregion properties

		#region functions
		/// <summary>
		/// Copies the current KmlCoordinate
		/// </summary>
		/// <returns>KmlCoordinate on success, null on failure</returns>
		public KmlCoordinate Copy () {
			KmlCoordinate result = new KmlCoordinate();
			result.Altitude = this.Altitude;
			result.Latitude = this.Latitude;
			result.Longitude = this.Longitude;
			return result;
		}
	
		#endregion functions

		#region interfaces
		/// <summary>
		/// Returns the longitude, latitude and altitude of the object
		/// </summary>
		/// <returns>String</returns>
		public override string ToString() {
			return _longitude.ToString() + "," + _latitude.ToString() + "," + _altitude.ToString();
		}

		public int CompareTo(KmlCoordinate value) {
			int result = (Altitude.CompareTo(value.Altitude));
			if (result == 0) {
				result = (Latitude.CompareTo(value.Latitude));
				if (result == 0)
					result = (Longitude.CompareTo(value.Longitude));
			}
			return result;
		}
		int IComparable.CompareTo (object obj) {
			KmlCoordinate temp = obj as KmlCoordinate;
			return this.CompareTo(temp);
		}//	CompareTo

		#endregion interfaces

		#region helpers

		public static List<KmlCoordinate> makeList (string coords, Logger log) {
			coords = coords.Replace(Environment.NewLine, " ");
			coords = System.Text.RegularExpressions.Regex.Replace(coords, "[\n\t\r]", " ");
			string[] splitter = {" "};
			string[] parts = coords.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
			List<KmlCoordinate> results = new List<KmlCoordinate>();
			foreach (string part in parts) {
				if (part.Trim().Length >0)
					results.Add(new KmlCoordinate(part, log));
			}
			return results;
		}
		public virtual void findElementsOfType<T> (List<object> elements) {
			if (this is T) elements.Add(this);
		}
		protected void debug (string message) {
			if (Log != null) Log(message);
		}
		public event Logger Log;
		#endregion helpers
	}//	class

}//	namespace
