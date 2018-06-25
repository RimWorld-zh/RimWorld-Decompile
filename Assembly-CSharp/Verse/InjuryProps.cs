using System;

namespace Verse
{
	// Token: 0x02000B42 RID: 2882
	public class InjuryProps
	{
		// Token: 0x0400298E RID: 10638
		public float painPerSeverity = 1f;

		// Token: 0x0400298F RID: 10639
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x04002990 RID: 10640
		public float bleedRate = 0f;

		// Token: 0x04002991 RID: 10641
		public bool canMerge = false;

		// Token: 0x04002992 RID: 10642
		public string destroyedLabel = null;

		// Token: 0x04002993 RID: 10643
		public string destroyedOutLabel = null;

		// Token: 0x04002994 RID: 10644
		public bool useRemovedLabel = false;
	}
}
