using System;

namespace Verse
{
	// Token: 0x02000B6E RID: 2926
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

		// Token: 0x06003FEB RID: 16363 RVA: 0x0021B097 File Offset: 0x00219497
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
