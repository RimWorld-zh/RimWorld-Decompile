using System;

namespace Verse
{
	// Token: 0x02000D08 RID: 3336
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x06004991 RID: 18833 RVA: 0x00267FFA File Offset: 0x002663FA
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x040031E2 RID: 12770
		public bool sendLetterWhenDiscovered = false;

		// Token: 0x040031E3 RID: 12771
		public string discoverLetterLabel = null;

		// Token: 0x040031E4 RID: 12772
		public string discoverLetterText = null;

		// Token: 0x040031E5 RID: 12773
		public MessageTypeDef messageType = null;

		// Token: 0x040031E6 RID: 12774
		public LetterDef letterType = null;
	}
}
