// 
//  KmlAltitudeModes.cs
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
