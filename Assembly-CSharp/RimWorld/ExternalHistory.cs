using System.Text;
using Verse;

namespace RimWorld
{
	public class ExternalHistory : IExposable
	{
		public string gameVersion = "?";

		public string gameplayID = "?";

		public string userName = "?";

		public string storytellerName = "?";

		public string realWorldDate = "?";

		public string firstUploadDate = "?";

		public int firstUploadTime = 0;

		public bool devMode = false;

		public History history = new History();

		public static string defaultUserName = "Anonymous";

		public string AllInformation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("storyteller: ");
				stringBuilder.Append(this.storytellerName);
				stringBuilder.Append("   userName: ");
				stringBuilder.Append(this.userName);
				stringBuilder.Append("   realWorldDate(UTC): ");
				stringBuilder.Append(this.realWorldDate);
				return stringBuilder.ToString();
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.gameVersion, "gameVersion", (string)null, false);
			Scribe_Values.Look<string>(ref this.gameplayID, "gameplayID", (string)null, false);
			Scribe_Values.Look<string>(ref this.userName, "userName", (string)null, false);
			Scribe_Values.Look<string>(ref this.storytellerName, "storytellerName", (string)null, false);
			Scribe_Values.Look<string>(ref this.realWorldDate, "realWorldDate", (string)null, false);
			Scribe_Values.Look<string>(ref this.firstUploadDate, "firstUploadDate", (string)null, false);
			Scribe_Values.Look<int>(ref this.firstUploadTime, "firstUploadTime", 0, false);
			Scribe_Values.Look<bool>(ref this.devMode, "devMode", false, false);
			Scribe_Deep.Look<History>(ref this.history, "history", new object[0]);
		}
	}
}
