using System;
using System.Collections.Generic;
using System.Text;

namespace TfL.Kml {

	public class KmlColourWheel {
		private KmlPallette _pallette;
		private int _currentBase = 0;
		private int _currentColour = 0;
		private int _tweak = 20;
		private int _sway = 30;
		private int _steps = 0;

		public KmlColourWheel() {
			_pallette = new KmlPallette();
			_currentColour = (int)_pallette.colourAsLong(10);
			_currentBase = (int)_pallette.colourAsLong(50);
			
		}
		
		public string newColour() {
			int id = genNewColour();
			return _pallette.colourAsABGR(id);
		}
		
		private int genNewColour() {
			int result = 0;
			int rnd = _pallette.Random();
			try {
				if (_steps < 0) {
					_steps = _pallette.cTweak(_sway, _sway / 3);
					_currentColour = _currentBase;
				} else --_steps;
				
				if (0 == (rnd % 5)) {
					result = (rnd % _pallette.NumColours);
				} else {
					result = _pallette.genConstrainedColour(_currentColour, _tweak);
					_currentBase = result;
				}
				return result;
			} catch {
				return rnd % _pallette.NumColours;
			}
		}//	genNewColour

		private class KmlPallette {
			#region members
			public const int RAND_MAX = 5000;
			private Random _rnd = new Random(DateTime.Now.Millisecond);
			private string[] _hexTable;
			private long[] _colours = {	3481377, 
													3546913, 
													3612704, 
													3744032, 
													3809825, 
													3875617, 
													4007201, 
													4072994, 
													4139042, 
													4270370, 
													4336418, 
													4402467, 
													4534051, 
													4599843, 
													4665891, 
													4797475, 
													4863779, 
													4929827, 
													5061412, 
													5127460, 
													5193508, 
													5325348, 
													5391652, 
													5457700, 
													5589540, 
													5655844, 
													5722148, 
													5853988, 
													5920292, 
													5986595, 
													6118691, 
													5987875, 
													5922595, 
													5857571, 
													5792291, 
													5727010, 
													5596194, 
													5531170, 
													5400354, 
													5335073, 
													5204513, 
													5008161, 
													4877344, 
													4746784, 
													4615968, 
													4485151, 
													4223519, 
													4027166, 
													3896350, 
													3700253, 
													3503901, 
													3176476, 
													3635229, 
													3962654, 
													4355872, 
													4683297, 
													5076257, 
													5338402, 
													5600291, 
													5993508, 
													6189861, 
													6517542, 
													6779431, 
													6975784, 
													7237929, 
													7368490, 
													7367211, 
													7300652, 
													7234092, 
													7233069, 
													7166510, 
													7165487, 
													7099184, 
													7098416, 
													7031857, 
													6965554, 
													6964787, 
													6898740, 
													6897716, 
													6831669, 
													6765622, 
													6765110, 
													6699063, 
													6699066, 
													6633533, 
													6633792, 
													6568515, 
													6502981, 
													6503240, 
													6437962, 
													6437964, 
													6372687, 
													6307153, 
													6307411, 
													6241876, 
													6242134, 
													6176856, 
													6176857, 
													6111578, 
													6046043, 
													6046300, 
													5914713, 
													5848919, 
													5717333, 
													5651538, 
													5519952, 
													5454158, 
													5322571, 
													5256777, 
													5125446, 
													5059396, 
													4928066, 
													4862015, 
													4730685, 
													4664891, 
													4533304, 
													4467510, 
													4336180, 
													4270386, 
													4138799, 
													4073005, 
													3941675, 
													3875881, 
													3744551, 
													3678501, 
													3547171
											};
			#endregion members

			public KmlPallette () {

				string[] hexSeries = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
				_hexTable = new string[hexSeries.Length * hexSeries.Length];
				int c = 0;
				for (int a = 0; a < hexSeries.Length; a++) {
					for (int b = 0; b < hexSeries.Length; b++) {
						_hexTable[c] = hexSeries[a] + hexSeries[b];
						++c;
					}
				}
			}//	constructor

			#region methods

			public int genConstrainedColour (int baseNum, int tweak) {
				int result = 1 + (Random() % tweak);
				int rnd = Random();
				if ((rnd & 1) == 1) {
					result = -result;
				}
				result = (baseNum + result) % NumColours;
				while (result < 0) {
					result += NumColours;
				};
				return result;
			}

			public int cTweak (int baseNum, int tweak) {
				int tweaked = (int)Math.Round((decimal)(Random() % (2 * tweak)));
				int result = (baseNum + (tweaked - tweak));
				if (result < 0) result *= -1;
				return (result < 255 ? result : 255);
			}

			public int Random () {
				return _rnd.Next(RAND_MAX);
			}
			#endregion methods

			#region properties

			public int NumColours {
				get { return _colours.Length; }
			}

			#endregion properties

			#region helpers
			public long colourAsLong (int index) {
				if (index < 0) index = 0;
				else if (index >= NumColours) index = NumColours - 1;
				return _colours[index];
			}
			public string colourAsABGR (int index) {
				return longToABGR(colourAsLong(index));
			}
			private string longToABGR (long input) {
				int r = (int)Math.Floor((decimal)input % 256);
				int g = (int)Math.Floor((decimal)(input / 256) % 256);
				int b = (int)Math.Floor((decimal)(input / 256 / 256) % 256);
				if (r < 128) {
					if (r % 2 == 0) r += 128;
				}
				if (g < 128) {
					if (g % 2 == 0) g += 128;
				}
				if (b < 128) {
					if (b % 2 == 0) b += 128;
				}
				return ("ff" + _hexTable[b] + _hexTable[g] + _hexTable[r]);
			}
			#endregion helpers

		}//	class

	}//	class

}//	namesapce
