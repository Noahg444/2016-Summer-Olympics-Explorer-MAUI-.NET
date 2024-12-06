using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SQLite;
namespace OlympicsMauiApp;
internal class DB {
	public static SQLiteConnection conn;
	private static string GetFullNameOfEmbeddedResource(string baseName) {
		string namespaceName = "OlympicsMauiApp";
		string fullFileName = namespaceName + "." + baseName;
		return fullFileName;
	}
	private static Stream GetInputStreamForEmbeddedResource(string baseName) {
		var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AthletesPage)).Assembly;
		string fullFileName = GetFullNameOfEmbeddedResource(baseName);
		return assembly.GetManifestResourceStream(fullFileName);
	}
	public static void InitDB() {
		string libFolder = FileSystem.Current.AppDataDirectory;
		string fullFileName = System.IO.Path.Combine(libFolder, "olympics.db");

		//File.Delete(fullFileName);
		//Environment.Exit(0);
		if (File.Exists(fullFileName)) {
			conn = new SQLiteConnection(fullFileName);
			return;
		}

		conn = new SQLiteConnection(fullFileName);
		conn.CreateTable<Participant2016Summer>();

		Stream stream = GetInputStreamForEmbeddedResource("olympics2016.tsv");

		using (StreamReader input = new StreamReader(stream)) {
			string? header = input.ReadLine();
			while (!input.EndOfStream) {
				string? line = input.ReadLine();
				if (line == null) {
					continue;
				}
				Participant2016Summer p = new Participant2016Summer(line);
				conn.Insert(p);
			}
		}
	}
}
