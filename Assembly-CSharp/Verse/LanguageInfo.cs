using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BF5 RID: 3061
	public class LanguageInfo
	{
		// Token: 0x04002D9F RID: 11679
		public string friendlyNameNative;

		// Token: 0x04002DA0 RID: 11680
		public string friendlyNameEnglish;

		// Token: 0x04002DA1 RID: 11681
		public bool canBeTiny = true;

		// Token: 0x04002DA2 RID: 11682
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04002DA3 RID: 11683
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
