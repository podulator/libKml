using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pod.Kml;

namespace KmlTest {
	class Program {
		private StreamWriter _streamer;
		private const string logFileName = "kml_validator.log";
		private const string kmlFileName = "kml_validator.kml";
		static void Main (string[] args) {
			if (args.Length > 0) {
				string kmlFile = string.Empty;
				foreach (string part in args) {
					kmlFile += part + " ";
				}
				new Program().run(kmlFile.Trim());
			} else {
				Console.WriteLine("Drop a kml file on this app to validate it");
				Console.WriteLine("Press enter to quit");
				Console.ReadLine();
			}
		}//	Main

		private void run(string sourceFile) {
			DateTime start = DateTime.Now;
			try {
				// start logging
				string appPath = Environment.GetCommandLineArgs()[0].Substring(0, Environment.GetCommandLineArgs()[0].LastIndexOf(Path.DirectorySeparatorChar));
				string logFile = appPath + Path.DirectorySeparatorChar + logFileName;
				string kmlFile = appPath + Path.DirectorySeparatorChar + kmlFileName;

				if (File.Exists(logFile)) File.Delete(logFile);
				_streamer = File.CreateText(logFile);
				log("Run started");
				log (string.Format("Opening file :: {0}", sourceFile));

				KmlFile doc = KmlIO.fromFile(sourceFile, log);

				if (doc != null) {
					log (string.Format("File {0} read completed", sourceFile));
				} else error("Couldn't read file :: " + sourceFile);
				
				log("Saving file to :: " + kmlFile);
				if (KmlIO.toFile(doc, kmlFile)) {
					log("Kml file saved ok");
				} else error("Couldn't save file :: " + kmlFile);
				
			} catch (Exception ex) {
				error (ex.Message);
			} finally {
				DateTime end = DateTime.Now;
				TimeSpan runTime = end - start;
				log("Run ended :: " + end.ToShortTimeString());
				log("Run took :: " + runTime.Minutes + "::" + runTime.Seconds);
				_streamer.Close();
				_streamer = null;
			}
		}

		/// <summary>
		/// Error logger
		/// </summary>
		/// <param name="message">the error message to log</param>
		private void error (string message) {
			log("Error :: " + message);
		}
		/// <summary>
		/// The logger, writes to console and to file if there si a filestream
		/// </summary>
		/// <param name="message">the message to write</param>
		private void log (string message) {
			if (Console.CursorLeft != 0) Console.WriteLine(string.Empty);
			Console.WriteLine(DateTime.Now.ToShortTimeString() + " :: " + message);
			if (null != _streamer) {
				_streamer.WriteLine(DateTime.Now.ToString() + " :: " + message);
				_streamer.Flush();
			}
		}

	}//	class
}//	namespace
