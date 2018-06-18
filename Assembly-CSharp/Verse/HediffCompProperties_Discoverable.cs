using System;

namespace Verse
{
	// Token: 0x02000D07 RID: 3335
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x0600498F RID: 18831 RVA: 0x00267FD2 File Offset: 0x002663D2
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x040031E0 RID: 12768
		public bool sendLetterWhenDiscovered = false;

		// Token: 0x040031E1 RID: 12769
		public string discoverLetterLabel = null;

		// Token: 0x040031E2 RID: 12770
		public string discoverLetterText = null;

		// Token: 0x040031E3 RID: 12771
		public MessageTypeDef messageType = null;

		// Token: 0x040031E4 RID: 12772
		public LetterDef letterType = null;
	}
}
