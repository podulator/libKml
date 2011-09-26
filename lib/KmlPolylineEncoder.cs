using System;
using System.Collections.Generic;
using System.Text;

// C# translation of Mark Rambow's Java reimplementation of Mark McClures Javascript PolylineEncoder
// Mark Walters 2008 - mark[at]sol5.co.uk

namespace TfL.Kml {

	public class KmlPolylineEncoder {

			private int numLevels = 18;
			private int zoomFactor = 2;
			private double verySmall = 0.00001;
			private bool forceEndpoints = true;
			private double[] zoomLevelBreaks;

			#region constructors
			public KmlPolylineEncoder (int numLevels, int zoomFactor, double verySmall, bool forceEndpoints) {

				this.numLevels = numLevels;
				this.zoomFactor = zoomFactor;
				this.verySmall = verySmall;
				this.forceEndpoints = forceEndpoints;

				this.zoomLevelBreaks = new double[numLevels];

				for (int i = 0; i < numLevels; i++) {
					this.zoomLevelBreaks[i] = verySmall
							  * Math.Pow(this.zoomFactor, numLevels - i - 1);
				}
			}

			public KmlPolylineEncoder () {
				this.zoomLevelBreaks = new double[numLevels];

				for (int i = 0; i < numLevels; i++) {
					this.zoomLevelBreaks[i] = verySmall
							  * Math.Pow(this.zoomFactor, numLevels - i - 1);
				}
			}
			#endregion constructors

			public Dictionary<String, String> dpEncode (List<KmlCoordinate> coords) {
				int i, maxLoc = 0;
				Stack<int[]> stack = new Stack<int[]>();
				double[] dists = new double[coords.Count];
				double maxDist, absMaxDist = 0.0, temp = 0.0;
				int[] current;
				String encodedPoints, encodedLevels;

				if (coords.Count > 2) {
					int[] stackVal = new int[] { 0, (coords.Count - 1) };
					stack.Push(stackVal);

					while (stack.Count > 0) {
						current = stack.Pop();
						maxDist = 0;

						for (i = current[0] + 1; i < current[1]; i++) {
							temp = this.distance(coords[i], coords[current[0]], coords[current[1]]);
							if (temp > maxDist) {
								maxDist = temp;
								maxLoc = i;
								if (maxDist > absMaxDist) {
									absMaxDist = maxDist;
								}
							}
						}
						if (maxDist > this.verySmall) {
							dists[maxLoc] = maxDist;
							int[] stackValCurMax = { current[0], maxLoc };
							stack.Push(stackValCurMax);
							int[] stackValMaxCur = { maxLoc, current[1] };
							stack.Push(stackValMaxCur);
						}
					}
				}

				encodedPoints = createEncodings(coords, dists);
				encodedPoints = encodedPoints.Replace("\\", "\\\\");

				encodedLevels = encodeLevels(coords, dists, absMaxDist);

				Dictionary<String, String> hm = new Dictionary<String, String>();
				hm.Add("encodedPoints", encodedPoints);
				hm.Add("encodedLevels", encodedLevels);
				return hm;

			}	//dpEncode

			public double distance (KmlCoordinate p0, KmlCoordinate p1, KmlCoordinate p2) {
				double u, result = 0.0;

				if (p1.Latitude == p2.Latitude
						  && p1.Longitude == p2.Longitude) {
					result = Math.Sqrt(Math.Pow(p2.Latitude - p0.Latitude, 2)
							  + Math.Pow(p2.Longitude - p0.Longitude, 2));
				} else {
					u = ((p0.Latitude - p1.Latitude)
							  * (p2.Latitude - p1.Latitude) + (p0
							  .Longitude - p1.Longitude)
							  * (p2.Longitude - p1.Longitude))
							  / (Math.Pow(p2.Latitude - p1.Latitude, 2) + Math
										 .Pow(p2.Longitude - p1.Longitude, 2));

					if (u <= 0) {
						result = Math.Sqrt(Math.Pow(p0.Latitude - p1.Latitude,
								  2)
								  + Math.Pow(p0.Longitude - p1.Longitude, 2));
					} else if (u >= 1) {
						result = Math.Sqrt(Math.Pow(p0.Latitude - p2.Latitude,
								  2)
								  + Math.Pow(p0.Longitude - p2.Longitude, 2));
					} else if (0 < u && u < 1) {
						result = Math.Sqrt(Math.Pow(p0.Latitude - p1.Latitude
								  - u * (p2.Latitude - p1.Latitude), 2)
								  + Math.Pow(p0.Longitude - p1.Longitude - u
											 * (p2.Longitude - p1.Longitude), 2));
					}
				}
				return result;
			}//	distance

