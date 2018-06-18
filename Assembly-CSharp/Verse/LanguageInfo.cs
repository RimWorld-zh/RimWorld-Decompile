using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BF4 RID: 3060
	public class LanguageInfo
	{
		// Token: 0x04002D9D RID: 11677
		public string friendlyNameNative;

		// Token: 0x04002D9E RID: 11678
		public string friendlyNameEnglish;

		// Token: 0x04002D9F RID: 11679
		public bool canBeTiny = true;

		// Token: 0x04002DA0 RID: 11680
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04002DA1 RID: 11681
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
