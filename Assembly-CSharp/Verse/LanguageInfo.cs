using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BF0 RID: 3056
	public class LanguageInfo
	{
		// Token: 0x04002DA5 RID: 11685
		public string friendlyNameNative;

		// Token: 0x04002DA6 RID: 11686
		public string friendlyNameEnglish;

		// Token: 0x04002DA7 RID: 11687
		public bool canBeTiny = true;

		// Token: 0x04002DA8 RID: 11688
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04002DA9 RID: 11689
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
