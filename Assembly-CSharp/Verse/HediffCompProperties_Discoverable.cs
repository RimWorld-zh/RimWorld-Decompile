using System;

namespace Verse
{
	// Token: 0x02000D07 RID: 3335
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x040031F2 RID: 12786
		public bool sendLetterWhenDiscovered = false;

		// Token: 0x040031F3 RID: 12787
		public string discoverLetterLabel = null;

		// Token: 0x040031F4 RID: 12788
		public string discoverLetterText = null;

		// Token: 0x040031F5 RID: 12789
		public MessageTypeDef messageType = null;

		// Token: 0x040031F6 RID: 12790
		public LetterDef letterType = null;

		// Token: 0x060049A3 RID: 18851 RVA: 0x002697A6 File Offset: 0x00267BA6
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}
	}
}
