using SQLite;

namespace OlympicsMauiApp;

[Table("particpant")]
public class Participant2016Summer {
	public Participant2016Summer() {
	}
	public enum MedalType { None, Bronze, Silver, Gold };
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public String Name { get; set; }
	public String Country { get; set; }
	public String Sport { get; set; }
	public String Event { get; set; }
	public MedalType Medal { get; set; }
	public Participant2016Summer(String str) {
		string[] toks = str.Split(new char[] { '\t' });
		Name = toks[1];
		Country = toks[6];
		Sport = toks[12];
		Event = toks[13];
		switch (toks[14]) {
			case "Gold":	Medal = MedalType.Gold; break;
			case "Silver":	Medal = MedalType.Silver; break;
			case "Bronze":	Medal = MedalType.Bronze; break;
			default:		Medal = MedalType.None; break;
		}
	}
	public override string ToString() { 
		return String.Format("{0} {1} {2}", Name, Country, Medal.ToString());
	}
	public string MedalImage { 
		get {
			switch (Medal) {
				case MedalType.Gold:	return "gold.png";
				case MedalType.Silver:	return "silver.png";
				case MedalType.Bronze:	return "bronze.png";
				default:				return null;
			}
		}
	}
	public Color TextColor {
		get {
			switch (Medal) {
				case MedalType.Gold: return Colors.Gold;
				case MedalType.Silver: return Colors.Silver;
				case MedalType.Bronze: return Colors.Brown;
				default:				return Colors.Black;
			}
		}
	}
}
