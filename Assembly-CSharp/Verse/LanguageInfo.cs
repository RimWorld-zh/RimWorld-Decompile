using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BF3 RID: 3059
	public class LanguageInfo
	{
		// Token: 0x04002DAC RID: 11692
		public string friendlyNameNative;

		// Token: 0x04002DAD RID: 11693
		public string friendlyNameEnglish;

		// Token: 0x04002DAE RID: 11694
		public bool canBeTiny = true;

		// Token: 0x04002DAF RID: 11695
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04002DB0 RID: 11696
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
