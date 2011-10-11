using System;
using System.Collections.Generic;
using System.Text;

namespace TfL.Kml {
	public class Converters {

		private static int EarthRadius = 6371;

		/// <summary>
		/// Calculates the cosine distance between 2 points
		/// </summary>
		/// <param name="lat1">Latitude1</param>
		/// <param name="long1">Longitude1</param>
		/// <param name="lat2">Latitude2</param>
		/// <param name="long2">Longitude2</param>
		/// <returns>Distance as a double</returns>
		public static double DistanceCosine(float lat1, float long1, float lat2, float long2) {
			double lat1Rad = toRadians(lat1);
			double lat2Rad = toRadians(lat2);
			return	Math.Acos(	Math.Sin(lat1Rad) * 
										Math.Sin(lat2Rad) +
										Math.Cos(lat1Rad) * 
										Math.Cos(lat2Rad) * 
										Math.Cos(toRadians(long2 - long1))
									) * EarthRadius;
		}//	DistanceCosine

		/// <summary>
		/// Calculates haversine distance between 2 points
		/// </summary>
		/// <param name="lat1">Latitude1</param>
		/// <param name="long1">Longitude1</param>
		/// <param name="lat2">Latitude2</param>
		/// <param name="long2">Longitude2</param>
		/// <returns>Distance as a double</returns>
		public static double DistanceHaversine(double lat1, double long1, double lat2, double long2) {

			// convert to radians
			double lat = toRadians(lat2 - lat1);
			double lon = toRadians(long2 - long1);
			lat1 = (toRadians(lat1));
			lat2 = (toRadians(lat2));
			
			double sinHalfLat = Math.Sin(lat / 2);
			double sinHalfLon = Math.Sin(lon / 2);
			double a =	sinHalfLat *
							sinHalfLat + 
							Math.Cos(lat1) * 
							Math.Cos(lat2) *
							sinHalfLon *
							sinHalfLon;
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return EarthRadius * c;
		}//	Distance

		/// <summary>
		/// Calculates the heading between 2 points
		/// </summary>
		/// <param name="lat1">Latitude1</param>
		/// <param name="long1">Longitude1</param>
		/// <param name="lat2">Latitude2</param>
		/// <param name="long2">Longitude2</param>
		/// <returns>Heading as a double</returns>
		public static double Heading(double lat1, double long1, double lat2, double long2) {

			// convert to radians
			double latt1 = toRadians(lat1);
			double latt2 = toRadians(lat2);
			double differenceLong = toRadians(long2 - long1);

			double lat2Cos = Math.Cos(latt2);
			double x = Math.Cos(latt1) * Math.Sin(latt2) - Math.Sin(latt1) * lat2Cos * Math.Cos(differenceLong);
			double y = Math.Sin(differenceLong) * lat2Cos;
			double heading = Math.Atan2(y, x);

			// scale it
			heading = heading % (2 * Math.PI);

			// convert radians to degrees
			return toDegrees(heading);
		}//	Heading

		#region helpers
		private static double toRadians(double input) {
			return input * (Math.PI / 180);
		}

		private static double toDegrees(double input) {
			input *= (180 / Math.PI);
			return input;
		}
		
		private static double toBearing(double input) {
			return (toDegrees(input) + 360) % 360;
		}
		#endregion helpers

	}//	class
}//	namespace
