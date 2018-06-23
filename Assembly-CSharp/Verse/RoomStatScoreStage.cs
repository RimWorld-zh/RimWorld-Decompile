using System;

namespace Verse
{
	// Token: 0x02000B6C RID: 2924
	public class RoomStatScoreStage
	{
		// Token: 0x04002ACE RID: 10958
		public float minScore = float.MinValue;

		// Token: 0x04002ACF RID: 10959
		public string label = null;

		// Token: 0x04002AD0 RID: 10960
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabel = null;

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0021AFBB File Offset: 0x002193BB
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
