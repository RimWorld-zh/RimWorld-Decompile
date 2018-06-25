using System;

namespace Verse
{
	// Token: 0x02000B43 RID: 2883
	public class InjuryProps
	{
		// Token: 0x04002995 RID: 10645
		public float painPerSeverity = 1f;

		// Token: 0x04002996 RID: 10646
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x04002997 RID: 10647
		public float bleedRate = 0f;

		// Token: 0x04002998 RID: 10648
		public bool canMerge = false;

		// Token: 0x04002999 RID: 10649
		public string destroyedLabel = null;

		// Token: 0x0400299A RID: 10650
		public string destroyedOutLabel = null;

		// Token: 0x0400299B RID: 10651
		public bool useRemovedLabel = false;
	}
}