			private static int floor1e5 (double Coordinate) {
				return (int)Math.Floor(Coordinate * 1e5);
			}

			private static String encodeSignedNumber (int num) {
				int sgn_num = num << 1;
				if (num < 0) {
					sgn_num = ~(sgn_num);
				}
				return (encodeNumber(sgn_num));
			}

			private static String encodeNumber (int num) {

				StringBuilder encodeString = new StringBuilder();
				while (num >= 0x20) {
					int nextValue = (0x20 | (num & 0x1f)) + 63;
					encodeString.Append((char)(nextValue));
					num >>= 5;
				}
				num += 63;
				encodeString.Append((char)(num));
				return encodeString.ToString();
			}//	encodeNumber

			private String encodeLevels (List<KmlCoordinate> points, double[] dists, double absMaxDist) {
				int i;
				StringBuilder encoded_levels = new StringBuilder();

				if (this.forceEndpoints) {
					encoded_levels.Append(encodeNumber(this.numLevels - 1));
				} else {
					encoded_levels.Append(encodeNumber(this.numLevels
							  - computeLevel(absMaxDist) - 1));
				}
				for (i = 1; i < points.Count - 1; i++) {
					if (dists[i] != 0) {
						encoded_levels.Append(encodeNumber(this.numLevels
								  - computeLevel(dists[i]) - 1));
					}
				}
				if (this.forceEndpoints) {
					encoded_levels.Append(encodeNumber(this.numLevels - 1));
				} else {
					encoded_levels.Append(encodeNumber(this.numLevels
							  - computeLevel(absMaxDist) - 1));
				}

				return encoded_levels.ToString();
			}//	encodeLevels

			private int computeLevel (double absMaxDist) {
				int lev = 0;
				if (absMaxDist > this.verySmall) {
					lev = 0;
					while (absMaxDist < this.zoomLevelBreaks[lev]) {
						lev++;
					}
					return lev;
				}
				return lev;
			}//	computeLevel

			private String createEncodings (List<KmlCoordinate> points, double[] dists) {
				StringBuilder encodedPoints = new StringBuilder();

				double maxlat = 0, minlat = 0, maxlon = 0, minlon = 0;

				int plat = 0;
				int plng = 0;

				for (int i = 0; i < points.Count; i++) {

					// determin bounds (max/min lat/lon)
					if (i == 0) {
						maxlat = minlat = points[i].Latitude;
						maxlon = minlon = points[i].Longitude;
					} else {
						if (points[i].Latitude > maxlat) {
							maxlat = points[i].Latitude;
						} else if (points[i].Latitude < minlat) {
							minlat = points[i].Latitude;
						} else if (points[i].Longitude > maxlon) {
							maxlon = points[i].Longitude;
						} else if (points[i].Longitude < minlon) {
							minlon = points[i].Longitude;
						}
					}

					if (dists[i] != 0 || i == 0 || i == points.Count - 1) {
						KmlCoordinate point = points[i];

						int late5 = floor1e5(point.Latitude);
						int lnge5 = floor1e5(point.Longitude);

						int dlat = late5 - plat;
						int dlng = lnge5 - plng;

						plat = late5;
						plng = lnge5;

						encodedPoints.Append(encodeSignedNumber(dlat));
						encodedPoints.Append(encodeSignedNumber(dlng));

					}
				}
				return encodedPoints.ToString();
			}//	createEncodings

	}//	class
}//	namespace
