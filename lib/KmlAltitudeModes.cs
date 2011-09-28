using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pod.Kml {
	public enum AltitudeModes : int {
		clampToGround = 0,
		relativeToGround,
		absolute,
		clampToSeafloor,
		relativeToSeaFloor
	}
	public static class KmlAltitudeModes {
		#region helpers
		public static AltitudeModes altitudeModeFromString (string mode) {
			switch (mode.ToLower()) {
				case "absolute":
					return AltitudeModes.absolute;
				case "relativetoground":
					return AltitudeModes.relativeToGround;
				case "relativetoseefloor":
					return AltitudeModes.relativeToSeaFloor;
				case "clamptoseafloor":
					return AltitudeModes.clampToSeafloor;
				default:
					return AltitudeModes.clampToGround;
			};
		}
		public static string altitudeModeToString (AltitudeModes mode) {
			switch (mode) {
				case AltitudeModes.absolute:
					return "absolute";
				case AltitudeModes.clampToSeafloor:
					return "clampToSeaFloor";
				case AltitudeModes.relativeToGround:
					return "relativeToGround";
				case AltitudeModes.relativeToSeaFloor:
					return "relativeToSeaFloor";
				default:
					return "clampToGround";
			};
		}
		#endregion helpers
	}
}
