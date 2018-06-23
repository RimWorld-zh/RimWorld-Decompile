using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F6 RID: 758
	public class ExternalHistory : IExposable
	{
		// Token: 0x0400083C RID: 2108
		public string gameVersion = "?";

		// Token: 0x0400083D RID: 2109
		public string gameplayID = "?";

		// Token: 0x0400083E RID: 2110
		public string userName = "?";

		// Token: 0x0400083F RID: 2111
		public string storytellerName = "?";

		// Token: 0x04000840 RID: 2112
		public string realWorldDate = "?";

		// Token: 0x04000841 RID: 2113
		public string firstUploadDate = "?";

		// Token: 0x04000842 RID: 2114
		public int firstUploadTime = 0;

		// Token: 0x04000843 RID: 2115
		public bool devMode = false;

		// Token: 0x04000844 RID: 2116
		public History history = new History();

		// Token: 0x04000845 RID: 2117
		public static string defaultUserName = "Anonymous";

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0006F710 File Offset: 0x0006DB10
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

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0006F77C File Offset: 0x0006DB7C
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.gameVersion, "gameVersion", null, false);
			Scribe_Values.Look<string>(ref this.gameplayID, "gameplayID", null, false);
			Scribe_Values.Look<string>(ref this.userName, "userName", null, false);
			Scribe_Values.Look<string>(ref this.storytellerName, "storytellerName", null, false);
			Scribe_Values.Look<string>(ref this.realWorldDate, "realWorldDate", null, false);
			Scribe_Values.Look<string>(ref this.firstUploadDate, "firstUploadDate", null, false);
			Scribe_Values.Look<int>(ref this.firstUploadTime, "firstUploadTime", 0, false);
			Scribe_Values.Look<bool>(ref this.devMode, "devMode", false, false);
			Scribe_Deep.Look<History>(ref this.history, "history", new object[0]);
		}
	}
}
