using System;

namespace Verse
{
	// Token: 0x02000D04 RID: 3332
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x040031EB RID: 12779
		public bool sendLetterWhenDiscovered = false;

		// Token: 0x040031EC RID: 12780
		public string discoverLetterLabel = null;

		// Token: 0x040031ED RID: 12781
		public string discoverLetterText = null;

		// Token: 0x040031EE RID: 12782
		public MessageTypeDef messageType = null;

		// Token: 0x040031EF RID: 12783
		public LetterDef letterType = null;

		// Token: 0x060049A0 RID: 18848 RVA: 0x002693EA File Offset: 0x002677EA
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}
	}
}
